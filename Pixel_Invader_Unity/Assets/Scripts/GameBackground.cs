using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBackground : MonoBehaviour {

    [SerializeField] private float scrollingSpeed = 2f;
    [SerializeField] private Transform[] parallaxBackgrounds;

    private SpriteRenderer[] backgroundGraphics;
    private Vector2 drawSize = Vector2.zero;

    // Use this for initialization
    void Start () {
        drawSize = WindowSizeUtil.instance.halfWindowSize * 2;

        backgroundGraphics = this.gameObject.GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < backgroundGraphics.Length; i++) {
            backgroundGraphics[i].size = drawSize;
        }

        if (parallaxBackgrounds.Length > 0) {
            for (int i = 0; i < parallaxBackgrounds.Length; i++) {
                if(i % 2 == 0) {
                    parallaxBackgrounds[i].position = new Vector3(parallaxBackgrounds[i].position.x, WindowSizeUtil.instance.halfWindowSize.y * 2, parallaxBackgrounds[i].position.z);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (parallaxBackgrounds.Length > 0) {
            for (int i = 0; i < parallaxBackgrounds.Length; i++) {
                if(i == 0 || i == 1) {
                    parallaxBackgrounds[i].Translate(0, -scrollingSpeed * Time.deltaTime, 0);
                }

                if(i == 2 || i == 3) {
                    parallaxBackgrounds[i].Translate(0, -scrollingSpeed/2 * Time.deltaTime, 0);
                }

                if (i == 4 || i == 5) {
                    parallaxBackgrounds[i].Translate(0, -scrollingSpeed/4 * Time.deltaTime, 0);
                }
                
                if (parallaxBackgrounds[i].position.y < -WindowSizeUtil.instance.halfWindowSize.y * 2) {
                    parallaxBackgrounds[i].position = new Vector3(parallaxBackgrounds[i].position.x, WindowSizeUtil.instance.halfWindowSize.y * 2, parallaxBackgrounds[i].position.z);
                }
            }
        }
        
	}
}
