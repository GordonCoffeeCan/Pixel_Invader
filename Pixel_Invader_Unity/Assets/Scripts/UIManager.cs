using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private Image[] Lifes;
    [SerializeField] private Image[] Lasers;
    [SerializeField] private Image[] Bombs;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < Lasers.Length; i++) {
            Lasers[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < Bombs.Length; i++) {
            Bombs[i].gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        ChangeUICount(GameManager.instance.playerCount, Lifes);
        ChangeUICount(GameManager.instance.bombCount, Bombs);
        ChangeUICount(GameManager.instance.laserCount, Lasers);
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
