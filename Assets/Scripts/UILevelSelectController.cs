using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XInputDotNetPure; // Required in C#

public class UILevelSelectController : MonoBehaviour {
	//string P1HoriztonalAxis = "Horizontal",   P1VerticalAxis = "Vertical",   P1FireButton = "SelectP1";
	//string P2HoriztonalAxis = "HorizontalP2", P2VerticalAxis = "VerticalP2", P2FireButton = "SelectP2";
	//string P3HoriztonalAxis = "HorizontalP3", P3VerticalAxis = "VerticalP3", P3FireButton = "SelectP3";
	//string P4HoriztonalAxis = "HorizontalP4", P4VerticalAxis = "VerticalP4", P4FireButton = "SelectP4";
	int[] LeftRightPos, UpDownPos;
	bool[] InputAllowed, LockedIn, ActivePlayer;
	int [] InputResetCounter, PlayerVote;
	float DeadZone = .25f;
	public UIAnimationScript[] UIButton;
	public int[] CurrentSelectedButton;
	public GameObject[] AnimObjs;
	public GameObject LevelTransistionHolder;
	SoundPlayerScript SPS;
	EternalScript ES;
	int NumOfPlayers = 0, NumOfPlayersReady = 0;
	int LevelToLoad = 0;
	PlayerIndex[] playerIndex;
	GamePadState[] state;
	GamePadState[] prevState;

	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		LeftRightPos = new int[4];
		UpDownPos = new int[4];
		InputAllowed = new bool[4];
		LockedIn = new bool[4];
		ActivePlayer = new bool[4];
		PlayerVote = new int[4];
		InputResetCounter = new int[4];
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();

		playerIndex = new PlayerIndex[4];
		state = new GamePadState[4];
		prevState = new GamePadState[4];

		for (int x = 0; x<4; x++) {
			playerIndex [x] = (PlayerIndex)x;
		}

		Invoke ("DelayInitialPosition", .1f);
	}
	IEnumerator LevelTransitionAnimation(){
		yield return new WaitForSeconds(1f);
		Component[] TransistionArray;
		TransistionArray = LevelTransistionHolder.GetComponentsInChildren<UIAnimationScript> ();
		foreach(UIAnimationScript UIAnim in TransistionArray){
			yield return new WaitForSeconds(.01f);
			UIAnim.MakeActive();
		}
	}
	void DelayInitialPosition(){
		if (ES.GetActivePlayer(0)){			
			UIButton[(0)].MakeActive();
			ActivePlayer[0] = true;
			NumOfPlayers++;
		}
		if (ES.GetActivePlayer(1)){			
			UIButton[(8)].MakeActive();
			ActivePlayer[1] = true;
			NumOfPlayers++;
		}
		if (ES.GetActivePlayer(2)){			
			UIButton[(16)].MakeActive();
			ActivePlayer[2] = true;
			NumOfPlayers++;
		}
		if (ES.GetActivePlayer(3)){			
			UIButton[(24)].MakeActive();
			ActivePlayer[3] = true;
			NumOfPlayers++;
		}
	}
	// Update is called once per frame
	void FixedUpdate () {

		for (int x = 0; x<4; x++) {
			if (!LockedIn [x]) {
				prevState [x] = state [x];
				state [x] = GamePad.GetState (playerIndex [x]);
			
				// check for accept button 
				if (state [x].Buttons.A == ButtonState.Pressed && prevState [x].Buttons.A == ButtonState.Released) {
					Activate (x);
				}
			
				// left movement
				if (state [x].ThumbSticks.Left.X < -DeadZone && InputAllowed [x] && !LockedIn [x]) {
					LeftRightMovement (x, -1);
					InputAllowed [x] = false;
				}
				// right movement
				if (state [x].ThumbSticks.Left.X > DeadZone && InputAllowed [x] && !LockedIn [x]) {
					LeftRightMovement (x, 1);
					InputAllowed [x] = false;
				}
				// up movement
				if (state [x].ThumbSticks.Left.Y > DeadZone && InputAllowed [x] && !LockedIn [x]) {
					UpDownMovement (x, -1);
					InputAllowed [x] = false;
				}
				// down movement
				if (state [x].ThumbSticks.Left.Y < -DeadZone && InputAllowed [x] && !LockedIn [x]) {
					UpDownMovement (x, 1);
					InputAllowed [x] = false;
				}
			
			}
		}

		/*
		// Activate
		if (Input.GetButtonDown(P1FireButton)&& !LockedIn[0]){
			Activate(0);
		}
		if (Input.GetButtonDown(P2FireButton)&& !LockedIn[1]){
			Activate(1);
		}
		if (Input.GetButtonDown(P3FireButton)&& !LockedIn[2]){
			Activate(2);
		}
		if (Input.GetButtonDown(P4FireButton)&& !LockedIn[3]){
			Activate(3);
		}

		
		// Left Movement
		if (Input.GetAxis(P1HoriztonalAxis) < -.25 && InputAllowed[0] == true && !LockedIn[0]){
			LeftRightMovement(0, -1);
			InputAllowed[0] = false;
		}
		if (Input.GetAxis(P2HoriztonalAxis) < -.25 && InputAllowed[1] == true && !LockedIn[1]){
			LeftRightMovement(1, -1);
			InputAllowed[1] = false;
		}
		if (Input.GetAxis(P3HoriztonalAxis) < -.25 && InputAllowed[2] == true && !LockedIn[2]){
			LeftRightMovement(2, -1);
			InputAllowed[2] = false;
		}
		if (Input.GetAxis(P4HoriztonalAxis) < -.25 && InputAllowed[3] == true && !LockedIn[3]){
			LeftRightMovement(3, -1);
			InputAllowed[3] = false;
		}
		// Right Movement
		if (Input.GetAxis(P1HoriztonalAxis) > .25 && InputAllowed[0] == true && !LockedIn[0]){
			LeftRightMovement(0, 1);
			InputAllowed[0] = false;
		}
		if (Input.GetAxis(P2HoriztonalAxis) > .25 && InputAllowed[1] == true && !LockedIn[1]){
			LeftRightMovement(1, 1);
			InputAllowed[1] = false;
		}
		if (Input.GetAxis(P3HoriztonalAxis) > .25 && InputAllowed[2] == true && !LockedIn[2]){
			LeftRightMovement(2, 1);
			InputAllowed[2] = false;
		}
		if (Input.GetAxis(P4HoriztonalAxis) > .25 && InputAllowed[3] == true && !LockedIn[3]){
			LeftRightMovement(3, 1);
			InputAllowed[3] = false;
		}
		// Down Movement
		if (Input.GetAxis(P1VerticalAxis) < -.25 && InputAllowed[0] == true && !LockedIn[0]){
			UpDownMovement(0, 1);
			InputAllowed[0] = false;
		}
		if (Input.GetAxis(P2VerticalAxis) < -.25 && InputAllowed[1] == true && !LockedIn[1]){
			UpDownMovement(1, 1);
			InputAllowed[1] = false;
		}
		if (Input.GetAxis(P3VerticalAxis) < -.25 && InputAllowed[2] == true && !LockedIn[2]){
			UpDownMovement(2, 1);
			InputAllowed[2] = false;
		}
		if (Input.GetAxis(P4VerticalAxis) < -.25 && InputAllowed[3] == true && !LockedIn[3]){
			UpDownMovement(3, 1);
			InputAllowed[3] = false;
		}
		// Up Movement
		if (Input.GetAxis(P1VerticalAxis) > .25 && InputAllowed[0] == true && !LockedIn[0]){
			UpDownMovement(0, -1);
			InputAllowed[0] = false;
		}
		if (Input.GetAxis(P2VerticalAxis) > .25 && InputAllowed[1] == true && !LockedIn[1]){
			UpDownMovement(1, -1);
			InputAllowed[1] = false;
		}
		if (Input.GetAxis(P3VerticalAxis) > .25 && InputAllowed[2] == true && !LockedIn[2]){
			UpDownMovement(2, -1);
			InputAllowed[2] = false;
		}
		if (Input.GetAxis(P4VerticalAxis) > .25 && InputAllowed[3] == true && !LockedIn[3]){
			UpDownMovement(3, -1);
			InputAllowed[3] = false;
		}
		*/

		// inputReseter
		if (InputAllowed[0] == false && ActivePlayer[0]){
			InputResetCounter[0]++;
			if (InputResetCounter[0] >= 10){ InputResetCounter[0] = 0; InputAllowed[0] = true;}
		}
		if (InputAllowed[1] == false && ActivePlayer[1]){
			InputResetCounter[1]++;
			if (InputResetCounter[1] >= 10){ InputResetCounter[1] = 0; InputAllowed[1] = true;}
		}
		if (InputAllowed[2] == false && ActivePlayer[2]){
			InputResetCounter[2]++;
			if (InputResetCounter[2] >= 10){ InputResetCounter[2] = 0; InputAllowed[2] = true;}
		}
		if (InputAllowed[3] == false && ActivePlayer[3]){
			InputResetCounter[3]++;
			if (InputResetCounter[3] >= 10){ InputResetCounter[3] = 0; InputAllowed[3] = true;}
		}	
	}

	void StartNextLevel(){
		Application.LoadLevel (LevelToLoad);
	}
	void Activate(int PNum){
		SPS.playSelect ();
		LockedIn [PNum] = true;
		PlayerVote [PNum] = CurrentSelectedButton [PNum];
		UIButton [CurrentSelectedButton [PNum] + (PNum * 8)].transform.localScale = new Vector3 (1.5f, 1.5f, 1f);
		NumOfPlayersReady++;
		checkReady ();
			
	}
	void checkReady (){

		if (NumOfPlayers == NumOfPlayersReady) {
			int rNum = Random.Range (0, 100), x = 0;
			bool finding = true;

			while (finding) {
				if (rNum > 75 && LockedIn [3]) {
					if (ES.CheckLevelWins (3) > 0) {
						ES.ChangeLevelWins (3, -1);
					} else {
						x = 3;
						ES.ChangeLevelWins (3, 1);
						finding = false;
					}
				} else if (rNum > 50  && rNum < 75 && LockedIn [2]) {
					if (ES.CheckLevelWins (2) > 0) {
						ES.ChangeLevelWins (2, -1);
					} else {
						x = 2;
						ES.ChangeLevelWins (2, 1);
						finding = false;
					}
				} else if (rNum > 25 && rNum < 50 && LockedIn [1]) {
					if (ES.CheckLevelWins (1) > 0) {
						ES.ChangeLevelWins (1, -1);
					} else {
						x = 1;
						ES.ChangeLevelWins (1, 1);
						finding = false;
					}
				} else if (rNum > 0  && rNum < 25 && LockedIn [0]) {
					if (ES.CheckLevelWins (0) > 0) {
						ES.ChangeLevelWins (0, -1);
					} else {
						x = 0;
						ES.ChangeLevelWins (0, 1);
						finding = false;
					}
				}

				rNum = Random.Range (0, 100);
			}
			LevelToLoad = PlayerVote[x] + 4;
			AnimObjs [PlayerVote[x]].SetActive (true);
			
			Component[] PickedStageParts;
			PickedStageParts = AnimObjs[PlayerVote[x]].GetComponentsInChildren<Image> ();
			foreach(Image UIAnim in PickedStageParts){
				UIAnim.color = ES.GetColor(x);
			}
			
			StartCoroutine(	LevelTransitionAnimation());
			Invoke("TransitionSound", .5f);
			Invoke("StartNextLevel", 3f);
		}	
	}
	void TransitionSound(){
		SPS.PlayButtonLevelTransition ();
	}
	void LeftRightMovement(int PNum, int Direction){
		if ((LeftRightPos [PNum] + Direction) <= 3 && (LeftRightPos [PNum] + Direction) >= 0) {
			LeftRightPos[PNum] += Direction;
			Vector3 old = UIButton[CurrentSelectedButton[PNum] + (PNum * 8)].transform.position;
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
			CurrentSelectedButton[PNum] = LeftRightPos[PNum] + UpDownPos[PNum];
			SetBoarderActive(PNum, old);	
			SPS.PlayButtonMove ();
		}		
	}
	void UpDownMovement(int PNum, int Direction){
		if ((UpDownPos [PNum] + Direction * 4) <= 4 && (UpDownPos [PNum] + Direction * 2) >= 0) {
			UpDownPos [PNum] += Direction * 4;
			Vector3 old = UIButton[CurrentSelectedButton[PNum] + (PNum * 8)].transform.position;
			SetBoarderDeActive(PNum, CurrentSelectedButton[PNum]);
			CurrentSelectedButton [PNum] = LeftRightPos [PNum] + UpDownPos [PNum];
			SetBoarderActive(PNum, old);
			SPS.PlayButtonMove ();
		}
	}
	void SetBoarderActive(int PNum, Vector3 old){
		UIButton[CurrentSelectedButton[PNum] + (PNum * 8)].MakeActiveStartPos(old);
	}
	void SetBoarderDeActive(int PNum, int ButtonToDeselect){
		UIButton[ButtonToDeselect + (PNum * 8)].MakeDeActiveNoAnim();
	}

}
