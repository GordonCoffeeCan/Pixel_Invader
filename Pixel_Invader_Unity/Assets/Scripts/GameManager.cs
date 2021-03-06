﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int wave = 0;
    public float enemySpeed = 0.85f;

    public int player1Count = 3;
    public int player1ShieldCount = 3;
    public int player1BombCount = 0;
    public int player1LaserCount = 0;

    public int player2Count = 3;
    public int player2ShieldCount = 3;
    public int player2BombCount = 0;
    public int player2LaserCount = 0;

    public bool player1IsDead = false;
    public bool player2IsDead = false;

    public enum GameMode {
        SinglePlayerMode,
        CoopMode
    }

    public GameMode gameMode;

    [HideInInspector] public float player1Score = 0;
    [HideInInspector] public float player2Score = 0;
    [HideInInspector] public float currentWaveEnemySpeed;
    [HideInInspector] public float cameraShakeAmount = 0;
    [HideInInspector] public int moveDirection = 1;
    [HideInInspector] public int enemiesCount = 0;
    [HideInInspector] public int currentLineEnemyDirection = 1;
    [HideInInspector] public int waveIndex = 1;
    [HideInInspector] public int playerID = 0;
    [HideInInspector] public float countDownTime = 10;
    [HideInInspector] public bool gameIsOver = false;
    [HideInInspector] public bool gameIsPause = false;
    [HideInInspector] public bool levelBuilt = false;
    [HideInInspector] public bool recreateLevel = false;
    [HideInInspector] public List<Enemy> enemyList = new List<Enemy>();
    [HideInInspector] public PlayerControl player1Clone;
    [HideInInspector] public PlayerControl player2Clone;

    [HideInInspector] public string currentGameMode = "";

    [SerializeField] private PlayerControl player1;
    [SerializeField] private PlayerControl player2;
    [SerializeField] private Text player1ScoreText;
    [SerializeField] private Text player2ScoreText;
    [SerializeField] private Text waveText;
    [SerializeField] private Button resumeButton;

    private bool hDirChanged = false;
    private bool vDirChanged = false;

    private float currentPlayer1Score = 0;
    private float currentPlayer2Score = 0;
    private float enemyPositionLimit = 0;

    private float deltaPosY;

    private EventSystem eventSystem;

    private void Awake() {
        instance = this;
        currentGameMode = SceneManager.GetActiveScene().name;
    }

    // Use this for initialization
    void Start () {
        currentPlayer1Score = player1Score;
        currentPlayer2Score = player2Score;
        eventSystem = EventSystem.current;
        SetEnemySpeed();
        wave = ProgressManager.currentWave;
        waveIndex = ProgressManager.currentWaveIndex;
        LevelBuilder.instance.BuildLevel(wave);
        levelBuilt = true;
        if (gameMode == GameMode.SinglePlayerMode) {
            StartCoroutine(ResetPlayer1());
        } else {
            //Add instantiate players for Coop Mode script here------------------------------------------*********---------------------------------------------------------------/
            StartCoroutine(ResetPlayer1(0, -1f));
            StartCoroutine(ResetPlayer2(0, 1f));
        }

        enemyPositionLimit = WindowSizeUtil.instance.halfWindowSize.x - 4.15f;
    }
	
	// Update is called once per frame
	void Update () {
        currentPlayer1Score = Mathf.Lerp(currentPlayer1Score, player1Score, 0.15f);
        currentPlayer2Score = Mathf.Lerp(currentPlayer2Score, player2Score, 0.15f);

        if (gameMode == GameMode.SinglePlayerMode) {
            player1ScoreText.text = Mathf.Round(currentPlayer1Score).ToString();
            waveText.text = waveIndex.ToString();
        }else if (gameMode == GameMode.CoopMode) {
            player1ScoreText.text = Mathf.Round(currentPlayer1Score).ToString();
            player2ScoreText.text = Mathf.Round(currentPlayer2Score).ToString();
        }
        

        if (enemiesCount <= 0 && levelBuilt == true) {
            levelBuilt = false;
            recreateLevel = true;
        }

        //Recreate Level after some second
        if (recreateLevel) {
            StartCoroutine(RecreateLevel(3.5f));
            recreateLevel = false;
        }
        //Recreate Level after some second

        for (int i = 0; i < enemyList.Count; i++) {
            if (enemyList[i].movementStyle == Enemy.MovementStyle.LeftAndRight) {
                enemyList[i].transform.position = new Vector2(enemyList[i].transform.position.x, enemyList[i].transform.position.y - deltaPosY);
            }

            //Enemy Breakthrough the final line, Game Over
            if (enemyList[i].transform.position.y <= -(WindowSizeUtil.instance.halfWindowSize.y - 1.5f) && enemyList[i].movementStyle != Enemy.MovementStyle.Towards) {
                player1IsDead = true;
                player2IsDead = true;
                player1Count = 0;
                player2Count = 0;
                gameIsOver = true;
            }
            //Enemy Breakthrough the final line, Game Over
        }

        //Check number to change speed
        if (enemyList.Count < 5 && enemyList.Count >= 2) {
            for (int i = 0; i < enemyList.Count; i++) {
                enemyList[i].speedMultiplier = 2;
            }
        } else if (enemyList.Count < 2) {
            for (int i = 0; i < enemyList.Count; i++) {
                enemyList[i].speedMultiplier = 4.5f;
            }
        }
        //Check number to change speed

        deltaPosY = Mathf.Lerp(deltaPosY, 0, 0.2f);

        for (int i = 0; i < enemyList.Count; i++) {
            //Change horizontal direction
            if (enemyList[i].transform.position.x > enemyPositionLimit || enemyList[i].transform.position.x < -enemyPositionLimit) {
                if (hDirChanged == false) {
                    for (int j = 0; j < enemyList.Count; j++) {
                        enemyList[j].speed *= -1;
                        deltaPosY = 0.1f;
                    }

                    hDirChanged = true;
                    Invoke("ResetHorizontalDirChange", Time.deltaTime);
                }
            }
            //Change horizontal direction

            //Change vertical direction for zigzag movement
            if (enemyList[i].movementStyle == Enemy.MovementStyle.Zigzag) {
                if (enemyList[i].transform.position.y > enemyList[i].currentPosY - 1 || enemyList[i].transform.position.y < enemyList[i].currentPosY - 3) {
                    if (vDirChanged == false) {
                        for (int j = 0; j < enemyList.Count; j++) {
                            enemyList[j].verticalSpeed *= -1;
                        }
                        vDirChanged = true;
                        Invoke("ResetVerticalDirChange", Time.deltaTime);
                    }
                }
            }
            //Change vertical direction for zigzag movement
        }

        //Shake Camera on Destroy enemy;
        cameraShakeAmount = Mathf.Lerp(cameraShakeAmount, 0, 0.2f);
        Camera.main.transform.position = new Vector3(Random.insideUnitCircle.x * cameraShakeAmount, Random.insideUnitCircle.y * cameraShakeAmount, -10);
        //Shake Camera on Destroy enemy;

        //Instanciate Player if player is dead
        if (gameMode == GameMode.SinglePlayerMode) {
            if (player1Count > 0 && player1IsDead) {
                StartCoroutine(ResetPlayer1(0.5f));
                player1IsDead = false;
            } else if (player1Count <= 0 && player1IsDead) {
                gameIsOver = true;
            }
        } else {
            //Add reset player for Coop Mode script here------------------------------------------*********---------------------------------------------------------------/
            if (player1Count > 0 && player1IsDead) {
                StartCoroutine(ResetPlayer1(0.5f, -1f));
                player1IsDead = false;
            }

            if (player2Count > 0 && player2IsDead) {
                StartCoroutine(ResetPlayer2(0.5f, 1f));
                player2IsDead = false;
            }

            if ((player1Count <= 0 && player1IsDead) && (player2Count <= 0 && player2IsDead)) {
                gameIsOver = true;
            }
        }
        //Instanciate Player if player is dead

        //Only for Dev
        /*if (Input.GetKeyDown(KeyCode.K)) {
            player1Count = 0;
            player1IsDead = true;
            player2Count = 0;
            player2IsDead = true;
            gameIsOver = true;
        }*/
        //Only for Dev

        //Record current game progress
        if (gameIsOver) {
            ProgressManager.currentWave = wave;
            ProgressManager.currentWaveIndex = waveIndex;
            if (countDownTime > 0) {
                countDownTime -= Time.deltaTime;
                if (Input.GetButtonDown("Start")) {
                    if (MenuSoundManager.instance != null) {
                        MenuSoundManager.instance.PlayeButtonSelectedSound();
                    }
                    ReloadLevel();
                }
            } else {
                LoadMainMenu();
            }
        } else {
            countDownTime = 10;
            if (Input.GetButtonDown("Pause")) {
                if (MenuSoundManager.instance != null) {
                    MenuSoundManager.instance.PlayeButtonSelectedSound();
                }
                eventSystem.SetSelectedGameObject(null);
                gameIsPause = !gameIsPause;


#if UNITY_EDITOR
                StartCoroutine(SelectButton());
#elif UNITY_STANDALONE
                StartCoroutine(SelectButton());
#endif
            }
        }
        //Record current game progress
    }

    private void SetEnemySpeed() {
        //enemySpeed = ((float)wave + 1) / 3;
        currentWaveEnemySpeed = enemySpeed;
    }

    private void ResetHorizontalDirChange() {
        hDirChanged = false;
    }

    private void ResetVerticalDirChange() {
        vDirChanged = false;
    }

    public void BombAll(int _playerID) {
        for (int i = 0; i < enemyList.Count; i++) {
            enemyList[i].health -= 36;
            enemyList[i].playerID = _playerID;
        }
    }

    private IEnumerator RecreateLevel(float _time = 0) {
        yield return new WaitForSeconds(_time);
        if (wave <= 9) {
            wave++;
            waveIndex = wave + 1;
        } else if(wave > 9){
            wave = Random.Range(10, 20);
            waveIndex++;
        }
        enemyList.Clear();
        LevelBuilder.instance.BuildLevel(wave);
        SetEnemySpeed();
        levelBuilt = true;
    }

    private IEnumerator ResetPlayer1(float _time = 0, float _xPos = 0) {
        yield return new WaitForSeconds(_time);
        player1Clone = Instantiate(player1, new Vector3(_xPos, -4, -2), Quaternion.identity) as PlayerControl;
    }

    private IEnumerator ResetPlayer2(float _time = 0, float _xPos = 0) {
        yield return new WaitForSeconds(_time);
        player2Clone = Instantiate(player2, new Vector3(_xPos, -4, -2), Quaternion.identity) as PlayerControl;
    }

    private IEnumerator SelectButton() {
        yield return new WaitForEndOfFrame();
        eventSystem.SetSelectedGameObject(resumeButton.gameObject);
    }

    public void ReloadLevel() {
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayeButtonSelectedSound();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu() {
#if UNITY_EDITOR
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayeButtonSelectedSound();
        }
        SceneManager.LoadScene("MainMenu");
#elif UNITY_STANDALONE
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayeButtonSelectedSound();
        }
        SceneManager.LoadScene("MainMenu");
#endif

#if UNITY_WEBGL
        SceneManager.LoadScene("MainMenu_Web");
#endif
    }

    public void ResumeGame() {
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayeButtonSelectedSound();
        }
        gameIsPause = false;
    }

    public void OnButtonSwitched() {
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayButtonSwitchSound();
        }
    }
}
