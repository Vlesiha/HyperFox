using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    private AudioSource expl;
    protected Rigidbody2D rb;
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        expl = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Jumpon()
    {
        anim.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;

    }
    void Death()
    {
        Destroy(this.gameObject);
    }
    private void BOOM()
    {
        expl.Play();
    }
}
