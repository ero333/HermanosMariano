using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using Cinemachine;
using UnityEngine.Analytics;

//【◉ᴥ◉】
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int levelIndex;
    public string levelName;

    [Header("Datos que se mantienen")]
    public static int maxEnergy = 6;
    public int maxLives = 4;
    public static int ahorros;

    public static int zoneProgress = 1;
    public static int paidDeudas = 0;
    public static int seenDialogues = 0;

    public static bool sound = true;

    [Header("Datos del nivel")]
    public int resetCount = 0;
    public bool victory = false;
    public int energy = 10;
    public int lives = 3;
    public Vector2 lastCheckpos;
    public int money;

    //Analitics
    bool CountGameTime = true;
    float GameTime = 0;
    public string CondicionDeVictoria;
    bool isTimer;
    public string ultimoCulpable;

    private void Awake()
    {
        //No tocar, permite mantener al gamemanager en todas las escenas y poder probar todos los niveles sin tener que empezar desde el menu
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;

            //gameObject.SendMessage("OnLevelWasLoaded", Application.loadedLevel);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this.gameObject);
        }       
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        levelIndex = SceneManager.GetActiveScene().buildIndex;
        levelName = SceneManager.GetActiveScene().name;
        levelName = AddSpacesToSentence(levelName);

        if (resetCount == 0)
        {
            Debug.Log(levelName + " loaded for the first time");
            
            victory = false;
            lives = maxLives;
            energy = maxEnergy;
            money = 0;

            CountGameTime = true;
            GameTime = 0;
            if(FindObjectOfType<CountDown>())
            {
                isTimer = FindObjectOfType<CountDown>().isActiveAndEnabled;
            }
            
            if (levelIndex != 0 && levelIndex != 2)
            {
                Analytics.CustomEvent("IniciarNivel", new Dictionary<string, object>
                {
                    {"Zona", levelIndex == 1 ? 0 : zoneProgress },
                    {"Nivel", levelName },
                    {"Ahorros", ahorros }//,
                    //{"CuantasVeces", 1 }
                });
            }
            
        }
        else
        {
            Debug.Log("Level loaded again");
        }
    }

    //void Start()
    //{
    //    //levelIndex = SceneManager.GetActiveScene().buildIndex;
    //}

    void Update()
    {
        if(energy > maxEnergy)
        {
            energy = maxEnergy;
        }

        if (energy <= 0)
        {
            lives -= 1;
            Reset();
        }

        if(CountGameTime) GameTime += 1 * Time.deltaTime;
    }    

    public void Reset()
    {
        if (lives <= 0)
        {
            resetCount = 0;
            //Game over

            CountGameTime = false;
            if (levelIndex != 0 && levelIndex != 2)
            {
                Analytics.CustomEvent("PerderNivel", new Dictionary<string, object>
                {
                    {"Zona", levelIndex == 1 ? 0 : zoneProgress },
                    {"Nivel", levelName },
                    {"Ahorros", ahorros },
                    {"Recolectado", instance.money },
                    {"VidasRestantes", instance.lives },
                    {"CondicionVictoria", instance.CondicionDeVictoria },
                    {"Timer", instance.isTimer },
                    {"TiempoDeJuego", instance.GameTime },
                    {"ObjetoQueLoMato", instance.ultimoCulpable }

                });
            }
        }
        else
        {   //muerte
            resetCount += 1;
            //SceneManager.LoadScene(levelIndex); <- para recargar la escena
            if (!FindObjectOfType<EnemySpawner>())
            {
                Enemy[] enemies = FindObjectsOfType<Enemy>();
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].trigger = false;
                }
            }           

            Player player = GameObject.FindObjectOfType<Player>();
            player.transform.position = instance.lastCheckpos;
            CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
            //cam.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y);
            cam.Follow = player.transform;

            if (levelIndex != 0 && levelIndex != 2)
            {
                Analytics.CustomEvent("Morir", new Dictionary<string, object>
                {
                    {"Zona", levelIndex == 1 ? 0 : zoneProgress },
                    {"Nivel", instance.levelName },
                    {"Recolectado", instance.money },
                    {"VidasRestantes", instance.lives },
                    {"ObjetoQueLoMato", instance.ultimoCulpable },
                    {"PosicionDeMuerte", player.transform.position }
                });
            }
        }
        energy = maxEnergy;
    }

    public void LevelReset()
    {
        resetCount = 0;
        SceneManager.LoadScene(levelIndex);
    }
    
    public void GainMoney(int gain)
    {
        instance.money += gain;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("CurrentZone", zoneProgress);
        PlayerPrefs.SetInt("DeudasPagadas", paidDeudas);
        PlayerPrefs.SetInt("Ahorros", ahorros);
        PlayerPrefs.SetInt("MaxLives", maxLives);
        PlayerPrefs.SetInt("MaxEnergy", maxEnergy);
    }

    public void BackToMap()
    {
        energy = maxEnergy;
        lives = maxLives;
        resetCount = 0;
        Time.timeScale = 1f;
        victory = false;
        SaveData();
        SceneManager.LoadScene("MapaZonas");        
    }

    public void VictoryCondition()
    {
        Debug.Log("Gano");
        victory = true;
        resetCount = 0;
        FindObjectOfType<CutsceneManager>().onVictory();

        CountGameTime = false;
    }

    public void WinAnaliticsEvent(int ganancia)
    {
        Analytics.CustomEvent("GanarNivel", new Dictionary<string, object>
        {
            {"Zona", instance.levelIndex == 1 ? 0 : zoneProgress },
            {"Nivel", instance.levelName },
            {"Ahorros", ahorros },
            {"Recolectado", instance.money },
            {"Ganancias", ganancia },
            {"Vidas", instance.lives },
            {"Energia", instance.energy },
            {"CondicionVictoria", instance.CondicionDeVictoria },
            {"Timer", instance.isTimer },
            {"TiempoDeJuego", instance.GameTime }
        });
    }

    public void UnlockZone (int currentZone)
    {
        if(currentZone == zoneProgress)
        {
            zoneProgress++;
            maxEnergy++;
        }
    }

    string AddSpacesToSentence(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
    }

    public void Cheat()
    {
        zoneProgress = 4;
    }

    public void ToggleMute()
    {
        if (sound)
        {
            sound = false;
            Debug.Log("The game is now muted");
        }
        else
        {
            sound = true;
            Debug.Log("The game now has sound");
        }        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
