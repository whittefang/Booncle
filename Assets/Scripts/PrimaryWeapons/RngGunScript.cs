using UnityEngine;
using System.Collections;

public class RngGunScript : MonoBehaviour {

	public GameObject projectile, Trail, projectileG;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	int ShotTimer, BulletNumber;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	EternalScript ES;
	ObjectPoolScript OPProjectile, OPGrenade, OPTrail;
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		OPProjectile = GameObject.Find ("ObjectPoolerRng").GetComponent<ObjectPoolScript> ();
		OPGrenade = GameObject.Find ("ObjectPoolerGrenadeRng").GetComponent<ObjectPoolScript> ();
		OPTrail = GameObject.Find ("ObjectPoolerTrailS").GetComponent<ObjectPoolScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetPrimaryMagazineSize (5);
		AS.SetReloadTime (2f);
		PCS.SetPrimaryWeapon (Shoot);
		ShotTimer = 0;
		ShootFireCooldown = 20;
		CanShoot = true;
		projectile = Resources.Load ("RngGunProjectile") as GameObject;
		projectileG = Resources.Load ("GrenadeRngPrefab") as GameObject;
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
			FireBullet(GunPoint, PointerRotation);
			SPS.playShoot();
			AS.ConsumePrimaryAmmo();
			CanShoot = false;
			ShotTimer = 0;
			StartCoroutine(MultiCast(GunPoint, PointerRotation));
		}	
	}

	void FireBullet(Vector3 GunPoint,Quaternion PointerRotation){


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

		Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
		CtoChange.r += .2f;
		CtoChange.g += .2f;
		CtoChange.b += .2f;
		TmpBullet.GetComponentInChildren<SpriteRenderer>().color = CtoChange;
	}
	void FireGrenade(Vector3 GunPoint,Quaternion PointerRotation){
	
		GameObject TmpBullet = OPGrenade.FetchObject(); //= Instantiate (projectile, GunPoint, PointerRotation) as GameObject;
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
		TmpBullet.GetComponentInChildren<ExplosionScript>().SetBulletNumber(BulletNumber);
		TmpBullet.GetComponentInChildren<ExplosionScript>().SetOwner(PCS.GetPlayerNum());
		Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
		CtoChange.r += .2f;
		CtoChange.g += .2f;
		CtoChange.b += .2f;
		TmpBullet.GetComponentInChildren<SpriteRenderer>().color = CtoChange;
	}
	IEnumerator MultiCast(Vector3 GunPoint,Quaternion PointerRotation){
		int tmp = Random.Range (0, 101);
		yield return new WaitForSeconds (.06f);
		if (tmp <= 65) {
			SPS.playShoot();
			
			PointerRotation = PointerRotation * Quaternion.Euler(0, 0, -1);
			FireBullet(GunPoint, PointerRotation);
			PointerRotation = PointerRotation * Quaternion.Euler(0, 0, 2);
			FireBullet(GunPoint, PointerRotation);
		}
		yield return new WaitForSeconds (.06f);
		if (tmp <= 40) {
			SPS.playShoot();
			FireBullet(GunPoint, PointerRotation);
			PointerRotation = PointerRotation * Quaternion.Euler(0, 0, -1);
			FireBullet(GunPoint, PointerRotation);
			PointerRotation = PointerRotation * Quaternion.Euler(0, 0, -1);
			FireBullet(GunPoint, PointerRotation);
		}

		yield return new WaitForSeconds (.06f);
		if (tmp <= 10) {
			SPS.playGrenadeToss();
			//FireGrenade(GunPoint, PointerRotation);
			FireBullet(GunPoint, PointerRotation);
			PointerRotation = PointerRotation * Quaternion.Euler(0, 0, 1);
			//FireGrenade(GunPoint, PointerRotation);
			FireBullet(GunPoint, PointerRotation);
			PointerRotation = PointerRotation * Quaternion.Euler(0, 0, 1);
			//FireGrenade(GunPoint, PointerRotation);
			FireBullet(GunPoint, PointerRotation);
		}
		BulletNumber++;
	}
}
