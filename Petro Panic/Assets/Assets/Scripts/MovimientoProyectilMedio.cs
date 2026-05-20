using UnityEngine;

public class MovimientoProyectilMedio : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Velocidad del proyectil")]
    [SerializeField] private float velocidad = 6f;
    [Tooltip("Tiempo de vida máximo (seguridad secundaria)")]
    [SerializeField] private float tiempoDeVida = 5f;

    [Header("Límites de Destrucción (Cámara)")]
    [Tooltip("Destruye el proyectil si supera estos límites para ahorrar memoria")]
    [SerializeField] private float limiteYSuperior = 10f;
    [SerializeField] private float limiteYInferior = -10f;
    [SerializeField] private float limiteXIzquierdo = -6f;
    [SerializeField] private float limiteXDerecho = 6f;

    private Vector2 direccion = Vector2.down;

    void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    public void EstablecerDireccion(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized;
    }

    void Update()
    {
        // Movimiento
        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);

        Vector3 pos = transform.position;
        if (pos.y > limiteYSuperior || pos.y < limiteYInferior ||
            pos.x < limiteXIzquierdo || pos.x > limiteXDerecho)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}