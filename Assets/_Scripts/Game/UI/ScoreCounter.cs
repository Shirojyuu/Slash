using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreCounter : MonoBehaviour {

    private TextMeshProUGUI tmpro;
    private GameManager gman;
	// Use this for initialization
	void Start () {
        tmpro = GetComponent<TextMeshProUGUI>();
        gman = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        tmpro.text = gman.score.ToString("D7");
	}
}
