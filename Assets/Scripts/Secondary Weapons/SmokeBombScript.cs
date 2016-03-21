using UnityEngine;
using System.Collections;

public class SmokeBombScript : MonoBehaviour {
	public GameObject projectile;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	string FireButton;
	int ShotTimer;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	ObjectPoolScript OPProjectile;
	// Use this for initialization
	void Start () {
		PCS = GetComponent<PlayerControlScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		OPProjectile = GameObject.Find ("ObjectPoolerSmokeBomb").GetComponent<ObjectPoolScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetSecondaryMagazineSize (2);
		PCS.SetSecondaryWeapon (Shoot);
		ShotTimer = 0;
	
		ShootFireCooldown = 50;
		CanShoot = true;
		projectile = Resources.Load ("GrenadePrefab") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	}
	void FixedUpdate () {
		if (ShotTimer < ShootFireCooldown) {
			ShotTimer++;
		} else if (!CanShoot){
			CanShoot = true;
		}
	}
	public void Shoot(Vector3 GunPoint,Quaternion PointerRotation){
		if (CanShoot && AS.CheckCanShootSecondary ()) {
			SPS.playGrenadeToss();
			AS.ConsumeSecondaryAmmo();
			GameObject TmpBullet = OPProjectile.FetchObject(); //= Instantiate (projectile, GunPoint, PointerRotation) as GameObject;
			TmpBullet.transform.rotation = PointerRotation;
			TmpBullet.transform.position = new Vector3 (transform.position.x, transform.position.y, -5);
			TmpBullet.SetActive(true);
			CanShoot = false;
			ShotTimer = 0;
		}
	}
}
