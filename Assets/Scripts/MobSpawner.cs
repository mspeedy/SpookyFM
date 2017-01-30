using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour {

    public GameObject[] MonsterPrefabs; // set in Inspector
    public GameObject[] HardMonsterPrefabs; // set in inspector

    public List<GameObject> MonsterPool; // set in Inspector
    public MusicStateManager SongManager; // set in Inspector

    public int pooledMobsPerType = 5;

    bool spawning = false;
    MusicStateManager.GameStates RedHate = MusicStateManager.GameStates.Green;
    MusicStateManager.GameStates GreenHate = MusicStateManager.GameStates.Blue;
    MusicStateManager.GameStates BlueHate = MusicStateManager.GameStates.Red;

    // Use this for initialization
    void Start () {
        foreach (GameObject go in MonsterPrefabs) {
            EnemyController ec = go.GetComponent<EnemyController> ();

            float rngsus = Random.value;
            MusicStateManager.GameStates HatedSong = MusicStateManager.GameStates.NoMusic;

            if (rngsus > 0.5f) {
                switch (ec.songPref) {
                case MusicStateManager.GameStates.Red:
                    RedHate = MusicStateManager.GameStates.Green;
                        HatedSong = RedHate;
                    break;
                case MusicStateManager.GameStates.Green:
                    GreenHate = MusicStateManager.GameStates.Blue;
                        HatedSong = GreenHate;
                        break;
                case MusicStateManager.GameStates.Blue:
                    BlueHate = MusicStateManager.GameStates.Red;
                        HatedSong = BlueHate;
                        break;
                }
            } else {
                switch (ec.songPref) {
                case MusicStateManager.GameStates.Red:
                    RedHate = MusicStateManager.GameStates.Blue;
                        HatedSong = RedHate;
                        break;
                case MusicStateManager.GameStates.Blue:
                    GreenHate = MusicStateManager.GameStates.Green;
                        HatedSong = BlueHate;
                        break;
                case MusicStateManager.GameStates.Green:
                        RedHate = MusicStateManager.GameStates.Red;
                            HatedSong = GreenHate;
                    break;
                }
            }

            
            for (int i = 0; i < pooledMobsPerType; i++) {
                GameObject obj = (GameObject) Instantiate (go);
                obj.transform.position = transform.position;
                //ec.setHatedSongs(new List<MusicStateManager.GameStates> { HatedSong });
                obj.GetComponent<EnemyController>().setHatedSongs(new List<MusicStateManager.GameStates> { HatedSong });
                obj.SetActive (false);
                MonsterPool.Add (obj);
            }
        }
        StartCoroutine("initialPause");        
	}
	
    // Use this for introducing monsters that hate everything
    void enterTheHaters () {
        foreach (GameObject go in MonsterPrefabs) {
            EnemyController ec = go.GetComponent<EnemyController> ();

            float rngsus = Random.value;
            List<MusicStateManager.GameStates> HatedSongs = null;

            switch (ec.songPref) {
            case MusicStateManager.GameStates.Red:
                HatedSongs = new List<MusicStateManager.GameStates>{MusicStateManager.GameStates.Green,MusicStateManager.GameStates.Blue};
                break;
            case MusicStateManager.GameStates.Green:
                HatedSongs = new List<MusicStateManager.GameStates>{MusicStateManager.GameStates.Red,MusicStateManager.GameStates.Blue};
                break;
            case MusicStateManager.GameStates.Blue:
                HatedSongs = new List<MusicStateManager.GameStates>{MusicStateManager.GameStates.Green,MusicStateManager.GameStates.Red};
                break;
            }

            for (int i = 0; i < pooledMobsPerType; i++) {
                GameObject obj = (GameObject)Instantiate (go);
                ec.setHatedSongs (HatedSongs);
                obj.SetActive (false);
                MonsterPool.Add (obj);
            }
        }        
    }

    void Update()
    {
        if(spawning)
        {
            StartCoroutine("spawnFoe");
            spawning = false;
        }
    }

    void spawn() {
        for(int i = 0; i < MonsterPool.Count; i++) {
            if (!MonsterPool[i].activeInHierarchy) {
                // Instantiate Monster from Pool
                break;
            }
        }
    }

    IEnumerator spawnFoe()
    {
        bool found = false;
        while (!found)
        {
            int choose = (int)Random.Range(0, MonsterPool.Count);
            if (!MonsterPool[choose].activeSelf)
            {
               //Transform [] temp = MonsterPool[choose].GetComponentsInChildren<Transform>();
                MonsterPool[choose].transform.position = transform.position;
                if (MonsterPool[choose].tag.Equals("Offset"))
                {
                    MonsterPool[choose].transform.position += new Vector3(0,.5f,0);
                    Debug.Log("Should work.");
                }
                if(MonsterPool[choose].transform.position.x < 0 && MonsterPool[choose].transform.localScale.x > 0)
                {
                    MonsterPool[choose].transform.localScale = new Vector3(MonsterPool[choose].transform.localScale.x  * - 1, MonsterPool[choose].transform.localScale.y);
                }
                else if (MonsterPool[choose].transform.position.x > 0 && MonsterPool[choose].transform.localScale.x < 0)
                {
                    MonsterPool[choose].transform.localScale = new Vector3(MonsterPool[choose].transform.localScale.x * -1, MonsterPool[choose].transform.localScale.y);
                }
                MonsterPool[choose].SetActive(true);
                Debug.Log("Spawned a " + MonsterPool[choose].name + " at " + MonsterPool[choose].transform.position +" with pref "+ MonsterPool[choose].GetComponent<EnemyController>().songPref.ToString());
                if (!MonsterPool[choose].GetComponent<EnemyController>().hasBeenSpawned)
                {
                    MonsterPool[choose].GetComponent<EnemyController>().hasBeenSpawned = true;
                    switch (MonsterPool[choose].GetComponent<EnemyController>().songPref)
                    {
                        case MusicStateManager.GameStates.Red:
                            MonsterPool[choose].GetComponent<EnemyController>().setHatedSongs(new List<MusicStateManager.GameStates> { RedHate });
                            Debug.Log("Should be hating");
                            break;
                        case MusicStateManager.GameStates.Green:
                            MonsterPool[choose].GetComponent<EnemyController>().setHatedSongs(new List<MusicStateManager.GameStates> { GreenHate });
                            Debug.Log("Should be hating");
                            break;
                        case MusicStateManager.GameStates.Blue:
                            MonsterPool[choose].GetComponent<EnemyController>().setHatedSongs(new List<MusicStateManager.GameStates> { BlueHate });
                            Debug.Log("Should be hating");
                            break;
                    }
                }
                //MonsterPool[choose].GetComponent<EnemyController>().DebugFeelings();
                found = true;
            }
        }
        yield return new WaitForSeconds(Random.Range(9f, 11f));//change this later to be flexible!
        spawning = true;
    }
    IEnumerator initialPause()
    {
        yield return new WaitForSeconds(15f);
        spawning = true;
    }
}
