using UnityEngine;
using System.Collections;

public class SplitGunScript : MonoBehaviour {

	public GameObject projectile, Trail;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	int ShotTimer;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	EternalScript ES;
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		SPS = GameObject.Find ("Main Camera").GetComponent<SoundPlayerScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetPrimaryMagazineSize (3);
		AS.SetReloadTime (2f);
		PCS.SetPrimaryWeapon (Shoot);
		ShotTimer = 0;
		ShootFireCooldown = 15;
		CanShoot = true;
		projectile = Resources.Load ("SplitProjectilePrefab") as GameObject;
		Trail = Resources.Load ("ShortTrail") as GameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (ShotTimer < ShootFireCooldown) {
			ShotTimer++;
		} else if (!CanShoot){
			CanShoot = true;
		}
	}
	public void Shoot(Vector3 GunPoint,Quaternion PointerRotation){
		if (CanShoot && AS.CheckCanShootPrimary()){
			SPS.playShoot();
			AS.ConsumePrimaryAmmo();
			GameObject TmpBullet = Instantiate (projectile, GunPoint, PointerRotation) as GameObject;
			GameObject TmpTrail = Instantiate (Trail, GunPoint, PointerRotation) as GameObject;
			TmpTrail.GetComponent<TrailFollowScript>().SetThingToFollow(TmpBullet.transform);
			Material TColor = TmpTrail.GetComponent<TrailRenderer>().material;
			TColor.SetColor("_Color", ES.GetColor(PCS.GetPlayerNum()));
			TmpBullet.layer = 17 + PCS.GetPlayerNum();
			TmpBullet.GetComponentInChildren<HitScript>().SetOwner(PCS.GetPlayerNum());
			Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
			CtoChange.r += .2f;
			CtoChange.g += .2f;
			CtoChange.b += .2f;
			TmpBullet.GetComponentInChildren<SpriteRenderer>().color = CtoChange;
			
			CanShoot = false;
			ShotTimer = 0;
		}	
	}
}
