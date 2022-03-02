using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator fadeTransition;
    
    public float transitionTime = 1f;
    
    public IEnumerator LoadLevel(int levelIndex)
    {
        //Play animation
        fadeTransition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);
        
        //Load scene
         SceneManager.LoadScene(levelIndex);
    }    
}
