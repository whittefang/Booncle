using UnityEngine;
using System.Collections;

public class SwordWeaponScript : MonoBehaviour {

	public GameObject projectile;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	int ShotTimer;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	EternalScript ES;
	Vector3 GunPointPerm;
	Quaternion RotationPerm;
	int ChargeAmount;
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetPrimaryMagazineSize (1);
		AS.SetReloadTime (.5f);
		PCS.SetPrimaryWeapon (Charge);
		ShotTimer = 0;
		ShootFireCooldown = 15;
		CanShoot = true;
		projectile = Resources.Load ("SwordPrefab") as GameObject;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (ShotTimer < ShootFireCooldown) {
			ShotTimer++;
		} else if (!CanShoot){
			CanShoot = true;
		}
	}
	void Update(){
		if (Input.GetButtonUp("FireP1Prim")){
			releaseCharge();
		}
	}
	void releaseCharge(){
		if (CanShoot && AS.CheckCanShootPrimary()){
			// change to sword sound
			SPS.playShoot();
			AS.ConsumePrimaryAmmo();
			GameObject TmpBullet = Instantiate (projectile, GunPointPerm, RotationPerm) as GameObject;
			TmpBullet.transform.parent = transform;
			Material TColor = TmpBullet.GetComponentInChildren<TrailRenderer>().material;
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
			ChargeAmount = 0;
		}	
	}
	public void Charge(Vector3 GunPoint,Quaternion PointerRotation){
		if (CanShoot && AS.CheckCanShootPrimary ()) {
			GunPointPerm = GunPoint;
			RotationPerm = PointerRotation;
			if (ChargeAmount <= 100){
				ChargeAmount++;
			}
		}
	}

}
