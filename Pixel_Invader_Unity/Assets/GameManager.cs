using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int wave = 0;
    public float enemySpeed = 0.85f;

    [HideInInspector] public float enemyTargetPosY;
    [HideInInspector] public float score = 0;

    [HideInInspector] public int moveDirection = 1;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text waveText;

    private float currentScore = 0;

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        currentScore = score;
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
    }
}
