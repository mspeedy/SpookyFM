using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingsUI : MonoBehaviour {

    private RectTransform trans;
    private List<Image> display = new List<Image>();

    public Image digit;
    public List<Sprite> digits;

    public int xOffset = 30;

    void Start() {
        trans = GetComponent<RectTransform> ();
    }

    // Update is called once per frame
    void Update()
    {
        int ratings = SpookyManager.ratings > 0 ? SpookyManager.ratings : 0;
        int[] intArray = GetIntArray (ratings);
        int numDigits = ratings.ToString ().Length;

        // Fill out display if needed
        if (display.Count < numDigits) {
            for (int i = display.Count; i < numDigits; i++) {
                Image img = Instantiate (digit) as Image;
                img.rectTransform.SetParent (trans);
                img.rectTransform.anchoredPosition = new Vector2 (170f + (xOffset * i), -15f);
                display.Add (img);
            }
        }


        List<Image> toRemove = new List<Image> ();
        // Update Images to display correct score
        foreach (Image img in display) {
            if (display.IndexOf(img) >= intArray.Length) {
                toRemove.Add (img);
            } else {
                img.sprite = digits [intArray [display.IndexOf (img)]];
            }
        }
        foreach (Image img in toRemove) {
            display.Remove (img);
        }
            



        for (int i = 0; i < display.Count; i++) {
            if (i >= intArray.Length) {
                // Disappearify
                display.RemoveAt(intArray.Length);
            }
            else {
                display [i].sprite = digits [intArray [i]];
            }
        }
    }

    int[] GetIntArray(int num)
    {
        List<int> listOfInts = new List<int>();
        while(num > 0)
        {
            listOfInts.Add(num % 10);
            num = num / 10;
        }
        listOfInts.Reverse();
        return listOfInts.ToArray();
    }
}
