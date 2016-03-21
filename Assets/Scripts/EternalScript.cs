using UnityEngine;
using System.Collections;

public class EternalScript : MonoBehaviour {
	public int P1Primary = 0, P2Primary = 0, P1Secondary = 0, P2Secondary = 0, P3Primary = 0, P4Primary = 0, P3Secondary = 0, P4Secondary = 0;
	public int map, KillNumber;
	public int NumberOfPlayers = 0;
	public Color P1Color, P2Color, P3Color, P4Color;
	public bool[] PlayerReady;
	public bool SecondVisit;
	int[] levelWins;


	// Use this for initialization
	void Start () {
		P1Color = Color.white;
		P2Color = Color.white;
		P3Color = Color.white;
		P4Color = Color.white;
		DontDestroyOnLoad (gameObject);
		Application.LoadLevel(1);
		KillNumber = 10;
		SecondVisit = false;
		levelWins = new int[4];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int GetKillNumber(){
		return KillNumber;
	}
	public int GetNumberOfPlayers(){
		return NumberOfPlayers;
	}
	public void SetKillNumber(int NewKills){
		 KillNumber = NewKills;
	}
	public void AddPlayer(){
		NumberOfPlayers++;
	}
	public void RemovePlayer(){
		NumberOfPlayers--;
	}
	public void setPrimary(int newChar, int playerNum){
		switch(playerNum){
		case 0:
			P1Primary = newChar;
			break;
		case 1:
			P2Primary = newChar;
			break;
		case 2:
			P3Primary = newChar;
			break;
		case 3:
			P4Primary = newChar;
			break;
		}
	}
	public void setSecondary(int newChar, int playerNum){
		switch(playerNum){
		case 0:
			P1Secondary = newChar;
			break;
		case 1:
			P2Secondary = newChar;
			break;
		case 2:
			P3Secondary = newChar;
			break;
		case 3:
			P4Secondary = newChar;
			break;
		}
	}
	public int GetPrimary(int PlayerNum){
		switch(PlayerNum){
		case 0:
			return P1Primary;
		case 1:
			return P2Primary;
		case 2:
			return P3Primary;
		case 3:
			return P4Primary;
		default:
			return 1;
		}

	}
	public int GetSecondary(int PlayerNum){
		switch (PlayerNum) {
		case 0:
			return P1Secondary;
		case 1:
			return P2Secondary;
		case 2:
			return P3Secondary;
		case 3:
			return P4Secondary;
		default:
			return 1;
		}
	}

	public void setMap(int nextMap){
		map = nextMap;
	}
	public void setColor(Color NewColor, int PlayerNum){
		switch (PlayerNum) {
		case 0:
			P1Color = NewColor;
			break;
		case 1:
			P2Color = NewColor;
			break;
		case 2:
			P3Color = NewColor;
			break;
		case 3:
			P4Color = NewColor;
			break;
		}
		
	}

	public Color GetColor(int PlayerNum){
		switch (PlayerNum) {
		case 0:
			return P1Color;
		case 1:
			return P2Color;
		case 2:
			return P3Color;
		case 3:
			return P4Color;
		default:
			return Color.white;
		}
	}
	public void setActivePlayers(bool P1, bool P2, bool P3, bool P4){
		PlayerReady [0] = P1;
		PlayerReady [1] = P2;
		PlayerReady [2] = P3;
		PlayerReady [3] = P4;
	}
	public bool GetActivePlayer(int PNum){
		return PlayerReady[PNum];
	}
	public bool CheckIfSecondVisit(){
		return SecondVisit;
	}
	public int CheckLevelWins(int PNum){
		return levelWins [PNum];
	}
	public void ChangeLevelWins(int PNum, int amount){
		levelWins [PNum] += amount;
	}
}
