using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Musica")]
    public AudioClip musica;

    [Range(0f, 1f)]
    public float volumen = 0.5f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = musica;
        audioSource.loop = true;
        audioSource.volume = volumen;
        audioSource.playOnAwake = false;

        audioSource.Play();
    }

    public void Jugar()
    {
        audioSource.Stop();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir()
    {
        audioSource.Stop();

        Debug.Log("saliendo del juego...");
        Application.Quit();
    }
}