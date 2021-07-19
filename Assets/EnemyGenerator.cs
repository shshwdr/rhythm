
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public float generateTime = 1;
    float generateTimer = 0;
    public float generateBossTime = 20;
    float generateBossTimer = 0;
    PlayerController playerController;
    public List<float> difficultyToGenerateTime = new List<float>()
    {
        1.3f,
        1f,
        0.9f,
        0.7f,
        0.6f,
        0.5f
    };
    public int currentDifficulty = 0;
    public List<float> difficultyToLevel = new List<float>()
    {
        200,
        600,
        1200,
        2200,
        3500,
        5000,

    };

    public List<int> difficultyToMaxEnemyType = new List<int>()
    {
        1,2,3,4,5
    };

    public List<int> difficultyToHPMultiplier = new List<int>()
    {
        1,1,1,1,1,2,2,3,3,4
    };

    public int bossShowLevel = 2;
    public int showMoveLevel = 1;

    public float coinRate = 0.1f;
    public float abilityRate = 0.05f;

    public float horizontalGenerateWidth = 20;
    public float horizontalGenerateHeight = 40;


    public float verticalGenerateHeight = 40;
    public float verticalGenerateWidth = 40;

    int maxBossCount = 1;
    int currentBossCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        generateTimer = difficultyToGenerateTime[currentDifficulty];
        EventPool.OptIn(EventPool.stopGameEvent, resetAll);
    }

    void resetAll()
    {
        currentBossCount = 0;
        currentDifficulty = 0;
        generateTimer = 0;
        generateBossTimer = 0;
    }

    void generateVertialMovingEnemy()
    {
        int enemyCount = Random.Range(4, 7);

        float x = transform.position.x + Random.Range(-verticalGenerateWidth, verticalGenerateWidth);
        float y = transform.position.y + horizontalGenerateHeight;


        Vector3 position = new Vector3(x, y, transform.position.z);
        Vector3 positionDir = new Vector3(0, -1, 0);

        List<string> walkingEnemyNames = new List<string>() { "fire1", "mushroom1", "zombie1" };
        var name = walkingEnemyNames[Random.Range(0, walkingEnemyNames.Count)];
        var generatedEnemies = GroupEnemyGenerator.generateEnemyGroup(name, enemyCount, Utils.randomEnumValue<EnemyGroupShape>(), position, positionDir, transform);

        //GameObject walkingEnemy = Instantiate(walkingEnemyPrefab, position, Quaternion.identity);
        foreach (var walkingEnemy in generatedEnemies)
        {
            addDrops(walkingEnemy, positionDir);
        }
    }

    void addDrops(GameObject walkingEnemy, Vector3 positionDir)
    {
        var script = walkingEnemy.GetComponent<EnemyController>();
        bool isCoin = Random.Range(0f, 1f) < coinRate;
        bool isAbility = !isCoin && Random.Range(0f, 1f) < abilityRate;
        script.init(positionDir, isCoin, isAbility,false, difficultyToHPMultiplier[currentDifficulty]);
    }

    void addBossDrops(GameObject walkingEnemy, Vector3 positionDir)
    {
        var script = walkingEnemy.GetComponent<EnemyController>();
        bool isCoin = Random.Range(0f, 1f) < coinRate;
        bool isAbility = !isCoin && Random.Range(0f, 1f) < abilityRate;
        script.init(positionDir, isCoin, isAbility, true,difficultyToHPMultiplier[currentDifficulty]);
    }


    void generateHorizontalMovingEnemy()
    {
        int enemyCount = Random.Range(4, 7);

        bool isOnRight = Utils.randomBool();
        float x = transform.position.x + horizontalGenerateWidth * (isOnRight ? 1:-1);
        float y = transform.position.y + Random.Range(horizontalGenerateHeight/3f, horizontalGenerateHeight);


        Vector3 position = new Vector3(x, y, transform.position.z);
        Vector3 positionDir = new Vector3(isOnRight ? -1 : 1, 0, 0);

        List<string> walkingEnemyNames = new List<string>() { "fire1","mushroom1","zombie1" };
        var name = walkingEnemyNames[Random.Range(0,walkingEnemyNames.Count)];

        var generatedEnemies = GroupEnemyGenerator.generateEnemyGroup(name, enemyCount, Utils.randomEnumValue< EnemyGroupShape>(), position, positionDir, transform);

        //GameObject walkingEnemy = Instantiate(walkingEnemyPrefab, position, Quaternion.identity);
        foreach (var walkingEnemy in generatedEnemies)
        {
            addDrops(walkingEnemy, positionDir);
        }

    }

    void generateObstacle()
    {
        int enemyCount = Random.Range(4, 7);

        bool isOnRight = Utils.randomBool();

        float x = transform.position.x + Random.Range(-verticalGenerateWidth, verticalGenerateWidth);
        float y = transform.position.y + horizontalGenerateHeight;


        Vector3 position = new Vector3(x, y, transform.position.z);
        Vector3 positionDir = new Vector3(1, 0, 0);

        List<string> walkingEnemyNames = new List<string>() { "colliderTrunk" };
        var name = walkingEnemyNames[Random.Range(0, walkingEnemyNames.Count)];

        var generatedEnemies = GroupEnemyGenerator.generateEnemyGroup(name, enemyCount, Utils.randomEnumValue<EnemyGroupShape>(), position, positionDir, transform,2,true);

        //GameObject walkingEnemy = Instantiate(walkingEnemyPrefab, position, Quaternion.identity);
        foreach (var walkingEnemy in generatedEnemies)
        {
           // walkingEnemy.transform.position = walkingEnemy.transform.position / 5;
        }

    }


    void generateHorizontalShootingEnemy()
    {
        int enemyCount = Random.Range(1,3);

        float x = transform.position.x + Random.Range(-verticalGenerateWidth, verticalGenerateWidth);
        float y = transform.position.y + horizontalGenerateHeight;


        Vector3 position = new Vector3(x, y, transform.position.z);
        Vector3 positionDir = new Vector3(0, -1, 0);

        List<string> walkingEnemyNames = new List<string>() { "fire2", "mushroom2", "zombie2" };
        var name = walkingEnemyNames[Random.Range(0, walkingEnemyNames.Count)];
        var generatedEnemies = GroupEnemyGenerator.generateEnemyGroup(name, enemyCount, Utils.randomEnumValue<EnemyGroupShape>(), position, positionDir, transform);

        //GameObject walkingEnemy = Instantiate(walkingEnemyPrefab, position, Quaternion.identity);
        foreach (var walkingEnemy in generatedEnemies)
        {
            addDrops(walkingEnemy, positionDir);
            //walkingEnemy.GetComponent<EnemyShooter>().isHorizontal = true;
        }

    }

    void generateVerticalShootingEnemy()
    {
        int enemyCount = Random.Range(1, 3);

        float x = transform.position.x + Random.Range(-verticalGenerateWidth, verticalGenerateWidth);
        float y = transform.position.y + horizontalGenerateHeight;


        Vector3 position = new Vector3(x, y, transform.position.z);
        Vector3 positionDir = new Vector3(1,0, 0);

        List<string> walkingEnemyNames = new List<string>() { "fire2", "mushroom2", "zombie2" };
        var name = walkingEnemyNames[Random.Range(0, walkingEnemyNames.Count)];
        var generatedEnemies = GroupEnemyGenerator.generateEnemyGroup(name, enemyCount, Utils.randomEnumValue<EnemyGroupShape>(), position, positionDir, transform);

        //GameObject walkingEnemy = Instantiate(walkingEnemyPrefab, position, Quaternion.identity);
        foreach (var walkingEnemy in generatedEnemies)
        {
            addDrops(walkingEnemy, positionDir);
            //walkingEnemy.GetComponent<EnemyShooter>().isHorizontal = false;

            //walkingEnemy.GetComponent<EnemyShooter>().shootTime = 0.5f;
        }

    }

    void generateEnemy()
    {
        var randomType = Random.Range(0, difficultyToMaxEnemyType[currentDifficulty]);
        switch (randomType)
        {
            case 0:
                generateHorizontalMovingEnemy();
                break;
            case 1:
                generateVertialMovingEnemy();
                break;
            case 2:

                generateObstacle();
                break;
            case 3:
                generateHorizontalShootingEnemy();
                break;
            case 4:
                generateVerticalShootingEnemy();
                break;
        }
    }

    public void returnBoss()
    {
        currentBossCount -= 1;
    }

    void generateBoss()
    {
        if (currentBossCount >= maxBossCount)
        {
            return;

        }


        currentBossCount += 1;
        float x = transform.position.x + Random.Range(-verticalGenerateWidth, verticalGenerateWidth);
        float y = transform.position.y - horizontalGenerateHeight;


        Vector3 position = new Vector3(x, y, transform.position.z);
        Vector3 positionDir = new Vector3(0,1, 0);

        List<string> walkingEnemyNames = new List<string>() { "fireBoss", "mushroomBoss", "zombieBoss" };
        var enemyName = walkingEnemyNames[Random.Range(0, walkingEnemyNames.Count)];

        GameObject go = ObjectPooler.Instance.GetPooledObject(enemyName);
        if (!go)
        {
            return;
        }
        go.transform.position = position;
        go.GetComponent<PoolObject>().fetch();
        addBossDrops(go, positionDir);

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isInGame)
        {
            return;
        }
        if (currentDifficulty < difficultyToLevel.Count && playerController.walkedDistance >= difficultyToLevel[currentDifficulty])
        {
            currentDifficulty += 1;
            if(currentDifficulty< difficultyToGenerateTime.Count)
            {
                generateTimer = difficultyToGenerateTime[currentDifficulty];
            }
            Debug.Log("generate time update to " + generateTimer + " when difficulty increased to " + currentDifficulty);
        }
        {

        }
            generateTimer += Time.deltaTime;
        if (generateTimer >= generateTime)
        {
            generateTimer = 0;
            generateEnemy();

        }

        if (currentDifficulty > bossShowLevel)
        {
            generateBossTimer += Time.deltaTime;
        }
        if (generateBossTimer >= generateBossTime)
        {
            generateBossTimer = 0;
            generateBoss();

        }

        //this is affected by time scale

    }
}
