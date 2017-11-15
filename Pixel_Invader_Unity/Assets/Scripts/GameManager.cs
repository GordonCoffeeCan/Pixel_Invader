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
    [HideInInspector] public float targetPosY;
    [HideInInspector] public int moveDirection = 1;
    [HideInInspector] public int enemiesCount = 0;

    [SerializeField] private GameObject levelHolder;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text waveText;

    private bool levelBuilt = false;

    private float currentScore = 0;

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        currentScore = score;

        LevelBuilder.instance.BuildLevel(wave);
        levelBuilt = true;

        Debug.Log(enemiesCount);
    }
	
	// Update is called once per frame
	void Update () {

        enemySpeed = ((float)wave + 1) / 4.5f;

        currentScore = Mathf.Lerp(currentScore, score, 0.15f);
        if(currentScore < 10) {
            scoreText.text = "00" + Mathf.Round(currentScore).ToString();
        }else if (currentScore >= 10 && currentScore < 100) {
            scoreText.text = "0" + Mathf.Round(currentScore).ToString();
        } else if(currentScore >= 100) {
            scoreText.text = Mathf.Round(currentScore).ToString();
        }

        waveText.text = (wave + 1).ToString();

        levelHolder.transform.position = new Vector2(levelHolder.transform.position.x, Mathf.Lerp(levelHolder.transform.position.y, targetPosY, 0.15f));

        if (levelHolder.transform.position.y < targetPosY + 0.01f) {
            currentWaveEnemySpeed = enemySpeed;
        } else {
            currentWaveEnemySpeed = 0;
        }

        if(enemiesCount <= 0 && levelBuilt == true) {
            levelBuilt = false;
            Debug.Log("Level Cleared!");
            wave++;
        }

        if (levelBuilt == false) {
            LevelBuilder.instance.BuildLevel(wave);
            levelBuilt = true;
        }
    }
}
