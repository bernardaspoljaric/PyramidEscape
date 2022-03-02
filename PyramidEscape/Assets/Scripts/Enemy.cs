using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    Vector2 initialPosition;
    public int direction;
    public float maxDist;
    public float minDist;
    public float movingSpeed;

    [Header("Look")]
    private SpriteRenderer spriteRenderer;

    [Header("Animations")]
    public Animator animatorEnemy;

    [Header("VFX")]
    public ParticleSystem destoryParticle;
    void Start()
    {
        initialPosition = transform.position;
        direction = -1;
    
        spriteRenderer = GetComponent<SpriteRenderer>();

        animatorEnemy = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (direction)
        {
            case -1:
                // Moving Left
                if (transform.position.x > minDist)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-movingSpeed, GetComponent<Rigidbody2D>().velocity.y);
                }
                else
                {
                    direction = 1;
                    spriteRenderer.flipX = !spriteRenderer.flipX;
                }
                break;
            case 1:
                //Moving Right
                if (transform.position.x < maxDist)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(movingSpeed, GetComponent<Rigidbody2D>().velocity.y);
                }
                else
                {
                    direction = -1;
                    spriteRenderer.flipX = !spriteRenderer.flipX;
                }
                break;
        }
    }
    public void EnemyHurt()
    {
        destoryParticle.gameObject.SetActive(true);
        this.spriteRenderer.enabled = false;
        this.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

        Invoke("EnemyDestory", 1f);
        
    }

    public void EnemyDestory()
    {
        this.gameObject.SetActive(false);
    }

    public void ChangeDirection()
    {
        direction = - direction;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
