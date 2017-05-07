using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneralController : MonoBehaviour {
    public enum EnemyType { FLY, GROUND, EARTH };

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
