using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Hieroglyphs positions")]
    public Sprite[] hieroglyphs;
    public SpriteRenderer[] position;

    [Header("Other scripts")]
    public Collectable collectable;

    private void Start()
    {
        // set random sprite for all hieroglyphs positioins
        for (int i = 0; i < position.Length; i++)
        {
            position[i].sprite = hieroglyphs[Random.Range(0, hieroglyphs.Length)];
        }

        // in case when player hits checkpoint in level 4
        if (SceneManager.GetActiveScene().name == "Level4")
        {
            for (int i = 0; i < position.Length; i++)
            {
                if (PlayerPrefs.HasKey("collectable" + position[i].gameObject.transform.position.x))
                {
                    position[i].gameObject.SetActive(false);
                    collectable.hieroglyphsCount++;
                    Debug.Log(collectable.hieroglyphsCount);
                    PlayerPrefs.SetInt("CollectedHieroglyphs", collectable.hieroglyphsCount);
                    PlayerPrefs.SetInt("AllCollectedHieroglyphs", PlayerPrefs.GetInt("AllCollectedHieroglyphs") + 1);
                }
            }
        }
    }
}
