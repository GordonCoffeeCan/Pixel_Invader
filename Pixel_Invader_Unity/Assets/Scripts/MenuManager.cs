﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    [SerializeField] private VideoPlayer inGameTrailer;
    [SerializeField] private GameBackground gameBackground;
    [SerializeField] private BackgroundMusic backgroundMusic;
    [SerializeField] private MenuSoundManager menuSound;

    [SerializeField] private float gameIdleTimer = 10f;

    [SerializeField] private Text path;

    private float currentGameIdleTimer;

    private void Awake() {
        if (SceneManager.GetActiveScene().name == "TitleScreen") {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Use this for initialization
    void Start () {
        if(SceneManager.GetActiveScene().name == "TitleScreen") {
            InstanciateNoneDestroyObjects();

            if (inGameTrailer != null) {
                inGameTrailer.gameObject.SetActive(false);
            }

            currentGameIdleTimer = gameIdleTimer;
        }

        if (path != null) {
            path.text = Application.absoluteURL;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().name == "TitleScreen") {
            if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Start")) {
                OnStartGame();
            }

            //In-Game Trailer video ---------------------------------------------------------
            if (inGameTrailer != null) {
                currentGameIdleTimer -= Time.deltaTime;

                if (currentGameIdleTimer <= 0) {
                    inGameTrailer.gameObject.SetActive(true);
                    inGameTrailer.Play();
                    currentGameIdleTimer = 0;
                }

                if (inGameTrailer.isPlaying) {
                    if ((int)inGameTrailer.frame == (int)inGameTrailer.frameCount) {
                        inGameTrailer.Stop();
                        inGameTrailer.gameObject.SetActive(false);
                        currentGameIdleTimer = gameIdleTimer;
                    }
                }
            }
            //In-Game Trailer video --------------------------------------------------------- end
        }

        if (SceneManager.GetActiveScene().name == "MainMenu") {
            if (Input.GetButtonDown("Cancel")) {
                OnExitGame();
            }
        }

        if (SceneManager.GetActiveScene().name == "HowToPlay") {
            if(Input.GetButtonDown("Submit") || Input.GetButtonDown("Start")) {
                OnNewGame();
            } else if (Input.GetButtonDown("Cancel")) {
                OnStartGame();
            }
        }

        if (SceneManager.GetActiveScene().name == "GameModes") {
            if (Input.GetButtonDown("Cancel")) {
                OnStartGame();
            }
        }
    }

    public void OnStartGame() {
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayeButtonSelectedSound();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void OnNewGame() {
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayeButtonSelectedSound();
        }
        SceneManager.LoadScene("GameModes");
    }

    public void OnOnePlayerGame() {
        ProgressManager.currentWave = 0;
        ProgressManager.currentWaveIndex = 1;
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayeButtonSelectedSound();
        }
        SceneManager.LoadScene("SinglePlayerMode");
    }

    public void OnTwoPlayerGame() {
        ProgressManager.currentWave = 0;
        ProgressManager.currentWaveIndex = 1;
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayeButtonSelectedSound();
        }
        SceneManager.LoadScene("CoopMode");
    }

    public void OnHowToPlay() {
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayeButtonSelectedSound();
        }
        SceneManager.LoadScene("HowToPlay");
    }

    public void OnMainMenu() {
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayeButtonSelectedSound();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void OnExitGame() {
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayeButtonSelectedSound();
        }
        
        SceneManager.LoadScene("TitleScreen");
    }

    public void OnButtonSwitched() {
        if (MenuSoundManager.instance != null) {
            MenuSoundManager.instance.PlayButtonSwitchSound();
        }
    }

    private void InstanciateNoneDestroyObjects() {
        GameBackground[] _gameBGs = GameObject.FindObjectsOfType<GameBackground>();
        BackgroundMusic[] _bgMusics = GameObject.FindObjectsOfType<BackgroundMusic>();
        MenuSoundManager[] _menuSound = GameObject.FindObjectsOfType<MenuSoundManager>();
        if (_gameBGs.Length > 1) {
            Destroy(_gameBGs[0].gameObject);
        } else if(_gameBGs.Length == 0) {
            Instantiate(gameBackground, Vector3.zero, Quaternion.identity);
        }

        if (_bgMusics.Length > 1) {
            Destroy(_bgMusics[0].gameObject);
        } else if(_bgMusics.Length == 0) {
            Instantiate(backgroundMusic, Vector3.zero, Quaternion.identity);
        }

        if (_menuSound.Length > 1) {
            Destroy(_menuSound[0].gameObject);
        } else if(_menuSound.Length == 0) {
            Instantiate(menuSound, Vector3.zero, Quaternion.identity);
        }
    }
}
