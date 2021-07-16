using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    public string displayName;
    public int currentLevel = -1;
    public int originLevel = -1;
    protected int damage = 1;
    float beatShootTime;
    bool isShooting = false;
    public List<int> levelToDamage = new List<int>()
    {
        1,2,3,4,5
    };

    public List<float> levelToShootTime = new List<float>()
    {
        0.3f,0.2f,
    };
    public List<List<Vector3>> levelToBulletStartOffset = new List<List<Vector3>>() { 
        new List<Vector3>() { new Vector3() } 
    };
    protected List<Vector3> allBulletsStartOffset;
    protected float shootTime = 0.3f;
    float realShootTime;
    float currentShootTimer = 0f;

    public void resetGame()
    {
        currentLevel = originLevel;
        allBulletsStartOffset = levelToBulletStartOffset[Mathf.Clamp(currentLevel, 0, levelToBulletStartOffset.Count - 1)];
        shootTime = levelToShootTime[Mathf.Clamp(currentLevel - (allBulletsStartOffset.Count-1), 0, levelToShootTime.Count - 1)];
        realShootTime = beatShootTime / shootTime;
        damage = levelToDamage[Mathf.Clamp(currentLevel - (allBulletsStartOffset.Count - 1) - (levelToShootTime.Count - 1), 0, levelToShootTime.Count - 1)];
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        beatShootTime = 60f / GameMaster.Instance.beatsPerMinute * 4;
        resetGame();
        EventPool.OptIn(EventPool.stopGameEvent, resetGame);
    }

    public void increaseLevel(bool forever = false)
    {
        currentLevel += 1;
        if (forever)
        {
            originLevel += 1;
        }
        if (levelToDamage.Count > currentLevel)
        {
            damage = levelToDamage[currentLevel];
        }
        else
        {
            damage = levelToDamage[levelToDamage.Count - 1];
        }
        if (levelToShootTime.Count > currentLevel)
        {
            shootTime = levelToShootTime[currentLevel];
        }
        else
        {
            shootTime = levelToShootTime[levelToDamage.Count - 1];
        }

        if (levelToBulletStartOffset.Count > currentLevel)
        {
            allBulletsStartOffset = levelToBulletStartOffset[currentLevel];
        }
        else
        {
            allBulletsStartOffset = levelToBulletStartOffset[levelToBulletStartOffset.Count-1];
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!isShooting)
        {
            return;
        }
        if (currentLevel < 0)
        {
            return;
        }
        currentShootTimer += Time.deltaTime;
        if (currentShootTimer >= realShootTime)
        {
            currentShootTimer = 0;
            Shoot();
        }
    }

    protected void generateNormalBullet(Vector3 dir, Vector3 startOffset = new Vector3())
    {
        var bullet = ObjectPooler.Instance.GetPooledObject("playerBullet");
        bullet.GetComponent<PlayerBullet>().fetch();
        bullet.GetComponent<PlayerBullet>().Init(transform.position + startOffset, dir, damage);
    }
    protected void generateHomingBullet(Vector3 startOffset = new Vector3())
    {
        var bullet = ObjectPooler.Instance.GetPooledObject("playerHomingBullet");
        bullet.GetComponent<PlayerHomingBullet>().fetch();
        bullet.GetComponent<PlayerHomingBullet>().Init(transform.position + startOffset, damage);
    }

    public void startShoot()
    {
        isShooting = true;
        currentShootTimer = 100;
        StartCoroutine(stopShooting());
    }

    IEnumerator stopShooting()
    {
        yield return new WaitForSeconds(beatShootTime);
        isShooting = false;
    }

    protected virtual void Shoot() { }
}
