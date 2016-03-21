using UnityEngine;
using System.Collections;

using XInputDotNetPure; // Required in C#

public class UIMainMenuControllerScript : MonoBehaviour {

	int[] LeftRightPos, UpDownPos;
	public bool[] InputAllowed, LockedIn;
	int [] InputResetCounter;
	float DeadZone = .25f;
	public UIAnimationScript[] UIButton;
	public int[] CurrentSelectedButton;
	public GameObject LevelTransistionHolder;
	SoundPlayerScript SPS;
	PlayerIndex[] playerIndex;
	GamePadState[] state;
	GamePadState[] prevState;
	// Use this for initialization
	void Start () {
		LeftRightPos = new int[4];
		UpDownPos = new int[4];
		InputResetCounter = new int[4];
		InputAllowed = new bool[4];
		LockedIn = new bool[4];
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		SetBoarderActive (0);



		playerIndex = new PlayerIndex[4];
		state = new GamePadState[4];
		prevState = new GamePadState[4];

		for (int x = 0; x<4; x++) {
			playerIndex [x] = (PlayerIndex)x;
		}
	}

	// Update is called once per frame
	void Update () {

		for (int x = 0; x<4; x++){
			prevState[x] = state[x];
			state[x] = GamePad.GetState(playerIndex[x]);
			
			// check for accept button 
			if (state[x].Buttons.A == ButtonState.Pressed && prevState[x].Buttons.A == ButtonState.Released){
				Activate(0);
			}
			

			// up movement
			if (state[x].ThumbSticks.Left.Y > DeadZone && InputAllowed[x] && !LockedIn[x]){
				UpDownMovement(0, -1);
				InputAllowed[x] = false;
				Debug.Log(x);
			}
			// down movement
			if (state[x].ThumbSticks.Left.Y < -DeadZone && InputAllowed[x] && !LockedIn[x]){
				UpDownMovement(0, 1);
				InputAllowed[x] = false;
			}

			// inputReseter
			if (InputAllowed[x] == false){
				InputResetCounter[x]++;
				if (InputResetCounter[x] >= 10){ InputResetCounter[x] = 0; InputAllowed[x] = true;}
			}
		}

		/*
		// Activate
		if (Input.GetButtonDown(P1FireButton)&& !LockedIn[0]){
			Activate(0);
		}		
	

		// Down Movement
		if (Input.GetAxis(P1VerticalAxis) < -.05 && InputAllowed[0] == true && !LockedIn[0]){
			UpDownMovement(0, 1);
			InputAllowed[0] = false;
		}

		// Up Movement
		if (Input.GetAxis(P1VerticalAxis) > .05 && InputAllowed[0] == true && !LockedIn[0]){
			UpDownMovement(0, -1);
			InputAllowed[0] = false;
		}
		
		// inputReseter
		if (InputAllowed[0] == false){
			InputResetCounter[0]++;
			if (InputResetCounter[0] >= 5){ InputResetCounter[0] = 0; InputAllowed[0] = true;}
		}
		*/
	}
	
	void StartNextLevel(){
		Application.LoadLevel (2);
	}
	void QuitGame(){
		Application.Quit ();
	}
	IEnumerator LevelTransitionAnimation(){
		Component[] TransistionArray;
		TransistionArray = LevelTransistionHolder.GetComponentsInChildren<UIAnimationScript> ();
		foreach(UIAnimationScript UIAnim in TransistionArray){
			yield return new WaitForSeconds(.01f);
			UIAnim.MakeActive();
		}
	}
	void Activate(int PNum){
		SPS.PlayButtonLevelTransition ();
		LockedIn [PNum] = true;
		if (CurrentSelectedButton [PNum] == 0) {
		StartCoroutine(	LevelTransitionAnimation());
			Invoke ("StartNextLevel", 2f);
		}else 	if (CurrentSelectedButton [PNum] == 1) {
			Invoke ("QuitGame", .5f);
		}
	}


	void UpDownMovement(int PNum, int Direction){

		if ((UpDownPos [PNum] + Direction ) <= 1 && (UpDownPos [PNum] + Direction ) >= 0) {
			UpDownPos [PNum] += Direction ;
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
			CurrentSelectedButton [PNum] = LeftRightPos [PNum] + UpDownPos [PNum];
			SetBoarderActive(PNum);
			SPS.PlayButtonMove ();
		}
	}
	void SetBoarderActive(int PNum){
		UIButton[CurrentSelectedButton[0]].MakeSelected();
	}
	void SetBoarderDeActive(int PNum, int ButtonToDeselect){
		UIButton[ButtonToDeselect].MakeDeSelected();
	}
}
