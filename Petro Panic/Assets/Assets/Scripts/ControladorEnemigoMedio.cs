using UnityEngine;

public class ControladorEnemigoMedio : MonoBehaviour
{
    [Header("Salud")]
    [Tooltip("Cantidad de golpes de la bala del jugador que necesita para ser destruido")]
    [SerializeField] private int vidas = 2;

    [Header("Movimiento Zigzag")]
    [Tooltip("Velocidad de desplazamiento diagonal")]
    [SerializeField] private float velocidadMovimiento = 1.2f;
    [Tooltip("Ángulo del zigzag en grados (45 = diagonal perfecta)")]
    [SerializeField] private float anguloZigzag = 45f;
    [Tooltip("Límite izquierdo del área de juego")]
    [SerializeField] private float limiteIzquierdo = -3.5f;
    [Tooltip("Límite derecho del área de juego")]
    [SerializeField] private float limiteDerecho = 3.5f;
    [Tooltip("Y en la que el enemigo se destruye para liberar memoria")]
    [SerializeField] private float limiteInferiorPantalla = -6f;

    [Header("Disparo")]
    [Tooltip("Prefab del proyectil del enemigo mediano")]
    [SerializeField] private GameObject prefabProyectil;
    [Tooltip("Tiempo entre cada ráfaga de proyectiles")]
    [SerializeField] private float cadenciaDisparo = 0.45f;
    [Tooltip("Tiempo antes de cambiar entre modo esquinas y modo ejes")]
    [SerializeField] private float tiempoEntrePatrones = 4f;
    [Tooltip("Punto de origen de los proyectiles")]
    [SerializeField] private Transform puntoDisparo;

    [Header("Entrada")]
    [Tooltip("Velocidad al bajar desde fuera de pantalla")]
    [SerializeField] private float velocidadEntrada = 4f;

    private Vector2 direccionActual;
    private bool llegoPosicion = false;
    private Vector3 posicionFormacion;
    private bool tieneFormacion = false;
    private float cronometroDisparo;
    private float cronometroPatron;
    private int modoDisparo = 0;

    void Start()
    {
        float rad = anguloZigzag * Mathf.Deg2Rad;
        direccionActual = new Vector2(Mathf.Cos(rad), -Mathf.Sin(rad)).normalized;

        cronometroDisparo = cadenciaDisparo;
        cronometroPatron = tiempoEntrePatrones;
    }

    public void AsignarPosicionFormacion(Vector3 pos)
    {
        posicionFormacion = pos;
        tieneFormacion = true;
    }

    void Update()
    {
        if (transform.position.y < limiteInferiorPantalla)
        {
            Destroy(gameObject);
            return;
        }

        if (tieneFormacion && !llegoPosicion)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                posicionFormacion,
                velocidadEntrada * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, posicionFormacion) < 0.05f)
            {
                transform.position = posicionFormacion;
                llegoPosicion = true;
            }
            return;
        }

        MoverZigzag();
        ManejarDisparo();
    }

    void MoverZigzag()
    {
        transform.Translate(direccionActual * velocidadMovimiento * Time.deltaTime, Space.World);
        float x = transform.position.x;

        if (x >= limiteDerecho && direccionActual.x > 0f)
        {
            direccionActual = new Vector2(-Mathf.Abs(direccionActual.x), direccionActual.y).normalized;
        }
        else if (x <= limiteIzquierdo && direccionActual.x < 0f)
        {
            direccionActual = new Vector2(Mathf.Abs(direccionActual.x), direccionActual.y).normalized;
        }
    }

    void ManejarDisparo()
    {
        cronometroPatron -= Time.deltaTime;
        if (cronometroPatron <= 0f)
        {
            modoDisparo = (modoDisparo + 1) % 2;
            cronometroPatron = tiempoEntrePatrones;
        }

        cronometroDisparo -= Time.deltaTime;
        if (cronometroDisparo <= 0f)
        {
            Disparar();
            cronometroDisparo = cadenciaDisparo;
        }
    }

    void Disparar()
    {
        Vector2[] dirs;
        if (modoDisparo == 0)
        {
            dirs = new Vector2[]
            {
                new Vector2( 1f,  1f).normalized,
                new Vector2(-1f,  1f).normalized,
                new Vector2( 1f, -1f).normalized,
                new Vector2(-1f, -1f).normalized
            };
        }
        else
        {
            dirs = new Vector2[]
            {
                Vector2.up,
                Vector2.down,
                Vector2.left,
                Vector2.right
            };
        }

        Vector3 origen = puntoDisparo != null ? puntoDisparo.position : transform.position;
        foreach (Vector2 dir in dirs)
        {
            GameObject proy = Instantiate(prefabProyectil, origen, Quaternion.identity);
            MovimientoProyectilMedio mov = proy.GetComponent<MovimientoProyectilMedio>();
            if (mov != null)
                mov.EstablecerDireccion(dir);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet-P"))
        {
            Destroy(col.gameObject);

            vidas--;

            if (UIManager.Instancia != null) UIManager.Instancia.SumarPuntos(200);

            if (vidas <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}