using UnityEngine;
using System.Collections;

public class WallProjectileScript : MonoBehaviour {
	public EdgeCollider2D Wall;
	LineRenderer Line;
	public Transform FirstBullit, SecondBullit;
	Vector2[] pts;
	int stage;
	public Color C;
	public float colorSpeed, colorVarience, currentMorphAmount;
	// Use this for initialization
	void Start () {
		//Wall = GetComponent<EdgeCollider2D> ();
		Line = GetComponent<LineRenderer> ();
		C.r = 0; C.b = 1f; C.g = 1f; C.a = 1;
		currentMorphAmount = colorVarience/2;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		ColorMorph ();
	}
	public void ActivateWall(Color PColor){
		StartCoroutine (LineOfSightCheck (PColor));
	}
	IEnumerator LineOfSightCheck(Color PColor){
		Invoke ("DestroyWall", 5f);
		for (int x = 0; x < 100; x++) {
			Vector3 diff = (SecondBullit.position - FirstBullit.position);
			RaycastHit2D hit = Physics2D.Raycast(FirstBullit.position, diff);
			Debug.DrawRay (FirstBullit.position, diff);
			if (hit.collider.tag == "WallNode"){
				Line.SetPosition (1, SecondBullit.position);
				Line.SetPosition (0, FirstBullit.position);
				pts = new Vector2[10];
				// x version
				pts [0] = new Vector2 (FirstBullit.position.x - .25f, FirstBullit.position.y);
				pts [1] = new Vector2 (FirstBullit.position.x + .25f, FirstBullit.position.y);
				pts [2] = new Vector2 (SecondBullit.localPosition.x + .25f, SecondBullit.localPosition.y);
				pts [3] = new Vector2 (SecondBullit.localPosition.x - .25f, SecondBullit.localPosition.y);
				pts [4] = new Vector2 (FirstBullit.position.x - .25f, FirstBullit.position.y);
				// y version
				pts [5] = new Vector2 (FirstBullit.position.x, FirstBullit.position.y - .25f);
				pts [6] = new Vector2 (FirstBullit.position.x, FirstBullit.position.y + .25f);
				pts [7] = new Vector2 (SecondBullit.localPosition.x, SecondBullit.localPosition.y + .25f);
				pts [8] = new Vector2 (SecondBullit.localPosition.x, SecondBullit.localPosition.y - .25f);
				pts [9] = new Vector2 (FirstBullit.position.x, FirstBullit.position.y - .25f);
				Wall.points = pts;
				C = PColor;
				Wall.enabled = true;

				break;
			} else { 
				yield return new WaitForSeconds(.05f);
			}
		}
	}
	void DestroyWall(){
		Destroy (SecondBullit.parent.gameObject);

	}
	void ColorMorph(){
		switch (stage){
		case 0:
			if (currentMorphAmount < colorVarience) {
				C.g += colorSpeed;
				C.r += colorSpeed;
				C.b += colorSpeed;
				currentMorphAmount += colorSpeed;
			}else{
				stage = 1;
			}
			break;
		case 1:
			if (currentMorphAmount > .01) {
				C.g -= colorSpeed;
				C.r -= colorSpeed;
				C.b -= colorSpeed;
				currentMorphAmount -= colorSpeed;
			}else{
				stage = 0;
			}
			break;
		}
		Line.SetColors (C,C);
	}
}
