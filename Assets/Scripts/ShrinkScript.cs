using UnityEngine;
using System.Collections;

public class ShrinkScript : MonoBehaviour {
	public float timeToZero, startDelay;
	Vector3 amountToShrink, currentScale;

	// Use this for initialization
	void Start () {
	
	}
	void OnEnable(){
		amountToShrink = new Vector3(1 / timeToZero, 1 / timeToZero, 1 / timeToZero);
		currentScale = Vector3.one;
		StartCoroutine (Shrink(startDelay));
	}
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator Shrink(float delay){
		yield return new WaitForSeconds (delay/60);
		while (true) {
			currentScale -= amountToShrink;
			Debug.Log(currentScale);
			transform.localScale = currentScale;
			yield return new WaitForSeconds(.015f);
		}
	}

}
