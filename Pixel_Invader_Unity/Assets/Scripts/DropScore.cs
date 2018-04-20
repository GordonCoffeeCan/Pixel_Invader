using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScore : MonoBehaviour {

    public float destroyDelay = 2;
    public float scaleDownSpeed = 0.02f;

    [HideInInspector] public float score = 0;

    private TextMesh textMesh;
    private Rigidbody2D rig;
    private float alpha = 1;

    private void Awake() {
        rig = this.GetComponent<Rigidbody2D>();
        textMesh = this.GetComponent<TextMesh>();
    }

    // Use this for initialization
    void Start () {
        textMesh.text = "+" + score.ToString();
        Vector2 force = new Vector2(Random.Range(Random.Range(-1f, -0.5f), Random.Range(0.5f, 1f)), Random.Range(2f, 3f));
        rig.AddForce(force, ForceMode2D.Impulse);
        Destroy(this.gameObject, destroyDelay);
    }
	
	// Update is called once per frame
	void Update () {
        textMesh.text = "+" + score.ToString();
        alpha = Mathf.MoveTowards(alpha, 0, scaleDownSpeed);
        textMesh.color = new Color(1, 1, 1, alpha);
	}
}
