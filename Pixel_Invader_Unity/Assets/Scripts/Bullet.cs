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

    [HideInInspector] public BulletType bulletType;

    [SerializeField] private Sprite[] bulletSprites;
    [SerializeField] private ParticleSystem[] bulletTrailParticles;

    private SpriteRenderer spriteRenderer;

    private float speed = 6;

    private int dir = 0;

	// Use this for initialization
	void Start () {
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
                dir = -1;
                break;
        }
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
}
