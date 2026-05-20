using System.Collections.Generic;
using UnityEngine;

public class GestorOleadasPequeño : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject enemigoPequeñoPrefab;

    [Header("Control de Población")]
    [Tooltip("Límite absoluto de enemigos pequeños permitidos simultáneamente en pantalla")]
    [SerializeField] private int maxEnemigosEnPantalla = 6;
    [Tooltip("Tiempo de espera entre el envío de una escuadra y otra")]
    [SerializeField] private float tiempoEntreEscuadras = 6f;

    [Header("Configuración de Escuadra")]
    [Tooltip("Cuántos enemigos componen cada mini-escuadra de ataque")]
    [SerializeField] private int tamañoEscuadra = 3;
    [Tooltip("Separación horizontal fija entre los miembros de la escuadra al nacer")]
    [SerializeField] private float separacionX = 1.5f;
    [SerializeField] private float alturaSpawnY = 8f;

    private List<GameObject> listaEnemigos = new List<GameObject>();
    private float cronometroEscuadra;

    void Start()
    {
        cronometroEscuadra = 1f;
    }

    void Update()
    {
        LimpiarListaEnemigosNull();
        ManejarEnvioEscuadras();
    }

    void LimpiarListaEnemigosNull()
    {
        for (int i = listaEnemigos.Count - 1; i >= 0; i--)
        {
            if (listaEnemigos[i] == null)
            {
                listaEnemigos.RemoveAt(i);
            }
        }
    }

    void ManejarEnvioEscuadras()
    {
        cronometroEscuadra -= Time.deltaTime;

        if (cronometroEscuadra <= 0f)
        {
            if (listaEnemigos.Count + tamañoEscuadra <= maxEnemigosEnPantalla)
            {
                SpawnearEscuadra();
                cronometroEscuadra = tiempoEntreEscuadras;
            }
            else
            {
                cronometroEscuadra = 1f;
            }
        }
    }

    void SpawnearEscuadra()
    {
        float anchoTotalEscuadra = (tamañoEscuadra - 1) * separacionX;
        float inicioX = -anchoTotalEscuadra / 2f;

        float variacionCentroX = Random.Range(-1.5f, 1.5f);

        for (int i = 0; i < tamañoEscuadra; i++)
        {
            float posX = inicioX + (i * separacionX) + variacionCentroX;
            Vector3 posicionSpawn = new Vector3(posX, alturaSpawnY, 0f);

            GameObject nuevoEnemigo = Instantiate(enemigoPequeñoPrefab, posicionSpawn, Quaternion.identity);
            listaEnemigos.Add(nuevoEnemigo);
        }
    }
}
