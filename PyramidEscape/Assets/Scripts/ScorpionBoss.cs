using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionBoss : MonoBehaviour
{
    [Header("Health")]
    int health = 3;
    public bool defeat = false;

    [Header("Attack")]
    public GameObject trigger; // trigger in which scorpion boss attacks

    [Header("Audio")]
    public AudioSource bossDefeatAudio;
    
    [Header("VFX")]
    public ParticleSystem destoryParticle;

    public void BossHurt()
    {
        if(health > 1)
        {
            health--;
            Debug.Log("Boss " + health);
        }
        else
        {
            bossDefeatAudio.Play();
            
            defeat = true;
            //turn off trigger for attack
            trigger.SetActive(false);

            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

            // animacija za uništavanje
            destoryParticle.gameObject.SetActive(true);
            Invoke("BossDestory", 1f);

        }
       
    }
    public void BossDestory()
    {
        Destroy(this.gameObject);
    }

}


