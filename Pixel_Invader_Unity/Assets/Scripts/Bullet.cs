using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public AudioSource soundFXSource;
    public AudioClip[] bulletSoundFXes;
    [SerializeField]
    private enum BulletType {
        Bullet,
        Bomb,
        Laser
    }
    [SerializeField] BulletType bulletType;
    [SerializeField] private ParticleSystem hitFX;

    [HideInInspector] public float power = 0;
    [HideInInspector] public int dir = 0;
    [HideInInspector] public float showObjecDelay = 0;
    [HideInInspector] public bool soundFXPlayable = true;
    [HideInInspector] public bool hitEnemy = false;

    private float speed = 6;

    private float deltaPosY = 14;

    // Use this for initialization
    void Start () {
        this.gameObject.SetActive(false);
        if (bulletType == BulletType.Bomb) {
            GameManager.instance.cameraShakeAmount += 0.15f;
        }else if (bulletType == BulletType.Laser) {
            GameManager.instance.cameraShakeAmount += 0.15f;
            Invoke("TurnOffCollider", 0.1f);
            Destroy(this.gameObject, 0.35f);
        }
        Invoke("ShowObject", showObjecDelay);
	}
	
	// Update is called once per frame
	void Update () {
        if (bulletType == BulletType.Bomb) {
            deltaPosY = Mathf.Lerp(deltaPosY, 0, 0.05f);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + deltaPosY * Time.deltaTime);
            if (this.transform.position.y >= 0) {
                Destroy(this.gameObject);
            }
        } else if(bulletType == BulletType.Bullet) {
            this.transform.Translate(new Vector2(0, speed * dir * Time.deltaTime));
            if (this.transform.position.y >= Camera.main.orthographicSize + 1 || this.transform.position.y <= -Camera.main.orthographicSize - 1) {
                Destroy(this.gameObject);
            }
        }
    }

    private void ShowObject() {
        this.gameObject.SetActive(true);
        if (bulletType != BulletType.Laser) {
            this.transform.parent = null;
        }
        PlaySoundFX(soundFXPlayable);
    }

    private void TurnOffCollider() {
        this.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void PlaySoundFX(bool _playSound = true) {
        if (_playSound && soundFXSource != null) {
            soundFXSource.Play();
            soundFXSource.transform.parent = null;
            Destroy(this.soundFXSource.gameObject, this.soundFXSource.clip.length);
        }
    }

    private void OnDestroy() {
        if ((hitEnemy && bulletType != BulletType.Laser) || bulletType == BulletType.Bomb) {
            Instantiate(hitFX, this.transform.position, Quaternion.identity);
        }

        if (bulletType == BulletType.Bomb) {
            GameManager.instance.vibrateValue = 2;
            GameManager.instance.cameraShakeAmount = 0.65f;
            GameManager.instance.BombAll();
        }
    }
}
