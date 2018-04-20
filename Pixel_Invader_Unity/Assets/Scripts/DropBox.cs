using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBox : MonoBehaviour {

    [SerializeField] Sprite[] sprites;

    [HideInInspector] public enum BoxType {
        Bomb,
        Laser,
        NewSpacecraft,
        HeavyGun,
        Shotgun,
        Shield
    }

    [HideInInspector] public BoxType boxType;

    private BoxType[] boxTypeArray;

    private SpriteRenderer spriteRender;
    private BoxCollider2D boxCollider;

	// Use this for initialization
	void Start () {
        spriteRender = this.GetComponent<SpriteRenderer>();
        boxCollider = this.GetComponent<BoxCollider2D>();

        //Randomly select drop box item;
        int _boxItemID = Random.Range(0, 6);
        boxType = (BoxType)_boxItemID;
        spriteRender.sprite = sprites[_boxItemID];
        //Randomly select drop box item;

        boxCollider.size = new Vector2(spriteRender.sprite.rect.size.x / spriteRender.sprite.pixelsPerUnit, spriteRender.sprite.rect.size.y / spriteRender.sprite.pixelsPerUnit);
    }
	
	// Update is called once per frame
	void Update () {
        if (this.transform.position.y < -Camera.main.orthographicSize - 1) {
            Destroy(this.gameObject);
        }
    }
}
