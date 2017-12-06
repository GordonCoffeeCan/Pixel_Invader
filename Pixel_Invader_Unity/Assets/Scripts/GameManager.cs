﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int wave = 0;
    public float enemySpeed = 0.85f;

    [HideInInspector] public float score = 0;
    [HideInInspector] public float currentWaveEnemySpeed;
    [HideInInspector] public float cameraShakeAmount = 0;
    [HideInInspector] public int moveDirection = 1;
    [HideInInspector] public int enemiesCount = 0;
    [HideInInspector] public int currentLineEnemyDirection = 1;

    [HideInInspector] public List<Enemy> enemyList = new List<Enemy>();

    [SerializeField] private GameObject levelHolder;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text waveText;

    private bool levelBuilt = false;
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
            if (enemyList[i].movementStyle == Enemy.MovementStyle.LeftAndRight) {
                enemyList[i].transform.position = new Vector2(enemyList[i].transform.position.x, enemyList[i].transform.position.y - deltaPosY);
            } 
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
            /*Invoke(){

            }*/
        }
    }

    private void bombAffectToEnemy(Enemy _enemy) {
        _enemy.health -= 20;
    }
}
