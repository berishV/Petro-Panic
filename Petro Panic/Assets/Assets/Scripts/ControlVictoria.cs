using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlVictoria : MonoBehaviour
{
    [Header("═ Configuración de Audio ═")]
    [Tooltip("Arrastra aquí la canción o tema de victoria.")]
    [SerializeField] private AudioClip musicaVictoria;

    [Range(0f, 1f)]
    [SerializeField] private float volumenMusica = 0.5f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (musicaVictoria != null)
        {
            audioSource.clip = musicaVictoria;
            audioSource.volume = volumenMusica;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void VolverAlMenu()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        SceneManager.LoadScene(0);
    }
}