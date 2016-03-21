using UnityEngine;
using System.Collections;

public class TmpTImeStop : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			if (Time.timeScale != 1){
				Time.timeScale = 1;
			}else {
				Time.timeScale = .1f;
			}
		}
	}
}
