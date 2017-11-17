using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int wave = 0;
    public float enemySpeed = 0.85f;

    [HideInInspector] public float score = 0;
    [HideInInspector] public float currentWaveEnemySpeed;
    [HideInInspector] public int moveDirection = 1;
    [HideInInspector] public int enemiesCount = 0;
    [HideInInspector] public int currentLineEnemyDirection = 1;

    [HideInInspector] public List<Enemy> enemyList = new List<Enemy>();

    [SerializeField] private GameObject levelHolder;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text waveText;

    private bool levelBuilt = false;
    private bool dirChanged = false;

    private float currentScore = 0;

    private float deltaPosY;

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        currentScore = score;
        SetEnemySpeed();
        LevelBuilder.instance.BuildLevel(wave);
        levelBuilt = true;
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
            wave++;
        }

        if (levelBuilt == false) {
            enemyList.Clear();
            LevelBuilder.instance.BuildLevel(wave);
            SetEnemySpeed();
            levelBuilt = true;
        }

        for (int i = 0; i < enemyList.Count; i++) {
            if (enemyList[i].transform.position.x > 4.5f || enemyList[i].transform.position.x < -4.5f) {
                if (dirChanged == false) {
                    for (int j = 0; j < enemyList.Count; j++) {
                        enemyList[j].speed *= -1;
                    }
                    deltaPosY = 0.1f;
                    dirChanged = true;
                    Invoke("ResetDirChange", Time.deltaTime);
                }
                
            }
        }

        deltaPosY = Mathf.Lerp(deltaPosY, 0, 0.25f);

        for (int i = 0; i < enemyList.Count; i++) {
            enemyList[i].transform.position = new Vector2(enemyList[i].transform.position.x, enemyList[i].transform.position.y - deltaPosY);
        }
    }

    private void SetEnemySpeed() {
        //enemySpeed = ((float)wave + 1) / 3;
        currentWaveEnemySpeed = enemySpeed;
    }

    private void ResetDirChange() {
        dirChanged = false;
    }
}
