using UnityEngine;
using System.Collections;

public class RandomShootingScript : MonoBehaviour {

	public GameObject projectile, Trail;
	public float Delay;
	public float FireRate;
	// Use this for initialization
	void Start () {
		Invoke ("Shoot", Delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void Shoot(){
		GameObject tmp =Instantiate (projectile, transform.position, Quaternion.Euler(0,0,Random.Range(0f, 360f))) as GameObject;
		GameObject TmpTrail = Instantiate (Trail, transform.position, Quaternion.identity) as GameObject;
		TmpTrail.GetComponent<TrailFollowScript>().SetThingToFollow(tmp.transform);
		//tmp.GetComponentInChildren<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
		Invoke ("Shoot", FireRate);
	}
}
