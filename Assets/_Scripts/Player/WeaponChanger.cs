using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour {
    //Weapon 0 is always the base/starter weapon.
    public List<GameObject> weapons;

    private ParticleSystem ps;

    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();

        foreach (Transform child in transform)
            weapons.Add(child.gameObject);

        TransformWeapon(0);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TransformWeapon(int id)
    {
        ps.Play();
        weapons[id].SetActive(true);

        for (int i = 0; i < weapons.Count; i++)
        {
            if (i != id)
                weapons[i].SetActive(false);
        }
    }
}
