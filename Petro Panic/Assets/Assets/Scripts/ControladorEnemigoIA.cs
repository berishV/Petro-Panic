using UnityEngine;

public class ControladorEnemigoIA : MonoBehaviour
{
    private enum Estado
    {
        Entrada,
        Patrulla,
        PreparandoPicado,
        Picado,
        Retorno
    }

    [Header("═ Salud ═")]
    [Tooltip("Cantidad de golpes necesarios para destruirlo")]
    [SerializeField] private int vidas = 4;

    [Header("═ Prefabs y Puntos ═")]
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform puntoDisparo;

    [Header("═ Entrada a Formación ═")]
    [SerializeField] private float velocidadEntrada = 4f;
    [SerializeField] private float alturaFormacion = 3f;

    [Header("═ Patrulla (formación) ═")]
    [SerializeField] private float velocidadPatrullaX = 2f;
    [SerializeField] private float rangoPatrullaX = 3f;

    [Header("═ Disparo ═")]
    [SerializeField] private float tiempoEntreDisparos = 2.5f;
    [SerializeField] private int disparosEnRafaga = 2;
    [SerializeField] private float tiempoEntreRafaga = 0.25f;

    [Header("═ Picado ═")]
    [SerializeField] private float probabilidadPicado = 0.004f;
    [SerializeField] private float tiempoPreparacion = 0.6f;
    [SerializeField] private float velocidadPicado = 7f;
    [SerializeField] private float velocidadRetorno = 9f;

    [Header("═ Límites ═")]
    [SerializeField] private float limiteInferiorY = -7f;
    [SerializeField] private float limiteRetornoY = 9f;

    private Estado estadoActual = Estado.Entrada;
    private Transform jugador;

    private Vector3 posicionFormacion;
    private bool posicionFormacionAsignada = false;

    private float fasePatrulla;

    private float cronometroDisparo;
    private int rafagaRestante;
    private float cronometroRafaga;

    private float cronometroPreparacion;
    private Vector3 dirPicado;

    void Start()
    {
        if (!posicionFormacionAsignada)
        {
            posicionFormacion = new Vector3(transform.position.x, alturaFormacion, 0f);
        }

        cronometroDisparo = Random.Range(0f, tiempoEntreDisparos);
        fasePatrulla = Random.Range(0f, Mathf.PI * 2f);

        GameObject go = GameObject.FindWithTag("Player");
        if (go != null) jugador = go.transform;
    }

    public void AsignarPosicionFormacion(Vector3 posicion)
    {
        posicionFormacion = posicion;
        posicionFormacionAsignada = true;
    }

    void Update()
    {
        switch (estadoActual)
        {
            case Estado.Entrada: ActualizarEntrada(); break;
            case Estado.Patrulla: ActualizarPatrulla(); break;
            case Estado.PreparandoPicado: ActualizarPreparacionPicado(); break;
            case Estado.Picado: ActualizarPicado(); break;
            case Estado.Retorno: ActualizarRetorno(); break;
        }

        GestionarDisparo();
        ComprobarLimiteInferior();
    }

    void ActualizarEntrada()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            posicionFormacion,
            velocidadEntrada * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, posicionFormacion) < 0.05f)
        {
            transform.position = posicionFormacion;
            CambiarEstado(Estado.Patrulla);
        }
    }

    void ActualizarPatrulla()
    {
        fasePatrulla += Time.deltaTime * velocidadPatrullaX;
        float offsetX = Mathf.Sin(fasePatrulla) * rangoPatrullaX;
        transform.position = new Vector3(posicionFormacion.x + offsetX, posicionFormacion.y, 0f);

        if (jugador != null && transform.position.y > jugador.position.y)
        {
            if (Random.value < probabilidadPicado)
            {
                CambiarEstado(Estado.PreparandoPicado);
            }
        }
    }

    void ActualizarPreparacionPicado()
    {
        cronometroPreparacion -= Time.deltaTime;
        if (cronometroPreparacion <= 0f)
        {
            if (jugador != null)
                dirPicado = (jugador.position - transform.position).normalized;
            else
                dirPicado = Vector3.down;

            CambiarEstado(Estado.Picado);
        }
    }

    void ActualizarPicado()
    {
        transform.position += dirPicado * velocidadPicado * Time.deltaTime;
    }

    void ActualizarRetorno()
    {
        transform.position += Vector3.up * velocidadRetorno * Time.deltaTime;

        if (transform.position.y >= limiteRetornoY)
        {
            transform.position = new Vector3(posicionFormacion.x, limiteRetornoY, 0f);
            CambiarEstado(Estado.Entrada);
        }
    }

    void GestionarDisparo()
    {
        if (estadoActual == Estado.PreparandoPicado || estadoActual == Estado.Retorno) return;
        if (proyectilPrefab == null) return;

        if (rafagaRestante > 0)
        {
            cronometroRafaga -= Time.deltaTime;
            if (cronometroRafaga <= 0f)
            {
                LanzarProyectil();
                rafagaRestante--;
                cronometroRafaga = tiempoEntreRafaga;
            }
            return;
        }

        cronometroDisparo -= Time.deltaTime;
        if (cronometroDisparo <= 0f)
        {
            rafagaRestante = disparosEnRafaga;
            cronometroRafaga = 0f;
            cronometroDisparo = tiempoEntreDisparos;
        }
    }

    void LanzarProyectil()
    {
        Transform origen = puntoDisparo != null ? puntoDisparo : transform;

        Vector2 direccion = Vector2.down;
        if (jugador != null)
        {
            direccion = (jugador.position - origen.position).normalized;
        }

        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotacion = Quaternion.Euler(0f, 0f, angulo);

        GameObject bala = Instantiate(proyectilPrefab, origen.position, rotacion);

        MovimientoProyectilEnemigo movimiento = bala.GetComponent<MovimientoProyectilEnemigo>();
        if (movimiento != null)
        {
            movimiento.EstablecerDireccion(direccion);
        }
    }

    void ComprobarLimiteInferior()
    {
        if (estadoActual == Estado.Picado && transform.position.y < limiteInferiorY)
        {
            CambiarEstado(Estado.Retorno);
        }
    }

    void CambiarEstado(Estado nuevo)
    {
        switch (nuevo)
        {
            case Estado.PreparandoPicado:
                cronometroPreparacion = tiempoPreparacion;
                break;
        }
        estadoActual = nuevo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet-P"))
        {
            Destroy(collision.gameObject);

            vidas--;

            if (UIManager.Instancia != null) UIManager.Instancia.SumarPuntos(300);

            if (vidas <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}