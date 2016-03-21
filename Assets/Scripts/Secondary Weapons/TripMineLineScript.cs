using UnityEngine;
using System.Collections;

public class TripMineLineScript : MonoBehaviour {
	LineRenderer LR;
	bool active = false;
	// Use this for initialization
	void OnEnable () {
		LR = GetComponent<LineRenderer> ();
		LR.SetVertexCount (2);
		active = false;
		Invoke ("TurnOn", 1.5f);
		LR.SetPosition (0, transform.position);
		LR.SetPosition (1, transform.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (active) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, transform.up);
			LR.SetPosition (0, transform.position);
			LR.SetPosition (1, hit.point);
			if (hit.collider.tag == "Player") {
				DestroySelf ();
			}
		}
	}
	public void SetColors(Color c){
		c.r += .2f;
		c.g += .2f;
		c.b += .2f;
		Color c2 = c;
		c2.a = .1f;
		LR.SetColors (c, c2);
	}
	void TurnOn(){
		CancelInvoke ();
		active = true;
	}
	void DestroySelf(){
		CancelInvoke ();
		transform.parent = null;
		active = false;
		gameObject.SetActive (false);
	}
}
