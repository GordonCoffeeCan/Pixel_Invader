using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private float health;

    private float speed = 0;

    private int score = 0;

    private Rigidbody2D rig;

    public enum EnemyType {
        RegularEnemy,
        EnemyCarrier,
        EnemySpawner,
        ArmeredEnemy,
        SuicideEnemy
    }

    public EnemyType enemyType;

	// Use this for initialization
	void Start () {
        switch (enemyType) {
            case EnemyType.RegularEnemy:
                health = 10;
                score = 10;
                break;
            case EnemyType.EnemyCarrier:
                health = 10;
                score = 60;
                break;
            case EnemyType.EnemySpawner:
                health = 20;
                score = 80;
                break;
            case EnemyType.ArmeredEnemy:
                health = 50;
                score = 120;
                break;
            case EnemyType.SuicideEnemy:
                health = 10;
                score = 75;
                break;
        }

        rig = this.GetComponent<Rigidbody2D>();
        speed = GameManager.instance.enemySpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0) {
            Destroy(this.gameObject);
        }

        if (this.transform.position.x < -4.5f) {
            this.transform.position = new Vector2(-4.5f, transform.position.y);
            if(GameManager.instance.moveDirection < 0) {
                GameManager.instance.moveDirection *= -1;
            }
        }else if(this.transform.position.x > 4.5f) {
            this.transform.position = new Vector2(4.5f, transform.position.y);
            if(GameManager.instance.moveDirection > 0) {
                GameManager.instance.moveDirection *= -1;
            }
        }
    }

    private void FixedUpdate() {
        rig.MovePosition(new Vector2(this.transform.position.x + speed * GameManager.instance.moveDirection * Time.deltaTime, this.transform.position.y)); 
    }

    private void OnCollisionEnter2D(Collision2D _col) {
        Debug.Log("SomeThing hit me!");
        if(_col.gameObject.tag == "Bullet") {
            health -= _col.gameObject.GetComponent<Bullet>().power;
            GameManager.instance.score += score;
            Destroy(_col.gameObject);
        }
    }
}
