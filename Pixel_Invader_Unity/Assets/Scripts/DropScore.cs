﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScore : MonoBehaviour {

    public float destroyDelay = 2;
    public float fadeOutSpeed = 0.03f;
    public Color32 scoreColor = Color.white;

    [HideInInspector] public float score = 0;

    private TextMesh textMesh;
    private Rigidbody2D rig;
    private float alpha = 1;
    private float currentFadeOutSpeed = 0;

    private void Awake() {
        rig = this.GetComponent<Rigidbody2D>();
        textMesh = this.GetComponent<TextMesh>();
    }

    // Use this for initialization
    void Start () {
        textMesh.text = "+" + score.ToString();
        Vector2 force = new Vector2(Random.Range(Random.Range(-2f, -1f), Random.Range(1f, 2f)), Random.Range(2.5f, 3.2f));
        rig.AddForce(force, ForceMode2D.Impulse);
        Destroy(this.gameObject, destroyDelay);
    }
	
	// Update is called once per frame
	void Update () {
        textMesh.text = "+" + score.ToString();
        textMesh.color = scoreColor;
        currentFadeOutSpeed = Mathf.Lerp(currentFadeOutSpeed, fadeOutSpeed, 0.06f);
        alpha = Mathf.MoveTowards(alpha, 0, currentFadeOutSpeed);
        //textMesh.color = new Color(1, 1, 1, alpha);
        this.transform.localScale = new Vector3(alpha, alpha, 1);
	}
}
