
using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#
using System.Text;
using System.IO;  

public class PlayerControlScript : MonoBehaviour {
	Rigidbody2D RB, ReticleRB;
	public GameObject projectile, AimingCursor;
	public Transform GunPoint;
	LineRenderer LR;
	public delegate void WeaponFunc(Vector3 x, Quaternion y);
	public WeaponFunc SecondaryWeapon, SecondaryWeaponRelease, PrimaryWeapon, PrimaryWeaponRelease;
	public bool Controller;
	bool InputEnabled = true, CanReleaseS = false, CanReleaseP = false, BounceReticle = true, distanceFar = false;
	public int PlayerNumber;
	//public string HoriztonalAxis = "Horizontal", VerticalAxis = "Vertical",HoriztonalAxisS2 = "S2P2Horizontal", VerticalAxisS2 = "S2P2Vertical", FireButton = "FireP1Prim", FireSecondaryButton = "FireP1Sec", MenuButton = "P1Restart", AimButton = "P2Aim";
	RespawnScript RS;
	UIPauseMenuScript PauseScript;
	bool IsAiming = false, canFirePrim = true;
	float ReticleSpeed = 8;
	float deadSize = .25f, xStick, yStick;
	EternalScript ES;
	Quaternion currentRot;

	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;



	//SoundPlayerScript SPS;
	//bool CanStepSound;
	//int StepCounter;

	// Use this for initialization
	void Start () {	
		//CanStepSound = true;
		//StepCounter = 0;
		//SPS = GameObject.Find ("Main Camera").GetComponent<SoundPlayerScript> ();
		PauseScript = GameObject.Find ("PauseMenuComponent").GetComponent<UIPauseMenuScript>();
		ES = GameObject.Find("EternalHolder").GetComponent<EternalScript> ();
		RB = GetComponent<Rigidbody2D> ();
		LR = GetComponent<LineRenderer> ();
		RS = GameObject.Find ("RespawnObject").GetComponent<RespawnScript>();
		PauseScript.SetPlayer (PlayerNumber, this);
		AimingCursor.SetActive(false);
		ReticleRB = AimingCursor.GetComponent<Rigidbody2D> ();
		AimingCursor.GetComponent<SpriteRenderer> ().color = GetComponent<SpriteRenderer> ().color;
		Color C = ES.GetColor (PlayerNumber);
		C.r += .2f;
		C.g += .2f;
		C.b += .2f;
		Color C2 = C;
		C2.a = .2f;
		LR.SetColors (C, C2);
		currentRot = Quaternion.identity;
		if (BounceReticle) {
			LR.SetVertexCount (4);
		}

	}
	
	// Update is called once per frame
	void Update () {

		prevState = state;
		state = GamePad.GetState(playerIndex, GamePadDeadZone.None);

		if (InputEnabled) {
			// axis crontrols for horizontal movement
			if (Mathf.Abs (state.ThumbSticks.Left.X) > deadSize) {
				xStick = state.ThumbSticks.Left.X;
			} else {
				xStick = 0;
			}
			if (Mathf.Abs (state.ThumbSticks.Left.Y) > deadSize) {
				yStick = state.ThumbSticks.Left.Y;
			} else {
				yStick = 0;
			}
			RB.velocity = new Vector3 (xStick * 5, yStick * 5, 0);
			/*
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
            */

			// aiming button controls
			if (state.Triggers.Left > .5f) {    //  && Controller){
				AimButtonFunction ();
			} else {
				if (AimingCursor.activeSelf == true) {
					AimingCursor.SetActive (false);
					IsAiming = false;
				}

			}
			transform.rotation = PointerRotation ();
			distanceFar = LineControl ();

			// input for shooting primary
			if (state.Triggers.Right > .5f && canFirePrim && distanceFar) {
				PrimaryWeapon (GunPoint.position, PointerRotation ());
				CanReleaseP = true;
			}
			// input for shooting primary release
			if (prevState.Triggers.Right > .5f && state.Triggers.Right <= .5f && CanReleaseP == true && PrimaryWeaponRelease != null) {
				PrimaryWeaponRelease (GunPoint.position, PointerRotation ());
				CanReleaseP = false;
			}
			// inptut for secondary released
			if (state.Buttons.RightShoulder == ButtonState.Released && prevState.Buttons.RightShoulder == ButtonState.Pressed && CanReleaseS == true && SecondaryWeaponRelease != null) {
				SecondaryWeaponRelease (GunPoint.position, PointerRotation ());
				CanReleaseS = false;
			}
			// input for shooting secondary
			if (state.Buttons.RightShoulder == ButtonState.Pressed && SecondaryWeapon != null) {
				SecondaryWeapon (GunPoint.position, PointerRotation ());
				CanReleaseS = true;
			}
			// menu control
			if (state.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released && Time.timeScale == 1) {

				InputEnabled = false;
				PauseScript.PullUpMenu (PlayerNumber); 

			}

		} else {
			RB.velocity =Vector3.zero;
		}
	}
	public void EnableControls(bool SetEnabled){
		InputEnabled = SetEnabled;
	}

	void AimButtonFunction(){
		// chekc if cursor is displayed
		if (AimingCursor.activeSelf == false){
			RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
			AimingCursor.transform.position = hit.point;
			AimingCursor.SetActive( true);
			IsAiming = true;
		}
		Vector3 tmp = Vector3.zero;
		if (Mathf.Abs (state.ThumbSticks.Right.X) > .25f) {
			tmp.x = state.ThumbSticks.Right.X * ReticleSpeed;
		}
		if (Mathf.Abs (state.ThumbSticks.Right.Y) > .25f) {
			tmp.y = state.ThumbSticks.Right.Y * ReticleSpeed;
		} 
			ReticleRB.velocity = tmp;
		

		/*
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
		*/


	}

	// return the angle to look at the pointer from this object
	Quaternion PointerRotation(){
		/*if (!Controller) {
			var v3 = Input.mousePosition;
			v3.z = 10.0f;
			v3 = Camera.main.ScreenToWorldPoint (v3);
			Vector3 v_diff = (v3 - transform.position);
			return Quaternion.AngleAxis ((Mathf.Atan2 (v_diff.y, v_diff.x) * Mathf.Rad2Deg) - 90, Vector3.forward);
		}*/
		if (IsAiming ) {
			Vector3 v_diff = (AimingCursor.transform.position - transform.position);
			currentRot = Quaternion.AngleAxis ((Mathf.Atan2 (v_diff.y, v_diff.x) * Mathf.Rad2Deg) - 90, Vector3.forward);
			return currentRot;
		} else if ((Mathf.Abs (state.ThumbSticks.Right.X) > .65f) || (Mathf.Abs (state.ThumbSticks.Right.Y) > .65f)) {
			currentRot = Quaternion.Euler (0, 0, (Mathf.Atan2 (state.ThumbSticks.Right.X, -state.ThumbSticks.Right.Y) * Mathf.Rad2Deg) - 180);
			return currentRot;
		} else {
			return currentRot;
		}
	}
	

	// function for controlling line renderer 
	bool LineControl(){
		int Layer = 1 << 9;
		RaycastHit2D hit = Physics2D.Raycast (transform.position, transform.up, Mathf.Infinity, Layer);
		/*if (hit.collider != null) {
			if ( state.ThumbSticks.Right.X  == 0 && state.ThumbSticks.Right.Y == 0 && !IsAiming) {  ///Controller &&
				LR.SetPosition (0, transform.position);
				LR.SetPosition (1, transform.position);
				LR.SetPosition (2, transform.position);
				LR.SetPosition (3, transform.position);

			} else {*/
				LR.SetPosition (0, transform.position);
				LR.SetPosition (1, Vector3.MoveTowards (hit.point, transform.position, .01f));
		if (IsAiming) {
			LR.SetPosition (1, Vector3.MoveTowards (hit.point, transform.position, .01f));
			LR.SetPosition (2, hit.point);
			Vector2 diff = new Vector2 (hit.point.x - transform.position.x, hit.point.y - transform.position.y);
			hit = Physics2D.Raycast (hit.point, Vector3.Reflect (diff.normalized, hit.normal), Mathf.Infinity, Layer);
			LR.SetPosition (3, hit.point);
		} else {
			LR.SetPosition (2, hit.point);
			LR.SetPosition (3, hit.point);
		}

		return (hit.distance > .5f);
			//}
		//}
	}


	public void SetPrimaryWeapon(WeaponFunc Primary){
		PrimaryWeapon = Primary;
	}
	public void SetPrimaryWeaponRelease(WeaponFunc PrimaryR){
		PrimaryWeaponRelease = PrimaryR;
	}
	public void SetSecondaryWeaponRelease(WeaponFunc SecondaryR){
		SecondaryWeaponRelease = SecondaryR;
	}
	public void SetSecondaryWeapon( WeaponFunc Seconday){
		SecondaryWeapon = Seconday;
	}

	// takes in player numer and control style and sets controls accordingly
	public void SetController(bool ControlsEnabled, int IsPlayerNumber){
		InputEnabled = ControlsEnabled;
		PlayerNumber = IsPlayerNumber;
		playerIndex = (PlayerIndex)PlayerNumber;

		/*
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
		*/
	}
	public int GetPlayerNum(){
		return PlayerNumber;
	}
	void OnDestroy(){
		RS.Respawn (PlayerNumber);
		Destroy (AimingCursor.gameObject);
	}
	public void SetCanShootPrim(bool NewThing){
		canFirePrim = NewThing;
	}

}
