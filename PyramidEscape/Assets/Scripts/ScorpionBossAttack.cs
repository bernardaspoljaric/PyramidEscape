using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// projectile
public class ScorpionBossAttack : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject poisonSpawnPoint;
    public GameObject target;
    public PlayerPlatformerController ppc;

    [Header("Projectile movement")]
    public float speed = 5f;
    float poisonSpawnPointX;
    float targetX;
    float targetY;
    float distance;
    float nextX;
    float baseY;
    float height;

    private void Start()
    {
        poisonSpawnPoint = GameObject.FindGameObjectWithTag("PoisonSpawn");
        target = GameObject.FindGameObjectWithTag("Player");
        ppc = target.GetComponent<PlayerPlatformerController>();

        poisonSpawnPointX = poisonSpawnPoint.transform.position.x;
        targetX = target.transform.position.x;
        targetY = target.transform.position.y;
    }
    private void Update()
    {
        distance = targetX - poisonSpawnPointX;
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(poisonSpawnPoint.transform.position.y, targetY, (nextX - poisonSpawnPointX) / distance);
        height = 2 * (nextX - poisonSpawnPointX) * (nextX - targetX) / (-0.25f * distance * distance);

        Vector3 movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;

        if(transform.position.x == targetX)
        {
            Destroy(gameObject);
        }
    }

    public static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }      
    }


}
