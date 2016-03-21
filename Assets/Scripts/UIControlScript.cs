
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using XInputDotNetPure; // Required in C#

public class UIControlScript : MonoBehaviour {
	EternalScript ES;
	bool P1Ready = false, P2Ready = false, P3Ready = false, P4Ready = false; 
	public UIAnimationScript[] P1StartUIAnim, P1OverviewUIAnim, P1PrimaryUIAnim, P1SecondaryUIAnim, P1ColorUIAnim, P1ReadyUIAnim;
	public UIAnimationScript[] P2StartUIAnim, P2OverviewUIAnim, P2PrimaryUIAnim, P2SecondaryUIAnim, P2ColorUIAnim, P2ReadyUIAnim;
	public UIAnimationScript[] P3StartUIAnim, P3OverviewUIAnim, P3PrimaryUIAnim, P3SecondaryUIAnim, P3ColorUIAnim, P3ReadyUIAnim;
	public UIAnimationScript[] P4StartUIAnim, P4OverviewUIAnim, P4PrimaryUIAnim, P4SecondaryUIAnim, P4ColorUIAnim, P4ReadyUIAnim;
	public UIAnimationScript[] CountDownOneAnim, CountDownTwoAnim, CountDownThreeAnim;
	public int[] CurrentSlide;
	public int[] CurrentSelectedButton;
	public Image P1ColorPreview, P2ColorPreview, P3ColorPreview, P4ColorPreview;
	public GameObject[] ReadyParticles;
	public Text[] PrimaryText;
	public Text[] SecondaryText;
	public float DeadZone;
	public GameObject LevelTransistionHolder;
	int[] LeftRightPos, UpDownPos;
	bool[] InputAllowedLR;
	bool[] InputAllowedUD;
	int [] InputResetCounterLR, InputResetCounterUD;
	bool lockedIn;
	int NumberOfPlayers;
	public Slider[] RGBSliderArray; 
	SoundPlayerScript SPS;
	PlayerIndex[] playerIndex;
	GamePadState[] state;
	GamePadState[] prevState;
	bool SoundOn;
	int soundCounter;

	// Use this for initialization
	void Start () {
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();

		LeftRightPos = new int[4];
		UpDownPos = new int[4];
		InputAllowedLR = new bool[4];
		InputAllowedUD = new bool[4];
		InputResetCounterLR = new int[4];
		InputResetCounterUD = new int[4];
		CurrentSlide = new int[4] {1,1,1,1};
		CurrentSelectedButton = new int[4];
		playerIndex = new PlayerIndex[4];
		state = new GamePadState[4];
		prevState = new GamePadState[4];
		lockedIn = false;
		NumberOfPlayers = 0;

		for (int x = 0; x<4; x++) {
			playerIndex [x] = (PlayerIndex)x;
		}

		if (ES.CheckIfSecondVisit()) {
			SetOverviewPrimaryText (0, ES.GetPrimary (0));
			SetOverviewPrimaryText (1, ES.GetPrimary (1));
			SetOverviewPrimaryText (2, ES.GetPrimary (2));
			SetOverviewPrimaryText (3, ES.GetPrimary (3));
			SetOverviewSecondaryText (0, ES.GetSecondary (0));
			SetOverviewSecondaryText (1, ES.GetSecondary (1));
			SetOverviewSecondaryText (2, ES.GetSecondary (2));
			SetOverviewSecondaryText (3, ES.GetSecondary (3));
		}
	}

		

	// Update is called once per frame
	void FixedUpdate () {
		if (!lockedIn) {
			for (int x = 0; x<4; x++) {
				prevState [x] = state [x];
				state [x] = GamePad.GetState (playerIndex [x]);

				// check for accept button 
				if (state [x].Buttons.A == ButtonState.Pressed && prevState [x].Buttons.A == ButtonState.Released) {
					Activate (x);
				}

				// back button
				if (state [x].Buttons.B == ButtonState.Pressed && prevState [x].Buttons.B == ButtonState.Released) {
					DeActivate (x);
				}

				// left movement
				if (state [x].ThumbSticks.Left.X < -DeadZone && InputAllowedLR [x]) {
					LeftRightMovement (x, -1);
					InputAllowedLR [x] = false;
				}
				// right movement
				if (state [x].ThumbSticks.Left.X > DeadZone && InputAllowedLR [x]) {
					LeftRightMovement (x, 1);
					InputAllowedLR [x] = false;
				}
				// up movement
				if (state [x].ThumbSticks.Left.Y > DeadZone && InputAllowedUD [x]) {
					UpDownMovement (x, -1);
					InputAllowedUD [x] = false;
				}
				// down movement
				if (state [x].ThumbSticks.Left.Y < -DeadZone && InputAllowedUD [x]) {
					UpDownMovement (x, 1);
					InputAllowedUD [x] = false;
				}

			}
		}

		/*
		// Activate
		if (Input.GetButtonDown(P1FireButton)){

		}
		if (Input.GetButtonDown(P2FireButton)){
			Activate(1);
		}
		if (Input.GetButtonDown(P3FireButton)){
			Activate(2);
		}
		if (Input.GetButtonDown(P4FireButton)){
			Activate(3);
		}

		// Deactivate
		if (Input.GetButtonDown(P1FireSecondaryButton) && !lockedIn){
			DeActivate(0);
		}
		if (Input.GetButtonDown(P2FireSecondaryButton) && !lockedIn){
			DeActivate(1);
		}
		if (Input.GetButtonDown(P3FireSecondaryButton) && !lockedIn){
			DeActivate(2);
		}
		if (Input.GetButtonDown(P4FireSecondaryButton) && !lockedIn){
			DeActivate(3);
		}

		// Left Movement
		if (Input.GetAxis(P1HoriztonalAxis) < -DeadZone && InputAllowedLR[0] == true){
			LeftRightMovement(0, -1);
			InputAllowedLR[0] = false;
		}
		if (Input.GetAxis(P2HoriztonalAxis) < -DeadZone && InputAllowedLR[1] == true){
			LeftRightMovement(1, -1);
			InputAllowedLR[1] = false;
		}
		if (Input.GetAxis(P3HoriztonalAxis) < -DeadZone && InputAllowedLR[2] == true){
			LeftRightMovement(2, -1);
			InputAllowedLR[2] = false;
		}
		if (Input.GetAxis(P4HoriztonalAxis) < -DeadZone && InputAllowedLR[3] == true){
			LeftRightMovement(3, -1);
			InputAllowedLR[3] = false;
		}
		// Right Movement
		if (Input.GetAxis(P1HoriztonalAxis) > DeadZone && InputAllowedLR[0] == true){
			LeftRightMovement(0, 1);
			InputAllowedLR[0] = false;
		}
		if (Input.GetAxis(P2HoriztonalAxis) > DeadZone && InputAllowedLR[1] == true){
			LeftRightMovement(1, 1);
			InputAllowedLR[1] = false;
		}
		if (Input.GetAxis(P3HoriztonalAxis) > DeadZone && InputAllowedLR[2] == true){
			LeftRightMovement(2, 1);
			InputAllowedLR[2] = false;
		}
		if (Input.GetAxis(P4HoriztonalAxis) > DeadZone && InputAllowedLR[3] == true){
			LeftRightMovement(3, 1);
			InputAllowedLR[3] = false;
		}
		// Down Movement
		if (Input.GetAxis(P1VerticalAxis) < -DeadZone && InputAllowedUD[0] == true){
			UpDownMovement(0, 1);
			InputAllowedUD[0] = false;
		}
		if (Input.GetAxis(P2VerticalAxis) < -DeadZone && InputAllowedUD[1] == true){
			UpDownMovement(1, 1);
			InputAllowedUD[1] = false;
		}
		if (Input.GetAxis(P3VerticalAxis) < -DeadZone && InputAllowedUD[2] == true){
			UpDownMovement(2, 1);
			InputAllowedUD[2] = false;
		}
		if (Input.GetAxis(P4VerticalAxis) < -DeadZone && InputAllowedUD[3] == true){
			UpDownMovement(3, 1);
			InputAllowedUD[3] = false;
		}
		// Up Movement
		if (Input.GetAxis(P1VerticalAxis) > DeadZone && InputAllowedUD[0] == true){
			UpDownMovement(0, -1);
			InputAllowedUD[0] = false;
		}
		if (Input.GetAxis(P2VerticalAxis) > DeadZone && InputAllowedUD[1] == true){
			UpDownMovement(1, -1);
			InputAllowedUD[1] = false;
		}
		if (Input.GetAxis(P3VerticalAxis) > DeadZone && InputAllowedUD[2] == true){
			UpDownMovement(2, -1);
			InputAllowedUD[2] = false;
		}
		if (Input.GetAxis(P4VerticalAxis) > DeadZone && InputAllowedUD[3] == true){
			UpDownMovement(3, -1);
			InputAllowedUD[3] = false;
		}

		*/

		// inputReseterUPDown
		if (InputAllowedUD[0] == false){
			InputResetCounterUD[0]++;
			if (InputResetCounterUD[0] >= 10){ InputResetCounterUD[0] = 0; InputAllowedUD[0] = true;}
		}
		if (InputAllowedUD[1] == false){
			InputResetCounterUD[1]++;
			if (InputResetCounterUD[1] >= 10){ InputResetCounterUD[1] = 0; InputAllowedUD[1] = true;}
		}
		if (InputAllowedUD[2] == false){
			InputResetCounterUD[2]++;
			if (InputResetCounterUD[2] >= 10){ InputResetCounterUD[2] = 0; InputAllowedUD[2] = true;}
		}
		if (InputAllowedUD[3] == false){
			InputResetCounterUD[3]++;
			if (InputResetCounterUD[3] >= 10){ InputResetCounterUD[3] = 0; InputAllowedUD[3] = true;}
		}
		// inputReseter left Right
		if (InputAllowedLR[0] == false){
			InputResetCounterLR[0]++;
			if (InputResetCounterLR[0] >= 2){ InputResetCounterLR[0] = 0; InputAllowedLR[0] = true;}
		}
		if (InputAllowedLR[1] == false){
			InputResetCounterLR[1]++;
			if (InputResetCounterLR[1] >= 2){ InputResetCounterLR[1] = 0; InputAllowedLR[1] = true;}
		}
		if (InputAllowedLR[2] == false){
			InputResetCounterLR[2]++;
			if (InputResetCounterLR[2] >= 2){ InputResetCounterLR[2] = 0; InputAllowedLR[2] = true;}
		}
		if (InputAllowedLR[3] == false){
			InputResetCounterLR[3]++;
			if (InputResetCounterLR[3] >= 2){ InputResetCounterLR[3] = 0; InputAllowedLR[3] = true;}
		}
		if (soundCounter <= 0) {
			SoundOn = true;
		} else {
			soundCounter--;
		}

	}
	void CheckIfReady(){
		bool AllReady = false;
		switch (NumberOfPlayers) {
		case 2:
			if (P1Ready && P2Ready){
				AllReady = true;
			}else if (P1Ready && P3Ready){
				AllReady = true;
			}else if (P1Ready && P4Ready){
				AllReady = true;
			}else if (P2Ready && P3Ready){
				AllReady = true;
			}else if (P2Ready && P4Ready){
				AllReady = true;
			}else if (P3Ready && P4Ready){
				AllReady = true;
			}
			break;
		case 3:
			if (P1Ready && P2Ready && P3Ready){
				AllReady = true;
			}else if (P1Ready && P3Ready && P4Ready){
				AllReady = true;
			}else if (P2Ready && P3Ready && P4Ready){
				AllReady = true;
			}else if (P1Ready && P2Ready && P4Ready){
				AllReady = true;
			}
			break;
		case 4:
			if (P1Ready && P2Ready && P3Ready && P4Ready){
				AllReady = true;
			}
			break;
		}
		if (AllReady){
			//lockedIn = true;
			ES.setActivePlayers (P1Ready, P2Ready, P3Ready, P4Ready);			
			ES.SecondVisit = true;
			StartCoroutine(	LevelTransitionAnimation());
		}
	}
	IEnumerator LevelTransitionAnimation(){
		// 3
		foreach(UIAnimationScript UIAnim in CountDownThreeAnim){
			yield return new WaitForSeconds(.01f);
			UIAnim.MakeActive();
		}
		yield return new WaitForSeconds (1f);
		//2
		foreach(UIAnimationScript UIAnim in CountDownThreeAnim){
			yield return new WaitForSeconds(.01f);
			UIAnim.MakeDeActive();
		}
		foreach(UIAnimationScript UIAnim in CountDownTwoAnim){
			yield return new WaitForSeconds(.01f);
			UIAnim.MakeActive();
		}
		yield return new WaitForSeconds (1f);
		//1
		foreach(UIAnimationScript UIAnim in CountDownTwoAnim){
			yield return new WaitForSeconds(.01f);
			UIAnim.MakeDeActive();
		}
		foreach(UIAnimationScript UIAnim in CountDownOneAnim){
			yield return new WaitForSeconds(.01f);
			UIAnim.MakeActive();
		}
		yield return new WaitForSeconds (1f);
		
		lockedIn = true;
		Component[] TransistionArray;
		TransistionArray = LevelTransistionHolder.GetComponentsInChildren<UIAnimationScript> ();
		SPS.PlayButtonLevelTransition ();
		foreach(UIAnimationScript UIAnim in TransistionArray){
			yield return new WaitForSeconds(.01f);
			UIAnim.MakeActive();
		}
		yield return new WaitForSeconds (.5f);
		Application.LoadLevel (3);
	}


	void Activate(int PNum){

		switch(CurrentSlide[PNum]){
			//select player screen move to overview
		case 1:
			NumberOfPlayers++;
			StopAllCoroutines();
			ClearNumbers();
			GoToOverview(PNum);
			LeaveStart(PNum);
			CurrentSelectedButton[PNum] = 0;
			LeftRightPos[PNum] = 0;
			UpDownPos [PNum] = 0;
			ButtonSelectSound();
			break;
			// check what button and go to appropriate slide
		case 2:
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
			LeaveOverview(PNum);
			if (CurrentSelectedButton[PNum] == 0){
				CurrentSelectedButton[PNum] = ES.GetPrimary(PNum);
				LeftRightPos[PNum] = ES.GetPrimary(PNum) % 2;
				if (ES.GetPrimary(PNum) > 5){UpDownPos [PNum] = 6;} else if (ES.GetPrimary(PNum) > 3){UpDownPos [PNum] = 4;} else if (ES.GetPrimary(PNum) > 1) {UpDownPos [PNum] = 2;}else {UpDownPos [PNum] = 0;}
				GoToPrimary(PNum);
				ButtonSelectSound();
			}else if (CurrentSelectedButton[PNum] == 1){
				CurrentSelectedButton[PNum] = ES.GetSecondary(PNum);
				LeftRightPos[PNum] = ES.GetSecondary(PNum) % 2;
				if (ES.GetSecondary(PNum) > 3){UpDownPos [PNum] = 4;} else if (ES.GetSecondary(PNum) > 1){UpDownPos [PNum] = 2;} else {UpDownPos [PNum] = 0;}
				GoToSecondary(PNum);
				ButtonSelectSound();
			}else if (CurrentSelectedButton[PNum] == 2){
				CurrentSelectedButton[PNum] = 0;
				LeftRightPos[PNum] = (int)(RGBSliderArray[CurrentSelectedButton[PNum] + (PNum * 3)].value * 30);
				UpDownPos [PNum] = 0;
				GoToColor(PNum);
				ButtonSelectSound();
			}else if (CurrentSelectedButton[PNum] == 3){
				CurrentSelectedButton[PNum] = 0;
				LeftRightPos[PNum] = 0;
				UpDownPos [PNum] = 0;
				ReadyParticles[PNum].SetActive(true);
				GoToReady(PNum);
				ButtonSelectSound();
			}
			break;
			// set primary based on button selection
		case 3:
			ES.setPrimary(CurrentSelectedButton[PNum], PNum);
			SetOverviewPrimaryText(PNum, ES.GetPrimary(PNum));
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
			LeavePrimary(PNum);
			CurrentSelectedButton[PNum] = 0;
			LeftRightPos[PNum] = 0;
			UpDownPos [PNum] = 0;
			GoToOverview(PNum);
			ButtonSelectSound();
			break;
			// set secondary based on button selection
		case 4:
			ES.setSecondary(CurrentSelectedButton[PNum], PNum);
			SetOverviewSecondaryText(PNum, ES.GetSecondary(PNum));
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
			LeaveSecondary(PNum);
			CurrentSelectedButton[PNum] = 1;
			LeftRightPos[PNum] = 0;
			UpDownPos [PNum] = 1;
			GoToOverview(PNum);
			ButtonSelectSound();
			break;
		case 5:
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);			
			CurrentSelectedButton[PNum] = 2;
			LeftRightPos[PNum] = 0;
			UpDownPos [PNum] = 2;
			GoToOverview(PNum);
			LeaveColor(PNum);
			ButtonSelectSound();
			break;
		}

	}
	void DeActivate(int PNum){

		// player one deactivate button options
		switch(CurrentSlide[PNum]){
		case 1:
			Application.LoadLevel(1);
			break;
		case 2:
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
			CurrentSelectedButton[PNum] = 0;
			LeftRightPos[PNum] = 0;
			UpDownPos [PNum] = 0;
			GoToStart(PNum);
			LeaveOverview(PNum);
			NumberOfPlayers--;
			ButtonBackSound();
			
			CheckIfReady ();

			break;
		case 3:
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
			CurrentSelectedButton[PNum] = 0;
			LeftRightPos[PNum] = 0;
			UpDownPos [PNum] = 0;
			GoToOverview(PNum);
			LeavePrimary(PNum);
			ButtonBackSound();

			break;
		case 4:
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
			CurrentSelectedButton[PNum] = 1;
			LeftRightPos[PNum] = 0;
			UpDownPos [PNum] = 1;
			GoToOverview(PNum);
			LeaveSecondary(PNum);
			ButtonBackSound();
			break;
		case 5:
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);			
			CurrentSelectedButton[PNum] = 2;
			LeftRightPos[PNum] = 0;
			UpDownPos [PNum] = 2;
			GoToOverview(PNum);
			LeaveColor(PNum);
			ButtonBackSound();
			break;
		case 6:
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
			CurrentSelectedButton[PNum] = 3;
			LeftRightPos[PNum] = 0;
			UpDownPos [PNum] = 3;
			GoToOverview(PNum);
			ReadyParticles[PNum].SetActive(false);
			ClearNumbers();
			StopAllCoroutines();
			LeaveReady(PNum);
			ButtonBackSound();
			break;
		}
					
	}
	void ClearNumbers(){
		foreach(UIAnimationScript UIAnim in CountDownThreeAnim){
			UIAnim.MakeDeActive();
		}
		foreach(UIAnimationScript UIAnim in CountDownTwoAnim){
			UIAnim.MakeDeActive();
		}
		foreach(UIAnimationScript UIAnim in CountDownOneAnim){
			UIAnim.MakeDeActive();
		}
	}
	void LeftRightMovement(int PNum, int Direction){
		switch (CurrentSlide [PNum]) {
		case 3:
		case 4:
			if ((LeftRightPos [PNum] + Direction) <= 1 && (LeftRightPos [PNum] + Direction) >= 0) {
				LeftRightPos[PNum] += Direction;
				SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
				CurrentSelectedButton[PNum] = LeftRightPos[PNum] + UpDownPos[PNum];
				SetBoarderActive(PNum);				
				ButtonMoveSound();
			}
			break;
		case 5:
			// control for color bars
			if ((LeftRightPos [PNum] + Direction) <= 30 && (LeftRightPos [PNum] + Direction) >= 0) {
				LeftRightPos[PNum] += Direction;
				RGBSliderArray[CurrentSelectedButton[PNum] + (PNum * 3)].value = (float)LeftRightPos[PNum]/30f;
				ButtonMoveSound();
			}
			break;
		}


	}
	void UpDownMovement(int PNum, int Direction){
		switch (CurrentSlide [PNum]) {
		case 2:
			if ((UpDownPos [PNum] + Direction) <= 3 && (UpDownPos [PNum] + Direction) >= 0) {
				UpDownPos [PNum] += Direction ;
				SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
				CurrentSelectedButton [PNum] = UpDownPos [PNum];
				SetBoarderActive(PNum);
				ButtonMoveSound();
			}
			break;
		case 3:
			if ((UpDownPos [PNum] + Direction * 2) <= 6 && (UpDownPos [PNum] + Direction * 2) >= 0) {
				UpDownPos [PNum] += Direction * 2;
				SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
				CurrentSelectedButton [PNum] = LeftRightPos [PNum] + UpDownPos [PNum];
				SetBoarderActive(PNum);
				ButtonMoveSound();
			}
			break;
		case 4:
			if ((UpDownPos [PNum] + Direction * 2) <= 4 && (UpDownPos [PNum] + Direction * 2) >= 0) {
				UpDownPos [PNum] += Direction * 2;
				SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
				CurrentSelectedButton [PNum] = LeftRightPos [PNum] + UpDownPos [PNum];
				SetBoarderActive(PNum);
				ButtonMoveSound();
			}
			break;
		case 5:
			if ((UpDownPos [PNum] + Direction) <= 2 && (UpDownPos [PNum] + Direction) >= 0) {
				UpDownPos [PNum] += Direction ;
				SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
				CurrentSelectedButton [PNum] = UpDownPos [PNum];
				SetBoarderActive(PNum);
				LeftRightPos[PNum] = (int)(RGBSliderArray[CurrentSelectedButton[PNum] + (PNum * 3)].value * 30);
				ButtonMoveSound();
			}
			break;

		}
		
	}
	void SetOverviewPrimaryText(int PNum, int WNum){

		switch(WNum){
		case 0:
			PrimaryText[PNum].text = "Assult Rifle";
			break;
		case 1:
			PrimaryText[PNum].text = "Shotgun";
			break;
		case 2:
			PrimaryText[PNum].text = "Sniper Rifle";
			break;
		case 3:
			PrimaryText[PNum].text = "Grenade Launcher";
			break;
		case 4:
			PrimaryText[PNum].text = "Burst Gun";
			break;
		case 5:
			PrimaryText[PNum].text = "Boomerang";
			break;
		case 6:
			PrimaryText[PNum].text = "Laser";
			break;
		case 7:
			PrimaryText[PNum].text = "Fire Arrow";
			break;
			}
			
	}void SetOverviewSecondaryText(int PNum, int WNum){
		
		switch(WNum){
		case 0:
			SecondaryText[PNum].text = "Grenade";
			break;
		case 1:
			SecondaryText[PNum].text = "Interceptor";
			break;
		case 2:
			SecondaryText[PNum].text = "Wall";
			break;
		case 3:
			SecondaryText[PNum].text = "Armor";
			break;
		case 4:
			SecondaryText[PNum].text = "Shield";
			break;
		case 5:
			SecondaryText[PNum].text = "Trip Mine";
			break;
		}
		
	}
	void SetBoarderActive(int PNum){
		switch (PNum) {
			// player one section
		case 0:
			switch (CurrentSlide[PNum]){
			case 2:
				P1OverviewUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			case 3:
				P1PrimaryUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			case 4:
				P1SecondaryUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			case 5:
				P1ColorUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			}
			break;
			// player two section
		case 1:
			switch (CurrentSlide[PNum]){
			case 2:
				P2OverviewUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			case 3:
				P2PrimaryUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			case 4:
				P2SecondaryUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			case 5:
				P2ColorUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			}
			break;
			// player three section
		case 2:
			switch (CurrentSlide[PNum]){
			case 2:
				P3OverviewUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			case 3:
				P3PrimaryUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			case 4:
				P3SecondaryUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			case 5:
				P3ColorUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			}
			break;
			// player four section
		case 3:
			switch (CurrentSlide[PNum]){
			case 2:
				P4OverviewUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			case 3:
				P4PrimaryUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			case 4:
				P4SecondaryUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			case 5:
				P4ColorUIAnim[CurrentSelectedButton[PNum]].MakeSelected();
				break;
			}
			break;
		}
	}
	void SetBoarderDeActive(int PNum, int ButtonToDeselect){
		switch (PNum) {
			// player one section
		case 0:
			switch (CurrentSlide[PNum]){
			case 2:
				P1OverviewUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			case 3:
				P1PrimaryUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			case 4:
				P1SecondaryUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			case 5:
				P1ColorUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			}
			break;
			// player two section
		case 1:
			switch (CurrentSlide[PNum]){
			case 2:
				P2OverviewUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			case 3:
				P2PrimaryUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			case 4:
				P2SecondaryUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			case 5:
				P2ColorUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			}
			break;
			// player three section
		case 2:
			switch (CurrentSlide[PNum]){
			case 2:
				P3OverviewUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			case 3:
				P3PrimaryUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			case 4:
				P3SecondaryUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			case 5:
				P3ColorUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			}
			break;
			// player four section
		case 3:
			switch (CurrentSlide[PNum]){
			case 2:
				P4OverviewUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			case 3:
				P4PrimaryUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			case 4:
				P4SecondaryUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			case 5:
				P4ColorUIAnim[ButtonToDeselect].MakeDeSelected();
				break;
			}
			break;
		}
	}
	void GoToStart(int PNum){
		switch (PNum) {
		case 0:
			CurrentSlide[PNum] = 1;
			SetBoarderActive(PNum);
			for (int i = 0; i < P1StartUIAnim.Length; i++){
				P1StartUIAnim[i].MakeActive();
			}
			break;
		case 1:
			CurrentSlide[PNum] = 1;
			SetBoarderActive(PNum);
			for (int i = 0; i < P2StartUIAnim.Length; i++){
				P2StartUIAnim[i].MakeActive();
			}
			break;
		case 2:
			CurrentSlide[PNum] = 1;
			SetBoarderActive(PNum);
			for (int i = 0; i < P3StartUIAnim.Length; i++){
				P3StartUIAnim[i].MakeActive();
			}
			break;
		case 3:
			CurrentSlide[PNum] = 1;
			SetBoarderActive(PNum);
			for (int i = 0; i < P4StartUIAnim.Length; i++){
				P4StartUIAnim[i].MakeActive();
			}
			break;
		}
	}
	void LeaveStart(int PNum){
		switch (PNum) {
		case 0:
			for (int i = 0; i < P1StartUIAnim.Length; i++){
				P1StartUIAnim[i].MakeDeActive();
			}
			break;
		case 1:
			for (int i = 0; i < P2StartUIAnim.Length; i++){
				P2StartUIAnim[i].MakeDeActive();
			}
			break;
		case 2:
			for (int i = 0; i < P3StartUIAnim.Length; i++){
				P3StartUIAnim[i].MakeDeActive();
			}
			break;
		case 3:
			for (int i = 0; i < P4StartUIAnim.Length; i++){
				P4StartUIAnim[i].MakeDeActive();
			}
			break;
		}
	}
	void GoToOverview(int PNum){

		switch (PNum) {
		case 0:
			CurrentSlide[PNum] = 2;
			SetBoarderActive(PNum);
			for (int i = 0; i < P1OverviewUIAnim.Length; i++){
				P1OverviewUIAnim[i].MakeActive();
			}
			break;
		case 1:
			CurrentSlide[PNum] = 2;
			SetBoarderActive(PNum);
			for (int i = 0; i < P2OverviewUIAnim.Length; i++){
				P2OverviewUIAnim[i].MakeActive();
			}
			break;
		case 2:
			CurrentSlide[PNum] = 2;
			SetBoarderActive(PNum);
			for (int i = 0; i < P3OverviewUIAnim.Length; i++){
				P3OverviewUIAnim[i].MakeActive();
			}
			break;
		case 3:
			CurrentSlide[PNum] = 2;
			SetBoarderActive(PNum);
			for (int i = 0; i < P4OverviewUIAnim.Length; i++){
				P4OverviewUIAnim[i].MakeActive();
			}
			break;
		}
	}
	void LeaveOverview(int PNum){

		switch (PNum) {
		case 0:
			for (int i = 0; i < P1OverviewUIAnim.Length; i++){
				P1OverviewUIAnim[i].MakeDeActive();
			}
			break;
		case 1:
			for (int i = 0; i < P2OverviewUIAnim.Length; i++){
				P2OverviewUIAnim[i].MakeDeActive();
			}
			break;
		case 2:
			for (int i = 0; i < P3OverviewUIAnim.Length; i++){
				P3OverviewUIAnim[i].MakeDeActive();
			}
			break;
		case 3:
			for (int i = 0; i < P4OverviewUIAnim.Length; i++){
				P4OverviewUIAnim[i].MakeDeActive();
			}
			break;
		}
	}
	void GoToPrimary(int PNum){
		switch (PNum) {
		case 0:
			CurrentSlide[PNum] = 3;
			SetBoarderActive(PNum);
			for (int i = 0; i < P1PrimaryUIAnim.Length; i++){
				P1PrimaryUIAnim[i].MakeActive();
			}
			break;
		case 1:
			CurrentSlide[PNum] = 3;
			SetBoarderActive(PNum);
			for (int i = 0; i < P2PrimaryUIAnim.Length; i++){
				P2PrimaryUIAnim[i].MakeActive();
			}
			break;
		case 2:
			CurrentSlide[PNum] = 3;
			SetBoarderActive(PNum);
			for (int i = 0; i < P3PrimaryUIAnim.Length; i++){
				P3PrimaryUIAnim[i].MakeActive();
			}
			break;
		case 3:
			CurrentSlide[PNum] = 3;
			SetBoarderActive(PNum);
			for (int i = 0; i < P4PrimaryUIAnim.Length; i++){
				P4PrimaryUIAnim[i].MakeActive();
			}
			break;
		}
	}
	void LeavePrimary(int PNum){
		switch (PNum) {
		case 0:
			for (int i = 0; i < P1PrimaryUIAnim.Length; i++){
				P1PrimaryUIAnim[i].MakeDeActive();
			}
			break;
		case 1:
			for (int i = 0; i < P2PrimaryUIAnim.Length; i++){
				P2PrimaryUIAnim[i].MakeDeActive();
			}
			break;
		case 2:
			for (int i = 0; i < P3PrimaryUIAnim.Length; i++){
				P3PrimaryUIAnim[i].MakeDeActive();
			}
			break;
		case 3:
			for (int i = 0; i < P4PrimaryUIAnim.Length; i++){
				P4PrimaryUIAnim[i].MakeDeActive();
			}
			break;
		}
	}
	void GoToSecondary(int PNum){
		switch (PNum) {
		case 0:
			CurrentSlide[PNum] = 4;
			SetBoarderActive(PNum);
			for (int i = 0; i < P1SecondaryUIAnim.Length; i++){
				P1SecondaryUIAnim[i].MakeActive();
			}
			break;
		case 1:
			CurrentSlide[PNum] = 4;
			SetBoarderActive(PNum);
			for (int i = 0; i < P2SecondaryUIAnim.Length; i++){
				P2SecondaryUIAnim[i].MakeActive();
			}
			break;
		case 2:
			CurrentSlide[PNum] = 4;
			SetBoarderActive(PNum);
			for (int i = 0; i < P3SecondaryUIAnim.Length; i++){
				P3SecondaryUIAnim[i].MakeActive();
			}
			break;
		case 3:
			CurrentSlide[PNum] = 4;
			SetBoarderActive(PNum);
			for (int i = 0; i < P4SecondaryUIAnim.Length; i++){
				P4SecondaryUIAnim[i].MakeActive();
			}
			break;
		}
	}
	void LeaveSecondary(int PNum){
		switch (PNum) {
		case 0:
			for (int i = 0; i < P1SecondaryUIAnim.Length; i++){
				P1SecondaryUIAnim[i].MakeDeActive();
			}
			break;
		case 1:
			for (int i = 0; i < P2SecondaryUIAnim.Length; i++){
				P2SecondaryUIAnim[i].MakeDeActive();
			}
			break;
		case 2:
			for (int i = 0; i < P3SecondaryUIAnim.Length; i++){
				P3SecondaryUIAnim[i].MakeDeActive();
			}
			break;
		case 3:
			for (int i = 0; i < P4SecondaryUIAnim.Length; i++){
				P4SecondaryUIAnim[i].MakeDeActive();
			}
			break;
		}
	}
	void GoToColor(int PNum){
		switch (PNum) {
		case 0:
			CurrentSlide[PNum] = 5;
			SetBoarderActive(PNum);
			for (int i = 0; i < P1ColorUIAnim.Length; i++){
				P1ColorUIAnim[i].MakeActive();
			}
			break;
		case 1:
			CurrentSlide[PNum] = 5;
			SetBoarderActive(PNum);
			for (int i = 0; i < P2ColorUIAnim.Length; i++){
				P2ColorUIAnim[i].MakeActive();
			}
			break;
		case 2:
			CurrentSlide[PNum] = 5;
			SetBoarderActive(PNum);
			for (int i = 0; i < P3ColorUIAnim.Length; i++){
				P3ColorUIAnim[i].MakeActive();
			}
			break;
		case 3:
			CurrentSlide[PNum] = 5;
			SetBoarderActive(PNum);
			for (int i = 0; i < P4ColorUIAnim.Length; i++){
				P4ColorUIAnim[i].MakeActive();
			}
			break;
		}
	}
	void LeaveColor(int PNum){
		switch (PNum) {
		case 0:
			for (int i = 0; i < P1ColorUIAnim.Length; i++){
				P1ColorUIAnim[i].MakeDeActive();
			}
			ES.setColor(P1ColorPreview.color, PNum);
			break;
		case 1:
			for (int i = 0; i < P2ColorUIAnim.Length; i++){
				P2ColorUIAnim[i].MakeDeActive();
			}
			ES.setColor(P2ColorPreview.color, PNum);
			break;
		case 2:
			for (int i = 0; i < P3ColorUIAnim.Length; i++){
				P3ColorUIAnim[i].MakeDeActive();
			}
			ES.setColor(P3ColorPreview.color, PNum);
			break;
		case 3:
			for (int i = 0; i < P4ColorUIAnim.Length; i++){
				P4ColorUIAnim[i].MakeDeActive();
			}
			ES.setColor(P4ColorPreview.color, PNum);
			break;
		}
	}
	void GoToReady(int PNum){
		switch (PNum) {
		case 0:
			CurrentSlide[PNum] = 6;
			P1Ready = true;
			for (int i = 0; i < P1ReadyUIAnim.Length; i++){
				P1ReadyUIAnim[i].MakeActive();
			}
			break;
		case 1:
			CurrentSlide[PNum] = 6;
			P2Ready = true;
			for (int i = 0; i < P2ReadyUIAnim.Length; i++){
				P2ReadyUIAnim[i].MakeActive();
			}
			break;
		case 2:
			CurrentSlide[PNum] = 6;
			P3Ready = true;
			for (int i = 0; i < P3ReadyUIAnim.Length; i++){
				P3ReadyUIAnim[i].MakeActive();
			}
			break;
		case 3:
			CurrentSlide[PNum] = 6;
			P4Ready = true;
			for (int i = 0; i < P4ReadyUIAnim.Length; i++){
				P4ReadyUIAnim[i].MakeActive();
			}
			break;
		}
		CheckIfReady ();
	}
	void LeaveReady(int PNum){
		switch (PNum) {
		case 0:
			P1Ready = false;
			CurrentSlide[PNum] = 2;
			for (int i = 0; i < P1ReadyUIAnim.Length; i++){
				P1ReadyUIAnim[i].MakeDeActive();
			}
			break;
		case 1:
			P2Ready = false;
			CurrentSlide[PNum] = 2;
			for (int i = 0; i < P2ReadyUIAnim.Length; i++){
				P2ReadyUIAnim[i].MakeDeActive();
			}
			break;
		case 2:
			P3Ready = false;
			CurrentSlide[PNum] = 2;
			for (int i = 0; i < P3ReadyUIAnim.Length; i++){
				P3ReadyUIAnim[i].MakeDeActive();
			}
			break;
		case 3:
			P4Ready = false;
			CurrentSlide[PNum] = 2;
			for (int i = 0; i < P4ReadyUIAnim.Length; i++){
				P4ReadyUIAnim[i].MakeDeActive();
			}
			break;
		}
	}

	void ButtonBackSound(){
		SPS.PlayButtonBack ();
	}
	void ButtonMoveSound(){
		if (SoundOn) {
			SPS.PlayButtonMove ();
			soundCounter = 10;
			SoundOn = false;
		}
	}
	void ButtonSelectSound(){
		SPS.playSelect ();
	}
}
