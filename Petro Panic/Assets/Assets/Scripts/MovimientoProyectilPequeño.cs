using UnityEngine;

public class MovimientoProyectilPequeño : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float tiempoDeVidaMaximo = 5f;

    [Header("Límites de Destrucción Automática")]
    [SerializeField] private float limiteYSuperior = 10f;
    [SerializeField] private float limiteYInferior = -10f;
    [SerializeField] private float limiteXIzquierdo = -6f;
    [SerializeField] private float limiteXDerecho = 6f;

    private Vector2 direccion = Vector2.down;

    void Start()
    {
        Destroy(gameObject, tiempoDeVidaMaximo);
    }

    public void EstablecerDireccion(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized;
    }

    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);

        Vector3 posActual = transform.position;
        if (posActual.y > limiteYSuperior || posActual.y < limiteYInferior ||
            posActual.x < limiteXIzquierdo || posActual.x > limiteXDerecho)
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