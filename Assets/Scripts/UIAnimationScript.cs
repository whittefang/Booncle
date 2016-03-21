using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIAnimationScript : MonoBehaviour {

	public Vector3 ActivePosition, canvasPosition;
	public bool AnimateOnStart = false;
	public GameObject SelectedObj;
	// Use this for initialization
	void Start () {
		if (SelectedObj != null){
			SelectedObj.SetActive (false);
		}
		ActivePosition = transform.localPosition;
		transform.position = generateRandomPosition ();
		if (AnimateOnStart) {
			MakeActive();		
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void SetEndPosition(Vector3 newEndPos){
		ActivePosition = newEndPos;
	}
	public void MakeActive(){
		StopAllCoroutines (); 
		StartCoroutine (LerpBar (generateRandomPosition (), ActivePosition));
	}
	public void MakeActiveStartPos(Vector3 StartPos){
		transform.position = StartPos;
		StopAllCoroutines (); 
		StartCoroutine (LerpBar (StartPos, ActivePosition));
		
	}
	public void MakeDeActive(){
		StopAllCoroutines (); 
		StartCoroutine (LerpBar (ActivePosition, generateRandomPosition (), true));

	}
	public void MakeDeActiveNoAnim(){
		StopAllCoroutines (); 
		transform.position = generateRandomPosition ();
	}
	public void MakeSelected(){
		SelectedObj.SetActive (true);
	}
	public void MakeDeSelected(){
		SelectedObj.SetActive (false);
	}
	public void SetBoarderColor(Color NewColor){
		if (SelectedObj != null) {
			SelectedObj.GetComponent<Image> ().color = NewColor;
		}
	}
	public void SetColor(Color NewColor){
			GetComponent<Image> ().color = NewColor;
	}
	Vector3 generateRandomPosition(){
		Vector3 tmp = new Vector3 (0, 0, 0);
		switch  (Random.Range(0, 4)) {
		case 0:
			tmp.x = -700;
			tmp.y = Random.Range(-700, 2000);
			
			break;
		case 1:
			tmp.x = 3000;
			tmp.y = Random.Range(-700, 2000);
			break;
		case 2:
			tmp.x =  Random.Range(-700, 3000);
			tmp.y = -700;
			break;
		case 3:
			tmp.x =  Random.Range(-700, 3000);
			tmp.y = 3000;
			break;
		}
		return tmp;
	}
	IEnumerator LerpBar(Vector2 Begin, Vector3 End, bool useWorld = false){

		transform.position = Begin;
		for (int x = 0; x < 100; x++){
			if (useWorld) {			
				transform.position = Vector3.Lerp(transform.position, End, .15f);
			} else {
				transform.localPosition = Vector3.Lerp(transform.localPosition, End, .15f);
			}
			float start = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < start + .015f) {
				yield return null;
			}
		}
		if (useWorld) {			
			transform.position = End;
		} else {
			transform.localPosition = End;
		}
	}
}
