using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Request
{
    public GameObject go;
    public float time;
    public MusicStateManager.GameStates color;
    public Request(GameObject g, MusicStateManager.GameStates c)
    {
        go = g;
        color = c;
    }

    public void RandomTime(float min, float max)
    {
        time = Random.Range(min, max);
    }
}

public class RequestManager : MonoBehaviour
{
    public GameObject redSprite;
    public GameObject greenSprite;
    public GameObject blueSprite;
    GameObject colorRequest;

    Request red;
    Request green;
    Request blue;

    Request currentRequest;

    float minSpawn = 10f;
    float maxSpawn = 15f;

    bool waitSpawn = false;
    bool waitRequest = false;

    float multiplier = 1f;

    // Use this for initialization
    void Start()
    {
        red = new Request(redSprite, MusicStateManager.GameStates.Red);
        green = new Request(greenSprite, MusicStateManager.GameStates.Green);
        blue = new Request(blueSprite, MusicStateManager.GameStates.Blue);
        if(redSprite == null)
        {
            Debug.LogError("No red sprite attached");
        }
        if (greenSprite == null)
        {
            Debug.LogError("No green sprite attached");
        }
        if (blueSprite == null)
        {
            Debug.LogError("No blue sprite attached");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitSpawn)
        {
            waitSpawn = true;
            StartCoroutine("countdownSpawn");
        }
        /*if (!waitRequest && currentRequest != null)
        {
            waitRequest = true;
            StartCoroutine("countdownRequest");
        }*/
    }

    void spawnRequest()
    {
        bool match = true;
        while (match)
        {
            float decider = Random.Range(0f, 1f);
            if (decider < 0.3333f)
            {
                currentRequest = red;
            }
            else if (decider < 0.6666f)
            {
                currentRequest = green;
            }
            else
            {
                currentRequest = blue;
            }
            if(currentRequest.color != SpookyManager.musicstate)
            {
                match = false;
            }
        }
        currentRequest.RandomTime(5f * multiplier, 7f * multiplier);
        //add sprite appearance
        if (currentRequest != null)
        {
            colorRequest = Instantiate(currentRequest.go);
            colorRequest.transform.position = new Vector3(0, 0, 0);//filler position
        }
        Debug.Log("Square should be there");
        Debug.Log("Playing: "+SpookyManager.musicstate.ToString() + ", Requested: " + currentRequest.color.ToString());
        StartCoroutine("countdownRequest");
    }

    IEnumerator countdownSpawn()
    {
        float counter = 0f;
        float mult = Time.time / 2000;
        if (mult > 1f)
        {
            mult = 1f;
        }
        float adjust = 7f * mult;
        float spawn = Random.Range(minSpawn - adjust, maxSpawn - adjust);
        while (counter < spawn)
        {
            counter += .1f;
            yield return new WaitForSeconds(.1f);
        }
        multiplier = 1f - Time.time / 1000;
        if (multiplier < 0.1f)
        {
            multiplier = 0.1f;
        }

        spawnRequest();
        
    }

    IEnumerator countdownRequest()
    {
        float counter = 0f;
        float request = currentRequest.time;
        //indicator
        while (counter < request && currentRequest != null)
        {
            if (SpookyManager.musicstate == currentRequest.color)
            {
                SpookyManager.addRating(40);
                currentRequest = null;
            }
            counter += .1f;
            yield return new WaitForSeconds(.1f);
        }
        if (currentRequest != null)
        {
            SpookyManager.dropRatingbyAmount(100);
            Debug.Log("Rating fell");
            currentRequest = null;
        }
        Destroy(colorRequest);
        waitSpawn = false;
        waitRequest = false;
    }
}
