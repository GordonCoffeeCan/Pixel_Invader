using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnNewGame() {
        ProgressManager.currentWave = 0;
        ProgressManager.currentWaveIndex = 1;
        SceneManager.LoadScene("CoopMode");
    }

    public void OnHowToPlay() {
        SceneManager.LoadScene("HowToPlay");
    }

    public void OnMainMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void OnExitGame() {
        Application.Quit();
    }
}
