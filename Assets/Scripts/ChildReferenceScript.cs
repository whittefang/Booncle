using UnityEngine;
using System.Collections;

public class ChildReferenceScript : MonoBehaviour {
	public GameObject PlayerReference;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public GameObject getObj(){
		return PlayerReference;
	}
}
