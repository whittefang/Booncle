using UnityEngine;
using System.Collections;

public class LightningGunScript : MonoBehaviour {
	//public GameObject projectile, Trail;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	int ShotTimer;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	EternalScript ES;
	ObjectPoolScript OPProjectile, OPTrail;
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		OPProjectile = GameObject.Find ("ObjectPoolerLightning").GetComponent<ObjectPoolScript> ();
		OPTrail = GameObject.Find ("ObjectPoolerTrailS").GetComponent<ObjectPoolScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetPrimaryMagazineSize (4);
		AS.SetReloadTime (2f);
		PCS.SetPrimaryWeapon (Shoot);
		ShotTimer = 0;
		ShootFireCooldown = 10;
		CanShoot = true;
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
			GameObject TmpBullet = OPProjectile.FetchObject();
			TmpBullet.transform.position = GunPoint;
			TmpBullet.transform.rotation = PointerRotation;
			TmpBullet.SetActive(true);
			GameObject TmpTrail = OPTrail.FetchObject();
			TmpTrail.SetActive(true);
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
