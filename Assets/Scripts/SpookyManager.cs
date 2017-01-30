using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpookyManager : MonoBehaviour {

    public MusicStateManager music;

    public static SpookyManager instance = null;
    public static int caffeine;
    public static int ratings;
    public static int score;
    public static float decrementcaffamount = .35f;
    public static MusicStateManager.GameStates musicstate;
    public static bool iscomcastshit;  //When True, decreases Ratings
    public static bool loadingCrash = true;
    private static float lockouttime;
    private static bool islockedout;  //When True, cannot change music states
    private static float wifispawntime;
    private bool isgameover;

    private float minSpawn;
    private float maxSpawn;
    private float multiplier;
        
    private Animator wifi_Anim;
    public GameObject WiFi; // set in Inspector
    private Animator onAir_Anim;
    public GameObject OnAir; // set in Inspector
    private Animator desk_Anim;
    public GameObject Desk; // set in Inspector
    
    // Use this for initialization
    void Start () {

        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
        
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        instance.wifi_Anim = WiFi.GetComponent<Animator> ();
        instance.onAir_Anim = OnAir.GetComponent<Animator> ();
        instance.desk_Anim = Desk.GetComponent<Animator> ();

        caffeine = 100;
        ratings = 1000;
        score = 0;
        InvokeRepeating("decrementCaffeine", 0, decrementcaffamount);
        musicstate = MusicStateManager.GameStates.NoMusic;
        //musicstate = MusicStateManager.GameStates.Red;
        iscomcastshit = false;
        instance.wifi_Anim.SetBool ("Active", true);
        instance.onAir_Anim.SetBool ("Active", true);
       // InvokeRepeating("testResults", 0, 1.75f);
        lockouttime = 3.0f;
        islockedout = false;

        minSpawn = 5f;
        maxSpawn = 15f;
        instance.StartCoroutine(comcastBS());
        isgameover = false;
	}

    public void stopRatingDrop()    
    {
        StopCoroutine(decrementRatingsWifi());
    }

    void testResults()
    {
        Debug.Log("Caffeine:  " + caffeine);
        Debug.Log("Ratings:  " + ratings); 
    }
	
	// Update is called once per frame
	void Update () {
        if (!iscomcastshit)
        {
            StopCoroutine(decrementRatingsWifi());
            if (!loadingCrash)
            {
                instance.StartCoroutine(comcastBS());
                loadingCrash = true;
            }
        }
        if (ratings <= 0 && !isgameover)
        {
            gameOver();
        }
	}

    public static void disableWifi()
    {
        Debug.Log("Wifi Disabled");
        instance.wifi_Anim.SetBool ("Active", false);
        instance.onAir_Anim.SetBool ("Active", false);
        SpookyManager.iscomcastshit = true;
        instance.invokeRatingDrop();
    }

    public static void fixWifi()
    {
        Debug.Log("Wifi Fixed");
        instance.wifi_Anim.SetBool ("Active", true);
        instance.onAir_Anim.SetBool ("Active", true);
        SpookyManager.iscomcastshit = false;
        loadingCrash = false;
        instance.stopRatingDrop();
    }

    public static void dropRatingbyAmount(int amount)
    {
        ratings -= amount;
    }


    private void decrementCaffeine()
    {
        if (caffeine > 0)
        {
            caffeine--;
        }
    }

    public static int getCaffeine()
    {
        return caffeine;
    }

    public static void addRating(int amount)
    {
        ratings += amount;
    }

    /// <summary>
    /// Return state for movement speed of player
    /// </summary>
    /// <returns> state number 1 through 4 </returns>
    public static int getCaffeineState()
    {
        /**
         * caffeine states:
         * 100 - 75: 4
         * 74 - 50: 3
         * 49 - 25: 2
         * 
         **/
        if (caffeine >= 75)
        {
            return 4;
        }
        else if(caffeine >= 50)
        {
            return 3;
        }
        else if (caffeine >= 25)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// drop the rating count by the given amount
    /// </summary>
    /// <param name="amount"></param>
    public static void dropRatings(int amount)
    {
        Debug.Log("Dropping ratings by:" + amount);
        ratings -= amount;
        if (ratings < 0)
        {
            gameOver();
        }
    }

    public static void restoreCaffeine()
    {

        caffeine = 100;
    }

    public static bool changeState(MusicStateManager.GameStates state)
    {
        if (!islockedout &&  !iscomcastshit)
        {
            if (state != musicstate)
            {
                Debug.Log("Changing state to :" + state.ToString());
                musicstate = state;
                //Invoke all other objects to update various sprites and such
                instance.StartCoroutine(lockoutMusic());
                addRating(20);
                //change Desk animation
                int temp = 0;
                if (state == MusicStateManager.GameStates.Red)
                {
                    temp = 1;
                }
                else if (state == MusicStateManager.GameStates.Green)
                {
                    temp = 3;
                }
                else if (state == MusicStateManager.GameStates.Blue)
                {
                    temp = 2;
                }
                    instance.desk_Anim.SetInteger("SongState", temp);
				return true;

            }
            else
            {
                Debug.Log("Same music State");
            }
        }
        else
        {
            Debug.Log("Locked Out");
		}
		return false;
    }


    static IEnumerator comcastBS()
    {
        if (!iscomcastshit)
        {
            float counter = 0f;
            float mult = Time.time / 2000;
            if (mult > 1f)
            {
                mult = 1f;
            }
            float adjust = 7f * mult;
            float spawn = UnityEngine.Random.Range(instance.minSpawn - adjust, instance.maxSpawn - adjust);
            while (counter < spawn)
            {
                counter += .1f;
                yield return new WaitForSeconds(.1f);
            }
            instance.multiplier = 1f - Time.time / 800;
            if (instance.multiplier < 0.1f)
            {
                instance.multiplier = 0.1f;
            }
            Debug.Log("About to get Comcasted");

            instance.onAir_Anim.SetBool("Active", false);
            //Initiate Cues for Wifi going down.
            yield return new WaitForSeconds(2.5f);
            if (!iscomcastshit)
            {
                disableWifi();
            }
        }
    }

    static IEnumerator lockoutMusic()
    {
        Debug.Log("StartedLockout  " + islockedout);
        islockedout = true;
        yield return new WaitForSeconds(lockouttime);
        islockedout = false;
    }

    public void invokeRatingDrop()
    {
        StartCoroutine(decrementRatingsWifi());
    }

    IEnumerator decrementRatingsWifi()
    {
        if (iscomcastshit)
        {
            if (ratings >= 5)
            {
                ratings -= 5;
                yield return new WaitForSeconds(.35f);
            }
            else
            {
                iscomcastshit = false;
                gameOver();
            }
        }
    }
    static void gameOver()
    {
        instance.isgameover = true;
        musicstate = MusicStateManager.GameStates.GameOver;
        instance.StopAllCoroutines();
        Debug.Log("Destroi everything");
        SceneManager.LoadScene(4);

        //var x = SceneManager.GetSceneByName("TitleScreen");
        //SceneManager.SetActiveScene(x);
    }
}
