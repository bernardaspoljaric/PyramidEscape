using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPlatformerController : PhysicsObject
{
    [Header("Movement:")]
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    [Header("Milo the mummy look:")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Header("Collectable:")]
    Collectable collectable;
    public bool hasCollided = false;

    [Header("Audio:")]
    public AudioSource jumpAudio, hurtAudio, levelFinishedAudio;
    public AudioSource enemyHurtAudio;

    [Header("Flickering:")]
    protected Coroutine flickerCoroutine;
    protected float flickeringDuration = 0.1f;
    protected WaitForSeconds flickeringWait;
    protected float recoveryTime;

    [Header("Other scripts")]
    public UIManager Uim;
    public GameManager gm;

    [Header("Animations")]
    public Animator checkpoint;
    public Animator checkpoint2;
    public Animator checkpoint3;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        CheckIfLevelFour();
    }

    private void Start()
    {  
        collectable = GetComponent<Collectable>();
        flickeringWait = new WaitForSeconds(flickeringDuration);
    }

    private void CheckIfLevelFour()
    {
        if (SceneManager.GetActiveScene().name == "Level4")
        {
            if (PlayerPrefs.HasKey("xPosition") && PlayerPrefs.HasKey("yPosition") && PlayerPrefs.HasKey("zPosition"))
            {
                this.gameObject.transform.position = new Vector3(PlayerPrefs.GetFloat("xPosition"), PlayerPrefs.GetFloat("yPosition"), PlayerPrefs.GetFloat("zPosition"));
            }
        }
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");
        
        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
            jumpAudio.Play();

            if (move.x > 0.1f && velocity.y == jumpTakeOffSpeed)
            {
                animator.SetBool("JumpRight", true);
            }
            if (move.x < -0.1 && velocity.y == jumpTakeOffSpeed)
            {
                animator.SetBool("JumpRight", true);
            }
            if (move.x == 0 && velocity.y == jumpTakeOffSpeed)
            {
                animator.SetBool("JumpRight", true);
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }

            animator.SetBool("JumpLeft", false);
            animator.SetBool("JumpRight", false);
        }

        targetVelocity = move * maxSpeed;

        if (move.x > 0.1f)
        {
            animator.SetBool("walkRight", true);
        }
        if (move.x < -0.1)
        {
            animator.SetBool("walkLeft", true);
        }
        if (move.x == 0)
        {
            animator.SetBool("walkLeft", false);
            animator.SetBool("walkRight", false);
        }
    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // loading next level
        if (collision.gameObject.tag == "Gate")
        {
            // check which level player is in
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                // check if enough collectables are collected
                if (PlayerPrefs.GetInt("CollectedHieroglyphs") == 1)
                {
                    levelFinishedAudio.Play();
                   
                    Debug.Log("Level2");
                    PlayerPrefs.SetInt("LevelOnePass", 1);
                    
                    SceneManager.LoadScene("Level2");

                }
            }

            if (SceneManager.GetActiveScene().name == "Level2")
            {
                // check if enough collectables are collected
                if (PlayerPrefs.GetInt("CollectedHieroglyphs") == 5)
                {
                    levelFinishedAudio.Play();
                    
                    Debug.Log("Level3");
                    PlayerPrefs.SetInt("LevelTwoPass", 1);

                    SceneManager.LoadScene("Level3");
                }
            }
            if (SceneManager.GetActiveScene().name == "Level3")
            {
                ScorpionBoss boss = GetComponent<ScorpionBoss>();
                // check if enough collectables are collected
                if (PlayerPrefs.GetInt("CollectedHieroglyphs") == 10 && boss == null)
                {
                    levelFinishedAudio.Play();
                    
                    Debug.Log("Level4");
                    PlayerPrefs.SetInt("LevelThreePass", 1);

                    SceneManager.LoadScene("Level4");
                }
            }
            if (SceneManager.GetActiveScene().name == "Level4")
            {
                // check if enough collectables are collected
                if (PlayerPrefs.GetInt("CollectedHieroglyphs") == 30)
                {
                    levelFinishedAudio.Play();

                    Debug.Log("Level5");
                    PlayerPrefs.SetInt("LevelFourPass", 1);

                    // delete checkpoints position
                    PlayerPrefs.DeleteKey("xPosition");
                    PlayerPrefs.DeleteKey("yPosition");
                    PlayerPrefs.DeleteKey("zPosition");

                    // delete collected collectables positions
                    for (int i = 0; i < gm.position.Length; i++)
                    {
                        PlayerPrefs.DeleteKey("collectable" + gm.position[i].gameObject.transform.position.x);
                    }

                    SceneManager.LoadScene("CreditsScene");
                }
            }
            //if (SceneManager.GetActiveScene().name == "Level5")
            //{
            //    // check if enough collectables are collected
            //    if (PlayerPrefs.GetInt("CollectedHieroglyphs") == 50)
            //    {
            //        levelFinishedAudio.Play();

            //        Debug.Log("Level6");
            //        PlayerPrefs.SetInt("LevelFivePass", 1);

            //        //SceneManager.LoadScene("Level5");
            //    }
            //}
        }

        //spikes
        if (collision.gameObject.tag == "Spikes")
        {
            if (hasCollided)
            {
                return;
            }
            else
            {
                hasCollided = true;
                Hurt();
                StartCoroutine(RecoveryTimeSpikes());  
            }
        }

        //poison
        if (collision.gameObject.tag == "Poison")
        {
                Hurt();
                StartCoroutine(RecoveryTimeSpikes());
        }

        if (collision.gameObject.tag == "Checkpoint")
        {
            checkpoint.enabled = true;
            collision.enabled = false;
            SavePosition();

            Debug.Log("Checkpoint!");
        }
        if (collision.gameObject.tag == "Checkpoint2")
        {
            checkpoint2.enabled = true;
            collision.enabled = false;
            SavePosition();

            Debug.Log("Checkpoint!");
        }
        if (collision.gameObject.tag == "Checkpoint3")
        {
            checkpoint3.enabled = true;
            collision.enabled = false;
            SavePosition();

            Debug.Log("Checkpoint!");
        }

    }

    public void SavePosition()
    {
        PlayerPrefs.SetFloat("xPosition", this.gameObject.transform.position.x);
        PlayerPrefs.SetFloat("yPosition", this.gameObject.transform.position.y);
        PlayerPrefs.SetFloat("zPosition", this.gameObject.transform.position.z);
    }

    // when player collides with enemy
    void OnCollisionEnter2D(Collision2D collision)
    {
        // scorpion boss
        if(collision.gameObject.tag == "BossScorpion")
        {
            ScorpionBoss boss = collision.collider.GetComponent<ScorpionBoss>();
            if (boss != null)
            {
                if (hasCollided)
                {
                    return;
                }
                else
                {
                    ContactPoint2D point = collision.GetContact(0);
                    hasCollided = true;
                    StartCoroutine(RecoveryBossTime());

                    if (point.normal.y >= 0.6f)
                    {
                        velocity.y = 5;
                        enemyHurtAudio.Play();
                        boss.BossHurt();   
                    }
                    else
                    {
                        Hurt();
                        StartFlickering();
                        velocity.x = 0;
                    }
                }
            }
        }

        // classic enemies
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (hasCollided)
                {
                    return;
                }
                else
                {
                    ContactPoint2D point = collision.GetContact(0);
                    hasCollided = true;
                    StartCoroutine(RecoveryTime());

                    if (point.normal.y >= 0.6f)
                    {
                        velocity.y = 5;
                        velocity.x = 0;
                        enemyHurtAudio.Play();
                        enemy.EnemyHurt();  
                    }
                    else
                    {
                        Hurt();
                        StartFlickering();
                        enemy.ChangeDirection();
                        velocity.x = 0;
                    }
                }
            }
        }
    }

    // timer for collision with enemy 
    IEnumerator RecoveryTime()
    {
        recoveryTime = 2f;

        yield return new WaitForSeconds(recoveryTime);
        hasCollided = false;
        StopFlickering();
    }
    // timer for after spikes collision
    IEnumerator RecoveryTimeSpikes()
    {
        recoveryTime = 4f;
        StartFlickering();
        yield return new WaitForSeconds(recoveryTime);
        hasCollided = false;
        StopFlickering();

    }
    // timer for after boss collision
    IEnumerator RecoveryBossTime()
    {
        recoveryTime = 2f;

        yield return new WaitForSeconds(recoveryTime);
        hasCollided = false;
        StopFlickering();
    }

    IEnumerator Flicker()
    {
        float timer = 0f;

        while (timer < recoveryTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return flickeringWait;
            timer += flickeringDuration;
        }

        spriteRenderer.enabled = true;
    }

    public void StartFlickering()
    {
        flickerCoroutine = StartCoroutine(Flicker());
    }
    public void StopFlickering()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            spriteRenderer.enabled = true;
        }
    }

    // when player in contact with enemy delete all collected collectables and relode scene -- first 3 levels
    public void Hurt()
    {
        hurtAudio.Play();

        if (collectable.life > 1)
        {
            collectable.life -= 1;
            if (collectable.life == 2)
            {
                collectable.life3.color = new Color(collectable.life3.color.r, collectable.life3.color.g, collectable.life3.color.b, 0.5f);
                transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);

            }
            else if (collectable.life == 1)
            {
                collectable.life2.color = new Color(collectable.life2.color.r, collectable.life2.color.g, collectable.life2.color.b, 0.5f);
                transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
                
            }

            Debug.Log("Player loses life." + collectable.life);
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            gameObject.GetComponent<Rigidbody2D>().position += new Vector2(0.5f, 0.5f);
        }
        else
        {
            PlayerPrefs.DeleteKey("AllCollectedHieroglyphs");
            PlayerPrefs.DeleteKey("CollectedHieroglyphs");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

           
    }
}
