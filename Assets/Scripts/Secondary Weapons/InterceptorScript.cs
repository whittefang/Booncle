using UnityEngine;
using System.Collections;

public class InterceptorScript : MonoBehaviour {

	public GameObject projectile;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	int ShotTimer;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	ObjectPoolScript OP;
	// Use this for initialization
	void Start () {
		PCS = GetComponent<PlayerControlScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		OP = GameObject.Find ("ObjectPoolerIntercept").GetComponent<ObjectPoolScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetSecondaryMagazineSize (2);
		PCS.SetSecondaryWeapon (Shoot);
		ShotTimer = 0;
		ShootFireCooldown = 50;
		CanShoot = true;
		projectile = Resources.Load ("InterCeptorPrefab") as GameObject;
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
			SPS.playInterceptor();
			AS.ConsumeSecondaryAmmo();
			GameObject TMP = OP.FetchObject();
			TMP.transform.position = transform.position;
			TMP.transform.rotation = transform.rotation;
			TMP.SetActive(true);
			CanShoot = false;
			ShotTimer = 0;
		}	
	}
}
