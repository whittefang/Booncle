using UnityEngine;
using System.Collections;

public class SwordAnimationScript : MonoBehaviour {
	public GameObject SwordSprite;


	// Use this for initialization
	void Start () {
		AnimateSword ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void AnimateSword(){
		StartCoroutine (SwordAnimation());
	}
	IEnumerator SwordAnimation(){

		for (int x = 0; x < 12; x++) {
			SwordSprite.transform.Rotate (new Vector3 (0, 0, 15));
			yield return new WaitForSeconds(.015f);
		}
		Destroy (gameObject);
		
	}
}
