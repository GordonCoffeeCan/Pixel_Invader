using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private float health;
    [SerializeField] private Bullet enemyBullet;
    [SerializeField] private RuntimeAnimatorController[] enemyAnimControllers;

    [HideInInspector] public float speed = 0;

    private float shootGap = 0;
    private float currentShootGap = 0;

    private SpriteRenderer spriteRender;

    private int score = 0;

    private Rigidbody2D rig;
    private Animator enemyAnim;
    private bool isAbleToShoot = false;

    public enum EnemyType {
        RegularEnemy,
        EnemyCarrier,
        EnemyMotherShip,
        ArmouredEnemy,
        SuicideEnemy
    }

    public EnemyType enemyType;

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
        }

        rig = this.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0) {
            GameManager.instance.score += score;
            GameManager.instance.enemiesCount--;
            GameManager.instance.enemyList.Remove(this);
            Destroy(this.gameObject);
        }

        if (isAbleToShoot && this.transform.position.y < Camera.main.orthographicSize + 0.25f) {
            currentShootGap -= Time.deltaTime;

            if (currentShootGap <= 0) {
                currentShootGap = shootGap;
                Bullet _enemyBulletClone = Instantiate(enemyBullet, this.transform.position, Quaternion.identity) as Bullet;
                _enemyBulletClone.bulletType = Bullet.BulletType.EnemyBullet;
                _enemyBulletClone.power = 100;
                _enemyBulletClone.gameObject.tag = "EnemyBullet";
                Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _enemyBulletClone.GetComponent<BoxCollider2D>());
            }
        }
    }

    private void FixedUpdate() {
        rig.MovePosition(new Vector2(this.transform.position.x + speed * Time.deltaTime, this.transform.position.y)); 
    }

    private void OnTriggerEnter2D(Collider2D _col) {
        if(_col.tag == "Bullet") {
            health -= _col.gameObject.GetComponent<Bullet>().power;
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
