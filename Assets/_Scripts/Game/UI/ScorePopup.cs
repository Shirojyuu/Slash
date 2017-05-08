using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScorePopup : MonoBehaviour {
    public TextMeshPro tmesh;
    public float distance;
    public int pointValue;
    public Color scoreColor;
    // Use this for initialization
    void Start () {
        tmesh = GetComponent<TextMeshPro>();
        tmesh.text = pointValue.ToString();
        if (scoreColor != null)
            tmesh.color = scoreColor;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0.0f, 0.075f));

        float alpha = tmesh.color.a;
        if(alpha > 0.0f)
            tmesh.color = new Color(tmesh.color.r, tmesh.color.g, tmesh.color.b, alpha -= 0.02f);

        if (alpha <= 0.1f)
            Destroy(gameObject);

	}
}
