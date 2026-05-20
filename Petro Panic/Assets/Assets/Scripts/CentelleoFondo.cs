using UnityEngine;

public class CentelleoFondo : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Configuración de Opacidad")]
    [Range(0f, 1f)] public float opacidadMinima = 0.0f;
    [Range(0f, 1f)] public float opacidadMaxima = 0.5f;

    [Header("Configuración de Tiempo")]
    public float tiempoCambioMin = 0.05f;
    public float tiempoCambioMax = 0.2f;

    private float tiempoSiguienteCambio;
    private float opacidadObjetivo;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        opacidadObjetivo = Random.Range(opacidadMinima, opacidadMaxima);
    }

    void Update()
    {
        if (spriteRenderer == null) return;

        if (Time.time >= tiempoSiguienteCambio)
        {
            opacidadObjetivo = Random.Range(opacidadMinima, opacidadMaxima);
            tiempoSiguienteCambio = Time.time + Random.Range(tiempoCambioMin, tiempoCambioMax);
        }

        Color colorActual = spriteRenderer.color;
        colorActual.a = Mathf.MoveTowards(colorActual.a, opacidadObjetivo, Time.deltaTime * 5f);
        spriteRenderer.color = colorActual;
    }
}
