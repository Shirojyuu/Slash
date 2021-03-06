﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneralController : MonoBehaviour {
    public enum EnemyType { FLY, FLY2, GROUND, EARTH };

    public EnemyType type;
    public bool onScreen;

    private bool seen = false;
    private GameManager gman;
	// Use this for initialization
	void Start () {
        gman = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        onScreen = OSTest();
        if (onScreen)
        {
            seen = true;
            if (type == EnemyType.FLY)
            {
                transform.Translate(new Vector3(-4.5f * Time.deltaTime, 0.0f));
            }

            if (type == EnemyType.FLY2)
            {
                transform.Translate(new Vector3(-9.5f * Time.deltaTime, Mathf.Sin(Time.deltaTime * 2.0f)));
            }

            if (type == EnemyType.GROUND)
            {
                transform.Translate(new Vector3(-2.5f * Time.deltaTime, 0.0f));
            }
        }

        if (seen && !onScreen)
            Destroy(gameObject);
    }

    private bool OSTest()
    {
        if (GetComponentInChildren<SkinnedMeshRenderer>().isVisible)
            return true;

        return false;
    }
  
}
