using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [HideInInspector] public float power = 0;

    [HideInInspector] public enum BulletType {
        PlayerBullet,
        EnemyBullet,
        Bomb
    }

    public AudioSource soundFXSource;
    public AudioClip[] bulletSoundFXes;

    [HideInInspector] public BulletType bulletType;

    [HideInInspector] public float showObjecDelay = 0;
    [HideInInspector] public bool soundFXPlayable = true;

    [SerializeField] private Sprite[] bulletSprites;
    [SerializeField] private ParticleSystem[] bulletTrailParticles;

    private SpriteRenderer spriteRenderer;

    private float speed = 6;

    private int dir = 0;

    // Use this for initialization
    void Start () {
        this.gameObject.SetActive(false);
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        
        switch (bulletType) {
            case BulletType.PlayerBullet:
                spriteRenderer.sprite = bulletSprites[0];
                bulletTrailParticles[0].gameObject.SetActive(true);
                dir = 1;
                break;
            case BulletType.EnemyBullet:
                spriteRenderer.sprite = bulletSprites[1];
                bulletTrailParticles[1].gameObject.SetActive(true);
                soundFXSource.clip = bulletSoundFXes[1];
                dir = -1;
                break;
        }
        Invoke("ShowObject", showObjecDelay * Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(new Vector2(0, speed * dir * Time.deltaTime));
        if (this.transform.position.y >= Camera.main.orthographicSize && (bulletType == BulletType.PlayerBullet || bulletType == BulletType.Bomb)) {
            Destroy(this.gameObject);
        }

        if (this.transform.position.y <= -Camera.main.orthographicSize && bulletType == BulletType.EnemyBullet) {
            Destroy(this.gameObject);
        }
    }

    private void ShowObject() {
        this.gameObject.SetActive(true);
        this.transform.parent = null;
        PlaySoundFX(soundFXPlayable);
    }

    private void PlaySoundFX(bool _playSound = true) {
        if (_playSound) {
            soundFXSource.Play();
            soundFXSource.transform.parent = null;
            Destroy(this.soundFXSource.gameObject, this.soundFXSource.clip.length);
        }
    }
}
