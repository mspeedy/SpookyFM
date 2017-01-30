using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorUI : MonoBehaviour {

    public Sprite redNote;
    public Sprite greenNote;
    public Sprite blueNote;
    public Image image;

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpookyManager.musicstate == MusicStateManager.GameStates.Red)
        {
            image.sprite = redNote;   
        }
        else if (SpookyManager.musicstate == MusicStateManager.GameStates.Blue)
        {
            image.sprite = blueNote;
        }
        else if (SpookyManager.musicstate == MusicStateManager.GameStates.Green)
        {
            image.sprite = greenNote;
        }

    }
}
