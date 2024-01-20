using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public enum GameState {TitleState, CreditsState, GameplayState, GameOverState, VictoryState, }; //Game States

public class GameStateChangedEvent : UnityEvent<GameState, GameState>
{

}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject spawnObject;
    public GameObject[] spawnPoints;
    public TextMeshProUGUI scoreText;

    public float spawnTimer = 0f; // Time in seconds 
    public float timeBetweenSpawns; 
    public float spawnSpeedMultiplier;
    private float scoreTimer = 0f;

    private int score = 0;
    //This bool needs to stay public because this is what all scripts in the game use to check if the player was hit.
    //It's awful I know but I don't have time to rewrite that logic rn
    public bool wasHit = false; 

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
        StartCoroutine(SpawnHazards());
        StartCoroutine(HitCheck());
    }

    void Update()
    {
        if (!wasHit)
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

    IEnumerator SpawnHazards()
    {
        while (true) //Run indefinitely with a while (true) loop
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > timeBetweenSpawns)
            {
                // reset spawn timer
                spawnTimer = 0f;

                // set random and then choose a spawnPoint to spawn the hazard from
                int random = Random.Range(0, 3);

                // Spawn em'
                Instantiate(spawnObject, spawnPoints[random].transform.position, Quaternion.identity);
            }

            yield return null; //Give control to the unity engine
        }
    }

    IEnumerator HitCheck()
    {
        while (true)
        {
            if (wasHit)
            {
                Debug.Log("Hit check true!");
                // When the player is hit, decrement the speed variable. 
                spawnSpeedMultiplier = 0f;

                // Remove 5 score from player 
                DecreaseScore(5);
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
}