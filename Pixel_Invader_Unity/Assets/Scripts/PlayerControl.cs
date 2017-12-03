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
    [SerializeField] private Bullet bullet;
    [SerializeField] private Bullet bomb;
    [SerializeField] private ParticleSystem playerExplosionFX;
    [SerializeField] private GameObject[] weapons;

    [HideInInspector] public enum GunType {
        RegularGun,
        HeavyGun,
        ShotGun
    }

    [HideInInspector] public GunType gunType;

    private Rigidbody2D rig;

    private float currentShootGap = 0;

    private Animator playerAnim;

	// Use this for initialization
	void Start () {
        rig = this.GetComponent<Rigidbody2D>();
        playerAnim = this.GetComponent<Animator>();
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
            gunType = GunType.RegularGun;
            bulletPower = 10;
            for (int i = 0; i < weapons.Length; i++) {
                weapons[i].SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            gunType = GunType.HeavyGun;
            bulletPower = 5;
            weapons[0].SetActive(true);
            for (int i = 0; i < weapons.Length; i++) {
                weapons[i].SetActive(false);
                if(i == 0) {
                    weapons[i].SetActive(true);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            gunType = GunType.ShotGun;
            bulletPower = 3;
            weapons[1].SetActive(true);
            for (int i = 0; i < weapons.Length; i++) {
                weapons[i].SetActive(false);
                if (i == 1) {
                    weapons[i].SetActive(true);
                }
            }
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

        if (Input.GetButtonDown("Bomb")) {
            Bullet _bombClone = Instantiate(bomb, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
            _bombClone.power = 100; ;
            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bombClone.GetComponent<BoxCollider2D>());
        }
	}

    private void FixedUpdate() {
        rig.MovePosition(new Vector2(this.transform.position.x + speed * Input.GetAxis("Horizontal") * Time.deltaTime, this.transform.position.y));
        playerAnim.SetFloat("Speed", Input.GetAxis("Horizontal"));
    }

    private void OnTriggerEnter2D(Collider2D _col) {
        if(_col.tag == "EnemyBullet") {
            Destroy(_col.gameObject);
            Instantiate(playerExplosionFX, this.transform.position, Quaternion.identity);
            GameManager.instance.cameraShakeAmount += 0.35f;
            //Destroy(this.gameObject);
        }
    }

    private void CreateRegularBullet() {
        Bullet _bulletClone = Instantiate(bullet, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
        _bulletClone.power = bulletPower;
        _bulletClone.bulletType = Bullet.BulletType.PlayerBullet;
        currentShootGap = bulletGap;
        _bulletClone.soundFXSource.clip = _bulletClone.bulletSoundFXes[0];
        Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bulletClone.GetComponent<BoxCollider2D>());
    }

    private void CreateTripleBullet() {
        for (int i = 0; i < 3; i++) {
            Vector3 _pos = new Vector2((this.transform.position.x - 0.15f) + 0.15f * Random.Range(0, 3), this.transform.position.y + (i % 2) * 0.2f);
            Bullet _bulletClone = Instantiate(bullet, _pos + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
            _bulletClone.transform.parent = this.transform;
            _bulletClone.power = bulletPower;
            _bulletClone.bulletType = Bullet.BulletType.PlayerBullet;
            _bulletClone.showObjecDelay = i * 4.5f;
            _bulletClone.soundFXSource.clip = _bulletClone.bulletSoundFXes[0];
            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bulletClone.GetComponent<BoxCollider2D>());
        }

        currentShootGap = tripleBulletGap;
    }

    private void CreateShotGunBullet() {
        for (int i = 0; i < 8; i++) {
            float _angle = 35 - 10 * i;
            Bullet _bulletClone = Instantiate(bullet, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.Euler(new Vector3(0, 0, _angle))) as Bullet;
            _bulletClone.transform.parent = this.transform;
            _bulletClone.power = bulletPower;
            _bulletClone.bulletType = Bullet.BulletType.PlayerBullet;
            if(i > 0) {
                _bulletClone.soundFXPlayable = false;
            }
            _bulletClone.soundFXSource.clip = _bulletClone.bulletSoundFXes[2];
            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bulletClone.GetComponent<BoxCollider2D>());
        }

        currentShootGap = shotgunBulletGap;
    }
}
