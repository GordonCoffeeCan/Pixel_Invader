using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [HideInInspector] public float power = 0;

    public AudioSource soundFXSource;
    public AudioClip[] bulletSoundFXes;

    [HideInInspector] public float showObjecDelay = 0;
    [HideInInspector] public bool soundFXPlayable = true;
    [HideInInspector] public int dir = 0;

    private float speed = 6;

    // Use this for initialization
    void Start () {
        this.gameObject.SetActive(false);
        Invoke("ShowObject", showObjecDelay * Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(new Vector2(0, speed * dir * Time.deltaTime));
        if (this.transform.position.y >= Camera.main.orthographicSize + 1 || this.transform.position.y <= -Camera.main.orthographicSize - 1) {
            Destroy(this.gameObject);
        }
    }

    private void ShowObject() {
        this.gameObject.SetActive(true);
        this.transform.parent = null;
        PlaySoundFX(soundFXPlayable);
    }

    private void PlaySoundFX(bool _playSound = true) {
        if (_playSound && soundFXSource != null) {
            soundFXSource.Play();
            soundFXSource.transform.parent = null;
            Destroy(this.soundFXSource.gameObject, this.soundFXSource.clip.length);
        }
    }
}
