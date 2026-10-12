using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public TextMeshProUGUI textoItems;
    public GameObject panelVictoria;

    private int itemsRecolectados = 0;
    private int itemsTotales;
    private bool gano = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        itemsTotales = GameObject.FindGameObjectsWithTag("Item").Length;

        if (panelVictoria != null)
        {
            panelVictoria.SetActive(false);
        }

        ActualizarUI();
    }

    void Update()
    {
        if (gano && Input.GetKeyDown(KeyCode.R))
        {
            Reiniciar();
        }
    }

    public void SumarItem()
    {
        itemsRecolectados++;
        ActualizarUI();

        if (itemsRecolectados >= itemsTotales)
        {
            Ganar();
        }
    }

    void ActualizarUI()
    {
        if (textoItems != null)
        {
            textoItems.text = "CANGREJOS: " + itemsRecolectados + " / " + itemsTotales;
        }
    }

    void Ganar()
    {
        gano = true;

        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
