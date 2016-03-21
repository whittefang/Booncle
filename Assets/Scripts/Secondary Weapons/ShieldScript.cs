using UnityEngine;
using System.Collections;

public class ShieldScript : MonoBehaviour {

	
	public GameObject projectile, projectileOutlinePreFab, POutline;
	PlayerControlScript PCS;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	EternalScript ES;
	ObjectPoolScript OPWall, OPOutline;
	
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetSecondaryMagazineSize (2);
		PCS.SetSecondaryWeapon (ShootOutline);
		PCS.SetSecondaryWeaponRelease (Shoot);
		CanShoot = true;
		projectile = Resources.Load ("ShieldPrefab") as GameObject;
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

			if (!POutline.activeSelf && CanShoot && AS.CheckCanShootSecondary ()){
				POutline.SetActive(true);
				PCS.SetCanShootPrim (false);
				AS.ConsumeSecondaryAmmo();
				Invoke("turnOff", 2f);
				SPS.PlayShieldUp ();
				CanShoot = false;
			}
			if (POutline.activeSelf){
				POutline.transform.position = GunPoint;
				POutline.transform.rotation = PointerRotation;
			
		}
	}
	public void Shoot(Vector3 GunPoint,Quaternion PointerRotation){
		if (POutline.activeSelf) {
			PCS.SetCanShootPrim (true);
			CancelInvoke ();
			turnOff ();
			CanShoot = true;
		}
			
	}
	void turnOff(){
		PCS.SetCanShootPrim (true);
		SPS.PlayShieldDown ();
		POutline.SetActive (false);
	}
	void OnDestroy(){
		Destroy (POutline);
	}
}
