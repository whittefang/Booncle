using UnityEngine;
using System.Collections;

public class ChaffScript : MonoBehaviour {

	public GameObject projectile;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	int ShotTimer;
	bool CanShoot;
	AmmoScript AS;
	// Use this for initialization
	void Start () {
		PCS = GetComponent<PlayerControlScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetSecondaryMagazineSize (3);
		PCS.SetSecondaryWeapon (Shoot);
		ShotTimer = 0;
		ShootFireCooldown = 50;
		CanShoot = true;
		projectile = Resources.Load ("ChaffPrefab") as GameObject;
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
			AS.ConsumeSecondaryAmmo();
			CanShoot = false;
			ShotTimer = 0;
			
			PointerRotation = PointerRotation * Quaternion.Euler(0, 0, -30);
			for (int i = 0; i < 20; i++){
				Instantiate (projectile, GunPoint, PointerRotation);
				PointerRotation = PointerRotation * Quaternion.Euler(0, 0, 3);
			}			
		}	
	}
}
