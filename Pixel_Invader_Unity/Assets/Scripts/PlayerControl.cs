using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    [SerializeField] private float speed = 7;
    [SerializeField] private float screenEdge = 4.5f;
    [SerializeField] private float bulletPower = 10;
    [SerializeField] private float shootGap = 0.35f;
    [SerializeField] private Bullet bullet;
    [SerializeField] private Bullet bomb;

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

        if (Input.GetButton("Fire") && currentShootGap <= 0) {
            Bullet _bulletClone = Instantiate(bullet, this.transform.position + Vector3.forward * 0.1f + Vector3.up * 0.25f, Quaternion.identity) as Bullet;
            _bulletClone.power = bulletPower;
            _bulletClone.bulletType = Bullet.BulletType.PlayerBullet;
            currentShootGap = shootGap;

            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), _bulletClone.GetComponent<BoxCollider2D>());
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
            Destroy(this.gameObject);
        }
    }
}
