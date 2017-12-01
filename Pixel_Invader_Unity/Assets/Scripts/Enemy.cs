using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private float health;
    [SerializeField] private Bullet enemyBullet;
    [SerializeField] private RuntimeAnimatorController[] enemyAnimControllers;
    [SerializeField] private ParticleSystem hitFX;
    [SerializeField] private ParticleSystem[] explosionFXes;

    [HideInInspector] public float speed = 0;
    [HideInInspector] public float verticalSpeed = 0;
    [HideInInspector] public float currentPosY = 0;

    private float shootGap = 0;
    private float currentShootGap = 0;

    private SpriteRenderer spriteRender;

    private ParticleSystem enemyExplosionFX;

    private int score = 0;

    private Rigidbody2D rig;
    private Animator enemyAnim;
    private bool isAbleToShoot = false;

    public enum EnemyType {
        RegularEnemy,
        EnemyCarrier,
        EnemyMotherShip,
        ArmouredEnemy,
        SuicideEnemy,
        SpawnPoint
    }

    public enum MovementStyle {
        LeftAndRight,
        Zigzag
    }

    public EnemyType enemyType;
    public MovementStyle movementStyle;

	// Use this for initialization
	void Start () {
        spriteRender = this.GetComponent<SpriteRenderer>();
        enemyAnim = this.GetComponent<Animator>();
        switch (enemyType) {
            case EnemyType.RegularEnemy:
                health = 10;
                isAbleToShoot = RegularEnemyShoot();
                if (isAbleToShoot) {
                    score = 15;
                } else {
                    score = 10;
                }
                shootGap = Random.Range(1.65f, 5.85f);
                currentShootGap = shootGap;
                enemyAnim.runtimeAnimatorController = enemyAnimControllers[0];
                enemyExplosionFX = explosionFXes[0];
                break;
            case EnemyType.EnemyCarrier:
                health = 10;
                score = 60;
                spriteRender.color = new Color32(211, 30, 240, 255);
                break;
            case EnemyType.EnemyMotherShip:
                health = 20;
                score = 80;
                enemyAnim.runtimeAnimatorController = enemyAnimControllers[1];
                enemyExplosionFX = explosionFXes[1];
                break;
            case EnemyType.ArmouredEnemy:
                health = 40;
                score = 120;
                isAbleToShoot = true;
                shootGap = Random.Range(3.15f, 10.85f);
                currentShootGap = shootGap;
                spriteRender.color = new Color32(45, 141, 253, 255);
                break;
            case EnemyType.SuicideEnemy:
                health = 10;
                score = 75;
                spriteRender.color = new Color32(30, 223, 191, 255);
                break;
            case EnemyType.SpawnPoint:
                break;
        }

        rig = this.GetComponent<Rigidbody2D>();

        if (movementStyle == MovementStyle.Zigzag) {
            currentPosY = this.transform.position.y;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0) {
            GameManager.instance.score += score;
            GameManager.instance.enemiesCount--;
            GameManager.instance.enemyList.Remove(this);
            Instantiate(enemyExplosionFX, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if (isAbleToShoot && this.transform.position.y < Camera.main.orthographicSize + 0.25f) {
            currentShootGap -= Time.deltaTime;

            if (currentShootGap <= 0) {
                currentShootGap = shootGap;
                Bullet _enemyBulletClone = Instantiate(enemyBullet, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
                _enemyBulletClone.bulletType = Bullet.BulletType.EnemyBullet;
                _enemyBulletClone.power = 100;
                _enemyBulletClone.gameObject.tag = "EnemyBullet";
                Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _enemyBulletClone.GetComponent<BoxCollider2D>());
            }
        }

        if (this.transform.position.y < -Camera.main.orthographicSize - 1) {
            Destroy(this.gameObject);
            GameManager.instance.enemyList.Remove(this);
        }
    }

    private void FixedUpdate() {
        switch (movementStyle) {
            case MovementStyle.LeftAndRight:
                rig.MovePosition(new Vector2(this.transform.position.x + speed * Time.deltaTime, this.transform.position.y));
                break;
            case MovementStyle.Zigzag:
                rig.MovePosition(new Vector2(this.transform.position.x + speed * Time.deltaTime, this.transform.position.y - verticalSpeed * Time.deltaTime));
                break;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D _col) {
        if(_col.tag == "Bullet") {
            health -= _col.gameObject.GetComponent<Bullet>().power;
            Instantiate(hitFX, _col.transform.position, Quaternion.identity);
            Destroy(_col.gameObject);
        }
    }

    private bool RegularEnemyShoot() {
        bool _isAbleToShoot = false;
        if (GameManager.instance.wave >= 2 && GameManager.instance.wave < 4) {
            switch(Random.Range(1, 21)) {
                case 2:
                    _isAbleToShoot = true;
                    break;
                case 18:
                    _isAbleToShoot = true;
                    break;
                default:
                    _isAbleToShoot = false;
                    break;
            }
        }else if (GameManager.instance.wave >= 4 && GameManager.instance.wave < 7) {
            switch (Random.Range(1, 13)) {
                case 6:
                    _isAbleToShoot = true;
                    break;
                default:
                    _isAbleToShoot = false;
                    break;
            }
        } else if (GameManager.instance.wave >= 7 && GameManager.instance.wave < 14) {
            switch (Random.Range(1, 7)) {
                case 5:
                    _isAbleToShoot = true;
                    break;
                default:
                    _isAbleToShoot = false;
                    break;
            }
        } else if (GameManager.instance.wave >= 14) {
            switch (Random.Range(1, 5)) {
                case 3:
                    _isAbleToShoot = true;
                    break;
                default:
                    _isAbleToShoot = false;
                    break;
            }
        }
        return _isAbleToShoot;
    }
}
