using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private Image[] lifes;
    [SerializeField] private Image[] lasers;
    [SerializeField] private Image[] bombs;
    [SerializeField] private Text waveClearText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text countDownText;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < lasers.Length; i++) {
            lasers[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < bombs.Length; i++) {
            bombs[i].gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        ChangeUICount(GameManager.instance.player1Count, lifes);
        //ChangeUICount(GameManager.instance.bombCount, bombs);
        //ChangeUICount(GameManager.instance.laserCount, lasers);

        if (GameManager.instance.levelBuilt == false) {
            waveClearText.text = "Wave " + (GameManager.instance.waveIndex).ToString() + " Clear!\r\nGet Ready for New Wave!!";
            waveClearText.gameObject.SetActive(true);
        } else {
            waveClearText.text = "";
            waveClearText.gameObject.SetActive(false);
        }

        if (GameManager.instance.gameIsOver) {
            gameOverText.gameObject.SetActive(true);
        } else {
            gameOverText.gameObject.SetActive(false);
        }

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
