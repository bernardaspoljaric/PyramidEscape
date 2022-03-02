using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialDialogue : MonoBehaviour
{
    public Image tutorial;
    public Text tutorialText;
    public Text enterText;

    public Animator anim;

    public GameObject triggerCollectable;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            StartCoroutine(MovementTutorial());
        }
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            StartCoroutine(EnemyTutorial());
        }

    }

    IEnumerator MovementTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        tutorialText.text = "Use LEFT and RIGHT arrows or A and D to move.\n\nPress SPACE to jump.";
        enterText.text = "Press ENTER to continue...";
        tutorial.gameObject.SetActive(true);
        anim.Play("TutorialDown");
        
    }

    IEnumerator EnemyTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        tutorialText.text = "Scorpion represents an enemy. Don't let them touch you!\n\nTry jumping on them to destroy them.";
        enterText.text = "Press ENTER to continue...";
        tutorial.gameObject.SetActive(true);
        anim.Play("TutorialDown");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            anim.Play("TutorialUp");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TutorialCollectable")
        {
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                tutorialText.text = "To pass level collect hieroglyphs.";
                enterText.text = "Press ENTER to continue...";
                tutorial.gameObject.SetActive(true);
                anim.Play("TutorialDown");
                StartCoroutine(DeactivateTriggerCollectbles());
            }
        }

        if (collision.gameObject.tag == "TutorialBossFight")
        {
            if (SceneManager.GetActiveScene().name == "Level3")
            {
                tutorialText.text = "This is the last segment of top part of the pyramid.\nBe aware of the boss guarding the gate!";
                enterText.text = "Press ENTER to continue...";
                tutorial.gameObject.SetActive(true);
                anim.Play("TutorialDown");
                StartCoroutine(DeactivateTriggerCollectbles());
            }
        }
    }

    IEnumerator DeactivateTriggerCollectbles()
    {
        yield return new WaitForSeconds(2);
        triggerCollectable.SetActive(false);
    }

}
