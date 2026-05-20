using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BossStageManager : MonoBehaviour
{
    [System.Serializable]
    public class EtapaBoss
    {
        public string nombreEtapa;
        [Tooltip("Segundo exacto en el que saldrán estos enemigos extra")]
        public float tiempoDeActivacion;
        public GameObject[] prefabsSpawns;
    }

    [Header("═ Supervivencia ═")]
    public float tiempoSupervivencia = 270f;
    public AudioClip musicaBoss;
    [Range(0f, 1f)] public float volumenMusica = 0.6f;

    [Header("═ Spawns Extras (Estorbos) ═")]
    public List<EtapaBoss> etapasExtras = new List<EtapaBoss>();

    private AudioSource audioSource;
    private float cronometro = 0f;
    private bool nivelTerminado = false;

    private HashSet<int> etapasActivadas = new HashSet<int>();
    private List<GameObject> spawnsInstanciados = new List<GameObject>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        if (musicaBoss != null)
        {
            audioSource.clip = musicaBoss;
            audioSource.volume = volumenMusica;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (nivelTerminado) return;

        cronometro += Time.deltaTime;

        for (int i = 0; i < etapasExtras.Count; i++)
        {
            if (!etapasActivadas.Contains(i) && cronometro >= etapasExtras[i].tiempoDeActivacion)
            {
                ActivarEtapa(i);
            }
        }

        if (cronometro >= tiempoSupervivencia)
        {
            FinalizarNivel();
        }
    }

    void ActivarEtapa(int index)
    {
        etapasActivadas.Add(index);
        foreach (GameObject prefab in etapasExtras[index].prefabsSpawns)
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
        if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
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
    }
}