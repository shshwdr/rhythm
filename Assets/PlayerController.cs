using DG.Tweening;
using Doozy.Engine.UI;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : HPObjectController
{
    Vector2 movement;
    public bool controlTime = true;
    public float moveTime = 1;
    [HideInInspector]
    public float walkedDistance = 0;
    public bool isConversation;
    public float gridSize = 0.15f;

    Vector3 originalPosition;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        walkedDistance = 0;
        animator = GetComponentInChildren<Animator>();
        //animator.SetFloat("speed", 1);
        originalPosition = transform.position;
        MoveController.Instance.addPlayer(this);
        EventPool.OptIn(EventPool.startGameEvent, reset);
    }


    public override Vector2 Move(Vector3 dir)
    {
        animator.SetTrigger("jump");
        Debug.Log("jump");
        var res  = base.Move(dir);
        MoveController.Instance.updatePlayer(res);
        movement = Vector2.zero;
        return res;
    }

    // Update is called once per frame
    protected override void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach(var door in FindObjectsOfType<Door>())
            {
                door.clearRoom();
            }
        }

        if (isConversation)
        {
            return;
        }
        base.Update();
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if(x == 0&&y == 0)
        {

        }
        else
        {

            if (x != 0)
            {
                movement.y = 0;
                movement.x = x;
            }
            else
            {

                movement.x = 0;
                movement.y = y;
            }
        }
        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        //float speed = movement.sqrMagnitude;
        //if (controlTime)
        //{Time.timeScale = speed;
        //    //if (speed > 0)
        //    {
        //        //Time.timeScale = 1;
        //        Time.timeScale = speed;// rb.velocity.magnitude ;

        //        //StartCoroutine(slowDown());
        //       // slowDown();
        //    }
        //}
    }
    private void LateUpdate()
    {
        //if (isConversation)
        //{
        //    return;
        //}
        //var speed = moveSpeed;
        //rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        //testFlip(movement);
        ////Time.fixedDeltaTime = Time.timeScale * 0.01f;
        //if (GameManager.Instance.isInGame)
        //{

        //    walkedDistance += movement.magnitude * speed * Time.fixedDeltaTime;
        //}
        //rb.AddForce(new Vector2(movement.x * moveSpeed, movement.y * moveSpeed));
        //rb.velocity += new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
    }

    protected override void Die()
    {
        base.Die();

        animator.SetTrigger("die");
        Debug.Log("die");
        //Destroy(gameObject);
        //restart game
        StartCoroutine(stopGame());
    }

    IEnumerator stopGame()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.stopGame();
    }

    public void reset()
    {

        transform.position = GameManager.Instance.currentRoom.respawnPoint.position;
        getHeal();
        isDead = false;
        MoveController.Instance.updatePlayer(Utils.positionToGridIndexCenter2d(gridSize, transform.position));
        animator.Rebind();

    }
    public void getIntoConversation()
    {
        isConversation = true;
    }
    public void getOutOfConversation()
    {

        isConversation = false;
    }
}
