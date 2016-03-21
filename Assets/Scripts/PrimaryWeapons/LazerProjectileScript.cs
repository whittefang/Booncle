using UnityEngine;
using System.Collections;

public class LazerProjectileScript : MonoBehaviour {
	public LineRenderer LR;
	public EdgeCollider2D EC;
	public GameObject particles;
	public BoxCollider2D[] hitbox;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShootLine(int count, Color c,Vector3 playerTrans, Vector3 playerUp, int pNum, int bulletNumber){
		for (int x = 0; x < 3; x++) {
			hitbox[x].gameObject.SetActive(false);
		}
		int Layer = 1 << 9;
		RaycastHit2D hit, hit2, hit3;
		hit = Physics2D.Raycast (playerTrans, playerUp, Mathf.Infinity, Layer);

		LR.SetVertexCount (count * 2);
		LR.SetPosition (0, playerTrans);
		LR.SetPosition (1, hit.point);
		particles.transform.position = hit.point;

		//calculate angle
		Vector2 diff = (Vector2)playerTrans - hit.point;
		float angle = Mathf.Atan2 (diff.y, diff.x) * Mathf.Rad2Deg;

		// set angle distance and turn on hitbox
		hitbox [0].transform.position = Vector3.MoveTowards (playerTrans, hit.point, hit.distance/2);
		hitbox [0].size = new Vector2 (hit.distance, .3f);
		hitbox [0].transform.eulerAngles = new Vector3(0,0,angle);
		// set the hitbox info for section 3
		HitScript HS = hitbox [0].GetComponent<HitScript> ();
		HS.BulletDamage = count;
		HS.SetBulletNumber(bulletNumber);
		HS.SetOwner(pNum);

		hitbox[0].gameObject.SetActive (true);
	
		// do next section if lazer is longer
		if (count > 1) {
			Vector2 diffMiddle = new Vector2( hit.point.x - playerTrans.x, hit.point.y - playerTrans.y);
			hit2 = Physics2D.Raycast (hit.point, Vector3.Reflect(diffMiddle.normalized, hit.normal), Mathf.Infinity, Layer);

			diff = (Vector2)hit.point - hit2.point;
			angle = Mathf.Atan2 (diff.y, diff.x) * Mathf.Rad2Deg;
			// set angle distance and turn on hitbox
			hitbox [1].transform.position = Vector3.MoveTowards (hit.point, hit2.point, hit2.distance/2);
			hitbox [1].size = new Vector2 (hit2.distance, .3f);
			hitbox [1].transform.eulerAngles = new Vector3(0,0,angle);
			// set the hitbox info for section 3
			HS = hitbox [1].GetComponent<HitScript> ();
			HS.BulletDamage = count;
			HS.SetBulletNumber(bulletNumber);
			HS.SetOwner(pNum);

			// linerender points
			LR.SetPosition(1,Vector3.MoveTowards(hit.point, playerTrans, .01f));
			LR.SetPosition (2, hit.point);
			LR.SetPosition (3, hit2.point);
			hitbox[1].gameObject.SetActive (true);
			// if final charge
			if (count > 2) {
				diffMiddle = new Vector2( hit2.point.x - hit.point.x, hit2.point.y - hit.point.y);
				hit3 = Physics2D.Raycast (hit2.point, Vector3.Reflect(diffMiddle.normalized, hit2.normal), Mathf.Infinity, Layer);
				
				diff = (Vector2)hit2.point - hit3.point;
				angle = Mathf.Atan2 (diff.y, diff.x) * Mathf.Rad2Deg;
				// set angle distance and turn on hitbox
				hitbox [2].transform.position = Vector3.MoveTowards (hit2.point, hit3.point, hit3.distance/2);
				hitbox [2].size = new Vector2 (hit3.distance, .3f);
				hitbox [2].transform.eulerAngles = new Vector3(0,0,angle);
				// set the hitbox info for section 3
				HS = hitbox [2].GetComponent<HitScript> ();
				HS.BulletDamage = count;
				HS.SetBulletNumber(bulletNumber);
				HS.SetOwner(pNum);

				// linerenderer points
				LR.SetPosition(3,Vector3.MoveTowards(hit2.point, hit.point, .01f));
				LR.SetPosition (4, hit2.point);
				LR.SetPosition (5, hit3.point);
				hitbox[2].gameObject.SetActive (true);
			}
		}

		gameObject.SetActive (true);
		StartCoroutine (colorAnim (c));
		Invoke ("TurnOff", .2f);


		/* old code that does not hangle corners well
		 int Layer = 1 << 9;
		RaycastHit2D hit = Physics2D.Raycast (playerTrans, playerUp, Mathf.Infinity, Layer);
		RaycastHit2D hitLeft = Physics2D.Raycast (playerTrans - (playerRight * .2f), playerUp, Mathf.Infinity, Layer);
		RaycastHit2D hitRight = Physics2D.Raycast (playerTrans + (playerRight * .2f), playerUp, Mathf.Infinity, Layer);
		RaycastHit2D hit2, hit2Left, hit2Right, hit3, hit3Left, hit3Right;


		LR.SetVertexCount (count * 2);
		LR.SetPosition (0, playerTrans);
		LR.SetPosition (1, hit.point);

		Vector2[] edges = new Vector2[2+(2*count)];
		edges [0] = playerTrans - (playerRight * .2f);		
		edges [1] = hitLeft.point;
		edges [2] = hitRight.point;
		edges [3] = playerTrans + (playerRight * .2f);
		particles.transform.position = hit.point;
		if (count > 1) {
			Vector2 diffMiddle = new Vector2( hit.point.x - playerTrans.x, hit.point.y - playerTrans.y);
			hit2Left = Physics2D.Raycast (hitLeft.point, Vector3.Reflect(diffMiddle.normalized, hit.normal), Mathf.Infinity, Layer);
			hit2 = Physics2D.Raycast (hit.point, Vector3.Reflect(diffMiddle.normalized, hit.normal), Mathf.Infinity, Layer);
			hit2Right = Physics2D.Raycast (hitRight.point, Vector3.Reflect(diffMiddle.normalized, hit.normal), Mathf.Infinity, Layer);

			LR.SetPosition(1,Vector3.MoveTowards(hit.point, playerTrans, .01f));
			LR.SetPosition (2, hit.point);
			LR.SetPosition (3, hit2.point);

			edges [2] = hit2Left.point;
			edges [3] = hit2Right.point;
			edges [4] = hitRight.point;
			edges [5] = playerTrans + (playerRight * .2f);
		
			particles.transform.position = hit2.point;
			if (count > 2){
				diffMiddle = new Vector2( hit2.point.x - hit.point.x, hit2.point.y - hit.point.y);
				hit3Left = Physics2D.Raycast (hit2Left.point, Vector3.Reflect(diffMiddle.normalized, hit2.normal), Mathf.Infinity, Layer);
				hit3 = Physics2D.Raycast (hit2.point, Vector3.Reflect(diffMiddle.normalized, hit2.normal), Mathf.Infinity, Layer);
				hit3Right = Physics2D.Raycast (hit2Right.point, Vector3.Reflect(diffMiddle.normalized, hit2.normal), Mathf.Infinity, Layer);
				
				LR.SetPosition(3,Vector3.MoveTowards(hit2.point, hit.point, .01f));
				LR.SetPosition (4, hit2.point);
				LR.SetPosition (5, hit3.point);
				
				
				edges [3] = hit3Left.point;
				edges [4] = hit3Right.point;
				edges [5] = hit2Right.point;
				edges [6] = hitRight.point;
				edges [7] = playerTrans + (playerRight * .2f);
				
				particles.transform.position = hit3.point;
			}
		}

		EC.points = edges;
		gameObject.SetActive (true);
		StartCoroutine (colorAnim (c));
		Invoke ("TurnOff", .2f);*/
	}
	
	IEnumerator colorAnim(Color c){
		for (int x = 0; x < 25; x++){
			LR.SetColors(c,c);
			c.r += .05f;
			c.g += .05f;
			c.b += .05f;
			yield return new WaitForSeconds(.015f);
		}
	}
	void TurnOff(){
		StopAllCoroutines ();
		gameObject.SetActive (false);
	}
}
