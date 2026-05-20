using UnityEngine;

public class Fondo : MonoBehaviour
{
    [Header("Configuración de Velocidad")]
    public float velocidadScroll = 8f;

    [Header("Configuración del Loop")]
    [Tooltip("La distancia exacta en Y que debe avanzar el fondo antes de reiniciarse.")]
    public float puntoDeReinicioY = 20f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.down * velocidadScroll * Time.deltaTime, Space.World);

        if (transform.position.y <= posicionInicial.y - puntoDeReinicioY)
        {
            transform.position = posicionInicial;
        }
    }
}
