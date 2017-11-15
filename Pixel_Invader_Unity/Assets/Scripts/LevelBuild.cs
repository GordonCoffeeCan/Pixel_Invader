using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelBuild : MonoBehaviour {
    [SerializeField] private Enemy enemy;

    private const string FILENAME = "Level_";

    private string filePath;
    private string rowContent;

    private float offSetX;
    private float offSetY;
    private float scalePosX = 0.6f;
    private float scalePosY = 1.5f;

    private GameObject levelHolder;

    private StreamReader streamReader;

    private int wave = 0;

    private float posY = 0;

	// Use this for initialization
	void Start () {
        wave = GameManager.instance.wave;
        levelHolder = GameObject.Find("LevelHolder");
        filePath = Application.dataPath + "/LevelDesign/" + FILENAME + wave + ".txt";
        streamReader = new StreamReader(filePath);

        posY = -scalePosY;

        BuildLevel();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    private void BuildLevel() {
        while (!streamReader.EndOfStream) {
            rowContent = streamReader.ReadLine();

            for (int i = 0; i < rowContent.Length; i++) {
                switch (rowContent[i]) {
                    case 'x':
                        SetEnemy(Enemy.EnemyType.RegularEnemy, new Vector2(i * scalePosX, posY));
                        break;
                }
            }

            posY += scalePosY;
        }

        offSetX = -(rowContent.Length / 2) * scalePosX;
        offSetY = Camera.main.orthographicSize + 1;
        levelHolder.transform.position = new Vector2(offSetX, offSetY);

        GameManager.instance.targetPosY = Camera.main.orthographicSize - posY;
    }

    private void SetEnemy(Enemy.EnemyType _enemyType, Vector2 _pos) {
        switch (_enemyType) {
            case Enemy.EnemyType.RegularEnemy:
                Enemy _enemyClone = Instantiate(enemy) as Enemy;
                _enemyClone.enemyType = Enemy.EnemyType.RegularEnemy;
                _enemyClone.transform.parent = levelHolder.transform;
                _enemyClone.transform.position = _pos;
                break;
        }
    }
}
