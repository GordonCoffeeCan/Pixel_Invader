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

    public void OnStartGame() {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnNewGame() {
        SceneManager.LoadScene("GameModes");
    }

    public void OnOnePlayerGame() {
        ProgressManager.currentWave = 0;
        ProgressManager.currentWaveIndex = 1;
        SceneManager.LoadScene("SinglePlayerMode");
    }

    public void OnTwoPlayerGame() {
        ProgressManager.currentWave = 0;
        ProgressManager.currentWaveIndex = 1;
        SceneManager.LoadScene("CoopMode");
    }

    public void OnHowToPlay() {
        SceneManager.LoadScene("HowToPlay");
    }

    public void OnMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnExitGame() {
        //Application.Quit();
        SceneManager.LoadScene("TitleScreen");
    }
}
