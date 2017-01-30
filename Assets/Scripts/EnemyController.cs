using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
            
    public GameObject control;  //set this in the inspector
    public GameObject player;   //set this in the inspector
    public MusicStateManager.GameStates songPref;// = MusicStateManager.GameStates.Blue;//change in inspector
    public enum SongReact { Hate, Neutral, Love };
    Dictionary<MusicStateManager.GameStates, SongReact> feelings = new Dictionary<MusicStateManager.GameStates, SongReact>();
    public bool hasBeenSpawned = false;

    //public SpookyManager spookManage;
    Vector2 dir;
    float velScale = 1f;
    float goalScale = 1f;
    float activeScale = 1f;
    bool waitingForCycle = false;
    bool delayLoss = false;
    bool touchingControl = false;

    SongReact currentFeeling;
    

    // Use this for initialization
    void Start()
    {
        if (control == null)
        {
            Debug.LogError("Set an object for the control.");
        }
        if (player == null)
        {
            //Debug.LogError("Set an object for the player.");
        }
        if (songPref == 0)
        {
            //Debug.LogError("Set a song preference");
        }
        if(!feelings.ContainsKey(MusicStateManager.GameStates.Red))
        {
            feelings[MusicStateManager.GameStates.Red] = SongReact.Neutral;
        }
        if (!feelings.ContainsKey(MusicStateManager.GameStates.Green))
        {
            feelings[MusicStateManager.GameStates.Green] = SongReact.Neutral;
        }
        if (!feelings.ContainsKey(MusicStateManager.GameStates.Blue))
        {
            feelings[MusicStateManager.GameStates.Blue] = SongReact.Neutral;
        }
        feelings[songPref] = SongReact.Love;
        //setHatedSongs(new List<MusicStateManager.GameStates>() { MusicStateManager.GameStates.Green });//only for testing, REMOVE at end
        //dir = Vector2.right;
        dir = control.transform.position - transform.position;
        dir.y = 0f;
        dir.Normalize();
        //feelings.TryGetValue(SpookyManager.musicstate, out currentFeeling);
    }

    // Update is called once per frame
    void Update()
    {
        DebugFeelings();
        if(!(songPref == MusicStateManager.GameStates.Red || songPref == MusicStateManager.GameStates.Green || songPref == MusicStateManager.GameStates.Blue))
        {
            Debug.Log("No song pref");
        }
        checkStuff();
        //Debug.Log("v: " + velScale + " g:" + goalScale + " a: " + activeScale);
        if (this.enabled)
        {
            activeScale = 1f;
        }
        else
        {
            activeScale = 0f;
        }

        if (feelings.ContainsKey(SpookyManager.musicstate))
        {
            currentFeeling = feelings[SpookyManager.musicstate];
        }
        else
        {
            currentFeeling = SongReact.Neutral;
        }
        if (currentFeeling == SongReact.Love)
        {
            velScale = -1f;
            goalScale = 1f;
            touchingControl = false;
        }
        else if (currentFeeling == SongReact.Neutral)
        {
            velScale = 0f;
        }
        else
        {
            velScale = 1f;
        }
        if (!waitingForCycle)
        {
            StartCoroutine("moveBurst");
            waitingForCycle = true;
        }
        if (touchingControl && !delayLoss)
        {
            StartCoroutine("scare");
        }
        if(transform.position.x < -25 || transform.position.x > 24)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void setHatedSongs(List<MusicStateManager.GameStates> hated)
    {
        if (!feelings.ContainsKey(MusicStateManager.GameStates.Red))
        {
            feelings[MusicStateManager.GameStates.Red] = SongReact.Neutral;
        }
        if (!feelings.ContainsKey(MusicStateManager.GameStates.Green))
        {
            feelings[MusicStateManager.GameStates.Green] = SongReact.Neutral;
        }
        if (!feelings.ContainsKey(MusicStateManager.GameStates.Blue))
        {
            feelings[MusicStateManager.GameStates.Blue] = SongReact.Neutral;
        }
        foreach (MusicStateManager.GameStates h in hated)
        {
            feelings[h] = SongReact.Hate;
            //Debug.Log("Hating added " + h.ToString() +". This is color " + this.songPref.ToString());
        }
        feelings[songPref] = SongReact.Love;
        //Debug.Log("In hating: " + "Red: " + feelings[MusicStateManager.GameStates.Red].ToString()
            //+ " Green: " + feelings[MusicStateManager.GameStates.Green].ToString()
            //+ " Blue: " + feelings[MusicStateManager.GameStates.Blue].ToString());/*
    }

    void initializeSong()
    {
        //int randSelector = (int)(Random.Range(0.5f, 1.5f));
        
    }
    void OnTriggerEnter2D (Collider2D c)
    {
        if (c.gameObject.tag.Equals("Controller"))
        {
            StartCoroutine("scare");
            touchingControl = true;
            if (currentFeeling != SongReact.Love)
            {
                goalScale = 0f;
            }
        }
    }
    IEnumerator moveBurst()
    {
        //dir = transform.position - control.transform.position;
        //dir.Normalize();
        transform.position = transform.position + (Vector3)(dir * velScale * goalScale * activeScale * .25f);
        yield return new WaitForSeconds(.1f);
        waitingForCycle = false;
    }
    IEnumerator scare()
    {
        delayLoss = true;
        yield return new WaitForSeconds(.2f);
        SpookyManager.dropRatingbyAmount(2);
        delayLoss = false;
    }
    public void DebugFeelings()
    {
        //Debug.Log("Red: " + feelings[MusicStateManager.GameStates.Red].ToString()
        //    + " Green: " + feelings[MusicStateManager.GameStates.Green].ToString()
        //    + " Blue: " + feelings[MusicStateManager.GameStates.Blue].ToString());
    }
    void checkStuff()
    {
        int count = 0;
        if(!feelings.ContainsKey(MusicStateManager.GameStates.Red))
        {
            feelings.Add(MusicStateManager.GameStates.Red, SongReact.Neutral);
        }
        if (!feelings.ContainsKey(MusicStateManager.GameStates.Green))
        {
            feelings.Add(MusicStateManager.GameStates.Green, SongReact.Neutral);
        }
        if (!feelings.ContainsKey(MusicStateManager.GameStates.Blue))
        {
            feelings.Add(MusicStateManager.GameStates.Blue, SongReact.Neutral);
        }

        if (feelings[MusicStateManager.GameStates.Red] == SongReact.Neutral)
        {
            count++;
        }
        if (feelings[MusicStateManager.GameStates.Green] == SongReact.Neutral)
        {
            count++;
        }
        if (feelings[MusicStateManager.GameStates.Blue] == SongReact.Neutral)
        {
            count++;
        }
        if(count==3)
        {
            feelings[songPref] = SongReact.Love;
            float rand = Random.Range(0, 1f);
            if(rand<0.5f)
            {
                if(songPref == MusicStateManager.GameStates.Red)
                {
                    feelings[MusicStateManager.GameStates.Green] = SongReact.Hate;
                }
                if (songPref == MusicStateManager.GameStates.Green)
                {
                    feelings[MusicStateManager.GameStates.Blue] = SongReact.Hate;
                }
                if (songPref == MusicStateManager.GameStates.Blue)
                {
                    feelings[MusicStateManager.GameStates.Red] = SongReact.Hate;
                }
            }
            else
            {
                if (songPref == MusicStateManager.GameStates.Red)
                {
                    feelings[MusicStateManager.GameStates.Blue] = SongReact.Hate;
                }
                if (songPref == MusicStateManager.GameStates.Green)
                {
                    feelings[MusicStateManager.GameStates.Red] = SongReact.Hate;
                }
                if (songPref == MusicStateManager.GameStates.Blue)
                {
                    feelings[MusicStateManager.GameStates.Green] = SongReact.Hate;
                }
            }
        }
    }
}
