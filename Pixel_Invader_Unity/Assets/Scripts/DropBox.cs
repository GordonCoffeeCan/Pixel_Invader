﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBox : MonoBehaviour {

    [SerializeField] Sprite[] sprites;

    [HideInInspector] public enum BoxType {
        Bomb,
        Laser,
        NewSpacecraft,
        HeavyGun,
        Shotgun
    }

    [HideInInspector] public BoxType boxType;

    private BoxType[] boxTypeArray;

    private SpriteRenderer spriteRender;
    private BoxCollider2D boxCollider;

	// Use this for initialization
	void Start () {
        spriteRender = this.GetComponent<SpriteRenderer>();
        boxCollider = this.GetComponent<BoxCollider2D>();
        switch (Random.Range(0, 5)) {
            case 0:
                boxType = BoxType.Bomb;
                spriteRender.sprite = sprites[0];
                break;
            case 1:
                boxType = BoxType.Laser;
                spriteRender.sprite = sprites[1];
                break;
            case 2:
                boxType = BoxType.NewSpacecraft;
                spriteRender.sprite = sprites[2];
                break;
            case 3:
                boxType = BoxType.HeavyGun;
                spriteRender.sprite = sprites[3];
                break;
            case 4:
                boxType = BoxType.Shotgun;
                spriteRender.sprite = sprites[4];
                break;
        }

        boxCollider.size = new Vector2(spriteRender.sprite.rect.size.x / spriteRender.sprite.pixelsPerUnit, spriteRender.sprite.rect.size.y / spriteRender.sprite.pixelsPerUnit);
    }
	
	// Update is called once per frame
	void Update () {
        if (this.transform.position.y < -Camera.main.orthographicSize - 1) {
            Destroy(this.gameObject);
        }
    }
}
