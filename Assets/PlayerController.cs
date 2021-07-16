using DG.Tweening;
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
    bool isConversation;

    Vector3 originalPosition;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        walkedDistance = 0;
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("speed", 1);
        originalPosition = transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isConversation)
        {
            return;
        }
        base.Update();
        movement.x = Input.GetAxisRaw("Horizontal");

        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        movement.y = Input.GetAxisRaw("Vertical");
        float speed = movement.sqrMagnitude;

        movement = Vector2.ClampMagnitude(movement, 1);
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
        if (isConversation)
        {
            return;
        }
        var speed = moveSpeed;
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        testFlip(movement);
        //Time.fixedDeltaTime = Time.timeScale * 0.01f;
        if (GameManager.Instance.isInGame)
        {

            walkedDistance += movement.magnitude * speed * Time.fixedDeltaTime;
        }
        //rb.AddForce(new Vector2(movement.x * moveSpeed, movement.y * moveSpeed));
        //rb.velocity += new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
    }

    protected override void Die()
    {
        //Destroy(gameObject);
        //restart game
        transform.position = originalPosition;
        walkedDistance = 0;
        GameManager.Instance.stopGame();
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
