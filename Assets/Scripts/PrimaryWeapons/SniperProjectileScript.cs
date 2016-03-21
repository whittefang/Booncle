using UnityEngine;
using System.Collections;

public class SniperProjectileScript : MonoBehaviour {

	public GameObject projectile, Trail;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	int ShotTimer, BulletNumber;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	EternalScript ES;
	ObjectPoolScript OPProjectile, OPTrail;
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		OPProjectile = GameObject.Find ("ObjectPoolerSniper").GetComponent<ObjectPoolScript> ();
		OPTrail = GameObject.Find ("ObjectPoolerTrailL").GetComponent<ObjectPoolScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetPrimaryMagazineSize (4);
		AS.SetReloadTime (3f);
		PCS.SetPrimaryWeapon (Shoot);
		ShotTimer = 0;
		ShootFireCooldown = 25;
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

			AS.ConsumePrimaryAmmo();
			GameObject TmpBullet = OPProjectile.FetchObject(); //= Instantiate (projectile, GunPoint, PointerRotation) as GameObject;
			TmpBullet.transform.rotation = PointerRotation;
			TmpBullet.transform.position = GunPoint;
			TmpBullet.SetActive(true);
			GameObject TmpTrail = OPTrail.FetchObject(); //Instantiate (Trail, GunPoint, PointerRotation) as GameObject;
			TmpTrail.SetActive(true);
			TmpTrail.GetComponent<TrailFollowScript>().SetThingToFollow(TmpBullet.transform);
			Material TColor = TmpTrail.GetComponent<TrailRenderer>().material;
			TColor.SetColor("_Color", ES.GetColor(PCS.GetPlayerNum()));
			TmpBullet.layer = 17 + PCS.GetPlayerNum();
			TmpBullet.GetComponentInChildren<HitScript>().SetOwner(PCS.GetPlayerNum());
			TmpBullet.GetComponentInChildren<HitScript>().SetBulletNumber(BulletNumber);
			BulletNumber++;
			Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
			CtoChange.r += .2f;
			CtoChange.g += .2f;
			CtoChange.b += .2f;
			TmpBullet.GetComponentInChildren<SpriteRenderer>().color = CtoChange;
			SPS.playShoot();
			CanShoot = false;
			ShotTimer = 0;
		}	
	}
}
