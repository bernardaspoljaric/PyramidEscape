using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    private Text UIText;
    private string textToWrite;
    private int characterIndex;
    private float timePerCharacter;
    private float timer;
    private bool invisibleCharacters;

    public void AddWriter(Text UIText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
    {
        this.UIText = UIText;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        this.invisibleCharacters = invisibleCharacters;
        characterIndex = 0;
    }

    private void Update()
    {
        if(UIText != null)
        {
            
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                
                timer += timePerCharacter;
                characterIndex++;
                string text = textToWrite.Substring(0, characterIndex);

                if (invisibleCharacters)
                {
                    text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                }
                UIText.text = text;

                if (characterIndex >= textToWrite.Length)
                {
                    UIText = null;
                    return;
                }
            }
        }
    }
}
