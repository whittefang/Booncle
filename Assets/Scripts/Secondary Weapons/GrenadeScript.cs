using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class GrenadeScript : MonoBehaviour {

	public GameObject projectile, Trail;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	string FireButton;
	int ShotTimer;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	EternalScript ES;
	float CookTime;
	float GrenadeDetonateTime;
	ObjectPoolScript OPProjectile, OPTrail;
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		OPProjectile = GameObject.Find ("ObjectPoolerGrenade").GetComponent<ObjectPoolScript> ();
		OPTrail = GameObject.Find ("ObjectPoolerTrailS").GetComponent<ObjectPoolScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetSecondaryMagazineSize (2);
		PCS.SetSecondaryWeapon (Cook);
		PCS.SetSecondaryWeaponRelease (Shoot);
		ShotTimer = 0;
		CookTime = 0;
		ShootFireCooldown = 50;
		CanShoot = true;
		projectile = Resources.Load ("GrenadePrefab") as GameObject;
		Trail = Resources.Load ("ShortTrail") as GameObject;
		GrenadeDetonateTime = projectile.GetComponent<ProjectileScript> ().SelfDestruct;


	}

	void Update(){

	}
	// Update is called once per frame
	void FixedUpdate () {
		if (ShotTimer < ShootFireCooldown) {
			ShotTimer++;
		} else if (!CanShoot){
			CanShoot = true;
		}
	}
	public void Cook(Vector3 GunPoint,Quaternion PointerRotation){
		if (CookTime > GrenadeDetonateTime) {
			Shoot(GunPoint, PointerRotation);
		} else {
			CookTime += Time.deltaTime;
		}
	}

	public void Shoot(Vector3 GunPoint,Quaternion PointerRotation){
		if (CanShoot && AS.CheckCanShootSecondary()){
			SPS.playGrenadeToss();
			AS.ConsumeSecondaryAmmo();
			GameObject TmpBullet = OPProjectile.FetchObject(); //= Instantiate (projectile, GunPoint, PointerRotation) as GameObject;
			TmpBullet.transform.rotation = PointerRotation;
			TmpBullet.transform.position = transform.position;
			GameObject TmpTrail = OPTrail.FetchObject(); //Instantiate (Trail, GunPoint, PointerRotation) as GameObject;
			TmpTrail.SetActive(true);
			TmpTrail.GetComponent<TrailFollowScript>().SetThingToFollow(TmpBullet.transform);
			Material TColor = TmpTrail.GetComponent<TrailRenderer>().material;
			TColor.SetColor("_Color", ES.GetColor(PCS.GetPlayerNum()));
			TmpBullet.layer = 17 + PCS.GetPlayerNum();
			Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
			CtoChange.r += .2f;
			CtoChange.g += .2f;
			CtoChange.b += .2f;
			
			TmpBullet.GetComponent<ProjectileScript>().SetSelfDestruct(CookTime); 
			TmpBullet.SetActive(true);
			
			TmpBullet.GetComponentInChildren<HitScript>().SetOwner(PCS.GetPlayerNum());
			TmpBullet.GetComponentInChildren<SpriteRenderer>().color = CtoChange;
			TmpBullet.GetComponentInChildren<HitScript>().SetOwner(PCS.GetPlayerNum());
			TmpBullet.GetComponentInChildren<ExplosionScript>().SetOwner(PCS.GetPlayerNum());
			

			CanShoot = false;
			ShotTimer = 0;
			CookTime = 0;
		}	
	}
}
