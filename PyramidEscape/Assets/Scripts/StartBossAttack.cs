using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossAttack : MonoBehaviour
{
    public GameObject poisonPrefab;
    public Transform startPoint;
    float counter = 3f;

    public AudioSource shootPoisonAudio;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (counter <= 0)
            {
                shootPoisonAudio.Play();
                Instantiate(poisonPrefab, startPoint.position, Quaternion.identity);
                counter = 3;
            }
        }

        
    }
    private void Update()
    {
        counter -= 1 * Time.deltaTime;
    }
}
