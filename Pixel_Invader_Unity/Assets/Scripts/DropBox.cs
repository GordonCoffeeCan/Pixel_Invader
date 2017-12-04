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
        Shotgun
    }

    [HideInInspector] public BoxType boxType;

    private BoxType[] boxTypeArray;

	// Use this for initialization
	void Start () {
        switch (Random.Range(0, 5)) {
            case 0:
                boxType = BoxType.Bomb;
                break;
            case 1:
                boxType = BoxType.Laser;
                break;
            case 2:
                boxType = BoxType.NewSpacecraft;
                break;
            case 3:
                boxType = BoxType.HeavyGun;
                break;
            case 4:
                boxType = BoxType.Shotgun;
                break;
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (this.transform.position.y < -Camera.main.orthographicSize - 1) {
            Destroy(this.gameObject);
        }
    }
}
