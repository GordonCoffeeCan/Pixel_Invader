using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private float health;

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
                break;
            case EnemyType.EnemyCarrier:
                health = 10;
                break;
            case EnemyType.EnemySpawner:
                health = 20;
                break;
            case EnemyType.ArmeredEnemy:
                health = 50;
                break;
            case EnemyType.SuicideEnemy:
                health = 10;
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (health <= 0) {
            Destroy(this.gameObject);
        }
	}

    private void FixedUpdate() {
        
    }

    private void OnCollisionEnter2D(Collision2D _col) {
        Debug.Log("SomeThing hit me!");
        if(_col.gameObject.tag == "Bullet") {
            health -= _col.gameObject.GetComponent<Bullet>().power;
            Destroy(_col.gameObject);
        }
    }
}
