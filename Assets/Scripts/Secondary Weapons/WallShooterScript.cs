using UnityEngine;
using System.Collections;

public class WallShooterScript : MonoBehaviour {

	public GameObject ProjectileWrapper, PrimaryBullet, SecondaryBullet;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	int ShotTimer;
	bool CanShoot;
	bool FirstShot = true;
	AmmoScript AS;
	SoundPlayerScript SPS;
	WallProjectileScript WPS;
	EternalScript ES;
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetSecondaryMagazineSize (2);
		PCS.SetSecondaryWeapon (Shoot);
		ShotTimer = 0;
		ShootFireCooldown = 10;
		CanShoot = true;
		ProjectileWrapper = Resources.Load ("WallProjectileWrapper") as GameObject;
		ProjectileWrapper = Instantiate (ProjectileWrapper, Vector3.zero, Quaternion.identity) as GameObject;
		PrimaryBullet = ProjectileWrapper.transform.GetChild(0).gameObject;
		SecondaryBullet = ProjectileWrapper.transform.GetChild(1).gameObject;
		WPS = PrimaryBullet.GetComponent<WallProjectileScript> ();
		PrimaryBullet.SetActive(false);
		SecondaryBullet.SetActive(false);
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
		if (CanShoot && AS.CheckCanShootSecondary () && FirstShot) {
			RaycastHit2D hit = Physics2D.Raycast(GunPoint, transform.up);
			Vector3 FinalPoint = hit.point;
			if (hit.point.x < transform.position.x){
				FinalPoint.x += .1f;
			}else{
				FinalPoint.x -= .1f;
			} 
			if (hit.point.y < transform.position.y){
				FinalPoint.y += .1f;
			}else{
				FinalPoint.y -= .1f;
			} 

			if (hit.collider != null) {
				SPS.playShoot();
				AS.ConsumeSecondaryAmmo ();
				PrimaryBullet.SetActive(true);
				PrimaryBullet.transform.position = FinalPoint;
				PrimaryBullet.GetComponent<Rigidbody2D>().isKinematic = true;
				Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
				CtoChange.r -= .1f;
				CtoChange.g -= .1f;
				CtoChange.b -= .1f;
				PrimaryBullet.GetComponentInChildren<SpriteRenderer>().color = CtoChange;
				CanShoot = false;
				FirstShot = false;
				ShotTimer = 0;
			}
		} else if (CanShoot && AS.CheckCanShootSecondary ()) {
			RaycastHit2D hit = Physics2D.Raycast(GunPoint, transform.up);
			Vector3 FinalPoint = hit.point;
			if (hit.point.x < transform.position.x){
				FinalPoint.x += .1f;
			}else{
				FinalPoint.x -= .1f;
			} 
			if (hit.point.y < transform.position.y){
				FinalPoint.y += .1f;
			}else{
				FinalPoint.y -= .1f;
			}
			if (hit.collider != null) {
				SPS.playShoot();
				AS.ConsumeSecondaryAmmo ();
				SecondaryBullet.SetActive(true);
				SecondaryBullet.transform.position = FinalPoint;
				SecondaryBullet.GetComponent<Rigidbody2D>().isKinematic = true;
				Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
				WPS.ActivateWall(CtoChange);
				CtoChange.r -= .1f;
				CtoChange.g -= .1f;
				CtoChange.b -= .1f;
				SecondaryBullet.GetComponentInChildren<SpriteRenderer>().color = CtoChange;

			}
		}	
	}
}
