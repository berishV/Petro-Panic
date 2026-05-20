using UnityEngine;
using System.Collections;

public class ControladorBoss : MonoBehaviour
{
    [Header("Movimiento Base")]
    public float velocidadMovimiento = 2f;
    public float amplitudFlote = 0.5f;

    [Header("Balas Prefabs")]
    public GameObject prefabMini;
    public GameObject prefabMedium;
    public GameObject prefabLarge;
    public GameObject objetoLaser;

    private Vector3 posInicial;
    private Transform jugador;
    private float cronometroBatalla = 0f;

    private float objetivoX;
    private float tiempoParaCambiarPos = 2f;
    private float cronometroMovimiento = 0f;

    void Start()
    {
        posInicial = transform.position;
        objetivoX = posInicial.x;

        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null) jugador = objJugador.transform;

        if (objetoLaser != null) objetoLaser.SetActive(false);

        StartCoroutine(RutinaAtaques());
    }

    void Update()
    {
        cronometroBatalla += Time.deltaTime;
        cronometroMovimiento += Time.deltaTime;

        float multiplicadorDificultad = 1f + (cronometroBatalla / 270f) * 1.5f;

        if (cronometroMovimiento >= (tiempoParaCambiarPos / multiplicadorDificultad))
        {
            objetivoX = Random.Range(-3.5f, 3.5f);
            cronometroMovimiento = 0f;

            if (multiplicadorDificultad > 1.5f)
            {
                if (SacudidaCamara.Instancia != null) SacudidaCamara.Instancia.Sacudir(0.15f, 0.1f);
            }
        }

        float nuevaY = posInicial.y + Mathf.Sin(Time.time * multiplicadorDificultad) * amplitudFlote;
        Vector3 destino = new Vector3(objetivoX, nuevaY, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, destino, (velocidadMovimiento * multiplicadorDificultad) * Time.deltaTime);
    }

    IEnumerator RutinaAtaques()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            float multiplicador = 1f + (cronometroBatalla / 270f) * 1.5f;

            int patron = Random.Range(0, 5);
            switch (patron)
            {
                case 0: yield return StartCoroutine(PatronDisparosDirigidos(multiplicador)); break;
                case 1: yield return StartCoroutine(PatronRafagaMini(multiplicador)); break;
                case 2: yield return StartCoroutine(PatronTrianguloLarge(multiplicador)); break;
                case 3: yield return StartCoroutine(PatronLaserGigante(multiplicador)); break;
                case 4: yield return StartCoroutine(PatronEspiralMuerte(multiplicador)); break;
            }

            yield return new WaitForSeconds(1.5f / multiplicador);
        }
    }


    IEnumerator PatronDisparosDirigidos(float multi)
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 dir = Vector2.down;
            if (jugador != null) dir = jugador.position - transform.position;

            Disparar(prefabMedium, dir, 6f * multi);
            yield return new WaitForSeconds(0.4f / multi);
        }
    }

    IEnumerator PatronRafagaMini(float multi)
    {
        for (int i = 0; i < 15; i++)
        {
            Vector2 dirAleatoria = new Vector2(Random.Range(-0.4f, 0.4f), -1f);
            Disparar(prefabMini, dirAleatoria, 8f * multi);
            yield return new WaitForSeconds(0.1f / multi);
        }
    }

    IEnumerator PatronTrianguloLarge(float multi)
    {
        for (int i = 0; i < 4; i++)
        {
            Disparar(prefabLarge, Vector2.down, 4f * multi);
            Disparar(prefabLarge, new Vector2(-0.5f, -1f), 4f * multi);
            Disparar(prefabLarge, new Vector2(0.5f, -1f), 4f * multi);
            yield return new WaitForSeconds(0.8f / multi);
        }
    }

    IEnumerator PatronLaserGigante(float multi)
    {
        if (SacudidaCamara.Instancia != null) SacudidaCamara.Instancia.Sacudir(0.5f, 0.3f);
        yield return new WaitForSeconds(1f / multi);

        if (objetoLaser != null)
        {
            objetoLaser.SetActive(true);
            if (SacudidaCamara.Instancia != null) SacudidaCamara.Instancia.Sacudir(2.5f, 0.15f);
            yield return new WaitForSeconds(2.5f);
            objetoLaser.SetActive(false);
        }
    }

    IEnumerator PatronEspiralMuerte(float multi)
    {
        float angulo = 0f;
        for (int i = 0; i < 30; i++)
        {
            float dirX = Mathf.Cos(angulo * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angulo * Mathf.Deg2Rad);

            Disparar(prefabMini, new Vector2(dirX, dirY), 5f * multi);
            angulo += 25f;
            yield return new WaitForSeconds(0.1f / multi);
        }
    }

    void Disparar(GameObject prefabBala, Vector2 direccion, float velocidad)
    {
        if (prefabBala == null) return;

        GameObject bala = Instantiate(prefabBala, transform.position, Quaternion.identity);
        ProyectilBoss script = bala.GetComponent<ProyectilBoss>();
        if (script != null) script.ConfigurarDisparo(direccion, velocidad);
    }
}
