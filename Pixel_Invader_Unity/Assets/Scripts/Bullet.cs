using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [HideInInspector] public float power = 0;

    [HideInInspector] public enum BulletType {
        PlayerBullet,
        EnemyBullet
    }

    [HideInInspector] public BulletType bulletType;

    private float speed = 6;

    private int dir = 0;

	// Use this for initialization
	void Start () {
        switch (bulletType) {
            case BulletType.PlayerBullet:
                dir = 1;
                break;
            case BulletType.EnemyBullet:
                dir = -1;
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(new Vector2(0, speed * dir * Time.deltaTime));
        if (this.transform.position.y >= Camera.main.orthographicSize && bulletType == BulletType.PlayerBullet) {
            Destroy(this.gameObject);
        }

        if (this.transform.position.y <= -Camera.main.orthographicSize && bulletType == BulletType.EnemyBullet) {
            Destroy(this.gameObject);
        }
    }
}
