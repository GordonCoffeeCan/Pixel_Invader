using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [HideInInspector] public float power = 0;

    public AudioSource soundFXSource;
    public AudioClip[] bulletSoundFXes;
    [SerializeField] private bool isBomb = false;
    [SerializeField] private ParticleSystem hitFX;

    [HideInInspector] public float showObjecDelay = 0;
    [HideInInspector] public bool soundFXPlayable = true;
    [HideInInspector] public bool hitEnemy = false;
    [HideInInspector] public int dir = 0;

    private float speed = 6;

    private float deltaPosY = 14;

    // Use this for initialization
    void Start () {
        this.gameObject.SetActive(false);
        Invoke("ShowObject", showObjecDelay * Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
        if (isBomb) {
            deltaPosY = Mathf.Lerp(deltaPosY, 0, 0.05f);
            Debug.Log(deltaPosY);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + deltaPosY * Time.deltaTime);
            if (this.transform.position.y >= 0) {
                Destroy(this.gameObject);
            }
        } else {
            this.transform.Translate(new Vector2(0, speed * dir * Time.deltaTime));
            if (this.transform.position.y >= Camera.main.orthographicSize + 1 || this.transform.position.y <= -Camera.main.orthographicSize - 1) {
                Destroy(this.gameObject);
            }
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

    private void OnDestroy() {
        if (hitEnemy) {
            Instantiate(hitFX, this.transform.position, Quaternion.identity);
        }
    }
}
