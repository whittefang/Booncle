using UnityEngine;
using System.Collections;

public class WallGunRevisedScript : MonoBehaviour {

	public GameObject projectile, projectileOutlinePreFab, POutline;
	PlayerControlScript PCS;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	EternalScript ES;
	ObjectPoolScript OPWall, OPOutline;
	float WTimer;
	
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetSecondaryMagazineSize (1);
		PCS.SetSecondaryWeapon (ShootOutline);
		PCS.SetSecondaryWeaponRelease (Shoot);
		CanShoot = true;
		projectile = Resources.Load ("WallRevisedProjectileoutlinePrefab") as GameObject;
		OPWall = GameObject.Find ("ObjectPoolerWall").GetComponent<ObjectPoolScript> ();
		POutline = Instantiate(projectile, Vector3.zero, Quaternion.identity) as GameObject;
		Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
		CtoChange.a = .75f;
		POutline.GetComponent<SpriteRenderer> ().color = CtoChange;
		POutline.SetActive (false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}
	public void ShootOutline(Vector3 GunPoint,Quaternion PointerRotation){
		if (CanShoot && AS.CheckCanShootSecondary ()) {
			if (!POutline.activeSelf){
				POutline.SetActive(true);
			}
			POutline.transform.position = GunPoint;
			POutline.transform.rotation = PointerRotation;
			
		}
	}
	public void Shoot(Vector3 GunPoint,Quaternion PointerRotation){
		if (CanShoot && AS.CheckCanShootSecondary()){
			SPS.playGrenadeToss();
			POutline.SetActive(false);
			GameObject TMP = OPWall.FetchObject();
			TMP.transform.position = GunPoint;
			TMP.transform.rotation = PointerRotation;
			TMP.GetComponent<SpriteRenderer>().color = ES.GetColor(PCS.GetPlayerNum());
			TMP.SetActive(true);
			AS.ConsumeSecondaryAmmo();
		}	
	}

	void OnDestroy(){
		Destroy (POutline);
	}

}
