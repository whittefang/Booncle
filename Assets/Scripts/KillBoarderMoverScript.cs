using UnityEngine;
using System.Collections;

public class KillBoarderMoverScript : MonoBehaviour {
	public Vector3 Final;
	public Vector3 StartPos;
	float TransSpeed = 1.5f;
	SpriteRenderer SR;
	// Use this for initialization
	void Start () {
		Final = transform.position;		
		transform.Translate(Vector3.up * -35);
		StartPos = transform.position;
		SR = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void AnimateBoarders(Color newColor, bool final = false){
		if (!final) {
			StartCoroutine (GreyBoarders (newColor));
		} else {
			StartCoroutine (LerpBar());
		}
	}
	public void StopBoarderAnimation(){
		StopAllCoroutines ();
		transform.localPosition = StartPos;
	}
	IEnumerator LerpBar(){
		while (Mathf.Abs(Final.x - transform.localPosition.x) > 1){


				transform.Translate(Vector3.up * TransSpeed);

			float startT = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < startT + .015f) {
				yield return null;
			}
		}
		while (Mathf.Abs(Final.y - transform.localPosition.y) > 1){

				transform.Translate(Vector3.up * TransSpeed );

			float startT = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < startT + .015f) {
				yield return null;
			}
		}
		transform.localPosition = Final;
		// hang time looking at player
		float startTT = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < startTT + 1.5f) {
			yield return null;
		}

		while (Mathf.Abs(StartPos.x - transform.localPosition.x) > 1){

			transform.Translate(Vector3.up * -TransSpeed );

	
			float startT = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < startT + .015f) {
				yield return null;
			}
		}
		while (Mathf.Abs(StartPos.y - transform.localPosition.y) > 1){
		
				transform.Translate(Vector3.up * -TransSpeed );

			float startT = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < startT + .015f) {
				yield return null;
			}
		}
		
	}

	IEnumerator GreyBoarders(Color newColor){
		newColor.a = .3f;
		SR.color = newColor;
		transform.localPosition = Final;
		float startT = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < startT + .4f) {
			yield return null;
		}
		transform.localPosition = StartPos;
	}
}
