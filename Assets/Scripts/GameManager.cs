using TMPro;
using UnityEngine;
public enum GameState
{
    Menu,
    Playing,
    Pause,
    Victory,
    GameOver
}
public class GameManager : MonoBehaviour
{
    [SerializeField] BlockGridSpawner blockSpawner;

    int blocksRemaining;
    int score;

    public GameState state = GameState.Menu;
    public GameState CurrentState => state;

    [SerializeField] int maxLives = 3;
    int currentLives;

    [SerializeField] BallController ball;
    [SerializeField] Transform ballSpawn;
    [SerializeField] TextMeshProUGUI livesText;

    [SerializeField] GameObject panelMenu;
    [SerializeField] GameObject panelHUD;
    [SerializeField] GameObject panelPause;
    [SerializeField] GameObject panelVictory;
    [SerializeField] GameObject panelGameOver;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip victoryClip;
    [SerializeField] AudioClip loseClip;
    [SerializeField] AudioClip menuClip;

    public void StartGame()

    {
        score = 0;
        currentLives = maxLives;
        UpdateScoreUI();
        UpdateLivesUI();
        SetState(GameState.Playing);
        if (blockSpawner != null)
        {
            blockSpawner.Spawn();
            blocksRemaining = blockSpawner.GetTotalBlocks();
        }
        ResetBall();
    }

    void Start()
    {
        SetState(GameState.Menu);
        ResetBall();
    }

    // Update is called once per frame
    void Update()
    {
        //Mejora para la detección de pulsar Esc para pausar el juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == GameState.Playing)
            {
                SetState(GameState.Pause);
            }
            else if (state == GameState.Pause)
            {
                ResetBall();
                SetState(GameState.Playing);
            }
        }
    }
   

    // Es una variable, que combina con void, ya que no devuelve un valor, es void(Vacío).
    public void AddScore(int amount)
    {
        // ( += ) Sumar y guardar el resultado en la misma variable.
        score += amount;

        // ( -- ) Restar 1 a una variable.
        blocksRemaining--;

        CheckVictory();

        // " Debug.Log() " Imprime mensajes en la consola de Unity.
        Debug.Log("Puntuación: " + score);
        Debug.Log("Bloques Restantes: " + blocksRemaining);

        UpdateScoreUI();
        
    }
    void CheckVictory()
    {
        // ( <= ) Menor o igual que. Esta parte cambia de estado.
        if (blocksRemaining <= 0)
        {
            SetState(GameState.Victory);
        }
    }
    void SetState(GameState newState)
    {
        state = newState;
        panelMenu.SetActive(newState == GameState.Menu);
        panelHUD.SetActive(newState == GameState.Playing);
        panelVictory.SetActive(newState == GameState.Victory);
        panelGameOver.SetActive(newState == GameState.GameOver);
        panelPause.SetActive(newState == GameState.Pause);

        if (newState == GameState.Victory)
        {
            audioSource.PlayOneShot(victoryClip);
        }
        if (newState == GameState.GameOver)
        {
            audioSource.PlayOneShot(loseClip);
        }
        if (newState == GameState.Menu)
        {
            audioSource.PlayOneShot(menuClip);
        }
        if (newState != GameState.Menu)
        {
            audioSource.Stop();
        }
      //Mejora de menú Pausa
        if (newState == GameState.Pause)
        {
            Time.timeScale = 0f; // congela el juego
        }
        else
        {
            Time.timeScale = 1f; // lo reanuda
        }
    }

    void UpdateLivesUI()
    {
        livesText.text = "Lives: " + currentLives;
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }
    public void LoseLife()
    {
        if (state != GameState.Playing)
            return;
        currentLives--;
        UpdateLivesUI();
        if (currentLives <= 0)
        {
            SetState(GameState.GameOver);
        }
        else
        {
            ResetBall();
        }
    }
        void ResetBall()
    {
        //ball.ResetBall(ballSpawn.position);
        ball.ResetBall(ballSpawn.position);
    }

    public void ReturnToMenu()
    {
        SetState(GameState.Menu);
        ResetBall();
    }

    public void QuitGame()

    {
        Application.Quit();
    }
    
    // Método del menú Pausa para añadir acción al botón "Resume"
    public void ResumeGame()
    {
        SetState(GameState.Playing);
    }
    public void NextLevel()
    {
        if (blockSpawner != null)
        {
            blockSpawner.NextLevel();
            blocksRemaining = blockSpawner.GetTotalBlocks();
        }

        ResetBall();
        SetState(GameState.Playing);
    }
}