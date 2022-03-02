using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriterController : MonoBehaviour
{
    public TextWriter textWriter;
    public Text messageText;

    private void Start()
    {
        Invoke("ImageOne", 0.3f);
        Invoke("ImageTwo", 5);
        Invoke("ImageThree", 10);
        Invoke("ImageFour", 15);
        Invoke("ImageFive", 20);
        Invoke("ImageSix", 30);
        Invoke("ImageSeven", 40);
    }

    void ImageOne()
    {
        textWriter.AddWriter(messageText, "Many years have passed since the last flame gave life to the walls of this old pyramid...", .05f, true);
    }
    void ImageTwo()
    {
        textWriter.AddWriter(messageText, "But, something hidden and long forgetten by all people...", .07f, true);
    }
    void ImageThree()
    {
        textWriter.AddWriter(messageText, "Has just woken up...", .1f, true);
    }
    void ImageFour()
    {
        textWriter.AddWriter(messageText, "Little mummy named Milo gave pyramid its life again...", .08f, true);
    }

    void ImageFive()
    {
        textWriter.AddWriter(messageText, "All alone in the dark tomb inside the pyramid Milo questioned himself what happened to mighty old pyramid...", .08f, true);
    }

    void ImageSix()
    {
        textWriter.AddWriter(messageText, "Looking up at the great walls, Milo noticed some writings on the wall describing what has been going on. But big part of the story was missing...", .06f, true);
    }
    void ImageSeven()
    {
        textWriter.AddWriter(messageText, "So, Milo decided to go on an adventure to find missing pieces and complete the story...", .07f, true);
    }
}
