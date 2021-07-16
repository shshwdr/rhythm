using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObjectController : MonoBehaviour
{
    public Animator animator;
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
    // Start is called before the first frame update
    virtual protected void Awake()
    {

        //emotesController = GetComponentInChildren<EmotesController>();
        hpBar = GetComponentInChildren<HPBarHandler>();
        rb = GetComponent<Rigidbody2D>();

       // player = GameObject.Find("Player");
        //hideDistance = player.GetComponent<PlayerController>().hideDistance;

    }
    virtual protected void Start()
    {
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
        hp += 1;
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


        //var scale = BattleManager.elementAdvantageScale(element, elementType);
        //int finalDamage = BattleManager.finalDamage(element, elementType, damage);
        //PopupTextManager.Instance.ShowPopupNumber(transform.position, finalDamage, scale);


        currentInvinsibleTimer = 0;
        hp -= (int)damage;
        playHurtSound();
        updateHP();
        if (hp == 0)
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
