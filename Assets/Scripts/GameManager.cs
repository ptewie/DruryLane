using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public enum GameState { TitleState, CreditsState, GameplayState, GameOverState, VictoryState }; //Game States

public class GameStateChangedEvent : UnityEvent<GameState, GameState> { }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController player;

    [Header("Game State")]
    public GameState currentGameState;
    private GameState previousGameState;
    public GameStateChangedEvent OnGameStateChanged = new GameStateChangedEvent();

    [Header("Hazard Variables")]
    public GameObject spawnObject1;
    public GameObject spawnObject2;
    //public GameObject spawnObject3;
    public GameObject[] spawnPoints;

    public List<Hazard> hazardsPool;
    
    public List<HazardData> hazardDataList;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public GameObject GameOverPanel;

    [Header("Audio")]
    public AudioManager audioManager; // Reference to AudioManager

    [Header("Gameplay Settings")]
    [Tooltip("Time in seconds between each spawn")]
    [CustomSlider(0.1f, 5f)]
    public float timeBetweenSpawns;
    [Tooltip("Multiplier for spawn speed")]
    [CustomSlider(1f, 10f)]
    public float spawnSpeedMultiplier;

    [HideInInspector]
    public bool wasHit = false;
    [HideInInspector]
    public bool isGamePaused = false;

    private float spawnTimer = 0f; // Time in seconds 
    private float scoreTimer = 0f;
    //private float hitTimer = 10f; 

    private int score = 0;
    private int hits = 0;
    private int maxHits = 3;

    private Coroutine spawnHazardsCoroutine;
    private Coroutine hitCheckCoroutine;
    private Coroutine hitTimerCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // On load, the game state is the title state
        // game state is set to gameplay state for debugging resons rn
        ChangeGameState(GameState.TitleState);
        GameOverPanel.gameObject.SetActive(false);
        //player.GetComponent<Health>().
        audioManager.PlayBGM();
    }

    public void StartGame()
    {
        ChangeGameState(GameState.GameplayState);
        GameOverPanel.gameObject.SetActive(false);
        audioManager.IncreasePitch(0.1f);
    }

    void Update()
    {
        if (currentGameState == GameState.GameplayState && !wasHit)
        {
            // Increase hazard speed by 0.1 every frame draw. 
            // Yes, this will eventually make them so fast it's impossible to dodge. I'll deal with that later.
            spawnSpeedMultiplier += Time.deltaTime * 0.1f;

            // update the score timer
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        // Increase the score timer by 1 every second
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= 1f)
        {
            // Increase score
            score += 1;
            // Reset the timer
            scoreTimer = 0f;

            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void ChangeGameState(GameState state)
    {
        SetCurrentGameState(state);
    }

    // Getter for current game state
    public GameState CurrentGameState
    {
        get { return currentGameState; }
    }

    // Setter for current game state
    public void SetCurrentGameState(GameState state)
    {
        previousGameState = currentGameState;
        currentGameState = state;
        OnGameStateChanged.Invoke(previousGameState, currentGameState);

        // Start or stop coroutines based on the current game state
        if (currentGameState == GameState.GameplayState)
        {
            // Only hitCheck and spawnhazards if we're in GameplayState
            spawnHazardsCoroutine = StartCoroutine(SpawnHazards());
            hitCheckCoroutine = StartCoroutine(HitCheck());
        }
        else
        {
            // Stop coroutines if we're not in GameplayState
            if (spawnHazardsCoroutine != null)
                StopCoroutine(spawnHazardsCoroutine);
            if (hitCheckCoroutine != null)
                StopCoroutine(hitCheckCoroutine);
        }
    }

    private HazardData GetRandomHazardData()
    {
        // Check if there are any hazard data available
        if (hazardDataList.Count == 0)
        {
            Debug.LogError("No hazard data available.");
            return null;
        }

        // Get a random index within the range of hazard data list
        int randomIndex = Random.Range(0, hazardDataList.Count);

        // Return the hazard data at the random index
        return hazardDataList[randomIndex];
    }

    IEnumerator SpawnHazards()
    {
        // apply hazards to each spawnpoint
        GameObject[] hazardArray = new GameObject[] { spawnObject1, spawnObject2}; 

        // LAMBDA expression to select a specific hazard for each spawn point 
        System.Func<int, GameObject> selectHazard = (random) =>
        {
            // clamp so value stays within array
           int clampedIndex = Mathf.Clamp(random, 0, hazardArray.Length - 1);

            // Return the hazard!
            return hazardArray[clampedIndex];
        };

        while (true)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > timeBetweenSpawns)
            {
                // reset spawn timer
                spawnTimer = 0f;

                // set random and then choose a spawnPoint to spawn the hazard from
                int random = Random.Range(0, 3);

                // select the hazard
                HazardData hazardDataToSpawn = GetRandomHazardData();

                // Spawn em'
                SpawnHazard(hazardDataToSpawn, random);
            }

            yield return null; // Give control to the unity engine
        }
    }

    public void SpawnHazard(HazardData hazardDataToSpawn, int spawnIndex)
    {
        // going thru whole hazards pool to find first that's not active.
        // When we find that onject, line 190 is true so we use that object 
        // But, if we run thru the loop and don't find anything. we need to spawn more hazards. so we instaitate.

        //Hazard hazardToSpawn = hazardObjectToSpawn.GetComponent<Hazard>();

        foreach (Hazard hazard in hazardsPool)
        {
            //if hazard is not active in heirarchy 
            //if (!hazard.gameObject.activeInHierarchy && hazardToSpawn.hazardType == hazard.hazardType)
            if (!hazard.gameObject.activeInHierarchy)
            {
                //move it to the location
                hazard.gameObject.transform.position = spawnPoints[spawnIndex].transform.position;

                //set it's rotation!
                hazard.gameObject.transform.rotation = spawnPoints[spawnIndex].transform.rotation;
                
                HazardProperties hazardProperties = hazard.gameObject.GetComponent<HazardProperties>();
                
                if (hazardProperties != null)
                {
                    hazardProperties.hazardData = hazardDataToSpawn;
                }

                //set it to active
                hazard.gameObject.SetActive(true);
                return;
            }
        }
        // If there are no inactive hazards in the pool, instantiate a new hazard object
        GameObject newHazard = Instantiate(spawnObject1, spawnPoints[spawnIndex].transform.position, Quaternion.identity);
        hazardsPool.Add(newHazard.GetComponent<Hazard>());

        // Set properties of the newly instantiated hazard object
        newHazard.transform.position = spawnPoints[spawnIndex].transform.position;
        newHazard.transform.rotation = spawnPoints[spawnIndex].transform.rotation;

        HazardProperties newHazardProperties = newHazard.GetComponent<HazardProperties>();
        if (newHazardProperties != null)
        {   
            newHazardProperties.hazardData = hazardDataToSpawn;
        }

    newHazard.SetActive(true);
    }

    IEnumerator HitCheck()
    {
        while (true)
        {
            if (wasHit)
            {
                Debug.Log("Hit check true!");

                // Increase hit count
                hits++;

                // When the player is hit three times, trigger game over
                if (hits >= maxHits)
                {
                    GameOver();
                }
                else
                {
                    // If the player is not yet hit three times, reset speed variable and score
                    spawnSpeedMultiplier = 0f;
                    DecreaseScore(5);
                }

                // Reset the hit state after processing
                wasHit = false;
            }

            yield return null;
        }
    }

    public void DecreaseScore(int amount)
    {
        // decrease score 
        // Make sure it cannot go below zero 
        score = Mathf.Max(0, score - amount);
        // Update GUI
        UpdateScoreText();
    }

    private void OnEnable()
    {
        // Subscribe to universal controller events
        UniversalController.OnQuitGame += QuitGame;
        UniversalController.OnTogglePause += TogglePause;
        //UniversalController.DebugHazardCheck += DebugHazardsStatus;
        //UniversalController.DebugHazardSort += DebugSortHazardsByType;
        //UniversalController.DebugHazardTypeList += DebugListHazardTypes;
    }

    private void OnDisable()
    {
        // Unsubscribe from universal controller events
        UniversalController.OnQuitGame -= QuitGame;
        UniversalController.OnTogglePause -= TogglePause;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game");
        Application.Quit();
    }

    public void GameOver()
    {
        TogglePause();
        ChangeGameState(GameState.GameOverState);
        Debug.Log("game over!!");
        GameOverPanel.gameObject.SetActive(true);
    }

    private void TogglePause()
    {
        //toggle the game pause bool
        isGamePaused = !isGamePaused;
        // TO DO: add UI to show pause
        if (isGamePaused)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }

        Debug.Log("Game " + (isGamePaused ? "Paused" : "Resumed"));
    }
} 
namespace DebugTools
{
    public static class DebugManager
    {
        
        public static void DebugHazardsStatus(List<Hazard> hazardsPool)
        {
            var activeHazards = hazardsPool.Where(hazard => hazard.gameObject.activeInHierarchy).ToList();
            var inactiveHazards = hazardsPool.Where(hazard => !hazard.gameObject.activeInHierarchy).ToList();

            foreach (var hazard in activeHazards)
            {
                Debug.Log("Active:" + hazard.gameObject.name);
            }

            foreach (var hazard in inactiveHazards)
            {
                Debug.Log("Inactive:" + hazard.gameObject.name);
            }
        }

        public static void DebugSortHazardsByType(List<Hazard> hazardsPool)
        {
            // Sort hazards by hazard type
            var sortedHazards = hazardsPool.OrderBy(hazard => hazard.hazardType).ToList();

            // Print sorted hazards in debug log
            Debug.Log("Sorted Hazards by Type.");
            foreach (var hazard in sortedHazards)
            {
                Debug.Log("Hazard Name: " + hazard.gameObject.name + ", Hazard Type: " + hazard.hazardType);
            }
        }

        public static void DebugListHazardTypes(List<Hazard> hazardsPool)
        {
            // Find distinct hazard types
            var distinctHazardTypes = hazardsPool.Select(hazard => hazard.hazardType).Distinct().ToList();

            // Print distinct hazard types in debug log
            Debug.Log("Hazard Types:");
            foreach (var hazardType in distinctHazardTypes)
            {
                Debug.Log(hazardType);
            }
        }
    }
}