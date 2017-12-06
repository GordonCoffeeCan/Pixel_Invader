using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, this.GetComponent<ParticleSystem>().main.duration + 5);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
