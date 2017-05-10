using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour {

    private AudioSource asrc;
    public AudioClip titleSong;
    public AudioClip menuSong;

    public GameObject titleLayer;
    public GameObject controlLayer;
	// Use this for initialization
	void Start () {
        asrc = GetComponent<AudioSource>();
        asrc.clip = titleSong;
        asrc.Play();

        titleLayer.SetActive(true);
        controlLayer.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (titleLayer.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(1);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                SwitchToControls();
            }
        }

        if(controlLayer.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                SwitchToTitle();
            }
        }
    }

    public void SwitchToTitle()
    {
        if(asrc.isPlaying)
            asrc.Stop();

        titleLayer.SetActive(true);
        controlLayer.SetActive(false);
    }

    public void SwitchToControls()
    {
        asrc.clip = menuSong;
        asrc.Play();

        titleLayer.SetActive(false);
        controlLayer.SetActive(true);
    }
}
