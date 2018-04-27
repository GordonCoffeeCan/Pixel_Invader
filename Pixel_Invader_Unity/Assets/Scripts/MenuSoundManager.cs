using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundManager : MonoBehaviour {

    public static MenuSoundManager instance;

    [SerializeField] private AudioClip[] menuAudio;

    private AudioSource audioSource;

    private void Awake() {
        instance = this;
        audioSource = this.GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayButtonSwitchSound() {
        audioSource.clip = menuAudio[0];
        audioSource.Play();
    }

    public void PlayeButtonSelectedSound() {
        audioSource.clip = menuAudio[1];
        audioSource.Play();
    }
}
