using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health;

    [SerializeField] private Bullet enemyBullet;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private RuntimeAnimatorController[] enemyAnimControllers;
    [SerializeField] private ParticleSystem[] explosionFXes;
    [SerializeField] private DropBox dropBox;

    [HideInInspector] public float speed = 0;
    [HideInInspector] public float verticalSpeed = 0;
    [HideInInspector] public float currentPosY = 0;

    private float shootGap = 0;
    private float currentShootGap = 0;
    private float cameraShakeAmount = 0;

    private SpriteRenderer spriteRender;

    private ParticleSystem enemyExplosionFX;

    private int score = 0;

    private Rigidbody2D rig;
    private BoxCollider2D boxCollider;
    private Animator enemyAnim;
    private bool isAbleToShoot = false;
    private bool hasDropBox = false;

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
        boxCollider = this.GetComponent<BoxCollider2D>();
        switch (enemyType) {
            case EnemyType.RegularEnemy:
                health = 20;
                isAbleToShoot = RegularEnemyShoot();
                if (isAbleToShoot) {
                    score = 15;
                } else {
                    score = 10;
                }
                shootGap = Random.Range(1.65f, 5.85f);
                cameraShakeAmount = Random.Range(0.1f, 0.2f);
                currentShootGap = shootGap;
                enemyAnim.runtimeAnimatorController = enemyAnimControllers[0];
                enemyExplosionFX = explosionFXes[0];
                spriteRender.sprite = sprites[0];
                if (Random.Range(0, 26) == 5) {
                    hasDropBox = true;
                }
                break;
            case EnemyType.EnemyCarrier:
                health = 20;
                score = 60;
                cameraShakeAmount = Random.Range(0.1f, 0.2f);
                
                enemyAnim.runtimeAnimatorController = enemyAnimControllers[2];
                enemyExplosionFX = explosionFXes[2];
                spriteRender.sprite = sprites[2];
                hasDropBox = true;
                break;
            case EnemyType.EnemyMotherShip:
                health = 30;
                score = 80;
                cameraShakeAmount = Random.Range(0.25f, 0.4f);
                enemyAnim.runtimeAnimatorController = enemyAnimControllers[1];
                enemyExplosionFX = explosionFXes[1];
                spriteRender.sprite = sprites[1];
                break;
            case EnemyType.ArmouredEnemy:
                health = 50;
                score = 120;
                isAbleToShoot = true;
                shootGap = Random.Range(3.15f, 10.85f);
                currentShootGap = shootGap;
                cameraShakeAmount = Random.Range(0.1f, 0.2f);
                spriteRender.color = new Color32(45, 141, 253, 255);
                enemyExplosionFX = explosionFXes[0];
                spriteRender.sprite = sprites[0];
                break;
            case EnemyType.SuicideEnemy:
                health = 10;
                score = 75;
                cameraShakeAmount = Random.Range(0.25f, 0.35f);
                spriteRender.color = new Color32(30, 223, 191, 255);
                enemyExplosionFX = explosionFXes[0];
                spriteRender.sprite = sprites[0];
                break;
            case EnemyType.SpawnPoint:
                break;
        }

        rig = this.GetComponent<Rigidbody2D>();
        boxCollider.size = new Vector2(spriteRender.sprite.rect.size.x / spriteRender.sprite.pixelsPerUnit, spriteRender.sprite.rect.size.y / spriteRender.sprite.pixelsPerUnit);

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
            GameManager.instance.cameraShakeAmount += cameraShakeAmount;
            Instantiate(enemyExplosionFX, this.transform.position, Quaternion.identity);
            if (hasDropBox) {
                Instantiate(dropBox, this.transform.position + Vector3.forward * -2.5f , Quaternion.identity);
            }
            Destroy(this.gameObject);
        }

        if (isAbleToShoot && this.transform.position.y < Camera.main.orthographicSize + 0.25f) {
            currentShootGap -= Time.deltaTime;

            if (currentShootGap <= 0) {
                currentShootGap = shootGap;
                Bullet _enemyBulletClone = Instantiate(enemyBullet, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
                _enemyBulletClone.power = 100;
                _enemyBulletClone.dir = -1;
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
            Bullet _bullet = _col.gameObject.GetComponent<Bullet>();
            health -= _bullet.power;
            _bullet.hitEnemy = true;
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
