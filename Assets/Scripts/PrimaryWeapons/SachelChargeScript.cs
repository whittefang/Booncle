using UnityEngine;
using System.Collections;

public class SachelChargeScript : MonoBehaviour {
	public GameObject projectile, Trail;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	int ShotTimer, numOfMines, BulletNumber;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	EternalScript ES;
	ObjectPoolScript OPProjectile;
	GameObject[] mines;
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		
		OPProjectile = GameObject.Find ("ObjectPoolerSachel").GetComponent<ObjectPoolScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetPrimaryMagazineSize (3);
		AS.SetReloadTime (3f);
		PCS.SetPrimaryWeapon (Shoot);
		ShotTimer = 0;
		ShootFireCooldown = 10;
		numOfMines = 0;

		mines = new GameObject[3];
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
		if (CanShoot && numOfMines > 2){
			for (int x = 0; x < numOfMines; x++){
				mines[x].GetComponent<SachelExplosionScript>().Detonate();

			}
			numOfMines = 0;
		}else if (CanShoot && AS.CheckCanShootPrimary()){
			SPS.playGrenadeToss();
			AS.ConsumePrimaryAmmo();
			GameObject TmpBullet = OPProjectile.FetchObject(); //= Instantiate (projectile, GunPoint, PointerRotation) as GameObject;
			TmpBullet.transform.rotation = PointerRotation;
			TmpBullet.transform.position = GunPoint;

			TmpBullet.SetActive(true);

			TmpBullet.layer = 17 + PCS.GetPlayerNum();
			TmpBullet.GetComponentInChildren<HitScript>().SetOwner(PCS.GetPlayerNum());
			Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
			CtoChange.r += .2f;
			CtoChange.g += .2f;
			CtoChange.b += .2f;
			TmpBullet.GetComponentInChildren<SpriteRenderer>().color = CtoChange;
			TmpBullet.GetComponentInChildren<HitScript>().SetOwner(PCS.GetPlayerNum());
			TmpBullet.GetComponentInChildren<SachelExplosionScript>().SetOwner(PCS.GetPlayerNum());
			TmpBullet.GetComponentInChildren<SachelExplosionScript>().SetBulletNumber(BulletNumber);
			BulletNumber++;
			CanShoot = false;
			mines[numOfMines] = TmpBullet;
			numOfMines++;
			ShotTimer = 0;
		}	
	}
	void OnDestroy(){
		for (int x = 0; x < numOfMines; x++){
			mines[x].GetComponent<SachelExplosionScript>().Detonate();
		}
		
		numOfMines = 0;
	}
}
