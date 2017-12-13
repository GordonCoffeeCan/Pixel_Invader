using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour {
    [SerializeField] private ParticleSystem explosionParticle;

    private float health = 10;

    private void OnTriggerEnter2D(Collider2D _col) {
        if(_col.tag == "Bullet") {
            health -= _col.GetComponent<Bullet>().power;
            Destroy(_col.gameObject);
        }
    }

    private void Update() {
        if (health <= 0) {
            Instantiate(explosionParticle, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}