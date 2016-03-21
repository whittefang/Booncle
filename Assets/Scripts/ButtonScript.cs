using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {
	public int level;
	public int Gun, GunSec;
	public int PlayerNumber;
	public int NumberOfPlayers;
	EternalScript ES;
	int kills  = 10;
	SoundPlayerScript SPS;
	// Use this for initialization
	void Start () {
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnLevelWasLoaded(int level) {
		if (level == 2) {
			// set button positions
		}
	}
	public void loadLevelSelect(){
		
		ES.setActivePlayers (true, true, false, false);
		Application.LoadLevel (3);
	}
	public void LoadNewLevel(){
		ButtonNoise ();
		//ES.setActivePlayers (true, true, false, false);
		ES.setActivePlayers (true, true, true, true);
		// gun then player
		ES.setPrimary(7, 0);
		ES.setPrimary(1, 1);
		ES.setSecondary(0, 0);
		ES.setSecondary(3, 1);
		ES.setColor (Color.blue, 0);
		ES.setColor (Color.grey, 1);
		ES.setColor (Color.green, 2);
		ES.setColor (Color.yellow, 3);

		Invoke ("LoadDelay", 0);
	}
	void LoadDelay(){
		Application.LoadLevel (4);
	}

	public void QuitButton(){
		Application.Quit ();
	}
	public void SetPrimaryWeapon(){
		ButtonNoise ();
		ES.setPrimary(Gun, PlayerNumber);
	}

	public void SetSecondaryWeapon(){
		ButtonNoise ();
		ES.setSecondary(Gun, PlayerNumber);
	}
	public void SetNumberOfPlayers(){
		ButtonNoise ();
		//ES.SetNumberOfPlayers(NumberOfPlayers);
	}
	public void SetKills(){

		kills = int.Parse(GetComponent<InputField> ().text);
		ES.SetKillNumber (kills);
	}
	void ButtonNoise(){
		SPS.playSelect ();
	}
}
