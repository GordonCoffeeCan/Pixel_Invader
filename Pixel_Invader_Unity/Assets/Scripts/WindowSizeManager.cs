using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSizeManager : MonoBehaviour {

    public static WindowSizeManager instance;

    [HideInInspector] public Vector2 halfWindowSize = Vector2.zero;

    private void Awake() {
        instance = this;
        halfWindowSize = Resize();
    }

    private Vector2 Resize() {
        Vector2 _halfWindowSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        return _halfWindowSize;
    }
}
