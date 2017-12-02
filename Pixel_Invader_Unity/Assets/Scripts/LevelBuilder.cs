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

	// Use this for initialization
	void Start () {
        instance = this;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void BuildLevel(int _wave) {
        filePath = Application.dataPath + "/LevelDesign/" + FILENAME + _wave + ".txt";
        streamReader = new StreamReader(filePath);

        posY = 0;

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

                    case 'e':
                        SetEnemy(Enemy.EnemyType.RegularEnemy, Enemy.MovementStyle.LeftAndRight, new Vector2(i * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                        GameManager.instance.enemiesCount++;
                        break;
                    case 'c':
                        SetEnemy(Enemy.EnemyType.EnemyCarrier, Enemy.MovementStyle.LeftAndRight, new Vector2(i * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                        GameManager.instance.enemiesCount++;
                        break;
                    case 'm':
                        SetEnemy(Enemy.EnemyType.EnemyMotherShip, Enemy.MovementStyle.Zigzag, new Vector2(i * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                        GameManager.instance.enemiesCount++;
                        break;
                    case 'a':
                        SetEnemy(Enemy.EnemyType.ArmouredEnemy, Enemy.MovementStyle.LeftAndRight, new Vector2(i * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                        GameManager.instance.enemiesCount++;
                        break;
                    case 's':
                        SetEnemy(Enemy.EnemyType.SuicideEnemy, Enemy.MovementStyle.LeftAndRight, new Vector2(i * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                        GameManager.instance.enemiesCount++;
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
        GameManager.instance.enemyList.Add(_enemyClone);
        id++;
    }
}
