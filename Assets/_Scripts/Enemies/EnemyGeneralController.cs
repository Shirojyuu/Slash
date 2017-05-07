using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneralController : MonoBehaviour {
    public enum EnemyType { FLY, GROUND, EARTH };

    public EnemyType type;
    private GameManager gman;
	// Use this for initialization
	void Start () {
        gman = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        if (type == EnemyType.FLY)
            gman.points_Komori++;


    }
}
