using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager instance;

    public CameraController mainCamera;

    public PlayerController player;

    public KeyCode pauseKey;

    public bool isPaused;

    public int lives = 3;

    public int enemiesRemaining = 0;

    public int currentWave;

    public AudioMixer audioMixer;

    public AudioSource audioSource;

    public AudioClip gameplayMusic;
    public AudioClip menuMusic;
    public AudioClip winMusic;
    public AudioClip loseMusic;

    //Prefabs
    public GameObject prefabPlayerController;
    public GameObject prefabPlayerPawn;
    public GameObject prefabAIController;
    public GameObject prefabAIPawn;
    public GameObject prefabEnemyUI;
    public Transform playerSpawnTransform;
    public Transform enemyAISpawnTransform;


    //Lists
    public List<PlayerController> players;
    public List<AIController> enemies;
    public List<PlayerSpawnPoint> playerSpawns;
    public List<AISpawnPoint> AISpawns;
    public List<WaveData> waves;

    public SpawnPoint[] spawnPoints;

    #endregion Variables

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to the sceneLoaded event
        }
        else
        {
            Destroy(gameObject);
        }

        players = new List<PlayerController>();
        enemies = new List<AIController>();
        playerSpawns = new List<PlayerSpawnPoint>();
        AISpawns = new List<AISpawnPoint>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused == false)
            {
                Pause();
            }
            else if(isPaused == true)
            {
                UnPause();
            }
        }
    }

    private void Start()
    {
        if (audioSource.clip != menuMusic)
        {
                audioSource.clip = menuMusic;
                audioSource.Play();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameplayScene") // Check if the loaded scene is the Gameplay scene
        {
            StartGame(); // Call the method to initialize gameplay elements
            if (audioSource.clip != gameplayMusic)
            {
                audioSource.clip = gameplayMusic;
                audioSource.Play();
            }

        }

        if (scene.name == "MainMenu")
        {
            if (audioSource.clip != menuMusic)
            {
                audioSource.clip = menuMusic;
                audioSource.Play();
            }       
        }

        if (scene.name == "Failure")
        {
            if (audioSource.clip != loseMusic)
            {
                audioSource.clip = loseMusic;
                audioSource.Play();
            }
        }

        if (scene.name == "Victory")
        {
            if (audioSource.clip != winMusic)
            {
                audioSource.clip = winMusic;
                audioSource.Play();
            }
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

    public void UnPause()
    {
        // Unload the pause screen
        SceneManager.UnloadSceneAsync("PauseMenu");

        // Unpause
        isPaused = false;
        Time.timeScale = 1.0f;
    }

    public void Pause()
    {
        // Load the pause screen
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);

        // Pause the game
        isPaused = true;
        Time.timeScale = 0.0f;
    }

    public void DoGameOver()
    {
        // Load victory scene
        SceneManager.LoadScene("Failure");
        audioSource.Stop();
    }

    public void StartGame()
    {
        // Set our current wave to 0
        currentWave = 0;

        // Connect to our camera
        FindCamera();

        // Load our spawn points
        LoadSpawnPoints();

        // Spawn player
        SpawnPlayer();

        // Spawn our current wave
        SpawnWave(waves[currentWave]);

    }

    private void LoadSpawnPoints()
    {
        spawnPoints = FindObjectsOfType<SpawnPoint>();
    }

    public Transform GetRandomSpawnPoint()
    {
        // if we have spawn points
        if (spawnPoints.Length > 0)
        {
            // return a random player spawnpoint
            return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
        }
        // Otherwise, return null
        return null;
    }

    public void DoVictory()
    {
        // Load victory scene
        SceneManager.LoadScene("Victory");
        audioSource.Stop();
    }

    public void FindCamera()
    {
        if (mainCamera != null)
        {
            // Find and store the camera controller
            mainCamera = FindObjectOfType<CameraController>();
        } else
        {
            // Find the main camera in the scene
            Camera cameraComponent = Camera.main;

            // Check if a main camera exists and it has a CameraController component
            if (cameraComponent != null)
            {
                mainCamera = cameraComponent.GetComponent<CameraController>();
            }
        }
    
        
    }

    public void QuitTheGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void OnPlayerDeath()
    {
        // Respawn the player
        RespawnPlayer();

        // TODO: Add anything else we need to do when the player dies
    }

    public void OnEnemyDeath()
    {
        // Subtract 1 from enemies remaining
        enemiesRemaining--;

        // TODO: Add anything else we need to do when the enemy dies

        // If we are out of enemies, advance to the next wave
        if (enemiesRemaining <= 0)
        {
            // advance to next wave
            currentWave++;

            // If it exists, spawn it.
            if (currentWave < waves.Count)
            {
                SpawnWave(waves[currentWave]);
            }
            // Otherwise, Victory!
            else
            {
                DoVictory();
            }
        }
    }

    public void RespawnPlayer()
    {
        // If we have enough lives
        if (player.lives > 0)
        {
            // Destroy their current pawn
            Destroy(player.pawn.gameObject);

            // Unpossess the current pawn
            player.UnpossessPawn();

            // Spawn a new pawn and possess it instead
            player.PossessPawn(SpawnPawn());

            // Connect the camera controller to the pawn
            mainCamera.target = player.pawn.transform;

            // Subtract one from lives
            player.lives--;

            // Subscribe to the player death event
            Health playerHealth = player.pawn.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.OnDeath.AddListener(OnPlayerDeath);
            }

        }
        // Otherwise, call the game over function
        else
        {
            // Destroy their current pawn
            Destroy(player.pawn.gameObject);
            Debug.Log("Test5");
            // Unpossess the current pawn
            player.UnpossessPawn();

            DoGameOver();
        }
    }

    public void SpawnWave(int waveNumber)
    {
        // Spawn the wave for that wave number using our overloaded function!
        SpawnWave(waves[waveNumber]);
    }

    public void SpawnWave(WaveData wave)
    {
        ClearEnemies();

        // For each enemy in the wave
        foreach (GameObject enemyToSpawn in wave.pawns)
        {
            // Spawn the enemy
            SpawnEnemy(enemyToSpawn);
        }

        // Save the number of enemies
        enemiesRemaining = wave.pawns.Count;
    }

    public void SpawnPlayer()
    {
        // Spawn the Player Controller at 0,0,0, and save it in our player variable
        GameObject newPlayerObj = Instantiate(prefabPlayerController, Vector3.zero, Quaternion.identity);

        player = newPlayerObj.GetComponent<PlayerController>();

        // Connect the controller and pawn!
        player.PossessPawn(SpawnPawn());


        // Connect the camera controller to the pawn
        mainCamera.target = player.pawn.transform;

        // Subscribe to the player death event
        Health playerHealth = player.pawn.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath.AddListener(OnPlayerDeath);
        }
    }

    public Pawn SpawnPawn()
    {
        return SpawnPawn(prefabPlayerPawn);
    }

    public Pawn SpawnPawn(GameObject pawnToSpawn)
    {
        // Spawn the Player at a random spawn point
        Transform randomSpawnPoint = GetRandomSpawnPoint();
        GameObject newPawnObj = Instantiate(pawnToSpawn, randomSpawnPoint.position, randomSpawnPoint.rotation);

        Pawn newPawn = newPawnObj.GetComponent<Pawn>();
        return newPawn;
    }

    public void SpawnAI()
    {
        GameObject newAIObj = Instantiate(prefabAIController, Vector3.zero, Quaternion.identity);
        GameObject newPawnObj = Instantiate(prefabPlayerPawn, enemyAISpawnTransform.position, enemyAISpawnTransform.rotation);

        AIController newController = newAIObj.GetComponent<AIController>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        newController.pawn = newPawn;
        newPawn.controller = newController;
    }
    public void SpawnEnemy()
    {
        SpawnEnemy(prefabAIPawn);
    }

    public void SpawnEnemy(GameObject pawnToSpawn)
    {
        // Spawn the AI Controller at 0,0,0, 
        GameObject newAIObj = Instantiate(prefabAIController, Vector3.zero, Quaternion.identity);
        AIController newAI = newAIObj.GetComponent<AIController>();

        //Save it in our AI list
        enemies.Add(newAI);

        // Connect the controller and pawn!
        newAI.PossessPawn(SpawnPawn(pawnToSpawn));

        // Subscribe to the new enemy's OnDeath event
        Health newAIHealth = newAI.pawn.GetComponent<Health>();
        if (newAIHealth != null)
        {
            newAIHealth.OnDeath.AddListener(OnEnemyDeath);
            // Spawn a UI and attach it to the enemy
            GameObject newEnemyUI = Instantiate(prefabEnemyUI, newAI.pawn.transform) as GameObject;
            // Connect the enemy health
            EnemyHealthDisplay newEnemyUIScript = newEnemyUI.GetComponent<EnemyHealthDisplay>();
            if (newEnemyUIScript != null)
            {
                newEnemyUIScript.enemyHealth = newAIHealth;
            }
        }
    }

    public void ClearEnemies()
    {
        // For every enemy in the enemy list
        foreach (AIController enemy in enemies)
        {
            // if that enemy exists
            if (enemy != null)
            {
                // If it has a pawn
                if (enemy.pawn != null)
                {
                    // destroy the pawn
                    Destroy(enemy.pawn.gameObject);
                }
                // Destroy the Controller
                Destroy(enemy.gameObject);
            }
        }
        // After we have destroyed all the enemies and pawns, clear the list of enemies
        enemies.Clear();
    }
}
