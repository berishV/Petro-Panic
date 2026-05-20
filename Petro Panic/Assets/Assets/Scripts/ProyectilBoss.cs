using UnityEngine;

public class ProyectilBoss : MonoBehaviour
{
    private Vector2 direccion;
    private float velocidad;
    private float tiempoDeVida = 8f;

    public void ConfigurarDisparo(Vector2 dir, float vel)
    {
        direccion = dir.normalized;
        velocidad = vel;
        Destroy(gameObject, tiempoDeVida);
    }

    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}