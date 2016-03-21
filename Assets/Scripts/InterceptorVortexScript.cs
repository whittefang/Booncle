using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InterceptorVortexScript : MonoBehaviour {
	public List<GameObject> ObjectList;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		for(var i = ObjectList.Count - 1; i > -1; i--)
		 {
			if (ObjectList[i] == null){
				ObjectList.RemoveAt(i);
			}else{
				//ObjectList[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				//Vector3 v_diff = (transform.position - ObjectList[i].transform.position);
				//ObjectList[i].transform.rotation = Quaternion.Euler( new Vector3(0, 0, Mathf.Atan2(v_diff.y, v_diff.x) * Mathf.Rad2Deg));
					
				//Debug.Log (Quaternion.Euler( new Vector3(0, 0, Mathf.Atan2(v_diff.y, v_diff.x) * Mathf.Rad2Deg)));
			}
		 }
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Bullet" || other.tag == "Grenade" || other.tag == "NoCollideGrenade") {
			//ObjectList.Add(other.transform.parent.gameObject);
			other.GetComponentInParent<ProjectileScript>().SetFocusVortex(transform);
		}
	}
}
