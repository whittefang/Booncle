using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
	Rigidbody2D	RB;
	Transform PlayerToReturnTo;
	bool CanDirect = true;
	public float SelfDestruct, SelfDestructUnchanged;
	public float Speed;
	public Transform SpriteTrans, FocusTrans;
	public bool HasFocus = false, IsBoomerang = false, CantSelfDestruct = false, IsKnife = false;
	SpriteRenderer SR;
	Collider2D ThisCollider;
	// Use this for initialization
	void Start(){
		
		//RB = GetComponent<Rigidbody2D>();
	}

	void OnEnable () {

		if (RB == null) {
			RB = GetComponent<Rigidbody2D>();
		}if (ThisCollider == null) {
			ThisCollider = GetComponent<CircleCollider2D> ();
		}
		if (SR == null) {
			SR = GetComponentInChildren<SpriteRenderer>();
		}

		RB.velocity =  transform.up * Speed;
		if (IsBoomerang) {
			CanDirect = true;
			ThisCollider.enabled = true;
			StartCoroutine (ReturnRang ()); 
		} else if (IsKnife) {
			StartCoroutine (KnifeControl ()); 
		}else if (!CantSelfDestruct){
			Invoke ("DelayedSuicide", SelfDestruct);
		}
	}
	void OnDisable(){
		CancelInvoke ();
		HasFocus = false;
		FocusTrans = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (!HasFocus ){
			// rotate to face direction that we are moving
			if (CanDirect){
				float Zrotation = 180+ Mathf.Atan2(RB.velocity.normalized.x, -RB.velocity.normalized.y) * 180.0f / Mathf.PI;
				transform.rotation = Quaternion.Euler(new Vector3(0, 0, Zrotation));
			}
		} else{
			Vector3 v_diff = (transform.position - FocusTrans.transform.position);
			transform.rotation = Quaternion.Euler( new Vector3(0, 0, Mathf.Atan2(v_diff.y, v_diff.x) * Mathf.Rad2Deg + 45));
			RB.velocity = transform.up * Speed;
		}

		// stretch more the faster object is moving
		if (RB.velocity.magnitude/7 > 1 && SpriteTrans != null) {
			SpriteTrans.localScale = new Vector3 (1, RB.velocity.magnitude/7, 1);
		} else if(SpriteTrans != null){
			SpriteTrans.localScale = new Vector3 (1, 1, 1);
		}
	}
	// Old version of boomerang
	/*
	IEnumerator ReturnRang(){
		yield return new WaitForSeconds (SelfDestruct/2f);
		RB.velocity =  transform.up * -Speed;
		yield return new WaitForSeconds (SelfDestruct /2f);
		tag = "Boomerang";
		GetComponentInChildren<HitScript> ().DamageInactive = true;
		while (RB.velocity.magnitude > .1f) {
			RB.velocity = Vector2.Lerp(RB.velocity, Vector2.zero, .05f);
			yield return new WaitForSeconds(.015f);
		}
		RB.velocity = Vector2.zero;
	}*/
	public void setPlayerReturn(Transform newPlayer){
		PlayerToReturnTo = newPlayer;
	}
	IEnumerator ReturnRang(){
		GetComponentInChildren<HitScript> ().BulletDamage = 1;
		yield return new WaitForSeconds (SelfDestruct/2f);

		SR.color = new Color (SR.color.r + .2f,SR.color.g + .2f,SR.color.b + .2f, 1);

		Vector2 diff;
		//RB.velocity = Vector2.zero;
		CanDirect = false;
		ThisCollider.enabled = false;
		HasFocus = false;
		FocusTrans = null;
		GetComponentInChildren<HitScript> ().BulletDamage = 2;
		for (int x = 0; x < 300; x++){
			if (PlayerToReturnTo != null){
				diff = transform.position - PlayerToReturnTo.position ;
				//transform.rotation = Quaternion.Euler(0, 0, 90);
				transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 90);
				RB.velocity = transform.up * Speed;
				yield return new WaitForSeconds(.015f);
			}else {
				GetComponentInChildren<HitScript> ().DamageInactive = true;
				RB.velocity = Vector2.zero;
				HasFocus = false;
				FocusTrans = null;
				break;
			}			
		}
	}
	IEnumerator KnifeControl(){
		yield return new WaitForSeconds (2.3f);
		tag = "Knife";
		GetComponentInChildren<HitScript> ().DamageInactive = true;
	}
	// destroyer fucntion
	void DelayedSuicide(){
		gameObject.SetActive (false);
	}
	public void SetFocusVortex(Transform NewTrans){
		if (CanDirect){
			HasFocus = true;
			FocusTrans = NewTrans;
		}
	}
	public void SetSelfDestruct(float TimeToSubtract){
		SelfDestruct = SelfDestructUnchanged;
		SelfDestruct -= TimeToSubtract;
	}
	public void CanDirectOff(){
		CanDirect = false;
	}
	
}
