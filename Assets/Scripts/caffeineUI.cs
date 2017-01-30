using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class caffeineUI : MonoBehaviour {
    
    private Text uitext;
    public Image meter;

	// Use this for initialization
	void Start () {
        uitext = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        meter.fillAmount = SpookyManager.caffeine / 100f;
	}
}
