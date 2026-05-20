using UnityEngine;

public class Camara : MonoBehaviour
{
    [Header("Objetivos")]
    public Transform jugador;
    public Transform fondoPrincipal;

    [Header("Movimiento y Suavizado")]
    public float suavizado = 2f;
    public float fuerzaAutoCentrado = 1f; 

    [Header("Límites de la Cámara")]
    public float limiteMinX = -3f;
    public float limiteMaxX = 3f;
    public float limiteMinY = -2f;
    public float limiteMaxY = 2f;

    private Vector3 posicionCentro;

    void Start()
    {
        posicionCentro = new Vector3(0f, 0f, transform.position.z);
    }

    void LateUpdate()
    {
        if (jugador == null) return;

        Vector3 posicionDeseadaJugador = new Vector3(jugador.position.x, jugador.position.y, transform.position.z);
        Vector3 posicionObjetivo = Vector3.Lerp(posicionCentro, posicionDeseadaJugador, 0.5f);

        Vector3 posicionSuave = Vector3.Lerp(transform.position, posicionObjetivo, suavizado * Time.deltaTime);

        float xRestringida = Mathf.Clamp(posicionSuave.x, limiteMinX, limiteMaxX);
        float yRestringida = Mathf.Clamp(posicionSuave.y, limiteMinY, limiteMaxY);

        transform.position = new Vector3(xRestringida, yRestringida, transform.position.z);
    }
}
