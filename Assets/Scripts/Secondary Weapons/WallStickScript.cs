using UnityEngine;
using System.Collections;

public class WallStickScript : MonoBehaviour {

	Rigidbody2D RB;
	bool Stuck = false;
	public ProjectileScript PS;
	public bool IsFireArrow;
	public ParticleSystem Particles;
	public Collider2D hitBox;
	// Use this for initialization
	void Start () {

	}
	void OnEnable(){

		if (RB == null){
			RB = GetComponent<Rigidbody2D> ();
		}
		if (IsFireArrow) {
			hitBox.enabled = false;
		}
		RB.isKinematic = false;
		transform.parent = null;
		transform.localScale = Vector3.one;
		Stuck = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Stuck) {
			int Layer = 768;
			RaycastHit2D hit = Physics2D.Raycast (transform.position, transform.up, Mathf.Infinity, Layer);
			Debug.DrawRay (transform.position, transform.up);
			Debug.Log(hit.collider.tag);
			if (hit.collider != null && hit.distance <= .5f && (hit.collider.tag == "Wall" || hit.collider.tag == "Player" || hit.collider.tag == "MovingWall")) {
				Stuck = true;

				if (hit.collider.tag == "Player" || hit.collider.tag == "MovingWall"){
					transform.SetParent(hit.collider.transform, true);
				}
				transform.position = hit.point;
				PS.CanDirectOff();
				RB.velocity = Vector2.zero;
				RB.isKinematic = true;
				if (IsFireArrow){
					StartCoroutine(BurnDamage());
				}
				Debug.Log(transform.eulerAngles + " local" + transform.localEulerAngles);

			}
		}
	}
	IEnumerator BurnDamage(){
		
		for (int x = 0; x <3; x++){
			hitBox.enabled = false;
			hitBox.enabled = true;
			Particles.Emit(75);
			//Particles.enableEmission = true;
			Particles.Play();
			yield return new WaitForSeconds (1f);
		}

		gameObject.SetActive (false);
	}
	void OnDisable(){

		StopAllCoroutines ();
	}
}
