using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrbs : MonoBehaviour {

    [SerializeField] private float showTrailTimer = 0.25f;
    [SerializeField] private TrailRenderer[] orbTrails;

    private float speed = 420;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < orbTrails.Length; i++) {
            orbTrails[i].enabled = false;
        }

    }
	
	// Update is called once per frame
	void Update () {
        showTrailTimer -= Time.deltaTime;

        if (showTrailTimer <= 0) {
            for (int i = 0; i < orbTrails.Length; i++) {
                if (orbTrails[i] != null) {
                    orbTrails[i].enabled = true;
                }
                
            }

            showTrailTimer = 0;
        }

        this.transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
