using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Collectable : MonoBehaviour
{
    [Header("Hieroglyphs")]
    public int hieroglyphsCount;
    public TextMeshProUGUI collected;

    [Header("Life")]
    public int life;
    public Image life1, life2, life3;

    [Header("Audio")]
    public AudioSource hieroglyphsAudioCollected;
    public AudioSource lifeCollectedAudio;

    [Header("Other scripts")]
    public PlayerPlatformerController ppc;
    public GameManager gm;


    private void Awake()
    {
        hieroglyphsCount = 0;
        PlayerPrefs.SetInt("CollectedHieroglyphs", hieroglyphsCount);
    }
    private void Start()
    {
        collected.text = PlayerPrefs.GetInt("AllCollectedHieroglyphs").ToString();
      
        if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level2")
        {
            life = 0;
            life1.color = new Color(life1.color.r, life1.color.g, life1.color.b, 0.5f);
            life2.color = new Color(life2.color.r, life2.color.g, life2.color.b, 0.5f);
            life3.color = new Color(life3.color.r, life3.color.g, life3.color.b, 0.5f);
            Debug.Log("0 lifes");
        }
        else
        {
            life = 3;
            life1.color = new Color(life1.color.r, life1.color.g, life1.color.b, 1f);
            life2.color = new Color(life2.color.r, life2.color.g, life2.color.b, 1f);
            life3.color = new Color(life3.color.r, life3.color.g, life3.color.b, 1f);
            Debug.Log("3 lifes");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hieroglyphs")
        {
            hieroglyphsAudioCollected.Play();
            hieroglyphsCount++;
            
            PlayerPrefs.SetInt("CollectedHieroglyphs", hieroglyphsCount);
            PlayerPrefs.SetInt("AllCollectedHieroglyphs", PlayerPrefs.GetInt("AllCollectedHieroglyphs") + 1);
            collected.text = PlayerPrefs.GetInt("AllCollectedHieroglyphs").ToString();

            if (SceneManager.GetActiveScene().name == "Level4")
            {
                for (int i = 0; i < gm.position.Length; i++)
                {
                    if (collision.gameObject.transform.position == gm.position[i].gameObject.transform.position)
                    {
                        PlayerPrefs.SetInt("collectable" + gm.position[i].gameObject.transform.position.x, 1);
                        Debug.Log("collectable" + gm.position[i].gameObject.transform.position.x);
                    }
                }
            }

            collision.gameObject.SetActive(false);   
        }

        if (collision.tag == "Life")
        {
            if (life < 3)
            {
                lifeCollectedAudio.Play();
                life++;
                if (life == 2)
                {
                    life2.color = new Color(life2.color.r, life2.color.g, life2.color.b, 1f);
                    ppc.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
                }
                else if (life == 3)
                {
                    life3.color = new Color(life3.color.r, life3.color.g, life3.color.b, 1f);
                    ppc.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                }
                collision.gameObject.SetActive(false);
            }
        }
    }
}
