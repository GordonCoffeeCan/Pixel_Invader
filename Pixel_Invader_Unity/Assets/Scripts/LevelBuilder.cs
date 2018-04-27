using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelBuilder : MonoBehaviour {

    public static LevelBuilder instance;

    [SerializeField] private Enemy enemy;

    private const string FILENAME = "Level_";

    private string filePath;
    private string rowContent = "";

    private float offSetX;
    private float offSetY;
    private float scalePosX = 0.5f;
    private float scalePosY = 0.55f;

    private StreamReader streamReader;

    private float posY = 0;

    private int id = 0;

    //Built-In Level Content-------------------------------------------------
    private string level_0 = "e-e-e-e-e-e-eN";
    private string level_1 = "e-e-e-e-e-eN-e-e-e-e-e-Ne-e-e-e-e-eN";
    private string level_2 = "----e-e----N---e-e-e---N--e-e-e-e--N-e-e-e-e-e-Ne-e-e-e-e-eN";
    private string level_3 = "---e-e-e-e---N--e-e-e-e-e--N-e-e-e-e-e-e-Ne-e-e-e-e-e-eNe-e-e-c-e-e-eNe-e-e-e-e-e-eN-e-e-e-e-e-e-N--e-e-e-e-e--N---e-e-e-e---N";
    private string level_4 = "-e---e-e-e---e-N-e---e-e-e---e-N-e-e-e-m-e-e-e-N-e-e-e-e-e-e-e-N-e-e-e-e-e-e-e-N-e-e-e-e-e-e-e-N-e-e-e-m-e-e-e-N-e---e-e-e---e-N-e---e-e-e---e-N";
    private string level_5 = "-e-e-e-e-e-e-e-N-e-e-e-e-e-e-e-N-e-e-e-a-e-e-e-N-e-e-a-m-a-e-e-N-e-e-e-a-e-e-e-N-e-e-e-e-e-e-e-N-e-a-e-a-e-a-e-N";
    private string level_6 = "-----e-e-e-----N---e-e-e-e-e---N-e-m-e-m-e-m-e-N-e-e-a-c-a-e-e-N-e-s-a-a-a-s-e-N---e-e-e-e-e---N-----e-e-e-----N";
    private string level_7 = "-e-e-e-s-e-e-e-N-e-e-e-e-e-e-e-N-e-e-s-e-s-e-e-N-e-e-a-e-a-e-e-N-e-e-e-e-e-e-e-N-e-s-e-e-e-s-e-N";
    private string level_8 = "-e-e-e-e-e-e-e-N-e-e-s-m-s-e-e-N-e-e-e-a-e-e-e-N-e-m-c-e-c-m-e-N-e-a-e-e-e-a-e-N-e-e-e-m-e-s-e-N-e-e-s-a-s-e-e-N";
    private string level_9 = "-e-e-e---e-e-e-N-s-m-s-e-s-m-s-N-e-a-e---e-a-e-N---e-c-m-c-e---N-e-e-e---e-e-e-N-a-c-a-e-a-c-a-N-e-a-e---e-a-e-N";
    private string level_10 = "---e-e-e-e-e---N-s-e-s-e-s-e-s-N-e-a-r-a-r-a-e--m-s-a-c-a-s-m-Ne-e-e-a-e-e-e-N-a-c-s-e-s-c-a-N---e-a-e-a-e---N";
    private string level_11 = "-r-e-e-e-e-e-r-N-s-e-c-e-s-c-s-N-e-a-r-a-r-a-e-N-e-r-a-r-a-r-e-N-e-e-e-a-e-e-e-N-a-r-s-r-s-r-a-N-e-e-r-s-r-e-e-N";
    private string level_12 = "-e-r-a-e-a-r-e-N-a-r-c-r-s-c-a-N-r-e-r-c-r-e-r-N-e-r-e-m-e-r-e-N-e-e-e-a-e-e-e-N-a-r-a-r-a-r-a-N-e-e-r-a-r-e-e-N";
    private string level_13 = "---r-r-e-r-r---N-e-r-c-m-c-r-e-N-r-e-r-c-r-e-r-N-r-r---r---r-r-N-e-r-e-m-e-r-e-N-r-r-r-c-r-r-r-N---e-r-a-r-e---N";
    private string level_14 = "-r-r-r-e-r-r-r-N---r--r-r--r---N-r-e-r-c-r-e-r-N-r-r-r-r-r-r-r-N-e-r-r-r-r-r-e-N-----r-c-r-----N-r-a-r-e-r-a-r-N";
    private string level_15 = "-r-r-r---r-r-r-N-r-c-r---r-c-r-N-r-r-r-a-r-r-r-N--r--a-r-a--r--N-r-r-r-a-r-r-r-N-r-c-r---r-c-r-N-r-r-r---r-r-r-N";
    private string level_16 = "-r-r-e-r-e-r-r-N--r--r-r-r--r--N-r-r-a-m-a-r-r-N--r--m-c-m--r--N-r-r-r-m-r-r-r-N--c-r-r-r-c-r--N-r-r-e-r-e-r-r-N";
    private string level_17 = "-e-e-r-r-r-e-e-N-c-r-r-r-r-r-c-N-r-r-r-a-r-r-r-N-r-r-r-m-r-r-r-N-e-r-r-a-r-r-e-N-e-e-r-r-r-e-e-N-e-e-e-a-e-e-e-N";
    private string level_18 = "-r-e-r-r-r-e-r-N-e-r-c-r-c-r-e-N-r-r-a-e-a-r-r-N-a---a-r-a---a-N-e-r-a-e-a-r-e-N-e-e-r---r-e-e-N-e-r-r-e-r-r-e-N";
    private string level_19 = "-a-a-a-a-a-a-a-N-r-c-r-c-r-c-r-N-m-m-m-m-m-m-m-N-c-c-c-c-c-c-c-N-r-r-r-r-r-r-r-N-r-r-r-r-r-r-r-N-a-a-a-a-a-a-a-N";

    private List<string> levels = new List<string>();
    //Built-In Level Content-------------------------------------------------

    private void Awake() {
        instance = this;

        levels.Add(level_0);
        levels.Add(level_1);
        levels.Add(level_2);
        levels.Add(level_3);
        levels.Add(level_4);
        levels.Add(level_5);
        levels.Add(level_6);
        levels.Add(level_7);
        levels.Add(level_8);
        levels.Add(level_9);
        levels.Add(level_10);
        levels.Add(level_11);
        levels.Add(level_12);
        levels.Add(level_13);
        levels.Add(level_14);
        levels.Add(level_15);
        levels.Add(level_16);
        levels.Add(level_17);
        levels.Add(level_18);
        levels.Add(level_19);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void BuildLevel(int _wave) {
        posY = -1;

#if UNITY_EDITOR
        BuildStandaloneLevel(_wave);
#elif UNITY_STANDALONE
        BuildStandaloneLevel(_wave);
#endif

#if UNITY_WEBGL
        BuildWebLevel(_wave);
#endif

    }

    private void BuildStandaloneLevel(int _wave) {
        filePath = Application.dataPath + "/LevelDesign/" + FILENAME + _wave + ".txt";
        streamReader = new StreamReader(filePath);
        //Standalone Read Level----------------------------------------------
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
        //Standalone Read Level----------------------------------------------
    }

    private void BuildWebLevel(int _wave) {
        for (int i = 0; i < levels[_wave].Length; i++) {
            if (levels[_wave][i].ToString() == "N") {
                posY += scalePosY;
                for (int j = 0; j < rowContent.Length; j++) {
                    switch (rowContent[j]) {
                        // 'e', RegularEnemy;
                        // 'c', EnemyCarrier;
                        // 'm', EnemyMotherShip;
                        // 'a', ArmeredEnemy;
                        // 's', SuicideEnemy;
                        // 'r', randam select enemy;

                        case 'e':
                            SetEnemy(Enemy.EnemyType.RegularEnemy, Enemy.MovementStyle.LeftAndRight, new Vector2(j * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                            break;
                        case 'c':
                            SetEnemy(Enemy.EnemyType.EnemyCarrier, Enemy.MovementStyle.LeftAndRight, new Vector2(j * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                            break;
                        case 'm':
                            SetEnemy(Enemy.EnemyType.EnemyMotherShip, Enemy.MovementStyle.Zigzag, new Vector2(j * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                            break;
                        case 'a':
                            SetEnemy(Enemy.EnemyType.ArmouredEnemy, Enemy.MovementStyle.LeftAndRight, new Vector2(j * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                            break;
                        case 's':
                            SetEnemy(Enemy.EnemyType.SuicideEnemy, Enemy.MovementStyle.LeftAndRight, new Vector2(j * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
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
                                SetEnemy(_types[_index], Enemy.MovementStyle.LeftAndRight, new Vector2(j * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                            } else {
                                SetEnemy(_types[_index], Enemy.MovementStyle.Zigzag, new Vector2(j * scalePosX - (rowContent.Length / 2) * scalePosX, Camera.main.orthographicSize - posY));
                            }
                            break;
                    }
                }
                GameManager.instance.currentLineEnemyDirection *= -1;
                rowContent = "";
            } else {
                rowContent += levels[_wave][i].ToString();
            }
        }

        rowContent = "";
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
