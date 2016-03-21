using UnityEngine;
using System.Collections;

public class TripMineScript : MonoBehaviour {
	//public GameObject projectile, Trail;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	int ShotTimer;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	EternalScript ES;
	ObjectPoolScript OPProjectile, OPTrail;
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		OPProjectile = GameObject.Find ("ObjectPoolerTripMine").GetComponent<ObjectPoolScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		AS = GetComponent<AmmoScript>();
		AS.SetSecondaryMagazineSize (2);
		PCS.SetSecondaryWeapon (Shoot);
		ShotTimer = 0;
		ShootFireCooldown = 25;
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
		if (CanShoot && AS.CheckCanShootSecondary()){

			int Layer = 1 << 9;
			RaycastHit2D hit = Physics2D.Raycast (transform.position, transform.up, Mathf.Infinity, Layer);
			if (hit.collider != null && (hit.collider.tag == "Wall" || hit.collider.tag == "MovingWall") && hit.distance < .5f) {
				SPS.playGrenadeToss();
				AS.ConsumeSecondaryAmmo();
				GameObject TmpBullet = OPProjectile.FetchObject();
				TmpBullet.transform.position = hit.point;
				Debug.Log( hit.normal);
				Debug.Log (Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg);
				TmpBullet.transform.rotation = Quaternion.identity;
				TmpBullet.transform.rotation = Quaternion.FromToRotation(TmpBullet.transform.up, hit.normal);

				TmpBullet.SetActive(true);
				TmpBullet.layer = 17 + PCS.GetPlayerNum();
				TmpBullet.GetComponentInChildren<HitScript>().SetOwner(PCS.GetPlayerNum());
				Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
				TmpBullet.GetComponentInChildren<SpriteRenderer>().color = CtoChange;
				TmpBullet.GetComponent<TripMineLineScript>().SetColors(CtoChange);
				TmpBullet.GetComponentInChildren<ExplosionScript>().SetOwner(PCS.GetPlayerNum());
				CanShoot = false;
				ShotTimer = 0;
				
				TmpBullet.transform.parent = hit.collider.transform;
			}


		}	
	}
}
