using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    [SerializeField] private float speed = 7;
    [SerializeField] private float screenEdge = 4.5f;
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

	// Use this for initialization
	void Start () {
        rig = this.GetComponent<Rigidbody2D>();
        playerAnim = this.GetComponent<Animator>();
        Instantiate(muteFX, this.transform.position + Vector3.down * 0.06f, Quaternion.identity, this.transform);
        muteForBulletTime = 2.5f;
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

        //Only for development------------------------------------
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            OnRegularGun();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            OnHeavyGun();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            OnShotGun();
        }
        //Only for development------------------------------------

        if (Input.GetButton("Fire") && currentShootGap <= 0) {
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

        if (Input.GetButtonDown("Bomb") && GameManager.instance.bombCount > 0) {
            Bullet _bombClone = Instantiate(bomb, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
            _bombClone.dir = 1;
            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bombClone.GetComponent<BoxCollider2D>());
            GameManager.instance.bombCount--;
        }

        if (Input.GetButtonDown("Laser") && GameManager.instance.laserCount > 0) {
            Bullet _laserClone = Instantiate(laser, this.transform.position + Vector3.forward * 0.1f, Quaternion.identity, this.transform) as Bullet;
            Debug.Log(_laserClone.transform.parent);
            _laserClone.power = 20;
            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _laserClone.GetComponent<BoxCollider2D>());
            GameManager.instance.laserCount--;
        }

        muteForBulletTime -= Time.deltaTime;

        if (muteForBulletTime <= 0) {
            muteForBulletTime = 0;
        }
	}

    private void FixedUpdate() {
        rig.MovePosition(new Vector2(this.transform.position.x + speed * Input.GetAxis("Horizontal") * Time.deltaTime, this.transform.position.y));
        playerAnim.SetFloat("Speed", Input.GetAxis("Horizontal"));
    }

    private void OnTriggerEnter2D(Collider2D _col) {
        if (_col.tag == "EnemyBullet") {
            Destroy(_col.gameObject);
            Instantiate(playerExplosionFX, this.transform.position, Quaternion.identity);
            GameManager.instance.cameraShakeAmount += 0.35f;
            if (muteForBulletTime <= 0) {
                OnRegularGun();
                GameManager.instance.playerIsDead = true;
                GameManager.instance.playerCount--;
                Destroy(this.gameObject);
            }
        }

        if (_col.tag == "DropBox") {
            DropBox _dropBox = _col.GetComponent<DropBox>();
            switch (_dropBox.boxType) {
                case DropBox.BoxType.Bomb:
                    Debug.Log("Bomb ++");
                    Instantiate(powerUpFX[0], this.transform);
                    if (GameManager.instance.bombCount < 3) {
                        GameManager.instance.bombCount++;
                    }
                    break;
                case DropBox.BoxType.HeavyGun:
                    OnHeavyGun();
                    break;
                case DropBox.BoxType.Laser:
                    Debug.Log("Laser ++");
                    Instantiate(powerUpFX[3], this.transform);
                    if (GameManager.instance.laserCount < 3) {
                        GameManager.instance.laserCount++;
                    }
                    break;
                case DropBox.BoxType.NewSpacecraft:
                    Instantiate(powerUpFX[1], this.transform);
                    Debug.Log("Health ++");
                    if (GameManager.instance.playerCount < 3) {
                        GameManager.instance.playerCount++;
                    }
                    break;
                case DropBox.BoxType.Shotgun:
                    OnShotGun();
                    break;
            }
            Destroy(_col.gameObject);
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
        bulletPower = 3;
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
            _bulletClone.showObjecDelay = i * 4.5f;
            _bulletClone.dir = 1;
            _bulletClone.soundFXSource.clip = _bulletClone.bulletSoundFXes[0];
            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bulletClone.GetComponent<BoxCollider2D>());
        }
        currentShootGap = tripleBulletGap;
    }

    private void CreateShotGunBullet() {
        for (int i = 0; i < 8; i++) {
            float _angle = 35 - 10 * i;
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
