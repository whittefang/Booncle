using UnityEngine;
using System.Collections;

public class SelfDestructScript : MonoBehaviour {
	public float timeTillDeath;
	// Use this for initialization
	void Start () {
	
	}
	void OnEnable(){
		Invoke ("DestroySelf", timeTillDeath);
	}
	void DestroySelf(){
		gameObject.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
