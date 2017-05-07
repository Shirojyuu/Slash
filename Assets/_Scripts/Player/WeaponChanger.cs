using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour {
    //Weapon 0 is always the base/starter weapon.
    public List<GameObject> weapons;
    public GameObject activeWeapon;

    private ScoutController player;
    private GameManager gman;
    private ParticleSystem ps;

    // Use this for initialization
    void Start () {
        player = GetComponentInParent<ScoutController>();
        gman = GameObject.Find("GameManager").GetComponent<GameManager>();
        ps = GetComponent<ParticleSystem>();

        foreach (Transform child in transform)
            weapons.Add(child.gameObject);

        TransformWeapon(0);

	}
	
	// Update is called once per frame
	void Update () {
		if(gman.points_Komori == 3)
        {
            TransformWeapon(1);
            gman.points_Lizard = 0;
            gman.points_Mole = 0;
            gman.points_Komori = 0;
        }
	}

    public void TransformWeapon(int id)
    {
        ps.Play();
        weapons[id].SetActive(true);
        activeWeapon = weapons[id];
        player.EnableAbility(id);

        for (int i = 0; i < weapons.Count; i++)
        {
            if (i != id)
                weapons[i].SetActive(false);
        }
    }
}
