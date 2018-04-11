using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    [SerializeField] private int playerID = 1;
    [SerializeField] private float speed = 7;
    [SerializeField] private float screenEdge = 0;
    [SerializeField] private float bulletPower = 0;
    [SerializeField] private float bulletGap = 0.35f;
    [SerializeField] private float tripleBulletGap = 0.45f;
    [SerializeField] private float shotgunBulletGap = 0.6f;
    [SerializeField] private Bullet regularBullet;
    [SerializeField] private Bullet heavyGunBullet;
    [SerializeField] private Bullet shotgunBullet;
    [SerializeField] private Bullet bomb;
    [SerializeField] private Bullet laser;
    [SerializeField] private ParticleSystem playerExplosionFX;
    [SerializeField] private ParticleSystem muteFX;
    [SerializeField] private ParticleSystem[] powerUpFX;
    [SerializeField] private GameObject[] weapons;

    [HideInInspector] public enum GunType {
        RegularGun,
        HeavyGun,
        ShotGun
    }

    [HideInInspector] public GunType gunType;

    private Rigidbody2D rig;
    private float currentShootGap = 0;
    private float muteForBulletTime = 0;
    private Animator playerAnim;
    private ControllerInputManager controllerInput;

	// Use this for initialization
	void Start () {
        rig = this.GetComponent<Rigidbody2D>();
        playerAnim = this.GetComponent<Animator>();
        controllerInput = this.GetComponent<ControllerInputManager>();
        controllerInput.SetupPlayerInput(playerID);
        Instantiate(muteFX, this.transform.position + Vector3.down * 0.06f, Quaternion.identity, this.transform);
        muteForBulletTime = 2.5f;
        screenEdge = WindowSizeUtil.instance.halfWindowSize.x - 4.15f;
    }
	
	// Update is called once per frame
	void Update () {
		if(this.transform.position.x < -screenEdge) {
            this.transform.position = new Vector2(-screenEdge, this.transform.position.y);
        }else if (this.transform.position.x > screenEdge) {
            this.transform.position = new Vector2(screenEdge, this.transform.position.y);
        }

        if(currentShootGap > 0) {
            currentShootGap -= Time.deltaTime;
        }

        /*//Only for development------------------------------------
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            OnRegularGun();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            OnHeavyGun();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            OnShotGun();
        }
        //Only for development------------------------------------*/

        if (controllerInput.ShootBullet() && currentShootGap <= 0) {
            switch (gunType) {
                case GunType.RegularGun:
                    CreateRegularBullet();
                    break;
                case GunType.HeavyGun:
                    CreateTripleBullet();
                    break;
                case GunType.ShotGun:
                    CreateShotGunBullet();
                    break;
            }
        }

        //Special weapons count----------------------------------------------------------------------------------------
        if (playerID == 1) {
            if (controllerInput.ShootBomb() && GameManager.instance.player1BombCount > 0) {
                GameManager.instance.vibrateValue = 2;
                Bullet _bombClone = Instantiate(bomb, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
                _bombClone.dir = 1;
                Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bombClone.GetComponent<BoxCollider2D>());
                GameManager.instance.player1BombCount--;
            }

            if (controllerInput.ShootLaser() && GameManager.instance.player1LaserCount > 0) {
                GameManager.instance.vibrateValue = 2;
                Bullet _laserClone = Instantiate(laser, this.transform.position + Vector3.forward * 0.1f, Quaternion.identity, this.transform) as Bullet;
                _laserClone.power = 80;
                Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _laserClone.GetComponent<BoxCollider2D>());
                GameManager.instance.player1LaserCount--;
            }
        }else if (playerID == 2) {
            if (controllerInput.ShootBomb() && GameManager.instance.player2BombCount > 0) {
                GameManager.instance.vibrateValue = 2;
                Bullet _bombClone = Instantiate(bomb, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
                _bombClone.dir = 1;
                Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bombClone.GetComponent<BoxCollider2D>());
                GameManager.instance.player2BombCount--;
            }

            if (controllerInput.ShootLaser() && GameManager.instance.player2LaserCount > 0) {
                GameManager.instance.vibrateValue = 2;
                Bullet _laserClone = Instantiate(laser, this.transform.position + Vector3.forward * 0.1f, Quaternion.identity, this.transform) as Bullet;
                _laserClone.power = 80;
                Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _laserClone.GetComponent<BoxCollider2D>());
                GameManager.instance.player2LaserCount--;
            }
        }
        //Special weapons count----------------------------------------------------------------------------------------


        muteForBulletTime -= Time.deltaTime;

        if (muteForBulletTime <= 0) {
            muteForBulletTime = 0;
        }

        if (GameManager.instance.gameIsOver) {
            Instantiate(playerExplosionFX, this.transform.position, Quaternion.identity);
            GameManager.instance.cameraShakeAmount += 0.35f;
            GameManager.instance.player1IsDead = true;
            Destroy(this.gameObject);
        }
	}

    private void FixedUpdate() {
        rig.MovePosition(new Vector2(this.transform.position.x + speed * controllerInput.MoveHorizontal() * Time.deltaTime, this.transform.position.y));
        playerAnim.SetFloat("Speed", controllerInput.MoveHorizontal());
    }

    private void OnTriggerEnter2D(Collider2D _col) {
        if (_col.tag == "EnemyBullet") {
            GameManager.instance.vibrateValue = 2;
            Destroy(_col.gameObject);
            Instantiate(playerExplosionFX, this.transform.position, Quaternion.identity);
            GameManager.instance.cameraShakeAmount += 0.35f;
            if (muteForBulletTime <= 0) {
                OnRegularGun();
                if(playerID == 1) {
                    GameManager.instance.player1IsDead = true;
                    GameManager.instance.player1Count--;
                }else if (playerID == 2) {
                    GameManager.instance.player2IsDead = true;
                    GameManager.instance.player2Count--;
                }
                
                Destroy(this.gameObject);
            }
        }

        if (_col.tag == "DropBox") {
            DropBox _dropBox = _col.GetComponent<DropBox>();
            switch (_dropBox.boxType) {
                case DropBox.BoxType.Bomb:
                    Instantiate(powerUpFX[0], this.transform);
                    if (playerID == 1) {
                        if (GameManager.instance.player1BombCount < 3) {
                            GameManager.instance.player1BombCount++;
                        }
                    } else if (playerID == 2) {
                        if (GameManager.instance.player2BombCount < 3) {
                            GameManager.instance.player2BombCount++;
                        }
                    }
                    break;
                case DropBox.BoxType.HeavyGun:
                    OnHeavyGun();
                    break;
                case DropBox.BoxType.Laser:
                    Instantiate(powerUpFX[3], this.transform);

                    if (playerID == 1) {
                        if (GameManager.instance.player1LaserCount < 3) {
                            GameManager.instance.player1LaserCount++; ;
                        }
                    } else if (playerID == 2) {
                        if (GameManager.instance.player2LaserCount < 3) {
                            GameManager.instance.player2LaserCount++; ;
                        }
                    }
                    break;
                case DropBox.BoxType.NewSpacecraft:
                    Instantiate(powerUpFX[1], this.transform);
                    if (playerID == 1) {
                        if (GameManager.instance.player1Count < 3) {
                            GameManager.instance.player1Count++;
                        }
                    }else if (playerID == 2) {
                        if (GameManager.instance.player2Count < 3) {
                            GameManager.instance.player2Count++;
                        }
                    }
                    
                    break;
                case DropBox.BoxType.Shotgun:
                    OnShotGun();
                    break;
            }
            Destroy(_col.gameObject);
        }

        if(_col.tag == "Enemy"){
            Enemy _enemy = _col.GetComponent<Enemy>();
            if (_enemy.enemyType == Enemy.EnemyType.SuicideEnemy) {
                _enemy.health -= 100;
                Instantiate(playerExplosionFX, this.transform.position, Quaternion.identity);
                GameManager.instance.cameraShakeAmount += 0.35f;
                if (muteForBulletTime <= 0) {
                    OnRegularGun();
                    GameManager.instance.player1IsDead = true;
                    GameManager.instance.player1Count--;
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void OnRegularGun() {
        gunType = GunType.RegularGun;
        bulletPower = 10;
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].SetActive(false);
        }
    }

    private void OnHeavyGun() {
        gunType = GunType.HeavyGun;
        bulletPower = 5;
        Instantiate(powerUpFX[2], this.transform);
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].SetActive(false);
            if (i == 0) {
                weapons[i].SetActive(true);
            }
        }
    }

    private void OnShotGun() {
        gunType = GunType.ShotGun;
        bulletPower = 5;
        Instantiate(powerUpFX[4], this.transform);
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].SetActive(false);
            if (i == 1) {
                weapons[i].SetActive(true);
            }
        }
    }

    private void CreateRegularBullet() {
        Bullet _bulletClone = Instantiate(regularBullet, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
        _bulletClone.power = bulletPower;
        _bulletClone.dir = 1;
        _bulletClone.soundFXSource.clip = _bulletClone.bulletSoundFXes[0];
        Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bulletClone.GetComponent<BoxCollider2D>());
        currentShootGap = bulletGap;
    }

    private void CreateTripleBullet() {
        for (int i = 0; i < 3; i++) {
            Vector3 _pos = new Vector2((this.transform.position.x - 0.15f) + 0.15f * Random.Range(0, 3), this.transform.position.y + (i % 2) * 0.2f);
            Bullet _bulletClone = Instantiate(heavyGunBullet, _pos + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
            _bulletClone.transform.parent = this.transform;
            _bulletClone.power = bulletPower;
            _bulletClone.showObjecDelay = i * 0.1f;
            _bulletClone.dir = 1;
            _bulletClone.soundFXSource.clip = _bulletClone.bulletSoundFXes[0];
            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bulletClone.GetComponent<BoxCollider2D>());
        }
        currentShootGap = tripleBulletGap;
    }

    private void CreateShotGunBullet() {
        for (int i = 0; i < 8; i++) {
            float _angle = 15 - 4.29f * i;
            Bullet _bulletClone = Instantiate(shotgunBullet, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.Euler(new Vector3(0, 0, _angle))) as Bullet;
            _bulletClone.transform.parent = this.transform;
            _bulletClone.power = bulletPower;
            if(i > 0) {
                _bulletClone.soundFXPlayable = false;
            }
            _bulletClone.dir = 1;
            _bulletClone.soundFXSource.clip = _bulletClone.bulletSoundFXes[2];
            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bulletClone.GetComponent<BoxCollider2D>());
        }

        currentShootGap = shotgunBulletGap;
    }
}
