using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehaviour : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private LayerMask ground;

    private Collider2D coll;
    //private Rigidbody2D rb;

    private bool facingLeft = true;
    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
        //rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < .1)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("Falling", true);

            }
        }
        
        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Jumping", false);
        }


    }

    void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                //проверяем направление спрайта и если что меняем
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                //проверяем на землю
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }



}
