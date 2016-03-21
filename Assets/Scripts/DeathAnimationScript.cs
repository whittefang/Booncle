using UnityEngine;
using System.Collections;

public class DeathAnimationScript : MonoBehaviour {

	Quaternion EndRotation;
	Vector3 EndPosition;
	Color DeathColor;
	SpriteRenderer SR;
	public bool IsBlood;

	// Use this for initialization
	void Start () {
		EndPosition = new Vector3 (Random.Range (-1f, 1f), Random.Range (-1f, 1f), 0);
		EndRotation = Quaternion.Euler (0f, 0f, Random.Range(0f, 360f));
		SR = GetComponent<SpriteRenderer> ();

		//PlayDeathAnim (Color.white);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void PlayDeathAnim(Color StartColor){
		if (IsBlood){
			StartColor = SR.color;
		}

		SR.color = StartColor;		
		float DiscolorAmount = Random.Range (0f, .3f);
		DeathColor = StartColor - new Color ( DiscolorAmount, DiscolorAmount, DiscolorAmount, 0f);
		StartCoroutine (DeathAnim());
	}
	IEnumerator DeathAnim(){
		for (int x = 0; x < 100; x++){
			transform.localPosition = Vector3.Lerp(transform.localPosition, EndPosition, .04f);
			transform.rotation = Quaternion.Lerp(transform.rotation, EndRotation, .04f);
			SR.color = Color.Lerp(SR.color, DeathColor, .04f);

			float startT = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < startT + .015f) {
				yield return null;
			}
		}
	}
}
