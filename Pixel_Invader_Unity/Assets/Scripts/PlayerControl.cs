using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    [SerializeField][Range(1, 2)] private int playerID = 1;
    [SerializeField] private float speed = 7;
    [SerializeField] private float screenEdge = 0;
    [SerializeField] private float regularBulletPower = 10;
    [SerializeField] private float machineGunBulletPower = 10;
    [SerializeField] private float shotgunBulletPower = 10;
    [SerializeField] private float bulletGap = 0.35f;
    [SerializeField] private float tripleBulletGap = 0.45f;
    [SerializeField] private float shotgunBulletGap = 0.6f;
    [SerializeField] private float gordModeTime = 0;
    [SerializeField] private float shieldActivatedTimer = 2;
    [SerializeField] private Bullet regularBullet;
    [SerializeField] private Bullet heavyGunBullet;
    [SerializeField] private Bullet shotgunBullet;
    [SerializeField] private Bullet bomb;
    [SerializeField] private Bullet laser;
    [SerializeField] private SpriteRenderer shield;
    [SerializeField] private ParticleSystem playerExplosionFX;
    [SerializeField] private ParticleSystem muteFX;
    [SerializeField] private ParticleSystem shieldDeestroyFX;
    [SerializeField] private ParticleSystem[] powerUpFX;
    [SerializeField] private GameObject[] weapons;
    


    [HideInInspector] public enum GunType {
        RegularGun,
        HeavyGun,
        ShotGun
    }

    [HideInInspector] public GunType gunType;

    private Rigidbody2D rig;
    private float bulletPower = 0;
    private float currentShootGap = 0;
    private Animator playerAnim;
    private ControllerInputManager controllerInput;
    private Shield shieldBehavior;
    private bool shieldActivated = false;
    private bool resourceShared = false;

    // Use this for initialization
    void Start () {
        rig = this.GetComponent<Rigidbody2D>();
        playerAnim = this.GetComponent<Animator>();
        controllerInput = this.GetComponent<ControllerInputManager>();
        shieldBehavior = shield.GetComponent<Shield>();
        bulletPower = regularBulletPower;
        controllerInput.SetupPlayerInput(playerID);
        Instantiate(muteFX, this.transform.position + Vector3.down * 0.06f, Quaternion.identity, this.transform);
        screenEdge = WindowSizeUtil.instance.halfWindowSize.x - 4.15f;
    }
	
	// Update is called once per frame
	void Update () {
        //Pause the Game------------------------------------
        if (GameManager.instance.gameIsPause) {
            currentShootGap = 0.35f;
            return;
        }
        //Pause the Game------------------------------------

        if (this.transform.position.x < -screenEdge) {
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
                Bullet _bombClone = Instantiate(bomb, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
                _bombClone.dir = 1;
                _bombClone.playerID = playerID;
                Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bombClone.GetComponent<BoxCollider2D>());
                GameManager.instance.player1BombCount--;
            }

            if (controllerInput.ShootLaser() && GameManager.instance.player1LaserCount > 0) {
                Bullet _laserClone = Instantiate(laser, this.transform.position + Vector3.forward * 0.1f, Quaternion.identity, this.transform) as Bullet;
                _laserClone.power = 80;
                _laserClone.playerID = playerID;
                Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _laserClone.GetComponent<BoxCollider2D>());
                GameManager.instance.player1LaserCount--;
            }
        }else if (playerID == 2) {
            if (controllerInput.ShootBomb() && GameManager.instance.player2BombCount > 0) {
                Bullet _bombClone = Instantiate(bomb, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
                _bombClone.dir = 1;
                _bombClone.playerID = playerID;
                Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bombClone.GetComponent<BoxCollider2D>());
                GameManager.instance.player2BombCount--;
            }

            if (controllerInput.ShootLaser() && GameManager.instance.player2LaserCount > 0) {
                Bullet _laserClone = Instantiate(laser, this.transform.position + Vector3.forward * 0.1f, Quaternion.identity, this.transform) as Bullet;
                _laserClone.power = 80;
                _laserClone.playerID = playerID;
                Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _laserClone.GetComponent<BoxCollider2D>());
                GameManager.instance.player2LaserCount--;
            }
        }
        //Special weapons count----------------------------------------------------------------------------------------

        //On Casting Sheild -----------------------------------------------------------------
        OnShield();
        //On Casting Sheild -----------------------------------------------------------------

        //On Sharing Resources -----------------------------------------------------------------
        if (GameManager.instance.gameMode == GameManager.GameMode.CoopMode) {
            OnShareResources();
        }
        //On Sharing Resources -----------------------------------------------------------------

        gordModeTime -= Time.deltaTime;

        if (gordModeTime <= 0) {
            gordModeTime = 0;
        }

        if (GameManager.instance.gameIsOver) {
            Instantiate(playerExplosionFX, this.transform.position, Quaternion.identity);
            GameManager.instance.cameraShakeAmount += 0.35f;
            GameManager.instance.player1IsDead = true;
            Destroy(this.gameObject);
        }
	}

    private void FixedUpdate() {
        //Pause the Game------------------------------------
        if (GameManager.instance.gameIsPause) {
            return;
        }
        //Pause the Game------------------------------------

        rig.MovePosition(new Vector2(this.transform.position.x + speed * controllerInput.MoveHorizontal() * Time.deltaTime, this.transform.position.y));
        playerAnim.SetFloat("Speed", controllerInput.MoveHorizontal());
    }

    private void OnTriggerEnter2D(Collider2D _col) {
        if (_col.tag == "EnemyBullet") {
            Destroy(_col.gameObject);
            Instantiate(playerExplosionFX, this.transform.position, Quaternion.identity);
            GameManager.instance.cameraShakeAmount += 0.35f;
            if (gordModeTime <= 0) {
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
                case DropBox.BoxType.Shield:
                    Instantiate(powerUpFX[5], this.transform);
                    if (playerID == 1) {
                        if (GameManager.instance.player1ShieldCount < 3) {
                            GameManager.instance.player1ShieldCount++;
                        }
                    } else if (playerID == 2) {
                        if (GameManager.instance.player2ShieldCount < 3) {
                            GameManager.instance.player2ShieldCount++;
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
                if (gordModeTime <= 0) {
                    OnRegularGun(); //Reset player's weapon-------------------
                    if (playerID == 1) {
                        GameManager.instance.player1IsDead = true;
                        GameManager.instance.player1Count--;
                    } else if (playerID == 2) {
                        GameManager.instance.player2IsDead = true;
                        GameManager.instance.player2Count--;
                    }
                    
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void OnRegularGun() {
        gunType = GunType.RegularGun;
        bulletPower = regularBulletPower;
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].SetActive(false);
        }
    }

    private void OnHeavyGun() {
        gunType = GunType.HeavyGun;
        bulletPower = machineGunBulletPower;
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
        bulletPower = shotgunBulletPower;
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
        _bulletClone.playerID = playerID;
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
            _bulletClone.playerID = playerID;
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
            _bulletClone.playerID = playerID;
            _bulletClone.soundFXSource.clip = _bulletClone.bulletSoundFXes[2];
            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bulletClone.GetComponent<BoxCollider2D>());
        }

        currentShootGap = shotgunBulletGap;
    }

    private void OnShield() {
        if (controllerInput.OnShield() == false) {
            shieldActivated = false;
        }

        if (playerID == 1) {
            if (controllerInput.OnShield() && shield.gameObject.activeSelf == false && shieldActivated == false && GameManager.instance.player1ShieldCount > 0) {
                GameManager.instance.player1ShieldCount--;
                shieldActivated = true;
                shieldBehavior.shieldTimer = shieldActivatedTimer;
            }
        }

        if (playerID == 2) {
            if (controllerInput.OnShield() && shield.gameObject.activeSelf == false && shieldActivated == false && GameManager.instance.player2ShieldCount > 0) {
                GameManager.instance.player2ShieldCount--;
                shieldActivated = true;
                shieldBehavior.shieldTimer = shieldActivatedTimer;
            }
        }

        if (shieldBehavior.shieldTimer > 0) {
            shield.gameObject.SetActive(true);
            shieldBehavior.shieldTimer -= Time.deltaTime;
            shield.color = new Color(1, 1, 1, shieldBehavior.shieldTimer/shieldActivatedTimer);

        } else {
            if (shield.gameObject.activeSelf) {
                Instantiate(shieldDeestroyFX, new Vector3(this.transform.position.x, this.transform.position.y - 1.19f, this.transform.position.z), Quaternion.identity);
            }
            shield.gameObject.SetActive(false);
            shield.color = new Color(1, 1, 1, 0);
            shieldBehavior.shieldTimer = 0;
        }
    }

    private void OnShareResources() {
        if (controllerInput.OnShareX() == 0 && controllerInput.OnShareY() == 0) {
            resourceShared = false;
        }

        //Player 1 Share Resourse to Player 2
        if (playerID == 1) {
            //Share Life
            if (GameManager.instance.player1Count > 1 && controllerInput.OnShareX() < 0 && GameManager.instance.player2Count < 3 && resourceShared == false) {
                resourceShared = true;
                if (GameManager.instance.player2Clone != null) {
                    Instantiate(powerUpFX[1], GameManager.instance.player2Clone.transform);
                }
                GameManager.instance.player1Count--;
                GameManager.instance.player2Count++;
                Debug.Log("Player 1 share 1 life to player 2");
            }

            //Share Shield
            if (GameManager.instance.player1ShieldCount > 0 && controllerInput.OnShareY() > 0 && GameManager.instance.player2ShieldCount < 3 && resourceShared == false && GameManager.instance.player2Clone != null) {
                resourceShared = true;
                GameManager.instance.player1ShieldCount--;
                GameManager.instance.player2ShieldCount++;
                Debug.Log("Player 1 share 1 shield to player 2");
            }

            //Share Laser
            if (GameManager.instance.player1LaserCount > 0 && controllerInput.OnShareX() > 0 && GameManager.instance.player2LaserCount < 3 && resourceShared == false && GameManager.instance.player2Clone != null) {
                resourceShared = true;
                Instantiate(powerUpFX[3], GameManager.instance.player2Clone.transform);
                GameManager.instance.player1LaserCount--;
                GameManager.instance.player2LaserCount++;
                Debug.Log("Player 1 share 1 laser to player2");
            }

            //Share Bomb
            if (GameManager.instance.player1BombCount > 0 && controllerInput.OnShareY() < 0 && GameManager.instance.player2BombCount < 3 && resourceShared == false && GameManager.instance.player2Clone != null) {
                resourceShared = true;
                Instantiate(powerUpFX[0], GameManager.instance.player2Clone.transform);
                GameManager.instance.player1BombCount--;
                GameManager.instance.player2BombCount++;
                Debug.Log("Player 1 share 1 bomb to player2");
            }
        }

        //Player 2 Share Resourse to Player 1
        if (playerID == 2) {
            //Share Life
            if (GameManager.instance.player2Count > 1 && controllerInput.OnShareX() < 0 && GameManager.instance.player1Count < 3 && resourceShared == false) {
                resourceShared = true;
                //Player 2 share 1 life to player 1;
                if (GameManager.instance.player1Clone != null) {
                    Instantiate(powerUpFX[1], GameManager.instance.player1Clone.transform);
                }
                GameManager.instance.player2Count--;
                GameManager.instance.player1Count++;
            }

            //Share Shield
            if (GameManager.instance.player2ShieldCount > 0 && controllerInput.OnShareY() > 0 && GameManager.instance.player1ShieldCount < 3 && resourceShared == false && GameManager.instance.player1Clone != null) {
                resourceShared = true;
                //Player 2 share 1 shield to player 1;
                GameManager.instance.player2ShieldCount--;
                GameManager.instance.player1ShieldCount++;
            }

            //Share Laser
            if (GameManager.instance.player2LaserCount > 0 && controllerInput.OnShareX() > 0 && GameManager.instance.player1LaserCount < 3 && resourceShared == false && GameManager.instance.player1Clone != null) {
                resourceShared = true;
                //Player 2 share 1 laser to player1;
                Instantiate(powerUpFX[3], GameManager.instance.player1Clone.transform);
                GameManager.instance.player2LaserCount--;
                GameManager.instance.player1LaserCount++;
            }

            //Share Bomb
            if (GameManager.instance.player2BombCount > 0 && controllerInput.OnShareY() < 0 && GameManager.instance.player1BombCount < 3 && resourceShared == false && GameManager.instance.player1Clone != null) {
                resourceShared = true;
                //Player 2 share 1 bomb to player1;
                Instantiate(powerUpFX[0], GameManager.instance.player1Clone.transform);
                GameManager.instance.player2BombCount--;
                GameManager.instance.player1BombCount++;
            }
        }
    }
}
