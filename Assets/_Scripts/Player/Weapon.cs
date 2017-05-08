using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    Rigidbody2D playerRB;
    ScoutController sc;
    private void Start()
    {
        playerRB = GetComponentInParent<Rigidbody2D>();
        sc = GetComponentInParent<ScoutController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Enemy_Fly"))
        {
            sc.FPlay(sc.slashSnd);
            GameManager gman = GameObject.Find("GameManager").GetComponent<GameManager>();

            Vector3 spawnMsg = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject pntGet = Instantiate(gman.normalScorePopup, spawnMsg, Quaternion.identity);
            pntGet.GetComponent<ScorePopup>().pointValue = 200;
            gman.score += 200;
            gman.points_Komori++;
            //playerRB.AddForce(new Vector2(0, 550.0f));
            Destroy(collision.gameObject);

        }

        if (collision.gameObject.tag.Equals("Enemy_Ground"))
        {
            sc.FPlay(sc.slashSnd);

            GameManager gman = GameObject.Find("GameManager").GetComponent<GameManager>();

            Vector3 spawnMsg = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject pntGet = Instantiate(gman.normalScorePopup, spawnMsg, Quaternion.identity);
            pntGet.GetComponent<ScorePopup>().pointValue = 100;

            gman.score += 100;
            gman.points_Lizard++;
            //playerRB.AddForce(new Vector2(0, 550.0f));
            Destroy(collision.gameObject);

        }

        if (collision.gameObject.tag.Equals("Enemy_Under"))
        {
            sc.FPlay(sc.slashSnd);

            GameManager gman = GameObject.Find("GameManager").GetComponent<GameManager>();

            Vector3 spawnMsg = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject pntGet = Instantiate(gman.normalScorePopup, spawnMsg, Quaternion.identity);
            pntGet.GetComponent<ScorePopup>().pointValue = 300;

            gman.score += 300;
            gman.points_Mole++;
            //playerRB.AddForce(new Vector2(0, 550.0f));
            Destroy(collision.gameObject);

        }
    }


}
