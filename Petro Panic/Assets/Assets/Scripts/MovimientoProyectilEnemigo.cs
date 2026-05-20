using UnityEngine;

public class MovimientoProyectilEnemigo : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float velocidad = 7f;
    [SerializeField] private float tiempoDeVida = 4f;

    private Vector2 direccion = Vector2.down;
    private bool direccionEstablecida = false;

    void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    public void EstablecerDireccion(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized;
        direccionEstablecida = true;
    }

    void Update()
    {
        Vector2 dir = direccionEstablecida ? direccion : Vector2.down;
        transform.Translate(dir * velocidad * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}