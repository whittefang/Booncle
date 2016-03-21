using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {
	public GameObject Shrapnel, Trail;
	SoundPlayerScript SPS;
	int PlayerNum, BulletNumber;
	EternalScript ES;
	ObjectPoolScript OPProjectile, OPTrail;
	bool first = true;
	// Use this for initialization
	void Start () {
		
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		OPProjectile = GameObject.Find ("ObjectPoolerGrenadeShrap").GetComponent<ObjectPoolScript> ();
		OPTrail = GameObject.Find ("ObjectPoolerTrailS").GetComponent<ObjectPoolScript> ();
		Trail = Resources.Load ("ShortTrail") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnDisable(){
		if (first) {
			first = false;
		} else {
			Quaternion PointerRotation = Quaternion.identity;
			SPS.playExplosion ();
			for (int i = 0; i < 20; i++) {
				GameObject TmpBullet = OPProjectile.FetchObject (); 
				TmpBullet.transform.rotation = PointerRotation;
				TmpBullet.transform.position = transform.position;

				GameObject TmpTrail = OPTrail.FetchObject ();

				TmpTrail.GetComponent<TrailFollowScript> ().SetThingToFollow (TmpBullet.transform);
				Material TColor = TmpTrail.GetComponent<TrailRenderer> ().material;
				TColor.SetColor ("_Color", ES.GetColor (PlayerNum));
				TmpBullet.layer = 17 + PlayerNum;

				Color CtoChange = ES.GetColor (PlayerNum);
				CtoChange.r += .2f;
				CtoChange.g += .2f;
				CtoChange.b += .2f;
				PointerRotation = PointerRotation * Quaternion.Euler (0, 0, 18);
				TmpBullet.SetActive (true);
				TmpTrail.SetActive (true);
				TmpBullet.GetComponentInChildren<HitScript> ().SetOwner (PlayerNum);
				TmpBullet.GetComponentInChildren<HitScript> ().SetBulletNumber (BulletNumber);
				TmpBullet.GetComponentInChildren<SpriteRenderer> ().color = CtoChange;
			}
		}
	}
	public void SetOwner(int PlayerNumber){
		PlayerNum = PlayerNumber;
	}
	public void SetBulletNumber(int NewNumber){
		BulletNumber = NewNumber;
	}
}
