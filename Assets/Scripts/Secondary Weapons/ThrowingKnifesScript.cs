using UnityEngine;
using System.Collections;

public class ThrowingKnifesScript : MonoBehaviour {

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
		PCS = GetComponent<PlayerControlScript> ();
		SPS = GameObject.Find ("Main Camera").GetComponent<SoundPlayerScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetSecondaryMagazineSize (2);
		PCS.SetSecondaryWeapon (Shoot);
		ShotTimer = 0;
		ShootFireCooldown = 50;
		CanShoot = true;
		projectile = Resources.Load ("KnifeProjectilePrefab") as GameObject;
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
		if (CanShoot && AS.CheckCanShootSecondary()){
			SPS.playGrenadeToss();
			AS.ConsumeSecondaryAmmo();
			GameObject tmp = Instantiate (projectile, GunPoint, PointerRotation) as GameObject;

			Material TColor = tmp.GetComponentInChildren<TrailRenderer>().material;
			TColor.SetColor("_Color", ES.GetColor(PCS.GetPlayerNum()));
			tmp.layer = 17 + PCS.GetPlayerNum();
			tmp.GetComponentInChildren<HitScript>().SetOwner(PCS.GetPlayerNum());
			Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
			CtoChange.r += .2f;
			CtoChange.g += .2f;
			CtoChange.b += .2f;
			tmp.GetComponentInChildren<SpriteRenderer>().color = CtoChange;
			CanShoot = false;
			ShotTimer = 0;
		}	
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Bullet" || other.tag == "Knife") {
			HitScript HS = other.GetComponent<HitScript> ();
			if ((HS.GetOwner() == PCS.GetPlayerNum()) && HS.IsKnife) {
				AS.ReloadSec ();
				HS.KillBoomerang();
			}
		}
	}
}
