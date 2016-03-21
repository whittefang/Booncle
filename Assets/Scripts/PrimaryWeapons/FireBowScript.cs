using UnityEngine;
using System.Collections;

public class FireBowScript : MonoBehaviour {

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
		OPProjectile = GameObject.Find ("ObjectPoolerFireArrow").GetComponent<ObjectPoolScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetPrimaryMagazineSize (3);
		AS.SetReloadTime (1.5f);
		PCS.SetPrimaryWeapon (Shoot);
		ShotTimer = 0;
		ShootFireCooldown = 15;
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
			SPS.PlayBoomerang();
			AS.ConsumePrimaryAmmo(false);
			GameObject TmpBullet = OPProjectile.FetchObject(); 
			TmpBullet.transform.position = GunPoint;
			TmpBullet.transform.rotation = PointerRotation;
			TmpBullet.SetActive(true);
			Material TColor =TmpBullet.GetComponentInChildren<TrailRenderer>().material;

			TmpBullet.layer = 17 + PCS.GetPlayerNum();
			TmpBullet.GetComponentInChildren<HitScript>().SetOwner(PCS.GetPlayerNum());
			TmpBullet.GetComponentInChildren<HitScript>().SetBulletNumber(BulletNumber);
			BulletNumber++;
			Color CtoChange = ES.GetColor(PCS.GetPlayerNum());

			Component[] sprites = TmpBullet.GetComponentsInChildren<SpriteRenderer>();
			foreach(SpriteRenderer s in sprites){
				s.color = CtoChange;
				CtoChange.r += .2f;
				CtoChange.g += .2f;
				CtoChange.b += .2f;
			}
			TColor.SetColor("_Color", CtoChange);			
			CanShoot = false;
			ShotTimer = 0;
		}	
	}
}
