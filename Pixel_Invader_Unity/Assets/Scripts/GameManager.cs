using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int wave = 0;
    public float enemySpeed = 0.85f;

    public int playerCount = 3;
    public int bombCount = 0;
    public int laserCount = 0;

    [HideInInspector] public float score = 0;
    [HideInInspector] public float currentWaveEnemySpeed;
    [HideInInspector] public float cameraShakeAmount = 0;
    [HideInInspector] public int moveDirection = 1;
    [HideInInspector] public int enemiesCount = 0;
    [HideInInspector] public int currentLineEnemyDirection = 1;
    [HideInInspector] public float countDownTime = 10;
    [HideInInspector] public bool playerIsDead = false;
    [HideInInspector] public bool gameIsOver = false;
    [HideInInspector] public bool levelBuilt = false;
    [HideInInspector] public bool recreateLevel = false;
    [HideInInspector] public List<Enemy> enemyList = new List<Enemy>();

    [SerializeField] private PlayerControl player;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text waveText;

    private bool hDirChanged = false;
    private bool vDirChanged = false;

    private float currentScore = 0;

    private float deltaPosY;

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        currentScore = score;
        SetEnemySpeed();
        wave = ProgressManager.currentWave;
        LevelBuilder.instance.BuildLevel(wave);
        levelBuilt = true;

        StartCoroutine(ResetPlayer());
    }
	
	// Update is called once per frame
	void Update () {
        currentScore = Mathf.Lerp(currentScore, score, 0.15f);
        if(currentScore < 10) {
            scoreText.text = "00" + Mathf.Round(currentScore).ToString();
        }else if (currentScore >= 10 && currentScore < 100) {
            scoreText.text = "0" + Mathf.Round(currentScore).ToString();
        } else if(currentScore >= 100) {
            scoreText.text = Mathf.Round(currentScore).ToString();
        }

        waveText.text = (wave + 1).ToString();

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

            //Enemy Breakthrough the finl line, Game Over
            if (enemyList[i].transform.position.y <= -(Camera.main.orthographicSize - 1.5f)) {
                playerIsDead = true;
                playerCount = 0;
                gameIsOver = true;
            }
            //Enemy Breakthrough the finl line, Game Over
        }

        deltaPosY = Mathf.Lerp(deltaPosY, 0, 0.2f);


        for (int i = 0; i < enemyList.Count; i++) {
            //Change horizontal direction
            if (enemyList[i].transform.position.x > 4.5f || enemyList[i].transform.position.x < -4.5f) {
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
                if (enemyList[i].transform.position.y > enemyList[i].currentPosY || enemyList[i].transform.position.y < enemyList[i].currentPosY - 1) {
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
        if (playerCount > 0 && playerIsDead) {
            StartCoroutine(ResetPlayer(1));
            playerIsDead = false;
        }else if (playerCount <= 0 && playerIsDead) {
            gameIsOver = true;
        }
        //Instanciate Player if player is dead

        //Only for Dev
        if (Input.GetKeyDown(KeyCode.K)) {
            playerCount = 0;
            playerIsDead = true;
            gameIsOver = true;
        }
        //Only for Dev

        //Record current game progress
        if (gameIsOver) {
            ProgressManager.currentWave = wave;
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
        wave++;
        enemyList.Clear();
        LevelBuilder.instance.BuildLevel(wave);
        SetEnemySpeed();
        levelBuilt = true;
    }

    private IEnumerator ResetPlayer(float _time = 0) {
        yield return new WaitForSeconds(_time);
        Instantiate(player, new Vector3(0, -4, -2), Quaternion.identity);
    }

    public void ReloadLevel() {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }
}
