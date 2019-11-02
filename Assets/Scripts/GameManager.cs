using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using System.Text;
using Cinemachine;
using UnityEngine.Analytics;

//【◉ᴥ◉】
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    Player player;

    int levelIndex;
    public string level;

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
    int levelNumber;
    float GameTime = 0;
    [HideInInspector]
    public string CondicionDeVictoria;
    [HideInInspector]
    public string ultimoCulpable;
    [HideInInspector]
    public int bulletCounter = 0;

    private void Awake()
    {
        //No tocar, permite mantener al gamemanager en todas las escenas y poder probar todos los niveles sin tener que empezar desde el menu
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;

            //gameObject.SendMessage("OnLevelWasLoaded", Application.loadedLevel); <- manera desactualizada de hacerlo, tira errores
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
        level = SceneManager.GetActiveScene().name;
        //levelName = AddSpacesToSentence(levelName);
        player = FindObjectOfType<Player>();


        //level indexer
        if (levelIndex != 0 && levelIndex != 2)
        {
            player = FindObjectOfType<Player>();

            levelNumber = -1;
            for (int i = 0; i < level.Length; i++)
            {
                if (char.IsNumber(level[i]))
                {
                    levelNumber = (int)char.GetNumericValue(level[i]);
                    break;
                }
            }

            if (level == "NivelIntroduccion")
            {
                levelNumber = 1;
            }
            else if (levelNumber == -1)
            {
                levelNumber = 4;
            }
            //Debug.Log(levelNumber);
        }

        if (resetCount == 0)
        {
            //Debug.Log(level + " loaded for the first time");            

            victory = false;
            lives = maxLives;
            energy = maxEnergy;
            money = 0;

            //Analytics            
            GameTime = Time.time;            
            bulletCounter = 0;
            //if (FindObjectOfType<CountDown>())
            //{
            //    isTimer = FindObjectOfType<CountDown>().isActiveAndEnabled;
            //}

            IniciarNivelAnalyticsEvent();
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
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }

        if (energy <= 0)
        {
            lives -= 1;
            Reset();
        }
    }

    public void Reset()
    {
        if (lives <= 0)
        {
            resetCount = 0;
            //Game over
            
            PerderAnalyticsEvent();
        }
        else
        {   //muerte
            resetCount += 1;
            //SceneManager.LoadScene(levelIndex); <- para recargar la escena

            if (!FindObjectOfType<EnemySpawner>()) //que los enemigos pierdan de vista al jugador
            {
                Enemy[] enemies = FindObjectsOfType<Enemy>();
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].trigger = false;
                }
            }

            //mandar jugador al checkpoint
            player = FindObjectOfType<Player>();

            MorirAnalyticsEvent();

            player.transform.position = instance.lastCheckpos;

            //reiniciar camara
            CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
            //cam.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y);
            cam.Follow = player.transform;
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
        //Debug.Log("Gano");
        victory = true;
        resetCount = 0;
        FindObjectOfType<CutsceneManager>().onVictory();

        //CountGameTime = false;
    }

    public void UnlockZone(int currentZone)
    {
        if (currentZone == zoneProgress)
        {
            zoneProgress++;
            maxEnergy++;
        }
    }

    //string AddSpacesToSentence(string text)
    //{
    //    if (string.IsNullOrWhiteSpace(text))
    //        return "";
    //    StringBuilder newText = new StringBuilder(text.Length * 2);
    //    newText.Append(text[0]);
    //    for (int i = 1; i < text.Length; i++)
    //    {
    //        if (char.IsUpper(text[i]) && text[i - 1] != ' ')
    //            newText.Append(' ');
    //        newText.Append(text[i]);
    //    }
    //    return newText.ToString();
    //}

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

    public int LevelIndexer(string level)
    {
        if (levelIndex != 0)
        {
            levelNumber = -1;
            for (int i = 0; i < level.Length; i++)
            {
                if (char.IsNumber(level[i]))
                {
                    levelNumber = (int)char.GetNumericValue(level[i]);
                    break;
                }
            }
            if (level == "NivelIntroduccion")
            {
                levelNumber = 1;
            }
            else if (levelNumber == -1)
            {
                levelNumber = 4;
            }
            
            //Debug.Log(levelNumber);
        }

        return levelNumber;
    }

    public int GetZone(int levelIndex)
    {
        if (levelIndex == 1) return 0;

        if (levelIndex >= 3 && levelIndex <= 6) return 1;

        if (levelIndex >= 7 && levelIndex <= 10) return 2;

        if (levelIndex >= 11 && levelIndex <= 14) return 3;

        if (levelIndex >= 15 && levelIndex <= 18) return 4;

        return -1;
    }

    void analyticsTrace(Dictionary<string, object> dictionary, string name)
    {
        string traceHolder = name + "    haz clic para ver más detalles" + "\n";
        foreach (var key in dictionary)
        {
            traceHolder += key.Key + " > " + key.Value + "\n";
        }

        Debug.Log(traceHolder);
    }

    //EVENTOS PARA EL UNITY ANALYTICS
    public void ContinuarAnalyticsEvent()
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>
        {
            {"UltimaZona", levelIndex == 1 ? 0 : zoneProgress },
            //{"Nivel", instance.levelNumber },
            {"Ahorros", ahorros }
        };

        analyticsTrace(dictionary, "ContinuarPartida");
        Analytics.CustomEvent("ContinuarPartida", dictionary);
    }

    void IniciarNivelAnalyticsEvent()
    {
        if (levelIndex != 0 && levelIndex != 2)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                {"Zona", GetZone(levelIndex) },
                {"Nivel", instance.levelNumber },
                {"Ahorros", ahorros }//,
                //{"CuantasVeces", 1 }
            };

            analyticsTrace(dictionary, "IniciarNivel");
            Analytics.CustomEvent("IniciarNivel", dictionary);
        }
    }

    public void GanarAnalyticsEvent(int ganancia)
    {
        float endTime = Time.time - instance.GameTime;

        Dictionary<string, object> dictionary = new Dictionary<string, object>
        {
            {"Zona", GetZone(levelIndex) },
            {"Nivel", instance.levelNumber },
            {"EjeX", Mathf.FloorToInt( player.transform.position.x ) },
            {"EjeY", Mathf.FloorToInt( player.transform.position.y ) },
            //{"Ahorros", ahorros },
            {"Recolectado", instance.money },
            //{"Ganancias", ganancia },
            {"Vidas", instance.lives },
            {"Energia", instance.energy },
            //{"CondicionVictoria", instance.CondicionDeVictoria },
            //{"Timer", instance.isTimer },
            {"TiempoDeJuego", endTime },
            {"BalasGastadas", instance.bulletCounter },
            {"EnemigosVivos", FindObjectsOfType<Enemy>().Length }
        };

        analyticsTrace(dictionary, "GanarNivel");

        Analytics.CustomEvent("GanarNivel", dictionary);
    }

    void MorirAnalyticsEvent()
    {
        if (levelIndex != 0 && levelIndex != 2)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                {"Zona", GetZone(levelIndex) },
                {"Nivel", instance.levelNumber },
                {"EjeX", Mathf.FloorToInt( player.transform.position.x ) },
                {"EjeY", Mathf.FloorToInt( player.transform.position.y ) },
                {"Recolectado", instance.money },
                {"VidasRestantes", instance.lives },
                {"ObjetoQueLoMato", instance.ultimoCulpable }
            };

            analyticsTrace(dictionary, "Morir");

            Analytics.CustomEvent("Morir", dictionary);
        }
    }

    void PerderAnalyticsEvent()
    {
        float endTime = Time.time - instance.GameTime;

        if (levelIndex != 0 && levelIndex != 2)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                {"Zona", GetZone(levelIndex) },
                {"Nivel", instance.levelNumber },
                {"EjeX", Mathf.FloorToInt( player.transform.position.x ) },
                {"EjeY", Mathf.FloorToInt( player.transform.position.y ) },
                //{"Ahorros", ahorros },
                {"Recolectado", instance.money },
                {"VidasRestantes", instance.lives },
                //{"CondicionVictoria", instance.CondicionDeVictoria },
                //{"Timer", instance.isTimer },
                {"TiempoDeJuego", endTime },
                {"ObjetoQueLoMato", instance.ultimoCulpable },
                {"BalasGastadas", instance.bulletCounter },
                {"EnemigosVivos", FindObjectsOfType<Enemy>().Length }
            };

            analyticsTrace(dictionary, "PerderNivel");

            Analytics.CustomEvent("PerderNivel", dictionary);
        }
    }

    public void ExitLevelAnalyticsEvent()
    {
        float endTime = Time.time - instance.GameTime;

        if (levelIndex != 0 && levelIndex != 2)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                {"Zona", GetZone(levelIndex) },
                {"Nivel", instance.levelNumber },
                {"EjeX", Mathf.FloorToInt( player.transform.position.x ) },
                {"EjeY", Mathf.FloorToInt( player.transform.position.y ) },
                {"Recolectado", instance.money },
                {"VidasRestantes", instance.lives },
                {"EnergiaRestante", instance.energy },
                {"TiempoDeJuego", endTime },
                {"EnemigosVivos", FindObjectsOfType<Enemy>().Length }
                //{"CondicionVictoria", instance.CondicionDeVictoria }
            };

            analyticsTrace(dictionary, "SalirNivelPausa");
            Analytics.CustomEvent("SalirNivelPausa", dictionary);
        }
    }

    public void ResetLevelPauseMenu()
    {
        float endTime = Time.time - instance.GameTime;

        Dictionary<string, object> dictionary = new Dictionary<string, object>
        {
            {"Zona", GetZone(levelIndex) },
            {"Nivel", instance.levelNumber },
            {"EjeX", Mathf.FloorToInt( player.transform.position.x ) },
            {"EjeY", Mathf.FloorToInt( player.transform.position.y ) },
            {"Recolectado", instance.money },
            {"VidasRestantes", instance.lives },
            {"EnergiaRestante", instance.energy },
            {"TiempoDeJuego", endTime },
            {"EnemigosVivos", FindObjectsOfType<Enemy>().Length }
            //{"CondicionVictoria", instance.CondicionDeVictoria }
        };

        analyticsTrace(dictionary, "ReiniciarNivelPausa");
        Analytics.CustomEvent("ReiniciarNivelPausa", dictionary);
    }

    public void CutsceneAnalyticsEvent(string tipo, int total, int ultimo)
    {
        if(ultimo == total-1)
        {        
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                {"Zona", GetZone(levelIndex) },
                {"Nivel", instance.levelNumber },
                {"Tipo", tipo },
                {"DialogosTotales", total - 1},
                //{"UltimoDialogo", ultimo }
            };
        
            analyticsTrace(dictionary, "CompletarCutscene");
            Analytics.CustomEvent("CompletarCutscene", dictionary);
        }
    }

    public void ReiniciarNivelGameOver()
    {        
        Dictionary<string, object> dictionary = new Dictionary<string, object>
        {
            {"Zona", GetZone(levelIndex) },
            {"Nivel", instance.levelNumber },
            {"EjeX", Mathf.FloorToInt( player.transform.position.x ) },
            {"EjeY", Mathf.FloorToInt( player.transform.position.y ) }
        };

        analyticsTrace(dictionary, "ReiniciarNivelGameOver");
        Analytics.CustomEvent("ReiniciarNivelGameOver", dictionary);
    }

    public void VolverAlMapaGameOver()
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>
        {
            {"Zona", GetZone(levelIndex) },
            {"Nivel", instance.levelNumber },
            {"EjeX", Mathf.FloorToInt( player.transform.position.x ) },
            {"EjeY", Mathf.FloorToInt( player.transform.position.y ) }
        };

        analyticsTrace(dictionary, "VolverAlMapaGameOver");
        Analytics.CustomEvent("VolverAlMapaGameOver", dictionary);
    }

    public void RecolectarObjeto(string name, float x, float y)
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>
        {
            {"Zona", GetZone(levelIndex) },
            {"Nivel", instance.levelNumber },
            {"EjeX", Mathf.FloorToInt( x ) },
            {"EjeY", Mathf.FloorToInt( y ) },
            {"Objeto", name }
        };

        analyticsTrace(dictionary, "RecolectarObjeto");
        Analytics.CustomEvent("RecolectarObjeto", dictionary);
    }    

    public void MatarEnemigo(float EjeX, float EjeY, string culpable)
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>
        {
            {"Zona", GetZone(levelIndex) },
            {"Nivel", instance.levelNumber },
            {"EjeX", Mathf.FloorToInt( EjeX ) },
            {"EjeY", Mathf.FloorToInt( EjeY ) },
            {"ObjetoQueLoMato", culpable },
        };

        analyticsTrace(dictionary, "MatarEnemigo");
        Analytics.CustomEvent("MatarEnemigo", dictionary);
    }

    //Inputs del Mapa
    public void IniciarMapa()
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>
        {
            {"ZonasDesbloqueadas", zoneProgress },
            {"Ahorros", ahorros }
        };

        analyticsTrace(dictionary, "IniciarMapa");
        Analytics.CustomEvent("IniciarMapa", dictionary);
    }

    public void MapaClicNivel(int level)
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>
        {
            {"Zona", GetZone(level) },
            {"Nivel", level }            
        };

        analyticsTrace(dictionary, "MapaClicNivel");
        //Analytics.CustomEvent("MapaClicNivel");
    }

    public void MapaClicErroneo(int zone)
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>
        {
            {"Zona", GetZone(zone) }        
        };

        analyticsTrace(dictionary, "MapaClicErroneo");
        Analytics.CustomEvent("MapaClicErroneo", dictionary);
    }

}