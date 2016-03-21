using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

using XInputDotNetPure; // Required in C#

public class EndGameStatsScript : MonoBehaviour {
	//public int playerNum;
	public Text[] Kfield, Dfield, Sfield, Afield, Pfield, placementField, SuicideField, DamageDoneField, titleText, weaponText;
	public int[] shotsFired, shotsHit, deaths, kills, damageDone, Suicides, place;
	EternalScript ES;
	public GameObject UIBars;
	public GameObject[] Holders;
	public Image[] SR, readyImage;
	public bool[] spotOccupied;
	PlayerIndex[] playerIndex;
	GamePadState[] state;
	GamePadState[] prevState;
	bool InputEnabled;
	bool[] ActivePlayer;
	int NumOfPlayers, NumOfPlayersReady;
	public TextAsset RightTitles, LeftTitles;
	public string[] RightLines, LeftLines;
	
	// Use this for initialization
	void Start () {
		
		ES = GameObject.Find("EternalHolder").GetComponent<EternalScript> ();
		/*
		Kfield = new Text[4];
		Dfield = new Text[4];
		Sfield = new Text[4];
		Afield = new Text[4];
		Pfield = new Text[4];*/
		spotOccupied = new bool[4];
		shotsFired = new int[4];
		shotsHit= new int[4];
		deaths= new int[4];
		kills= new int[4];
		Suicides= new int[4];
		damageDone= new int[4];
		place = new int[4];
		InputEnabled = false;
		playerIndex = new PlayerIndex[4];
		state = new GamePadState[4];
		prevState = new GamePadState[4];
		ActivePlayer = new bool[4];
		for (int x = 0; x<4; x++) {
			playerIndex [x] = (PlayerIndex)x;
			if (ES.GetActivePlayer(x)){		
				ActivePlayer[x] = true;
				NumOfPlayers++;
			}
		}
		
		PopulateArray ();
	}
	
	// Update is called once per frame
	void Update () {
		if (InputEnabled) {
			for (int x = 0; x<4; x++) {
				prevState[x] = state[x];
				state[x] = GamePad.GetState (playerIndex[x], GamePadDeadZone.None);
				
				// check for accept button 
				if (state [x].Buttons.A == ButtonState.Pressed && prevState [x].Buttons.A == ButtonState.Released) {
					Activate(x);
				}
			}
			
			
		}
		
		
	}
	public void IncrementShots(int pNum){
		shotsFired[pNum]++;
	}
	public void IncrementHits(int pNum){
		shotsHit[pNum]++;
	}
	public void IncrementDeaths(int pNum){
		deaths[pNum]++;
	}
	public void IncrementKills(int pNum){
		kills[pNum]++;
	}
	public void IncrementSuicides(int pNum){
		Suicides[pNum]++;
	}
	public void IncrementDamageDone(int pNum, int amount){
		damageDone[pNum] += amount;
	}
	public void ShowStats(){
		Invoke ("StatsDelay", 3f);
	}
	string DeterminePlace(int PlayerNum){
		int[] tmp = new int[4];
		for (int i = 0; i < 4; i++) {
			tmp[i] = kills[i];
		}
		
		Array.Sort (tmp);
		Array.Reverse (tmp);
		int x;
		for (x = 0; x < 4; x++) {
			if (kills[PlayerNum] == tmp[x]){
				break;
			}
		}
		
		FindSpot (PlayerNum ,x);
		switch (x) {
		case 0:
			return "1st";
		case 1:
			return "2nd";
		case 2:
			return "3rd";
		default:
			return "4th";
		}
	}
	
	void FindSpot(int player, int place){
		while (spotOccupied[place] || place > 4) {
			
			place++;
		}
		spotOccupied[place] = true;
		
		Vector3 EndPoint;
		switch (place){
		case 0:
			EndPoint = new Vector2(-720 ,0);
			break;
		case 1:
			EndPoint = new Vector2(-240 ,0);
			break;
		case 2:
			EndPoint = new Vector2(240 ,0);
			break;
		default:
			EndPoint = new Vector2(720 ,0);
			break;
		}
		Holders [player].GetComponent<UIAnimationScript> ().SetEndPosition (EndPoint);
		Holders[player].GetComponent<UIAnimationScript>().MakeActive();
	}		
	
	
	void StatsDelay(){
		InputEnabled = true;
		UIBars.transform.localPosition = new Vector2 (0, -2000f);
		for (int x = 0; x < 4; x++) {
			Kfield[x].text = kills[x].ToString ();
			Dfield[x].text = deaths[x].ToString ();
			Sfield[x].text = shotsFired[x].ToString ();
			DamageDoneField[x].text = damageDone[x].ToString();
			SuicideField[x].text = Suicides[x].ToString();

			float Percent;
			if (shotsFired[x] == 0){
				Percent = 0;
			}else {
				Percent = (((float)shotsHit[x] / (float)shotsFired[x]) * 100f);
				if (Percent > 100){ Percent = 100;}
				Percent = Mathf.Round(Percent);
			}
			Afield[x].text = Percent.ToString () + "%";
			
			Color Ctmp = Color.grey;
			if (ES.GetActivePlayer(x)){
				Ctmp = ES.GetColor(x);
				weaponText[x].text = getWeapon(x);
				Ctmp.a = .75f;
				SR[x].color =Ctmp;
				placementField[x].text = DeterminePlace(x);
				titleText[x].text = GetRandomLine();
			}else {				
				readyImage[x].color = Color.green;
				Ctmp.a = .75f;
				SR[x].color = Ctmp;
				DeterminePlace(x);
				placementField[x].text = "Non Participant";
			}
		}
		Invoke ("EndMatch", 30f);
	}
	string getWeapon(int pNum){
		int weaponNum = ES.GetPrimary (pNum);
		string name = "";
		switch (weaponNum) {
		case 0:
			name = "Assult Rifle";
			break;
		case 1:
			name = "Shotgun";
			break;
		case 2:
			name = "Sniper Rifle";
			break;
		case 3:
			name = "Grenade Launcher";
			break;
		case 4:
			name = "Burst Gun";
			break;
		case 5:
			name = "Boomerang";
			break;
		case 6:
			name = "Lazer";
			break;
		case 7:
			name = "Fire Arrow";
			break;
		}
		return name;
	}
	void EndMatch(){
		Time.timeScale = 1;
		Application.LoadLevel(2);
	}
	void Activate(int playerNumber){
		if (ActivePlayer [playerNumber]){
			ActivePlayer [playerNumber] = false;
			NumOfPlayersReady++;
			readyImage[playerNumber].color = Color.green;
			CheckReady();
		}
	}
	void CheckReady(){		
		if (NumOfPlayers == NumOfPlayersReady) {			
			EndMatch();
		}
	}
	void PopulateArray(){
		RightLines = RightTitles.text.Split ("\n" [0]);
		LeftLines = LeftTitles.text.Split("\n"[0]);
	}
	string GetRandomLine(){
		return LeftLines[UnityEngine.Random.Range (0, LeftLines.Length-1)] + " " +  RightLines[UnityEngine.Random.Range (0, RightLines.Length-1)] ;
		
	}

}
