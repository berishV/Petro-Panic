using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    [System.Serializable]
    public class EtapaDificultad
    {
        public string nombreEtapa = "Nueva Etapa";
        [Tooltip("Segundo exacto del nivel en el que se activará esta etapa")]
        public float tiempoDeActivacion;
        [Tooltip("Arrastra aquí los PREFABS (archivos azules) de los spawns que aparecerán en esta etapa")]
        public GameObject[] prefabsSpawns;
    }

    [Header("═ Música del Nivel ═")]
    [Tooltip("Arrastra aquí directamente tu archivo de música amarillo (AudioClip)")]
    public AudioClip musicaNivel;
    [Range(0f, 1f)] public float volumenMusica = 0.5f;

    [Header("═ Diseñador de Etapas / Fases ═")]
    [Tooltip("Crea aquí cuántas etapas quieras para el nivel")]
    public List<EtapaDificultad> etapas = new List<EtapaDificultad>();

    private AudioSource audioSource;
    private float cronometroNivel = 0f;
    private float tiempoTotalNivel;
    private bool nivelTerminado = false;

    private HashSet<int> etapasActivadas = new HashSet<int>();
    private List<GameObject> spawnsInstanciados = new List<GameObject>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        if (musicaNivel != null)
        {
            audioSource.clip = musicaNivel;
            audioSource.volume = volumenMusica;
            audioSource.Play();
            
            tiempoTotalNivel = musicaNivel.length;
        }
        else
        {
            Debug.LogWarning("¡No asignaste música en el StageManager! El nivel durará 60 segundos por defecto.");
            tiempoTotalNivel = 60f;
        }
    }

    void Update()
    {
        if (nivelTerminado) return;

        cronometroNivel += Time.deltaTime;

        for (int i = 0; i < etapas.Count; i++)
        {
            if (!etapasActivadas.Contains(i) && cronometroNivel >= etapas[i].tiempoDeActivacion)
            {
                ActivarEtapa(i);
            }
        }

        if (cronometroNivel >= tiempoTotalNivel)
        {
            FinalizarNivel();
        }
    }

    void ActivarEtapa(int index)
    {
        etapasActivadas.Add(index);
        Debug.Log($"<color=cyan>⚡ Iniciando Etapa: {etapas[index].nombreEtapa} al segundo {cronometroNivel:F1}</color>");

        foreach (GameObject prefab in etapas[index].prefabsSpawns)
        {
            if (prefab != null)
            {
                GameObject nuevoSpawn = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                spawnsInstanciados.Add(nuevoSpawn);
            }
        }
    }

    public void DetenerMusicaNivel()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void FinalizarNivel()
    {
        nivelTerminado = true;
        DetenerMusicaNivel();

        foreach (GameObject spawn in spawnsInstanciados)
        {
            if (spawn != null) Destroy(spawn);
        }

        int siguienteEscenaIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (siguienteEscenaIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(siguienteEscenaIndex);
        }
        else
        {
            Debug.Log("<color=green>¡Felicidades, terminaste todas las escenas del juego!</color>");
        }
    }
}