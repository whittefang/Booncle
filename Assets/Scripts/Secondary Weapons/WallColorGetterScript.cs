using UnityEngine;
using System.Collections;

public class WallColorGetterScript : MonoBehaviour {
	public ColorMorphingRedScript CMRS;
	public ColorMorphingBlueScript CMBS;
	SpriteRenderer Scolor;
	// Use this for initialization
	void Start () {
		Scolor = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (CMRS != null) {
			Scolor.color = CMRS.GetColor ();
		} else {
			Scolor.color = CMBS.GetColor ();
		}
	}
}
