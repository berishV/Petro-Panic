using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instancia;

    [Header("Puntaje")]
    public TextMeshProUGUI textoPuntaje;
    private int puntajeTotal = 0;

    [Header("Barra de Vidas")]
    public Image imagenBarraVidas;
    public Sprite barra3Vidas;
    public Sprite barra2Vidas;
    public Sprite barra1Vida;

    [Header("Game Over")]
    public GameObject panelGameOver;
    public AudioClip sonidoGameOver;
    [Range(0f, 1f)] public float volumenGameOver = 0.8f;

    private AudioSource audioSource;
    private bool gameOverActivado = false;

    void Awake()
    {
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (panelGameOver != null) panelGameOver.SetActive(false);
        ActualizarPuntajeVisual();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void SumarPuntos(int puntos)
    {
        if (gameOverActivado) return;
        puntajeTotal += puntos;
        ActualizarPuntajeVisual();
    }

    private void ActualizarPuntajeVisual()
    {
        if (textoPuntaje != null)
        {
            textoPuntaje.text = "SCORE: " + puntajeTotal.ToString("000000");
        }
    }

    public void ActualizarVidas(int vidasRestantes)
    {
        if (imagenBarraVidas == null) return;

        if (vidasRestantes == 3) imagenBarraVidas.sprite = barra3Vidas;
        else if (vidasRestantes == 2) imagenBarraVidas.sprite = barra2Vidas;
        else if (vidasRestantes == 1) imagenBarraVidas.sprite = barra1Vida;
        else if (vidasRestantes <= 0) imagenBarraVidas.enabled = false;
    }

    public void MostrarGameOver()
    {
        {
            if (gameOverActivado) return;
            gameOverActivado = true;

            if (panelGameOver != null) panelGameOver.SetActive(true);

            if (sonidoGameOver != null)
            {
                audioSource.PlayOneShot(sonidoGameOver, volumenGameOver);
            }

            StageManager stageManager = FindFirstObjectByType<StageManager>();
            if (stageManager != null)
            {
                stageManager.DetenerMusicaNivel();
            }
        }
    }


    public void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}