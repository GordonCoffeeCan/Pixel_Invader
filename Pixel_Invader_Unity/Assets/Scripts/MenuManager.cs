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
        SceneManager.LoadScene(1);
    }

    public void OnHowToPlay() {
        SceneManager.LoadScene(1);
    }

    public void OnExitGame() {
        Application.Quit();
    }
}
