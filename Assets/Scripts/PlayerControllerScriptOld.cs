
using UnityEngine;
using System.Collections;

public class PlayerControllerScriptOld : MonoBehaviour {
	Rigidbody2D RB, ReticleRB;
	public GameObject projectile, AimingCursor;
	public Transform GunPoint;
	LineRenderer LR;
	public delegate void WeaponFunc(Vector3 x, Quaternion y);
	public WeaponFunc PrimaryWeapon;
	public WeaponFunc SecondaryWeapon;
	public bool Controller;
	bool InputEnabled = true;
	public int PlayerNumber;
	public string HoriztonalAxis = "Horizontal", VerticalAxis = "Vertical",HoriztonalAxisS2 = "S2P2Horizontal", VerticalAxisS2 = "S2P2Vertical", FireButton = "FireP1Prim", FireSecondaryButton = "FireP1Sec", MenuButton = "P1Restart", AimButton = "P2Aim";
	RespawnScript RS;
	UIPauseMenuScript PauseScript;
	bool IsAiming = false;
	float ReticleSpeed = 8;
	
	
	//SoundPlayerScript SPS;
	//bool CanStepSound;
	//int StepCounter;
	
	// Use this for initialization
	void Start () {	
		//CanStepSound = true;
		//StepCounter = 0;
		//SPS = GameObject.Find ("Main Camera").GetComponent<SoundPlayerScript> ();
		PauseScript = GameObject.Find ("PauseMenuComponent").GetComponent<UIPauseMenuScript>();
		RB = GetComponent<Rigidbody2D> ();
		LR = GetComponent<LineRenderer> ();
		RS = GameObject.Find ("RespawnObject").GetComponent<RespawnScript>();
		//PauseScript.SetPlayer (PlayerNumber, this);
		AimingCursor.SetActive(false);
		ReticleRB = AimingCursor.GetComponent<Rigidbody2D> ();
		AimingCursor.GetComponent<SpriteRenderer> ().color = GetComponent<SpriteRenderer> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		
		// debug
		Debug.Log ("Player numbner : " + PlayerNumber + "   " + Input.GetAxisRaw(FireButton));
		
		if (InputEnabled) {
			// axis crontrols for horizontal movement
			if (Input.GetAxis (HoriztonalAxis) > .05) {
				RB.velocity = new Vector3 (5 * Input.GetAxis (HoriztonalAxis), RB.velocity.y, 0);
			} else if (Input.GetAxis (HoriztonalAxis) < -.05) {
				RB.velocity = new Vector3 (5* Input.GetAxis (HoriztonalAxis), RB.velocity.y, 0);
			} else {
				RB.velocity = new Vector3 (0, RB.velocity.y, 0);
			}
			
			// axis controls for vertical movement
			if (Input.GetAxis (VerticalAxis) > .05) {
				RB.velocity = new Vector3 (RB.velocity.x, 5 * Input.GetAxis (VerticalAxis), 0);
			} else if (Input.GetAxis (VerticalAxis) < -.05) {
				RB.velocity = new Vector3 (RB.velocity.x, 5 * Input.GetAxis (VerticalAxis), 0);
			} else {
				RB.velocity = new Vector3 (RB.velocity.x, 0, 0);
			}
			
			
			// aiming button controls
			if (Input.GetAxis(AimButton) > .5f && Controller){
				AimButtonFunction();
			}else {
				if (AimingCursor.activeSelf == true){
					AimingCursor.SetActive(false);
					IsAiming = false;
				}
				
			}
			transform.rotation = PointerRotation ();
			LineControl ();
			
			// input for shooting primary
			if (Input.GetAxis(FireButton) > .5f) {
				PrimaryWeapon (GunPoint.position, PointerRotation ());
			}
			// input for shooting secondary
			if (Input.GetButton (FireSecondaryButton)) {
				SecondaryWeapon (GunPoint.position, PointerRotation ());
			}
			// menu control
			if (Input.GetButtonDown (MenuButton)) {
				if (Time.timeScale == 1){
					InputEnabled = false;
					PauseScript.PullUpMenu (PlayerNumber); 
				}
			}
			
		}
	}
	public void EnableControls(bool SetEnabled){
		InputEnabled = SetEnabled;
	}
	
	void AimButtonFunction(){
		// chekc if cursor is displayed
		if (AimingCursor.activeSelf == false){
			RaycastHit2D hit = Physics2D.Raycast(GunPoint.position, transform.up);
			AimingCursor.transform.position = hit.point;
			AimingCursor.SetActive( true);
			IsAiming = true;
		}
		
		if (Input.GetAxis (HoriztonalAxisS2) > .05) {
			ReticleRB.velocity = new Vector3 (ReticleSpeed * Input.GetAxis (HoriztonalAxisS2), ReticleRB.velocity.y, 0);
		} else if (Input.GetAxis (HoriztonalAxisS2) < -.05) {
			ReticleRB.velocity = new Vector3 (ReticleSpeed* Input.GetAxis (HoriztonalAxisS2), ReticleRB.velocity.y, 0);
		} else {
			ReticleRB.velocity = new Vector3 (0, ReticleRB.velocity.y, 0);
		}
		
		// axis controls for vertical movement
		if (Input.GetAxis (VerticalAxisS2) > .05) {
			ReticleRB.velocity = new Vector3 (ReticleRB.velocity.x, ReticleSpeed * Input.GetAxis (VerticalAxisS2), 0);
		} else if (Input.GetAxis (VerticalAxisS2) < -.05) {
			ReticleRB.velocity = new Vector3 (ReticleRB.velocity.x, ReticleSpeed * Input.GetAxis (VerticalAxisS2), 0);
		} else {
			ReticleRB.velocity = new Vector3 (ReticleRB.velocity.x, 0, 0);
		}
		
		
	}
	
	// return the angle to look at the pointer from this object
	Quaternion PointerRotation(){
		if (!Controller) {
			var v3 = Input.mousePosition;
			v3.z = 10.0f;
			v3 = Camera.main.ScreenToWorldPoint (v3);
			Vector3 v_diff = (v3 - transform.position);
			return Quaternion.AngleAxis ((Mathf.Atan2 (v_diff.y, v_diff.x) * Mathf.Rad2Deg) - 90, Vector3.forward);
		} else if (IsAiming) {
			Vector3 v_diff = (AimingCursor.transform.position - transform.position);
			return Quaternion.AngleAxis ((Mathf.Atan2 (v_diff.y, v_diff.x) * Mathf.Rad2Deg) - 90, Vector3.forward);
		}else {
			return Quaternion.Euler (0, 0, (Mathf.Atan2 (Input.GetAxis (HoriztonalAxisS2), -Input.GetAxis (VerticalAxisS2)) * Mathf.Rad2Deg) - 180);
		}
	}
	
	
	// function for controlling line renderer 
	void LineControl(){
		
		RaycastHit2D hit = Physics2D.Raycast (GunPoint.position, transform.up);
		if (hit.collider != null) {
			if (Controller && Input.GetAxis (HoriztonalAxisS2) == 0 && Input.GetAxis (VerticalAxisS2) == 0 && !IsAiming) {
				LR.SetPosition (1, transform.position);
				
			} else {
				LR.SetPosition (1, hit.point);
			}
		}
		
		LR.SetPosition (0, transform.position);
	}
	
	public void SetPrimaryWeapon(WeaponFunc Primary){
		PrimaryWeapon = Primary;
	}
	public void SetSecondaryWeapon( WeaponFunc Seconday){
		SecondaryWeapon = Seconday;
	}
	
	// takes in player numer and control style and sets controls accordingly
	public void SetController(bool IsController, int IsPlayerNumber){
		Controller = IsController;
		PlayerNumber = IsPlayerNumber;
		switch (PlayerNumber){
		case 0:
			HoriztonalAxis = "Horizontal";
			VerticalAxis = "Vertical";
			FireButton = "FireP1Prim";
			FireSecondaryButton = "FireP1Sec";
			MenuButton = "P1Restart";
			break;
		case 1:	
			HoriztonalAxis = "HorizontalP2";
			VerticalAxis = "VerticalP2";
			HoriztonalAxisS2 = "S2P2Horizontal";
			VerticalAxisS2 = "S2P2Vertical";
			FireButton = "FireP2Prim";
			FireSecondaryButton = "FireP2Sec";
			MenuButton = "P2Restart";
			AimButton = "P2Aim";
			break;
		case 2:	
			HoriztonalAxis = "HorizontalP3";
			VerticalAxis = "VerticalP3";
			HoriztonalAxisS2 = "S2P3Horizontal";
			VerticalAxisS2 = "S2P3Vertical";
			FireButton = "FireP3Prim";
			FireSecondaryButton = "FireP3Sec";
			MenuButton = "P3Restart";
			AimButton = "P3Aim";
			break;
		case 3:	
			HoriztonalAxis = "HorizontalP4";
			VerticalAxis = "VerticalP4";
			HoriztonalAxisS2 = "S2P4Horizontal";
			VerticalAxisS2 = "S2P4Vertical";
			FireButton = "FireP4Prim";
			FireSecondaryButton = "FireP4Sec";
			MenuButton = "P4Restart";
			AimButton = "P4Aim";
			break;
		}
	}
	public int GetPlayerNum(){
		return PlayerNumber;
	}
	void OnDestroy(){
		RS.Respawn (PlayerNumber);
		Destroy (AimingCursor.gameObject);
	}
}

