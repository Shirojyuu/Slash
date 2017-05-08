using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    Rigidbody2D playerRB;
    private void Start()
    {
        playerRB = GetComponentInParent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Enemy_Fly"))
        {
            GameManager gman = GameObject.Find("GameManager").GetComponent<GameManager>();

            Vector3 spawnMsg = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject pntGet = Instantiate(gman.normalScorePopup, spawnMsg, Quaternion.identity);
            pntGet.GetComponent<ScorePopup>().pointValue = 200;
            pntGet.GetComponent<ScorePopup>().scoreColor = new Color(138, 43, 226);
            gman.score += 200;
            gman.points_Komori++;
            //playerRB.AddForce(new Vector2(0, 550.0f));
            Destroy(collision.gameObject);

        }

        if (collision.gameObject.tag.Equals("Enemy_Ground"))
        {
            GameManager gman = GameObject.Find("GameManager").GetComponent<GameManager>();

            Vector3 spawnMsg = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject pntGet = Instantiate(gman.normalScorePopup, spawnMsg, Quaternion.identity);
            pntGet.GetComponent<ScorePopup>().pointValue = 100;
            pntGet.GetComponent<ScorePopup>().scoreColor = new Color(255, 127, 80);

            gman.score += 100;
            gman.points_Lizard++;
            //playerRB.AddForce(new Vector2(0, 550.0f));
            Destroy(collision.gameObject);

        }
    }


}
