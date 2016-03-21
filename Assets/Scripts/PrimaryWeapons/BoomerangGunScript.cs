using UnityEngine;
using System.Collections;

public class BoomerangGunScript : MonoBehaviour {

	public GameObject projectile, Trail;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	int ShotTimer, BulletNumber;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	EternalScript ES;
	ObjectPoolScript OPProjectile;
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		OPProjectile = GameObject.Find ("ObjectPoolerBoomerang").GetComponent<ObjectPoolScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetPrimaryMagazineSize (1);
		AS.SetReloadTime (2f);
		PCS.SetPrimaryWeapon (Shoot);
		ShotTimer = 0;
		ShootFireCooldown = 35;
		CanShoot = true;
		projectile = Resources.Load ("BoomerangProjectilePrefab") as GameObject;
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
			SPS.PlayBoomerang();
			AS.ConsumePrimaryAmmoNoReload();
			GameObject TmpBullet = OPProjectile.FetchObject(); //= Instantiate (projectile, GunPoint, PointerRotation) as GameObject;
			TmpBullet.transform.rotation = PointerRotation;
			TmpBullet.transform.position = GunPoint;
			TmpBullet.SetActive(true);
			TmpBullet.GetComponent<ProjectileScript>().setPlayerReturn(transform);
			Component[] TrailArray;
			TrailArray = TmpBullet.GetComponentsInChildren<TrailRenderer> ();
			foreach(TrailRenderer Current in TrailArray){
				Material TColor = Current.material;
				TColor.SetColor("_Color", ES.GetColor(PCS.GetPlayerNum()));
			}


			TmpBullet.layer = 17 + PCS.GetPlayerNum();
			TmpBullet.GetComponentInChildren<HitScript>().SetOwner(PCS.GetPlayerNum());
			TmpBullet.GetComponentInChildren<HitScript>().SetBulletNumber(BulletNumber);
			BulletNumber++;
			Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
			CtoChange.r += .2f;
			CtoChange.g += .2f;
			CtoChange.b += .2f;
			TmpBullet.GetComponentInChildren<SpriteRenderer>().color = CtoChange;
			
			CanShoot = false;
			ShotTimer = 0;
		}	
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Bullet" || other.tag == "Boomerang") {
			HitScript HS = other.GetComponent<HitScript> ();
			if ((HS.GetOwner() == PCS.GetPlayerNum()) && HS.IsBoomerang) {
				AS.ReloadSingle ();
				HS.KillBoomerang();
			}
		}
	}
}
