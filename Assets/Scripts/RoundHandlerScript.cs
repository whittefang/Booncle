using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoundHandlerScript : MonoBehaviour {
	public GameObject RandomShooter;
	public UIPauseMenuScript PMS;
	public float P1Kills, P2Kills, P3Kills, P4Kills;
	public float KillsToWin = 1;
	public Text WinText;
	public Image WarBarP1, WarBarP2, WarBarP3, WarBarP4;
	EternalScript ES;
	bool CanCheck = true;
	EndGameStatsScript Stats;
	RespawnScript RS;


	public GameObject[] WinBackgrounds;

	// Use this for initialization
	void Start () {
		
		RS = GameObject.Find ("RespawnObject").GetComponent<RespawnScript>();
		Stats = GameObject.Find ("TotalStatsHolder").GetComponent<EndGameStatsScript>();
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		WinText = GameObject.Find ("WinTextObj").GetComponent<Text>();
		WarBarP1 = GameObject.Find ("KillBarP1").GetComponent<Image> ();
		WarBarP2 = GameObject.Find ("KillBarP2").GetComponent<Image> ();
		WarBarP3 = GameObject.Find ("KillBarP3").GetComponent<Image> ();
		WarBarP4 = GameObject.Find ("KillBarP4").GetComponent<Image> ();

		WarBarP1.color = ES.GetColor (0);
		WarBarP2.color = ES.GetColor (1);
		WarBarP3.color = ES.GetColor (2);
		WarBarP4.color = ES.GetColor (3);

		WarBarP1.fillAmount = 0;
		WarBarP2.fillAmount = 0;
		WarBarP3.fillAmount = 0;
		WarBarP4.fillAmount = 0;
		P1Kills = 0;
		P2Kills = 0;
		P3Kills = 0;
		P4Kills = 0;

		KillsToWin = ES.GetKillNumber ();
		WinText.text = "";
		WinText.enabled = false;

		// temporary dev tool
		KillsToWin = 10;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public bool UpdateScore(int PlayerNum, bool suicide = false){
		if (CanCheck){
			switch (PlayerNum){
			case 0:
				if (!suicide){
					P1Kills++;
					Stats.IncrementKills(PlayerNum);
				}else {
					if (P1Kills > 0) {P1Kills--;}
				}
				break;
			case 1:
				if (!suicide){
					P2Kills++;
					Stats.IncrementKills(PlayerNum);
				}else {
					if (P2Kills > 0) {P2Kills--;}
				}
				break;
			case 2:
				if (!suicide){
					P3Kills++;
					Stats.IncrementKills(PlayerNum);
				}else {
					if (P3Kills > 0) {P3Kills--;}
				}
				break;	
			case 3:
				if (!suicide){
					P4Kills++;
					Stats.IncrementKills(PlayerNum);
				}else {
					if (P4Kills > 0) {P4Kills--;}

				}
				break;
			}
			StartCoroutine(LerpBar());
			return CheckWin ();
		}
		return false;

	}
	bool CheckWin(){

		if (P1Kills >= KillsToWin && CanCheck) {
			WinText.text = "PLAYER ONE WINS";
			WinAnimations(0);
			return true;
		} else if (P2Kills >= KillsToWin && CanCheck){
			WinText.text = "PLAYER TWO WINS";
			WinAnimations(1);
			return true;
		}else if (P3Kills >= KillsToWin && CanCheck){
			WinText.text = "PLAYER THREE WINS";
			WinAnimations(2);
			return true;
		}else if (P4Kills >= KillsToWin && CanCheck){
			WinText.text = "PLAYER FOUR WINS";
			WinAnimations(3);


			return true;
		}
		return false;
	}
	void RestartMatch(){
		Time.timeScale = 1;
		Application.LoadLevel (2);
	}
	void WinAnimations(int WinPnum){
		PMS.disablePlayerControls ();
		Stats.ShowStats();
		//Invoke("RestartMatch", 10f);
		CanCheck = false;
		RS.DisableControls ();

		GameObject tmp;
		tmp = Instantiate (RandomShooter, new Vector3(8,4,0), Quaternion.identity) as GameObject;
		tmp.GetComponent<ParticleSystem> ().startColor = ES.GetColor (WinPnum);
		tmp = Instantiate (RandomShooter, new Vector3(8,-4,0), Quaternion.identity) as GameObject;
		tmp.GetComponent<ParticleSystem> ().startColor = ES.GetColor (WinPnum);
		tmp = Instantiate (RandomShooter, new Vector3(-8,4,0), Quaternion.identity) as GameObject;
		tmp.GetComponent<ParticleSystem> ().startColor = ES.GetColor (WinPnum);
		tmp = Instantiate (RandomShooter, new Vector3(-8,-4,0), Quaternion.identity) as GameObject;
		tmp.GetComponent<ParticleSystem> ().startColor = ES.GetColor (WinPnum);
		tmp = Instantiate (RandomShooter, new Vector3(0,4,0), Quaternion.identity) as GameObject;
		tmp.GetComponent<ParticleSystem> ().startColor = ES.GetColor (WinPnum);
		tmp = Instantiate (RandomShooter, new Vector3(0,-4,0), Quaternion.identity) as GameObject;
		tmp.GetComponent<ParticleSystem> ().startColor = ES.GetColor (WinPnum);
		for (int i = 0; i < 10; i++){
			//WinBackgrounds[i].GetComponent<Image>().color = ES.GetColor(WinPnum);
			//WinBackgrounds[i].GetComponent<UIAnimationScript>().MakeActive();
		}
		//WinText.enabled = true;
	}
	IEnumerator LerpBar(){
		while (Mathf.Abs(WarBarP1.fillAmount - (P1Kills/KillsToWin)) > .002f || Mathf.Abs(WarBarP2.fillAmount - (P2Kills/KillsToWin)) > .002f || 
		       Mathf.Abs(WarBarP3.fillAmount - (P3Kills/KillsToWin)) > .002f || Mathf.Abs(WarBarP4.fillAmount - (P4Kills/KillsToWin)) > .002f){
			if (Mathf.Abs(WarBarP1.fillAmount - (P1Kills/KillsToWin)) > .002f){

				if (WarBarP1.fillAmount > P1Kills/KillsToWin){
					WarBarP1.fillAmount -= .001f;
				}else{
					WarBarP1.fillAmount += .001f;
				}					
			}
			if (Mathf.Abs(WarBarP2.fillAmount - (P2Kills/KillsToWin)) > .002f){
				if (WarBarP2.fillAmount > P2Kills/KillsToWin){
					WarBarP2.fillAmount -= .001f;
				}else{
					WarBarP2.fillAmount += .001f;
				}					
			}
			if (Mathf.Abs(WarBarP3.fillAmount - (P3Kills/KillsToWin)) > .002f){
				if (WarBarP3.fillAmount > P3Kills/KillsToWin){
					WarBarP3.fillAmount -= .001f;
				}else{
					WarBarP3.fillAmount += .001f;
				}					
			}
			if (Mathf.Abs(WarBarP4.fillAmount - (P4Kills/KillsToWin)) > .002f){
				if (WarBarP4.fillAmount > P4Kills/KillsToWin){
					WarBarP4.fillAmount -= .001f;
				}else{
					WarBarP4.fillAmount += .001f;
				}					
			}
			yield return new WaitForSeconds(.015f);
		}
	}


}
