using UnityEngine;
using System.Collections;

using XInputDotNetPure; // Required in C#

public class UIPauseMenuScript : MonoBehaviour {


	int[] LeftRightPos, UpDownPos;
	bool[] InputAllowed, CurrentPlayer;
	int [] InputResetCounter;
	float DeadZone = .25f;
	public UIAnimationScript[] UIButton;
	public int[] CurrentSelectedButton;
	public UIAnimationScript[] FullPauseMenu;
	SoundPlayerScript SPS;
	public bool MenuActive = false;
	EternalScript ES;
	PlayerControlScript[]  PCS;
	PlayerIndex[] playerIndex;
	GamePadState[] state;
	GamePadState[] prevState;
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		LeftRightPos = new int[4];
		UpDownPos = new int[4];
		InputResetCounter = new int[4];
		InputAllowed = new bool[4];
		CurrentPlayer = new bool[4];
		PCS = new PlayerControlScript[4];
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
			if (state[x].Buttons.A == ButtonState.Pressed && prevState[x].Buttons.A == ButtonState.Released  && MenuActive){
				Activate(x);
			}
			

			// up movement
			if (state[x].ThumbSticks.Left.Y > DeadZone && InputAllowed[x] && CurrentPlayer[x] && MenuActive){
				UpDownMovement(x, -1);
				InputAllowed[x] = false;
			}
			// down movement
			if (state[x].ThumbSticks.Left.Y < -DeadZone && InputAllowed[x] && CurrentPlayer[x] && MenuActive){
				UpDownMovement(x, 1);
				InputAllowed[x] = false;
			}

			// inputReseter
			if (InputAllowed[x] == false){
				InputResetCounter[x]++;
				if (InputResetCounter[x] >= 10){ InputResetCounter[x] = 0; InputAllowed[x] = true;}
			}

			if (state[x].Buttons.Start == ButtonState.Pressed && prevState[x].Buttons.Start == ButtonState.Released  && MenuActive) {
				PutMenuAway(x);
			}
		}

		/*
		// Activate
		if (Input.GetButtonDown(P1FireButton) && CurrentPlayer[0] && MenuActive){
			Activate(0);
		}		
		// Down Movement
		if (Input.GetAxisRaw(P1VerticalAxis) < -.05 && InputAllowed[0] == true && CurrentPlayer[0] && MenuActive){

			UpDownMovement(0, 1);
			InputAllowed[0] = false;
		}		
		// Up Movement
		if (Input.GetAxisRaw(P1VerticalAxis) > .05 && InputAllowed[0] == true && CurrentPlayer[0] && MenuActive){
			UpDownMovement(0, -1);
			InputAllowed[0] = false;
		}
		// inputReseter
		if (InputAllowed[0] == false){
			InputResetCounter[0]++;
			if (InputResetCounter[0] >= 10){ InputResetCounter[0] = 0; InputAllowed[0] = true;}
		}

		// Activate
		if (Input.GetButtonDown(P2FireButton) && CurrentPlayer[1] && MenuActive){
			Activate(1);
		}		
		// Down Movement
		if (Input.GetAxisRaw(P2VerticalAxis) < -.05 && InputAllowed[1] == true && CurrentPlayer[1] && MenuActive){
			
			UpDownMovement(1, 1);
			InputAllowed[1] = false;
		}		
		// Up Movement
		if (Input.GetAxisRaw(P2VerticalAxis) > .05 && InputAllowed[1] == true && CurrentPlayer[1] && MenuActive){
			UpDownMovement(1, -1);
			InputAllowed[1] = false;
		}
		// inputReseter
		if (InputAllowed[1] == false){
			InputResetCounter[1]++;
			if (InputResetCounter[1] >= 10){ InputResetCounter[1] = 0; InputAllowed[1] = true;}
		}

		// Activate
		if (Input.GetButtonDown(P4FireButton) && CurrentPlayer[3] && MenuActive){
			Activate(3);
		}		
		// Down Movement
		if (Input.GetAxisRaw(P4VerticalAxis) < -.05 && InputAllowed[3] == true && CurrentPlayer[3] && MenuActive){
			
			UpDownMovement(3, 1);
			InputAllowed[3] = false;
		}		
		// Up Movement
		if (Input.GetAxisRaw(P4VerticalAxis) > .05 && InputAllowed[3] == true && CurrentPlayer[3] && MenuActive){
			UpDownMovement(3, -1);
			InputAllowed[3] = false;
		}
		// inputReseter
		if (InputAllowed[3] == false){
			InputResetCounter[3]++;
			if (InputResetCounter[3] >= 10){ InputResetCounter[3] = 0; InputAllowed[3] = true;}
		}

		// Activate
		if (Input.GetButtonDown(P3FireButton) && CurrentPlayer[2] && MenuActive){
			Activate(2);
		}		
		// Down Movement
		if (Input.GetAxisRaw(P3VerticalAxis) < -.05 && InputAllowed[2] == true && CurrentPlayer[2] && MenuActive){
			
			UpDownMovement(2, 1);
			InputAllowed[2] = false;
		}		
		// Up Movement
		if (Input.GetAxisRaw(P3VerticalAxis) > .05 && InputAllowed[2] == true && CurrentPlayer[2] && MenuActive){
			UpDownMovement(2, -1);
			InputAllowed[2] = false;
		}
		// inputReseter
		if (InputAllowed[2] == false){
			InputResetCounter[2]++;
			if (InputResetCounter[2] >= 10){ InputResetCounter[2] = 0; InputAllowed[2] = true;}
		}

*/
	}
	public void PullUpMenu(int PNum){
		if (MenuActive == false && Time.timeScale == 1) {
			CurrentPlayer [PNum] = true;
			for (int i = 0; i < PCS.Length; i++) {
				if (PCS [i] != null) {
					PCS [i].EnableControls (false);
				}
			}
			MenuActive = true;
			Time.timeScale = 0;
			for (int i = 0; i <= 3; i++) {
				FullPauseMenu [i].SetColor (ES.GetColor (PNum) - new Color (.2f, .2f, .2f, 0f));
				FullPauseMenu [i].MakeActive ();
			}
			for (int i = 4; i <= 11; i++) {
				FullPauseMenu [i].SetColor (ES.GetColor (PNum) + new Color (.3f, .3f, .3f, 0f));
				FullPauseMenu [i].MakeActive ();
			}
			for (int i = 0; i < UIButton.Length; i++) {
				UIButton [i].SetBoarderColor (ES.GetColor (PNum) + new Color (.3f, .3f, .3f, 0f));
				UIButton [i].MakeActive ();
				SetBoarderDeActive (0, i);
			}
			SetBoarderActive (PNum);
		} 

	}
	public void disablePlayerControls(){
		for (int i = 0; i < PCS.Length; i++) {
			if (PCS [i] != null) {
				PCS [i].EnableControls (false);
			}
		}
	}

	void PutMenuAway(int PNum){
		if (MenuActive == true) {
			MenuActive = false;
			for (int i = 0; i < FullPauseMenu.Length; i++) {
				FullPauseMenu [i].MakeDeActive ();
			}
			for (int i = 0; i < UIButton.Length; i++) {
				UIButton [i].MakeDeActive ();
			}
			CurrentPlayer[PNum] = false;
			StartCoroutine(SlightDelay());
		}
	}
	IEnumerator SlightDelay(){
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + .3f) {
			yield return null;
		}
		for (int i = 0; i < PCS.Length; i++){
			if (PCS[i] != null){
				PCS[i].EnableControls(true);
			}
		}
		
		Time.timeScale = 1;
	}
	public void SetPlayer(int PNum, PlayerControlScript TmpPCS){
		PCS [PNum] = TmpPCS;
	}
	void StartMainMenuLevel(){
		Application.LoadLevel (1);
	}
	void StartPlayerSelectLevel(){
		Application.LoadLevel (2);
	}
	void Activate(int PNum){
		SPS.PlayButtonLevelTransition ();
		if (CurrentSelectedButton [PNum] == 0) {
			PutMenuAway(PNum);
		}else if (CurrentSelectedButton [PNum] == 1) {
			Time.timeScale = 1;
			Invoke ("StartPlayerSelectLevel", .5f);
		}else if (CurrentSelectedButton [PNum] == 2) {
			Time.timeScale = 1;
			Invoke ("StartMainMenuLevel", .5f);
		}
	}
	
	
	void UpDownMovement(int PNum, int Direction){
		if ((UpDownPos [PNum] + Direction ) <= 2 && (UpDownPos [PNum] + Direction ) >= 0) {
			UpDownPos [PNum] += Direction ;
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
			CurrentSelectedButton [PNum] = LeftRightPos [PNum] + UpDownPos [PNum];
			SetBoarderActive(PNum);
			SPS.PlayButtonMove ();
		}
	}
	void SetBoarderActive(int PNum){
		UIButton[CurrentSelectedButton[PNum]].MakeSelected();
	}
	void SetBoarderDeActive(int PNum, int ButtonToDeselect){
		UIButton[ButtonToDeselect].MakeDeSelected();
	}
}
