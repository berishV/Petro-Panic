using System.Collections;
using UnityEngine;

public class ControladorEnemigoPequeño : MonoBehaviour
{
    [Header("Movimiento Escalonado (Tipo Galaga/Space Invaders)")]
    [SerializeField] private float velocidadHorizontal = 3.5f;
    [SerializeField] private float limiteIzquierdo = -3.5f;
    [SerializeField] private float limiteDerecho = 3.5f;
    [Tooltip("Distancia en el eje Y que baja el enemigo cada vez que rebota en un lateral")]
    [SerializeField] private float bajadaPorNivel = 0.75f;

    [Header("Bucle de Pantalla (Ataques Continuos)")]
    [Tooltip("Límite inferior de la cámara. Al cruzarlo, volverá arriba en vez de destruirse")]
    [SerializeField] private float limiteInferiorPantalla = -6f;
    [Tooltip("Altura superior a la que teletransportará al enemigo para reiniciar su descenso")]
    [SerializeField] private float alturaReaparicionTop = 7.5f;

    [Header("Disparo en Ráfagas")]
    [SerializeField] private GameObject prefabProyectil;
    [Tooltip("Tiempo de espera entre ráfagas completas (Cadencia lenta general)")]
    [SerializeField] private float tiempoEntreRafagas = 4.5f;
    [Tooltip("Separación de tiempo milimétrica entre las balas de una misma ráfaga")]
    [SerializeField] private float retrasoEntreBalasRafaga = 0.18f;
    [SerializeField] private Transform puntoDisparo;

    private Vector2 direccionHorizontal = Vector2.right;
    private float cronometroRafaga;
    private bool estaDisparandoRafaga = false;

    void Start()
    {
        cronometroRafaga = tiempoEntreRafagas;

        direccionHorizontal = (Random.value > 0.5f) ? Vector2.right : Vector2.left;
    }

    void Update()
    {
        ManejarMovimientoEscalonado();
        ManejarDisparoContinuo();
    }

    void ManejarMovimientoEscalonado()
    {
        transform.Translate(direccionHorizontal * velocidadHorizontal * Time.deltaTime, Space.World);
        float xActual = transform.position.x;

        if (xActual >= limiteDerecho && direccionHorizontal.x > 0f)
        {
            direccionHorizontal = Vector2.left;
            BajarUnNivel();
        }
        else if (xActual <= limiteIzquierdo && direccionHorizontal.x < 0f)
        {
            direccionHorizontal = Vector2.right;
            BajarUnNivel();
        }

        if (transform.position.y < limiteInferiorPantalla)
        {
            transform.position = new Vector3(transform.position.x, alturaReaparicionTop, transform.position.z);
        }
    }

    void BajarUnNivel()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - bajadaPorNivel, transform.position.z);
    }

    void ManejarDisparoContinuo()
    {
        if (estaDisparandoRafaga) return;

        cronometroRafaga -= Time.deltaTime;
        if (cronometroRafaga <= 0f)
        {
            StartCoroutine(DispararRafagaFlujo());
            cronometroRafaga = tiempoEntreRafagas;
        }
    }

    private IEnumerator DispararRafagaFlujo()
    {
        estaDisparandoRafaga = true;

        int cantidadDisparos = Random.Range(2, 5);

        for (int i = 0; i < cantidadDisparos; i++)
        {
            Vector3 origen = puntoDisparo != null ? puntoDisparo.position : transform.position;
            GameObject proy = Instantiate(prefabProyectil, origen, Quaternion.identity);

            MovimientoProyectilPequeño movP = proy.GetComponent<MovimientoProyectilPequeño>();
            if (movP != null)
            {
                movP.EstablecerDireccion(Vector2.down);
            }
            else
            {
                MovimientoProyectilMedio movM = proy.GetComponent<MovimientoProyectilMedio>();
                if (movM != null) movM.EstablecerDireccion(Vector2.down);
            }

            yield return new WaitForSeconds(retrasoEntreBalasRafaga);
        }

        estaDisparandoRafaga = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet-P"))
        {
            if (UIManager.Instancia != null) UIManager.Instancia.SumarPuntos(300);

            Destroy(col.gameObject);

            Destroy(gameObject);
        }
    }
}
