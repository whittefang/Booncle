using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnActivateSetColorScript : MonoBehaviour {
	ParticleSystem PS;
	EternalScript ES;
	public int PNum;
	// Use this for initialization
	void Awake () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		GetComponent<Image> ().color = ES.GetColor (PNum);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		

}
