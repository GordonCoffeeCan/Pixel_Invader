using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private Image[] player1Lifes;
    [SerializeField] private Image[] player1Shields;
    [SerializeField] private Image[] player1Bombs;
    [SerializeField] private Image[] player1Lasers;

    [SerializeField] private Image[] player2Lifes;
    [SerializeField] private Image[] player2Shields;
    [SerializeField] private Image[] player2Bombs;
    [SerializeField] private Image[] player2Lasers;

    [SerializeField] private Text waveClearText;

    [SerializeField] private Image gameOverPanel;
    [SerializeField] private Text countDownText;

    [SerializeField] private Image pausePanel;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        //Change Player1 properties UI--------------------------------------------------
        ChangeUICount(GameManager.instance.player1Count, player1Lifes);
        ChangeUICount(GameManager.instance.player1ShieldCount, player1Shields);
        ChangeUICount(GameManager.instance.player1BombCount, player1Bombs);
        ChangeUICount(GameManager.instance.player1LaserCount, player1Lasers);
        //Change Player1 properties UI-------------------------------------------------- end

        //Change Player2 properties UI--------------------------------------------------
        ChangeUICount(GameManager.instance.player2Count, player2Lifes);
        ChangeUICount(GameManager.instance.player2ShieldCount, player2Shields);
        ChangeUICount(GameManager.instance.player2BombCount, player2Bombs);
        ChangeUICount(GameManager.instance.player2LaserCount, player2Lasers);
        //Change Player2 properties UI-------------------------------------------------- end

        if (GameManager.instance.levelBuilt == false) {
            waveClearText.text = "Wave " + (GameManager.instance.waveIndex).ToString() + " Clear!\r\nGet Ready for New Wave!!";
            waveClearText.gameObject.SetActive(true);
        } else {
            waveClearText.text = "";
            waveClearText.gameObject.SetActive(false);
        }

        gameOverPanel.gameObject.SetActive(GameManager.instance.gameIsOver);

        pausePanel.gameObject.SetActive(GameManager.instance.gameIsPause);

        countDownText.text = Mathf.RoundToInt(GameManager.instance.countDownTime).ToString();
    }

    private void ChangeUICount(int _number, Image[] _uiImages) {
        switch (_number) {
            case 3:
                for (int i = 0; i < _uiImages.Length; i++) {
                    _uiImages[i].gameObject.SetActive(true);
                }
                break;
            case 2:
                for (int i = 0; i < _uiImages.Length; i++) {
                    if (i == 2) {
                        _uiImages[i].gameObject.SetActive(false);
                    } else {
                        _uiImages[i].gameObject.SetActive(true);
                    }
                }
                break;
            case 1:
                for (int i = 0; i < _uiImages.Length; i++) {
                    if (i == 0) {
                        _uiImages[i].gameObject.SetActive(true);
                    } else {
                        _uiImages[i].gameObject.SetActive(false);
                    }
                }
                break;
            case 0:
                for (int i = 0; i < _uiImages.Length; i++) {
                    _uiImages[i].gameObject.SetActive(false);
                }
                break;
        }
    }
}
