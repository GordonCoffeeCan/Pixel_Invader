using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int wave = 0;
    public float enemySpeed = 0.85f;

    public int player1Count = 3;
    public int player1BombCount = 0;
    public int player1LaserCount = 0;

    public int player2Count = 3;
    public int player2BombCount = 0;
    public int player2LaserCount = 0;

    public enum GameMode {
        SinglePlayerMode,
        CoopMode
    }

    public GameMode gameMode;

    [HideInInspector] public float score = 0;
    [HideInInspector] public float currentWaveEnemySpeed;
    [HideInInspector] public float cameraShakeAmount = 0;
    [HideInInspector] public int moveDirection = 1;
    [HideInInspector] public int enemiesCount = 0;
    [HideInInspector] public int currentLineEnemyDirection = 1;
    [HideInInspector] public int waveIndex = 1;
    [HideInInspector] public float countDownTime = 10;
    [HideInInspector] public bool player1IsDead = false;
    [HideInInspector] public bool player2IsDead = false;
    [HideInInspector] public bool gameIsOver = false;
    [HideInInspector] public bool levelBuilt = false;
    [HideInInspector] public bool recreateLevel = false;
    [HideInInspector] public List<Enemy> enemyList = new List<Enemy>();
    [HideInInspector] public PlayerControl player1Clone;
    [HideInInspector] public PlayerControl player2Clone;
    [HideInInspector] public float vibrateValue = 0;

    [SerializeField] private PlayerControl player1;
    [SerializeField] private PlayerControl player2;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text waveText;
    [SerializeField] private bool controllerVibration = true;

    private bool hDirChanged = false;
    private bool vDirChanged = false;

    private float currentScore = 0;
    private float enemyPositionLimit = 0;

    private float deltaPosY;

    private PlayerIndex playerIndex = PlayerIndex.One;

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        currentScore = score;
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
        currentScore = Mathf.Lerp(currentScore, score, 0.15f);

        scoreText.text = Mathf.Round(currentScore).ToString();
        waveText.text = waveIndex.ToString();

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
                StartCoroutine(ResetPlayer1(1));
                player1IsDead = false;
            } else if (player1Count <= 0 && player1IsDead) {
                gameIsOver = true;
            }
        } else {
            //Add reset player for Coop Mode script here------------------------------------------*********---------------------------------------------------------------/
            if (player1Count > 0 && player1IsDead) {
                StartCoroutine(ResetPlayer1(1, -1f));
                player1IsDead = false;
            }

            if (player2Count > 0 && player2IsDead) {
                StartCoroutine(ResetPlayer2(1, 1f));
                player2IsDead = false;
            }

            if ((player1Count <= 0 && player1IsDead) && (player2Count <= 0 && player2IsDead)) {
                gameIsOver = true;
            }
        }
        //Instanciate Player if player is dead

        //Only for Dev
        if (Input.GetKeyDown(KeyCode.K)) {
            player1Count = 0;
            player1IsDead = true;
            gameIsOver = true;
        }
        //Only for Dev

        //Record current game progress
        if (gameIsOver) {
            ProgressManager.currentWave = wave;
            ProgressManager.currentWaveIndex = waveIndex;
            if (countDownTime > 0) {
                countDownTime -= Time.deltaTime;
            } else {
                LoadMainMenu();
            }
        } else {
            countDownTime = 10;
        }
        //Record current game progress
    }

    private void FixedUpdate() {
        if (controllerVibration) {
            vibrateValue = Mathf.MoveTowards(vibrateValue, 0, 0.3f);
        } else {
            vibrateValue = 0;
        }
        
        GamePad.SetVibration(playerIndex, vibrateValue, vibrateValue);
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

    public void BombAll() {
        for (int i = 0; i < enemyList.Count; i++) {
            enemyList[i].health -= Random.Range(10f, 30f);
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

    public void ReloadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }
}
