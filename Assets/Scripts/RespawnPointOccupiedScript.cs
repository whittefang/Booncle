using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RespawnPointOccupiedScript : MonoBehaviour {
	public bool IsOccupied;
	public List<GameObject> ObjectList;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		for (var i = ObjectList.Count - 1; i > -1; i--) {
			if (ObjectList [i] == null) {
				ObjectList.RemoveAt (i);
			}
		}
		if (ObjectList.Count == 0){
			IsOccupied = false;
		} else {
			IsOccupied = true;
		}

	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			bool alreadyExists = false;
			for (var i = ObjectList.Count - 1; i > -1; i--) {
				if (other.gameObject == ObjectList[i]){
					alreadyExists = true;
				}
			}
			if (!alreadyExists){
				ObjectList.Add(other.gameObject);
			}
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			for (var i = ObjectList.Count - 1; i > -1; i--) {
				if (other.gameObject == ObjectList[i]){
					ObjectList.RemoveAt (i);
				}
			}
		}
	}

	public bool CheckOccupied(){

		return IsOccupied;
	}
}
