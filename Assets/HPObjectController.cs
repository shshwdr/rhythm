using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObjectController : MonoBehaviour
{
    public Animator animator;
    public bool isActive = false;
    public float moveSpeed;
    public int maxHp = 10;
    protected int hp = 0;
    HPBarHandler hpBar;
    public bool isDead;
    public bool isStuned;

    public AudioClip[] beHitClips;

    public Rigidbody2D rb;

    public float stunTime = 0.3f;
    float currentStunTimer = 0;

    public bool hasInvinsibleTime;
    public float invinsibleTime = 0.3f;
    float currentInvinsibleTimer;
    //protected EmotesController emotesController;
    protected GameObject spriteObject;
    public string elementType;
    GameObject player;
    public GameObject hideWhenFarAway;
    float hideDistance = 100f;
    public bool isImmortal = false;
    float moveTime = 0.12f;

    protected float gridSize;

    public int moveMode;
    protected AudioSource deathSound;
    public AudioClip deathClip;
    // Start is called before the first frame update
    virtual protected void Awake()
    {

        //emotesController = GetComponentInChildren<EmotesController>();
        hpBar = GetComponentInChildren<HPBarHandler>();
        rb = GetComponent<Rigidbody2D>();
        gridSize = GameMaster.Instance.gridSize;

        transform.position = Utils.snapToGridCenter(gridSize, transform.position);
        // player = GameObject.Find("Player");
        //hideDistance = player.GetComponent<PlayerController>().hideDistance;

    }

    protected bool checkIfCollideWall(Vector3 dir)
    {
        int layerMask = LayerMask.GetMask("wall") | LayerMask.GetMask("chest");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, gridSize * 0.9f, layerMask);
        // Does the ray intersect any objects excluding the player layer
        if (hit)
        {
            //Debug.DrawRay(transform.position, dir * 100 * 0.9f, Color.yellow);
            //Debug.Log("Did Hit");
            return true;
        }
        else
        {
            //Debug.DrawRay(transform.position, dir * 100 * 0.9f, Color.red);
            //Debug.Log("Did not Hit");
            return false;
        }
    }

    protected GameObject checkIfCollide(Vector3 dir)
    {
        var position = transform.position + dir;
        var indexPosition = Utils.positionToGridIndexCenter2d(gridSize,position);
        var go = MoveController.Instance. checkPositionItem(indexPosition);
        return go;
        //int layerMask = LayerMask.GetMask("wall");

        //RaycastHit hit;
        //// Does the ray intersect any objects excluding the player layer
        //if (Physics2D.Raycast(transform.position, dir, gridSize*0.9f, layerMask))
        //{
        //    Debug.DrawRay(transform.position, dir * 100*0.9f, Color.yellow);
        //    Debug.Log("Did Hit");
        //    return true;
        //}
        //else
        //{
        //    Debug.DrawRay(transform.position, dir * 100 * 0.9f, Color.red);
        //    Debug.Log("Did not Hit");
        //    return false;
        //}
    }

    public virtual Vector2 Move(Vector3 dir)
    {
        //do extra work for enemy
        bool willCollideWall = checkIfCollideWall(dir);
        bool willCollider = checkIfCollide(dir);
        if (willCollideWall)
        {
            return Vector2.negativeInfinity;

        }
        else if (!willCollider)
        {

            //detect if collide
            //transform.Translate(dir * gridSize);
            if(moveMode == 0)
            {

                rb.MovePosition(rb.position + (Vector2)dir * gridSize);

                // transform.Translate(dir * gridSize);
                //transform.DOMove(transform.position + dir * gridSize, moveTime);//.SetEase(Ease.OutBack);
            }
            else if(moveMode == 1)
            {

                rb.MovePosition(rb.position + (Vector2)dir * gridSize);

                // transform.Translate(dir * gridSize);
                //transform.DOJump(transform.position + dir * gridSize, 0.4f, 1, moveTime);
            }
            return Utils.positionToGridIndexCenter2d(gridSize, (Vector3)rb.position + dir * gridSize);
            //return true;
        }
        else
        {
            return Vector2.negativeInfinity;
            //dir = -dir;
            //return false;
        }

    }


    virtual protected void Start()
    {
        deathSound = GameObject.Find("deathSound").GetComponent<AudioSource>();
        hp = maxHp;
        updateHP();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        currentInvinsibleTimer += Time.deltaTime;
        //if (hideWhenFarAway)
        //{

        //    if ((transform.position - player.transform.position).sqrMagnitude >= hideDistance)
        //    {
        //        hideWhenFarAway.SetActive(false);
        //    }
        //    else
        //    {
        //        hideWhenFarAway.SetActive(true);
        //    }
        //}
    }

    public void updateHP()
    {
        hp = Mathf.Clamp(hp, 0, maxHp);
        if (hpBar)
        {

            hpBar.SetMaxHp(maxHp);
            hpBar.SetHealthBarValue(hp);
        }
    }
    virtual protected void playHurtSound()
    {

    }

    public void getHeal()
    {
        if (isDead)
        {
            return;
        }
        if (hasInvinsibleTime && currentInvinsibleTimer < invinsibleTime)
        {
            return;
        }
        hp += 100;
        updateHP();
        if (GetComponent<AudioSource>())
        {
           // GetComponent<AudioSource>().PlayOneShot(MusicManager.Instance.pickup);
        }
    }
    public virtual void getDamage(float damage = 1, string element = "")
    {
        if (isDead)
        {
            return;
        }
        if (isImmortal)
        {
            Debug.Log(gameObject.name+ " is immortal but it gets damage");
            return;
        }
        if (hasInvinsibleTime && currentInvinsibleTimer < invinsibleTime)
        {
            return;
        }
        if (!isActive)
        {
            return;
        }

        //var scale = BattleManager.elementAdvantageScale(element, elementType);
        //int finalDamage = BattleManager.finalDamage(element, elementType, damage);
        //PopupTextManager.Instance.ShowPopupNumber(transform.position, finalDamage, scale);


        currentInvinsibleTimer = 0;
        hp -= (int)damage;
        playHurtSound();
        updateHP();
        if (hp <= 0)
        {
            Die();
        }
        else
        {
            isStuned = true;
            currentStunTimer = 0;
            if (animator)
            {

                //animator.SetTrigger("hit");
            }
            if (GetComponent<AudioSource>())
            {
                //GetComponent<AudioSource>().PlayOneShot(MusicManager.Instance.damage);
            }
        }

    }

    protected virtual void Die()
    {
        isDead = true;
        deathSound.PlayOneShot(deathClip);
    }


    public bool facingRight = true;
    void flip()
    {
        facingRight = !facingRight;
        if (spriteObject)
        {

            Vector3 scaler = spriteObject.transform.localScale;
            scaler.x = -scaler.x;
            // spriteObject.transform.position = new Vector3(spriteObject.transform.position.x + 1, spriteObject.transform.position.y, -1);
            spriteObject.transform.localScale = scaler;
            //spriteObject.GetComponent<SpriteRenderer>().flipX = !facingRight;
        }
    }
    public void testFlip(Vector3 movement)
    {
        if (facingRight == false && movement.x > 0f)
        {
            flip();
        }
        if (facingRight == true && movement.x < 0f)
        {
            flip();
        }
    }


}
