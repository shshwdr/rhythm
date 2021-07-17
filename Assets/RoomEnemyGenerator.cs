using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnemyGenerator : MonoBehaviour
{

    Dictionary<System.Tuple<int, int>, int> roomAvailable;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(setup());
    }

    IEnumerator setup()
    {
        yield return new WaitForSeconds(0.1f);
        //generateHorizontalMovingEnemy();
        //generateHorizontalMovingEnemy();
        //generateHorizontalMovingEnemy();
        //generateHorizontalMovingEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void addDrops(GameObject walkingEnemy, Vector3 positionDir)
    {
        var script = walkingEnemy.GetComponent<EnemyController>();
        bool isCoin = false;// Random.Range(0f, 1f) < coinRate;
        bool isAbility = false;// !isCoin && Random.Range(0f, 1f) < abilityRate;
        script.init(positionDir, isCoin, isAbility, false, 1/*difficultyToHPMultiplier[currentDifficulty]*/);
    }
    void generateHorizontalMovingEnemy()
    {
        int enemyCount = Random.Range(4, 7);

        bool isOnRight = Utils.randomBool();
        float x = transform.position.x + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);
        float y = transform.position.y + Random.Range(-transform.localScale.y/2, transform.localScale.y / 2);


        Vector3 position = new Vector3(x, y, transform.position.z);
        position = Utils.snapToGrid(GameMaster.Instance.gridSize, position);
        Vector3 positionDir = new Vector3(isOnRight ? -1 : 1, 0, 0);

        List<string> walkingEnemyNames = new List<string>() { "fire1", "mushroom1", "zombie1" };
        var name = walkingEnemyNames[Random.Range(0, walkingEnemyNames.Count)];

        var generatedEnemies = GroupEnemyGenerator.generateEnemyGroup(name, 1, Utils.randomEnumValue<EnemyGroupShape>(), position, positionDir, transform);

        //GameObject walkingEnemy = Instantiate(walkingEnemyPrefab, position, Quaternion.identity);
        foreach (var walkingEnemy in generatedEnemies)
        {
            addDrops(walkingEnemy, positionDir);
        }

    }
}
