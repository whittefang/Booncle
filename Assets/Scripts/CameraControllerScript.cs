using UnityEngine;
using System.Collections;

public class CameraControllerScript : MonoBehaviour {
	Camera MainCamera;
	public GameObject BoadersHolder;
	// Use this for initialization
	void Start () {
		MainCamera = GameObject.Find ("Main Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void PlayDeathAnimations(Vector3 Position, Color killerColor, bool final = false){
		Time.timeScale = 0f;
		Component[] KillBoarderArray;
		BoadersHolder.transform.position = Position;
		KillBoarderArray = BoadersHolder.GetComponentsInChildren<KillBoarderMoverScript> ();
		foreach(KillBoarderMoverScript CurrentKillBoarder in KillBoarderArray){
			CurrentKillBoarder.AnimateBoarders( killerColor, final);
		}
		if (!final) {
			StartCoroutine (PauseTime ());
		} else {
			StartCoroutine (CameraLerp(Position));
		}
	} 
	public void CancelDeathAnimations(){
		StopAllCoroutines ();
		Time.timeScale = 1;

		Component[] KillBoarderArray;
		KillBoarderArray = BoadersHolder.GetComponentsInChildren<KillBoarderMoverScript> ();
		foreach(KillBoarderMoverScript CurrentKillBoarder in KillBoarderArray){
			CurrentKillBoarder.StopBoarderAnimation();
		}

	}
	IEnumerator PauseTime(){
		float startT = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < startT + .4f) {
			yield return null;
		}
		Time.timeScale = 1;
	}

	IEnumerator CameraLerp(Vector3 FinalPosition){
		FinalPosition = new Vector3 (FinalPosition.x, FinalPosition.y, -15);
		int StartSize = 5;
		{
			float startT = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < startT + .2f) {
				yield return null;
			}
		}
		for (int x = 0; x < 50; x++) {
			MainCamera.orthographicSize = Mathf.Lerp (MainCamera.orthographicSize, 3, .15f);
			MainCamera.transform.position = Vector3.Lerp (transform.position, FinalPosition, .15f);
		
			float startT = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < startT + .015f) {
				yield return null;
			}
		}
		{
			float startT = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < startT + 1f) {
				yield return null;
			}
		}

		for (int x = 0; x < 25; x++) {
			MainCamera.orthographicSize = Mathf.Lerp (MainCamera.orthographicSize, StartSize, .15f);
			MainCamera.transform.position = Vector3.Lerp (transform.position, new Vector3(0, 0, -15), .15f);			
			
			float startT = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < startT + .015f) {
				yield return null;
			}
		}
		MainCamera.orthographicSize = 5;
		MainCamera.transform.position = new Vector3(0, 0, -15);	
		
		Time.timeScale = 1;
	}
}
