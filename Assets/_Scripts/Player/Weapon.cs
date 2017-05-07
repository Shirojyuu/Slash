﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Enemy_Fly"))
        {
            GameManager gman = GameObject.Find("GameManager").GetComponent<GameManager>();

            Vector3 spawnMsg = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject pntGet = Instantiate(gman.normalScorePopup, spawnMsg, Quaternion.identity);
            pntGet.GetComponent<ScorePopup>().pointValue = 200;
            gman.score += 200;
            Destroy(collision.gameObject);

        }
    }


}
