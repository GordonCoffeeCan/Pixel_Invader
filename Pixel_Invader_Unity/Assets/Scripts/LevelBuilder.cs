using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelBuilder : MonoBehaviour {

    public static LevelBuilder instance;

    [SerializeField] private Enemy enemy;
    [SerializeField] private Enemy enemyCarrier;
    [SerializeField] private Enemy enemyMotherShip;
    [SerializeField] private Enemy armouredEnemy;
    [SerializeField] private Enemy suicideEnemy;

    private const string FILENAME = "Level_";

    private string filePath;
    private string rowContent;

    private float offSetX;
    private float offSetY;
    private float scalePosX = 0.5f;
    private float scalePosY = 0.35f;

    private GameObject levelHolder;

    private StreamReader streamReader;

    private float posY = 0;

	// Use this for initialization
	void Start () {
        instance = this;
        levelHolder = GameObject.Find("LevelHolder");
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void BuildLevel(int _wave) {
        filePath = Application.dataPath + "/LevelDesign/" + FILENAME + _wave + ".txt";
        streamReader = new StreamReader(filePath);

        levelHolder.transform.position = Vector2.zero;
        posY = 0;
        while (!streamReader.EndOfStream) {
            rowContent = streamReader.ReadLine();

            for (int i = 0; i < rowContent.Length; i++) {
                switch (rowContent[i]) {
                    // 'e', RegularEnemy;
                    // 'c', EnemyCarrier;
                    // 'm', EnemyMotherShip;
                    // 'a', ArmeredEnemy;
                    // 's', SuicideEnemy;

                    case 'e':
                        SetEnemy(Enemy.EnemyType.RegularEnemy, new Vector2(i * scalePosX, posY));
                        GameManager.instance.enemiesCount++;
                        break;
                    case 'c':
                        SetEnemy(Enemy.EnemyType.EnemyCarrier, new Vector2(i * scalePosX, posY));
                        GameManager.instance.enemiesCount++;
                        break;
                    case 'm':
                        SetEnemy(Enemy.EnemyType.EnemyMotherShip, new Vector2(i * scalePosX, posY));
                        GameManager.instance.enemiesCount++;
                        break;
                    case 'a':
                        SetEnemy(Enemy.EnemyType.ArmouredEnemy, new Vector2(i * scalePosX, posY));
                        GameManager.instance.enemiesCount++;
                        break;
                    case 's':
                        SetEnemy(Enemy.EnemyType.SuicideEnemy, new Vector2(i * scalePosX, posY));
                        GameManager.instance.enemiesCount++;
                        break;
                }
            }

            posY += scalePosY;
        }

        offSetX = -(rowContent.Length / 2) * scalePosX;
        offSetY = Camera.main.orthographicSize + 3;
        levelHolder.transform.position = new Vector2(offSetX, offSetY);

        GameManager.instance.targetPosY = Camera.main.orthographicSize - posY;
    }

    private void SetEnemy(Enemy.EnemyType _enemyType, Vector2 _pos) {
        switch (_enemyType) {
            case Enemy.EnemyType.RegularEnemy:
                Enemy _enemyClone = Instantiate(enemy) as Enemy;
                _enemyClone.transform.parent = levelHolder.transform;
                _enemyClone.transform.position = _pos;
                break;
            case Enemy.EnemyType.EnemyCarrier:
                Enemy _enemyCarrierClone = Instantiate(enemyCarrier) as Enemy;
                _enemyCarrierClone.transform.parent = levelHolder.transform;
                _enemyCarrierClone.transform.position = _pos;
                break;
            case Enemy.EnemyType.EnemyMotherShip:
                Enemy _enemyMotherShipClone = Instantiate(enemyMotherShip) as Enemy;
                _enemyMotherShipClone.transform.parent = levelHolder.transform;
                _enemyMotherShipClone.transform.position = _pos;
                break;
            case Enemy.EnemyType.ArmouredEnemy:
                Enemy _enemyArmouredClone = Instantiate(armouredEnemy) as Enemy;
                _enemyArmouredClone.transform.parent = levelHolder.transform;
                _enemyArmouredClone.transform.position = _pos;
                break;
            case Enemy.EnemyType.SuicideEnemy:
                Enemy _enemySuicideClone = Instantiate(suicideEnemy) as Enemy;
                _enemySuicideClone.transform.parent = levelHolder.transform;
                _enemySuicideClone.transform.position = _pos;
                break;
        }
    }
}
