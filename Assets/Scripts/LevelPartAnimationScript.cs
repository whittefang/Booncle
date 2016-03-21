using UnityEngine;
using System.Collections;

public class LevelPartAnimationScript : MonoBehaviour {
	Vector3 Final;
	// Use this for initialization
	void Start () {
		Final = transform.position;
		transform.position = transform.position * -2;
		StartCoroutine (LerpBar());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator LerpBar(){
		for (int x = 0; x < 150; x++){

			transform.position = Vector3.Lerp(transform.position, Final, .05f);
			yield return new WaitForSeconds(.015f);
		}
		transform.position = Final;

	}
}
