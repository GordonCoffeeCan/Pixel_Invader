using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelBuilder : MonoBehaviour {

    public static LevelBuilder instance;

    [SerializeField] private Enemy enemy;
    /*[SerializeField] private Enemy enemyCarrier;
    [SerializeField] private Enemy enemyMotherShip;
    [SerializeField] private Enemy armouredEnemy;
    [SerializeField] private Enemy suicideEnemy;*/

    private const string FILENAME = "Level_";

    private string filePath;
    private string rowContent;

    private float offSetX;
    private float offSetY;
    private float scalePosX = 0.5f;
    private float scalePosY = 0.55f;

    private StreamReader streamReader;

    private float posY = 0;

    private int id = 0;

    private void Awake() {
        instance = this;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void BuildLevel(int _wave) {
#if UNITY_STANDALONE
        filePath = Application.dataPath + "/LevelDesign/" + FILENAME + _wave + ".txt";
#endif

#if UNITY_EDITOR
        filePath = Application.dataPath + "/LevelDesign/" + FILENAME + _wave + ".txt";
#endif

#if UNITY_WEBGL
        filePath = Application.dataPath + "LevelDesign/" + FILENAME + _wave + ".txt";
#endif

        streamReader = new StreamReader(filePath);

        posY = -1;

        while (!streamReader.EndOfStream) {
            rowContent = streamReader.ReadLine();
            posY += scalePosY;
            for (int i = 0; i < rowContent.Length; i++) {
                switch (rowContent[i]) {
                    // 'e', RegularEnemy;
                    // 'c', EnemyCarrier;
                    // 'm', EnemyMotherShip;
                    // 'a', ArmeredEnemy;
                    // 's', SuicideEnemy;
                    // 'r', randam select enemy;

                    case 'e':
                        SetEnemy(Enemy.EnemyType.RegularEnemy, Enemy.MovementStyle.LeftAndRight, new Vector2(i * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                        break;
                    case 'c':
                        SetEnemy(Enemy.EnemyType.EnemyCarrier, Enemy.MovementStyle.LeftAndRight, new Vector2(i * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                        break;
                    case 'm':
                        SetEnemy(Enemy.EnemyType.EnemyMotherShip, Enemy.MovementStyle.Zigzag, new Vector2(i * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                        break;
                    case 'a':
                        SetEnemy(Enemy.EnemyType.ArmouredEnemy, Enemy.MovementStyle.LeftAndRight, new Vector2(i * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                        break;
                    case 's':
                        SetEnemy(Enemy.EnemyType.SuicideEnemy, Enemy.MovementStyle.LeftAndRight, new Vector2(i * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                        break;
                    case 'r':
                        int _index = Random.Range(0, 5);
                        Enemy.EnemyType[] _types = new Enemy.EnemyType[5];
                        _types[0] = Enemy.EnemyType.RegularEnemy;
                        _types[1] = Enemy.EnemyType.EnemyCarrier;
                        _types[2] = Enemy.EnemyType.EnemyMotherShip;
                        _types[3] = Enemy.EnemyType.ArmouredEnemy;
                        _types[4] = Enemy.EnemyType.SuicideEnemy;

                        if (_index != 2) {
                            SetEnemy(_types[_index], Enemy.MovementStyle.LeftAndRight, new Vector2(i * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                        } else {
                            SetEnemy(_types[_index], Enemy.MovementStyle.Zigzag, new Vector2(i * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                        }
                        break;
                }
            }
            GameManager.instance.currentLineEnemyDirection *= -1;
        }
    }

    private void SetEnemy(Enemy.EnemyType _enemyType, Enemy.MovementStyle _movementStyle, Vector2 _pos) {
        Enemy _enemyClone = Instantiate(enemy) as Enemy;
        _enemyClone.speed = GameManager.instance.currentWaveEnemySpeed * GameManager.instance.currentLineEnemyDirection;
        _enemyClone.verticalSpeed = GameManager.instance.currentWaveEnemySpeed;
        _enemyClone.enemyType = _enemyType;
        _enemyClone.movementStyle = _movementStyle;
        _enemyClone.gameObject.name = "Enemy " + _enemyClone.enemyType + id;
        _enemyClone.transform.position = _pos;
        GameManager.instance.enemiesCount++;
        GameManager.instance.enemyList.Add(_enemyClone);
        id++;
    }
}
