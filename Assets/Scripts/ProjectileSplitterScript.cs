using UnityEngine;
using System.Collections;

public class ProjectileSplitterScript : MonoBehaviour {
	Quaternion CurrentRot;
	public GameObject PrefabBullet;
	bool waitPeriod = true;
	public int splitNumber = 0;
	// Use this for initialization
	void Start () {
		waitPeriod = true;
		Invoke ("EndWait", .1f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (splitNumber < 2){

			if (CurrentRot != transform.rotation && !waitPeriod){
				Debug.Log (CurrentRot + "     " + transform.rotation);
				Split();
			}
			CurrentRot = transform.rotation;
		}
	}
	void EndWait(){
		waitPeriod = false;
	}
	void Split(){
		//GameObject TmpBullet = Instantiate (projectile, GunPoint, PointerRotation) as GameObject;
		GameObject tmp = Instantiate (PrefabBullet, transform.position, transform.rotation) as GameObject;
		tmp.transform.Rotate (new Vector3 (0, 0, -2));
		tmp.GetComponent<ProjectileSplitterScript> ().splitNumber = splitNumber + 1;

		GameObject tmp2 = Instantiate (PrefabBullet, transform.position, transform.rotation) as GameObject;
    	tmp2.transform.Rotate (new Vector3 (0, 0, 2));
		tmp2.GetComponent<ProjectileSplitterScript> ().splitNumber = splitNumber + 1;
		Destroy (gameObject);
	}
}
