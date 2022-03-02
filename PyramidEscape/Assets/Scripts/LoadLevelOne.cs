using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelOne : MonoBehaviour
{
    private void Start()
    {
        Invoke("Load", 50.083f);
    }

   void Load()
    {
        SceneManager.LoadScene("Level1");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Level1");
            CancelInvoke();
        }
    }
}
