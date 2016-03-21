using UnityEngine;
using System.Collections;

public class PlayerControlScriptController : MonoBehaviour {

	Rigidbody2D RB;
	LineRenderer LR;
	public GameObject projectile;
	public Transform GunPoint;
	public float Haxis, Vaxis;
	
	// Use this for initialization
	void Start () {
		RB = GetComponent<Rigidbody2D> ();
		LR = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frameS
	void Update () {

		// axis crontrols for horizontal movement
		if (Input.GetAxis ("HorizontalP2") > .05) {
			RB.velocity = new Vector3 (5, RB.velocity.y, 0);
		} else if (Input.GetAxis ("HorizontalP2") < -.05) {
			RB.velocity = new Vector3 (-5, RB.velocity.y, 0);
		}else {
			RB.velocity = new Vector3 (0, RB.velocity.y, 0);
		}
		
		// axis controls for vertical movement
		if (Input.GetAxis ("VerticalP2") > .05) {
			RB.velocity = new Vector3 (RB.velocity.x, 5, 0);
		} else if (Input.GetAxis ("VerticalP2") < -.05) {
			RB.velocity = new Vector3 (RB.velocity.x, -5, 0);
		}else {
			RB.velocity = new Vector3 (RB.velocity.x, 0, 0);
		}
		
		// input for shooting
		if (Input.GetButtonDown("Fire2")){
			ShootBulet();
		}
		
		transform.rotation = PointerRotation ();
		LineControl ();
		Haxis = Input.GetAxis ("S2Horizontal");
		Vaxis = Input.GetAxis ("S2Vertical");
	}
	
	// return the angle to look at the pointer from this object
	Quaternion PointerRotation(){

		//return Quaternion.AngleAxis((Mathf.Atan2(Input.GetAxis("S2Horizontal"), Input.GetAxis("S2Vertical")) * Mathf.Rad2Deg) + 180, Vector3.forward);
		return Quaternion.Euler (0, 0, (Mathf.Atan2 (Input.GetAxis ("S2Horizontal"), Input.GetAxis ("S2Vertical")) * Mathf.Rad2Deg) + 180);
	}
	
	//funciton to create and then fire projectile
	void ShootBulet(){
		Instantiate (projectile, GunPoint.position, PointerRotation ());
	}

	// function for controlling line renderer 
	void LineControl(){
		RaycastHit2D hit = Physics2D.Raycast(GunPoint.position, transform.up);
		if (hit.collider != null) {
			LR.SetPosition (1, hit.point);
		}
		LR.SetPosition (0, transform.position);
	}
}
