using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public bool deadPlayer = false;
    void Start()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void LateUpdate () {
		if (player != null && !deadPlayer) {
			transform.position = new Vector3 (player.transform.position.x + 2.75f, player.transform.position.y, player.transform.position.z - 1);
		}
	}    

}
