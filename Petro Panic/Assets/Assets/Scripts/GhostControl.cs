using UnityEngine;

public class GhostControl : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Configuración del Efecto")]
    public float velocidadDesvanecer = 5f;
    public float velocidadEncoger = 2f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color colorActual = sr.color;
        colorActual.a -= velocidadDesvanecer * Time.deltaTime;
        sr.color = colorActual;

        transform.localScale -= Vector3.one * velocidadEncoger * Time.deltaTime;

        if (colorActual.a <= 0 || transform.localScale.x <= 0)
        {
            Destroy(gameObject);
        }
    }
}
