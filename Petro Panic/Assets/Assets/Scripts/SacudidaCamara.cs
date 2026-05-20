using UnityEngine;
using System.Collections;

public class SacudidaCamara : MonoBehaviour
{
    public static SacudidaCamara Instancia;
    private Vector3 posicionOriginal;

    void Awake()
    {
        if (Instancia == null) Instancia = this;
    }

    void Start()
    {
        posicionOriginal = transform.position;
    }

    public void Sacudir(float duracion, float magnitud)
    {
        StopAllCoroutines();
        StartCoroutine(GenerarSacudida(duracion, magnitud));
    }

    IEnumerator GenerarSacudida(float duracion, float magnitud)
    {
        float tiempoPasado = 0f;

        while (tiempoPasado < duracion)
        {
            float x = posicionOriginal.x + Random.Range(-1f, 1f) * magnitud;
            float y = posicionOriginal.y + Random.Range(-1f, 1f) * magnitud;

            transform.position = new Vector3(x, y, posicionOriginal.z);
            tiempoPasado += Time.deltaTime;

            yield return null;
        }

        transform.position = posicionOriginal;
    }
}
