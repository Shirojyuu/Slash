using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public uint score = 0;
    public uint life = 2;

    public uint transformationLevel = 0;
    public uint points_Komori = 0;
    public uint points_Lizard = 0;
    public uint points_Mole = 0;

    public Image faderImage;
    public float deathRestartCounter = 4.5f;
    public bool death = false;

    public GameObject normalScorePopup;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(death)
        {
            deathRestartCounter -= Time.deltaTime;
        }

        if(deathRestartCounter <= 3.5f)
        {
            float nAlpha = faderImage.color.a + 0.05f;
            faderImage.color = new Color(faderImage.color.r, faderImage.color.g, faderImage.color.b, nAlpha);
        }

        if(deathRestartCounter <= 1.5f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }

}
