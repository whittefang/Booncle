using UnityEngine;
using System.Collections;

public class OnActivateChangeColorScript : MonoBehaviour {
	ParticleSystem PS;
	EternalScript ES;
	public int PNum;
	// Use this for initialization
	void Awake () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		PS = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		PS.startColor = ES.GetColor (PNum);
	}
}
