using UnityEngine;

public class GestorOleadasMedio : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject enemigoPrefab;

    [Header("Configuración de Oleada")]
    [Tooltip("Máximo de enemigos medianos permitidos en pantalla al mismo tiempo")]
    [SerializeField] private int maxEnemigosEnPantalla = 2;

    [Tooltip("Límites horizontales para que no aparezcan amontonados")]
    [SerializeField] private float limiteIzquierdoSpawn = -3f;
    [SerializeField] private float limiteDerechoSpawn = 3f;

    [SerializeField] private float alturaSpawn = 8f;
    [SerializeField] private float alturaFormacion = 3f;

    [Header("Tiempo")]
    [Tooltip("Tiempo de espera para reponer un enemigo destruido")]
    [SerializeField] private float tiempoEntreSpawns = 2f;

    private GameObject[] enemigosActivos;
    private float cronometroSpawn;

    void Start()
    {
        enemigosActivos = new GameObject[maxEnemigosEnPantalla];
        cronometroSpawn = tiempoEntreSpawns;
    }

    void Update()
    {
        ManejarReaparicion();
    }

    void ManejarReaparicion()
    {
        cronometroSpawn -= Time.deltaTime;

        if (cronometroSpawn <= 0f)
        {
            for (int i = 0; i < enemigosActivos.Length; i++)
            {
                if (enemigosActivos[i] == null)
                {
                    CrearEnemigo(i);
                    cronometroSpawn = tiempoEntreSpawns;
                    break; 
                }
            }
        }
    }

    void CrearEnemigo(int indice)
    {
        float xAleatorio = Random.Range(limiteIzquierdoSpawn, limiteDerechoSpawn);

        Vector3 spawnPos = new Vector3(xAleatorio, alturaSpawn, 0f);
        Vector3 formacionPos = new Vector3(xAleatorio, alturaFormacion, 0f);

        GameObject nuevoEnemigo = Instantiate(enemigoPrefab, spawnPos, Quaternion.identity);
        enemigosActivos[indice] = nuevoEnemigo;

        ControladorEnemigoMedio ctrl = nuevoEnemigo.GetComponent<ControladorEnemigoMedio>();
        if (ctrl != null)
        {
            ctrl.AsignarPosicionFormacion(formacionPos);
        }
    }
}