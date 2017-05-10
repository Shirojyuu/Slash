using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCounter : MonoBehaviour {

    public Sprite heartImage;
    public GameObject[] lifeIcons;
    private GameManager gman;
	// Use this for initialization
	void Start () {
        gman = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log(lifeIcons);
	}
	
	// Update is called once per frame
	void Update () {
	    switch(gman.life)
        {
            case 0:
                if (lifeIcons[0] != null)
                    lifeIcons[0].GetComponent<Animator>().Play("LoseHeart");
                break;

            case 1:
                if(lifeIcons[1] != null)
                    lifeIcons[1].GetComponent<Animator>().Play("LoseHeart");
                break;
        }
	}

   
}
