using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    [HideInInspector] public float shieldTimer = 0;

    private void OnTriggerEnter2D(Collider2D _col) {
        if (_col.tag == "EnemyBullet") {
            Bullet _bullet = _col.gameObject.GetComponent<Bullet>();
            _bullet.hitObject = true;
            Destroy(_col.gameObject);
            GameManager.instance.cameraShakeAmount += 0.35f;
            shieldTimer -= 3f;
        }

        if (_col.tag == "Enemy") {
            Enemy _enemy = _col.GetComponent<Enemy>();
            if (_enemy.enemyType == Enemy.EnemyType.SuicideEnemy) {
                _enemy.health -= 100;
                GameManager.instance.cameraShakeAmount += 0.35f;
                shieldTimer -= 5f;
            }
        }
    }
}
