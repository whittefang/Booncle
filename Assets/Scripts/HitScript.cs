using UnityEngine;
using System.Collections;

public class HitScript : MonoBehaviour {
	public GameObject splatter;
	public int BulletDamage, PlayerBulletNumber;
	public bool SplatterOnDeath, DontKillOnHit, IsBoomerang, IsKnife, DamageInactive;
	public int PlayerNum;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		if (IsBoomerang) {
			DamageInactive = false;
		}
	}
	// function called when bullet hits to show 
	public void Kill(Vector3 targetPos, bool PlaySplatter){
		if (SplatterOnDeath && PlaySplatter) {
			if (transform.parent != null){
				Instantiate (splatter, targetPos, Quaternion.Euler (0, 0, transform.parent.rotation.eulerAngles.z + 90));
			}else {				
				Instantiate (splatter, targetPos, Quaternion.Euler (0, 0, 0));
			}
		}
		if (!DontKillOnHit){
			transform.parent.gameObject.SetActive (false);
			//Destroy (transform.parent.gameObject);
		}
	}
	public void KillBoomerang(){
		transform.parent.gameObject.SetActive (false);
		//Destroy (transform.parent.gameObject);
	}
	public int GetDamage(){

		return BulletDamage;
	}
	public int GetOwner(){
		
		return PlayerNum;
	}
	public void SetBulletNumber(int NewNumber){
		PlayerBulletNumber = NewNumber;
	}
	public int GetBulletNumber(){
		
		return PlayerBulletNumber;
	}
	public void SetOwner(int PlayerNumber){
		PlayerNum = PlayerNumber;
	}
}

