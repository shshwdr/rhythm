using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnemyGenerator : MonoBehaviour
{
    public string roomId;
    public int roomMusic = 3;
    Dictionary<System.Tuple<int, int>, int> roomAvailable;

    List<EnemyController> enemies = new List<EnemyController>();

    public Transform respawnPoint;
    bool isCleared = false;
    bool isActivated = false;
    Collider2D collider;
    AudioSource audioSource;

    public Door toCloseDoor;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(setup());
        collider = GetComponent<Collider2D>();
        EventPool.OptIn(EventPool.startGameEvent,resetRoom);
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator setup()
    {
        yield return new WaitForSeconds(0.1f);
        //generateHorizontalMovingEnemy();
        //generateHorizontalMovingEnemy();
        //generateHorizontalMovingEnemy();
        //generateHorizontalMovingEnemy();
        var allEnemies = new List<EnemyController>( GetComponentsInChildren<EnemyController>());

        foreach (var enemy in allEnemies)
        {
            if (enemy.enabled&& enemy.gameObject.activeInHierarchy)
            {

                enemies.Add(enemy);
                enemy.room = this;
            }
        }

        respawnPoint = gameObject.transform.Find("respawnPoint");
        if (toCloseDoor)
            toCloseDoor.openDoor();
    }

    public void enemyDie(EnemyController e)
    {
        //enemies.Remove(e);
        //if(enemies.Count == 0)
        //{
        //    clearRoom();
        //}
        foreach (var enemy in enemies)
        {
            if (!enemy.isDead)
            {
                return;
            }
        }
            clearRoom();
    }

    public void cleanRoom()
    {
        foreach (var enemy in enemies)
        {
            if (!enemy.isDead)
            {
                enemy.getDamage(100);
            }
        }
        if (toCloseDoor)
            toCloseDoor.openDoor();
    }

    public void clearRoom()
    {
        if (isCleared)
        {
            Debug.Log("repeat clear room");
            return;
        }
        if(roomMusic==3||roomMusic>0)
        {

            audioSource.Play();
        }
        isCleared = true;
        EventPool.Trigger<string>("clearRoom", roomId);
        GameMaster.Instance.removeAudioSource(roomMusic);
        if (toCloseDoor)
            toCloseDoor.openDoor();
    }

    public void resetRoom()
    {
        if (isCleared)
        {
            return;
        }
        disactivateRoom();
        foreach (var enemy in enemies)
        {
            if (enemy.isDead)
            {
                enemy.reset();
            }
            else if (GetComponent<DialogEnemy>())
            {
                enemy.reset();
            }
            else
            {
                enemy.softReset();
                //enemy.getDamage(1000);
                //enemy.reset();
            }
        }
        if (toCloseDoor)
            toCloseDoor.openDoor();
    }

    public void leaveRoom()
    {
        GameMaster.Instance.removeAudioSource(roomMusic);
        foreach (var movementTask in GetComponentsInChildren<FollowMovementTask>())
        {
            movementTask.disactive();
        }
        if (toCloseDoor)
            toCloseDoor.openDoor();
    }
    public void disactivateRoom()
    {
        if (!isActivated)
        {
            return;
        }
        if (isCleared)
        {
            return;
        }
        isActivated = false;
        foreach (var enemy in enemies)
        {
            enemy.disactivate();
        }
        GameMaster.Instance.removeAudioSource(roomMusic);
        foreach (var movementTask in GetComponentsInChildren<FollowMovementTask>())
        {
            movementTask.disactive();
        }

        if (toCloseDoor)
            toCloseDoor.openDoor();
    }
    public void activateRoom()
    {
        if (isActivated)
        {
            return;
        }
        if (isCleared)
        {
            return;
        }

        isActivated = true;
        foreach (var enemy in enemies)
        {
            enemy.activate();
        }
        foreach(var movementTask in GetComponentsInChildren<FollowMovementTask>())
        {
            movementTask.active();
        }
        GameManager.Instance.currentRoom = this;
        if (roomMusic > 0)
        {

            GameMaster.Instance.addAudioSource(roomMusic);
        }
        if (toCloseDoor)
        {

            toCloseDoor.closeDoor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0 + int.Parse( roomId)))
        {
            FindObjectOfType<PlayerController>().gameObject.transform.position = transform.Find("activator").position;
        }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {

            activateRoom();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.GetComponent<PlayerController>())
        {

            leaveRoom();
        }
    }
}
