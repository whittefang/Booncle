using UnityEngine;
using System.Collections;

public class SachelExplosionScript : MonoBehaviour {
	SoundPlayerScript SPS;
	int PlayerNum, BulletNumber;
	EternalScript ES;
	ObjectPoolScript OPProjectile, OPTrail;
	bool first = true;
	// Use this for initialization
	void Start () {
		
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		OPProjectile = GameObject.Find ("ObjectPoolerSachelShrap").GetComponent<ObjectPoolScript> ();		
		OPTrail = GameObject.Find ("ObjectPoolerTrailS").GetComponent<ObjectPoolScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Detonate(){
		Quaternion PointerRotation = Quaternion.identity;
		SPS.playExplosion ();

		for (int i = 0; i < 20; i++) {
			GameObject TmpBullet = OPProjectile.FetchObject (); //= Instantiate (projectile, transform.position, PointerRotation) as GameObject;
			TmpBullet.transform.rotation = PointerRotation;
			TmpBullet.transform.position = transform.position;
			
			GameObject TmpTrail = OPTrail.FetchObject (); //Instantiate (Trail, transform.position, PointerRotation) as GameObject;
			
			TmpTrail.GetComponent<TrailFollowScript> ().SetThingToFollow (TmpBullet.transform);
			Material TColor = TmpTrail.GetComponent<TrailRenderer> ().material;
			TColor.SetColor ("_Color", ES.GetColor (PlayerNum));
			TmpBullet.layer = 17 + PlayerNum;
			
			Color CtoChange = ES.GetColor (PlayerNum);
			CtoChange.r += .2f;
			CtoChange.g += .2f;
			CtoChange.b += .2f;
			PointerRotation = PointerRotation * Quaternion.Euler (0, 0, 18);
			
			//TmpBullet.GetComponent<ProjectileScript>().Speed = pSpeed;
			TmpBullet.SetActive (true);
			TmpTrail.SetActive (true);
			TmpBullet.GetComponentInChildren<HitScript> ().SetOwner (PlayerNum);
			
			TmpBullet.GetComponentInChildren<HitScript>().SetBulletNumber(BulletNumber);
			TmpBullet.GetComponentInChildren<SpriteRenderer> ().color = CtoChange;
		}
		gameObject.SetActive (false);
	}

	public void SetOwner(int PlayerNumber){
		PlayerNum = PlayerNumber;
	}
	public void SetBulletNumber(int NewNumber){
		BulletNumber = NewNumber;
	}
}
