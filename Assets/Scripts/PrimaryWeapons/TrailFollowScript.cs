using UnityEngine;
using System.Collections;

public class TrailFollowScript : MonoBehaviour {
	public Transform ThingTofollow;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (ThingTofollow != null) {
			if (ThingTofollow.gameObject.activeSelf == false) {
				StopFollowing();
			}else {
				transform.position = ThingTofollow.position;
			}
		}


	}
	public void SetThingToFollow(Transform Thing){
		ThingTofollow = Thing;
		Invoke("StopFollowing", ThingTofollow.GetComponent<ProjectileScript>().SelfDestruct);
	}
	void StopFollowing(){
		ThingTofollow = null;
	}
}
