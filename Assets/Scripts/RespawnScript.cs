using UnityEngine;
using System.Collections;

public class RespawnScript : MonoBehaviour {
	EternalScript ES;
	public GameObject Character;
	public float Delay;
	public RoundHandlerScript RHS;
	public RespawnPointOccupiedScript[] SpawnPoints;
	public GameObject[] SpawnPointsProtection;
	public bool[] SpawnInQue;
	bool SpawnWithNoControls;
	
	// Use this for initialization
	void Start () {
		Delay = 2;
		SpawnWithNoControls = true;
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		RHS = GameObject.Find ("WinTextObj").GetComponent<RoundHandlerScript> ();

		SpawnInQue = new bool[4];
		for (int i = 0; i < 4; i++){
			SpawnInQue[i] = false;
			SpawnPointsProtection[i].SetActive(false);
		}
		if (ES.GetActivePlayer (0)) {
			Respawn(0);
		}
		if (ES.GetActivePlayer (1)) {
			Respawn(1);
		}
		if (ES.GetActivePlayer (2)) {
			Respawn(2);
		}
		if (ES.GetActivePlayer (3)) {
			Respawn(3);
		}
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Respawn(int PlayerNum){
		StartCoroutine (RespawnSub(PlayerNum));
	}

	IEnumerator RemoveFromQue(int Spawn){
		yield return new WaitForSeconds (1.9f);
		SpawnInQue [Spawn] = false;
		SpawnPointsProtection[Spawn].SetActive(false);
	}
	IEnumerator RespawnSub(int PlayerNum){
		yield return new WaitForSeconds (.1f);
		Vector3 SpawnLocation = FindSpawn(PlayerNum);
		yield return new WaitForSeconds(Delay);

		int PrimaryWeapon, SecondaryWeapon;
		GameObject tmp;
		tmp = Instantiate (Character, SpawnLocation, Quaternion.identity) as GameObject;
		tmp = tmp.GetComponent<ChildReferenceScript> ().getObj();
		tmp.GetComponent<SpriteRenderer> ().color = ES.GetColor (PlayerNum);
		PrimaryWeapon = ES.GetPrimary (PlayerNum);
		SecondaryWeapon = ES.GetSecondary (PlayerNum);

	
		tmp.GetComponent<PlayerControlScript> ().SetController (SpawnWithNoControls, PlayerNum);
	
		// set primary weapon
		if (PrimaryWeapon == 0) {
			tmp.AddComponent<AssultRifeScript> ();
		}else if (PrimaryWeapon == 1){
			tmp.AddComponent<ShotgunScript> ();
		}else if (PrimaryWeapon == 2){
			tmp.AddComponent<SniperProjectileScript> ();
		}else if (PrimaryWeapon == 3){
			tmp.AddComponent<GrenadeLauncherScript> ();
		}else if (PrimaryWeapon == 4){
			tmp.AddComponent<RngGunScript> ();
		}else if (PrimaryWeapon == 5){
			tmp.AddComponent<BoomerangGunScript> ();
		}else if (PrimaryWeapon == 6){
			tmp.AddComponent<LazerSniperScript> ();
		}else if (PrimaryWeapon == 7){
			tmp.AddComponent<FireBowScript> ();
		}

		// set secondary weapon

		if (SecondaryWeapon == 0) {
			tmp.AddComponent<GrenadeScript> ();
		}else if (SecondaryWeapon == 1){
			tmp.AddComponent<InterceptorScript> ();
		}else if (SecondaryWeapon == 2){
			tmp.AddComponent<WallGunRevisedScript> ();
		}else if (SecondaryWeapon == 3){
			tmp.AddComponent<SmokeBombScript> ();
		}else if (SecondaryWeapon == 4){
			tmp.AddComponent<ShieldScript> ();
		}else if (SecondaryWeapon == 5){
			tmp.AddComponent<TripMineScript> ();
		}

		yield return new WaitForSeconds(Delay/2);
	}
	Vector3 FindSpawn(int PlayerNum){
		bool NotFound = true;
		while (NotFound) {
			int RNum = Random.Range(0, 4);
			if (!(SpawnPoints [0].CheckOccupied ()) && !SpawnInQue [0] && RNum == 0) {
				SpawnInQue [0] = true;
				SpawnPointsProtection [0].SetActive (true);
				SpawnPointsProtection[0].GetComponentInChildren<ParticleColorMorpherScript>().SetColor(ES.GetColor(PlayerNum));
				StartCoroutine (RemoveFromQue (0));
				NotFound = true;
				return new Vector3 (7.3f, 3.3f, 0);
			} else if (!(SpawnPoints [1].CheckOccupied ()) && !SpawnInQue [1] && RNum == 1) {
				SpawnInQue [1] = true;
				SpawnPointsProtection [1].SetActive (true);
				SpawnPointsProtection[1].GetComponentInChildren<ParticleColorMorpherScript>().SetColor(ES.GetColor(PlayerNum));
				StartCoroutine (RemoveFromQue (1));
				NotFound = true;
				return new Vector3 (-7.3f, 3.3f, 0);
			} else if (!(SpawnPoints [2].CheckOccupied ()) && !SpawnInQue [2] && RNum == 2) {
				SpawnInQue [2] = true;
				SpawnPointsProtection [2].SetActive (true);
				SpawnPointsProtection[2].GetComponentInChildren<ParticleColorMorpherScript>().SetColor(ES.GetColor(PlayerNum));
				StartCoroutine (RemoveFromQue (2));
				NotFound = true;
				return new Vector3 (7.3f, -3.3f, 0);
			} else if (!(SpawnPoints [3].CheckOccupied ()) && !SpawnInQue [3] && RNum == 3){
				SpawnInQue [3] = true;
				SpawnPointsProtection [3].SetActive (true);
				SpawnPointsProtection[3].GetComponentInChildren<ParticleColorMorpherScript>().SetColor(ES.GetColor(PlayerNum));
				StartCoroutine (RemoveFromQue (3));
				NotFound = true;
				return new Vector3 (-7.3f, - 3.3f, 0);
			} 
		}
		Debug.Log ("respawn fucked up, save me");
		return new Vector3 (0, 0, 0);
		
	}
	public void DisableControls(){
		SpawnWithNoControls = false;
	}

}
