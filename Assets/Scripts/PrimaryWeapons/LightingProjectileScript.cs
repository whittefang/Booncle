using UnityEngine;
using System.Collections;

public class LightingProjectileScript : MonoBehaviour {
	bool First = true;
	public GameObject FirstNode, SecondNode;
	public LineRenderer Line;
	Vector2[] pts;
	public EdgeCollider2D Wall;
	Quaternion TmpRotLast;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		/*if (TmpRotLast != transform.rotation) {
			if (First){
				FirstNode = Instantiate(FirstNode, transform.position, Quaternion.identity) as GameObject;
				FirstNode.transform.transform.position = transform.transform.position;
				First = false;
			}else{
				SecondNode = Instantiate(SecondNode, transform.position, Quaternion.identity) as GameObject;
				SecondNode.transform.transform.position = transform.transform.position;
				//activateNodes();
			}
		}
		TmpRotLast = transform.rotation; */
	}
	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Wall" || other.gameObject.tag == "MovingWall"){
			if (First){
				FirstNode = Instantiate(FirstNode, transform.position, Quaternion.identity) as GameObject;
				FirstNode.transform.transform.position = transform.transform.position;
				First = false;
			}else{
				SecondNode = Instantiate(SecondNode, transform.position, Quaternion.identity) as GameObject;
				SecondNode.transform.transform.position = transform.transform.position;
				//activateNodes();
			}
		}
	}
	void activateNodes(){
		Line.SetPosition (1, SecondNode.transform.transform.position);
		Line.SetPosition (0, FirstNode.transform.position);


		pts = new Vector2[10];
		// x version
		pts [0] = new Vector2 (FirstNode.transform.position.x - .25f, FirstNode.transform.position.y);
		pts [1] = new Vector2 (FirstNode.transform.position.x + .25f, FirstNode.transform.position.y);
		pts [2] = new Vector2 (SecondNode.transform.localPosition.x + .25f, SecondNode.transform.localPosition.y);
		pts [3] = new Vector2 (SecondNode.transform.localPosition.x - .25f, SecondNode.transform.localPosition.y);
		pts [4] = new Vector2 (FirstNode.transform.position.x - .25f, FirstNode.transform.position.y);
		// y version
		pts [5] = new Vector2 (FirstNode.transform.position.x, FirstNode.transform.position.y - .25f);
		pts [6] = new Vector2 (FirstNode.transform.position.x, FirstNode.transform.position.y + .25f);
		pts [7] = new Vector2 (SecondNode.transform.localPosition.x, SecondNode.transform.localPosition.y + .25f);
		pts [8] = new Vector2 (SecondNode.transform.localPosition.x, SecondNode.transform.localPosition.y - .25f);
		pts [9] = new Vector2 (FirstNode.transform.position.x, FirstNode.transform.position.y - .25f);
		Wall.points = pts;
	}
}
