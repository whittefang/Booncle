using UnityEngine;
using System.Collections;

public class LazerSniperScript : MonoBehaviour {

	public GameObject projectile, particlesOBJ;
	ParticleSystem particlesSystem;
	PlayerControlScript PCS;
	public int ShootFireCooldown;
	int ShotTimer, BulletNumber;
	bool CanShoot;
	AmmoScript AS;
	SoundPlayerScript SPS;
	EternalScript ES;
	ObjectPoolScript OPProjectile, OPTrail;
	public int chargeLevel;
	float chargeAmount;
	bool firstPress = true;
	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		OPProjectile = GameObject.Find ("ObjectPoolerLazer").GetComponent<ObjectPoolScript> ();
		particlesOBJ = Resources.Load ("ChargeParticleHolder") as GameObject;
		AS = GetComponent<AmmoScript>();
		AS.SetPrimaryMagazineSize (6);
		AS.SetReloadTime (3f);
		PCS.SetPrimaryWeapon (Charge);
		PCS.SetPrimaryWeaponRelease (Shoot);
		ShotTimer = 0;
		chargeLevel = 1;
		ShootFireCooldown = 7;
		CanShoot = true;
		particlesOBJ = Instantiate (particlesOBJ, transform.position, transform.rotation) as GameObject;
		particlesSystem = particlesOBJ.GetComponentInChildren<ParticleSystem> ();
		particlesSystem.startColor = ES.GetColor(PCS.GetPlayerNum());
		particlesOBJ.SetActive (false);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (ShotTimer < ShootFireCooldown) {
			ShotTimer++;
		} else if (!CanShoot){
			CanShoot = true;
		}
	}
	public void Charge(Vector3 GunPoint,Quaternion PointerRotation){
		if (CanShoot && AS.CheckCanShootPrimary ()) {
			// code to charge beam, need a new player controller functionallity
			if (!particlesOBJ.activeSelf) {
				particlesOBJ.SetActive (true);
				SPS.PlayLazerCharge(chargeLevel);
			}
			particlesOBJ.transform.position = GunPoint;
			particlesOBJ.transform.rotation = PointerRotation;
			if (firstPress){
				chargeAmount = Time.time;
				firstPress = false;
			}
		
			if ((Time.time - chargeAmount >= 1 && chargeLevel < 2) || (Time.time - chargeAmount >= 2 && chargeLevel < 3)) {
				chargeLevel++;
				SPS.PlayLazerCharge(chargeLevel);
				Color c = particlesSystem.startColor;
				c.r += .2f;
				c.g += .2f;
				c.b += .2f;
				c.a = 1;
				particlesSystem.startColor = c;
				particlesSystem.Emit (20);
				particlesSystem.Play ();
			}

			if (chargeLevel == 3 && Time.time - chargeAmount > 2){
				particlesOBJ.transform.localScale = new Vector2(.5f,.5f);
				particlesSystem.emissionRate = 40f;
			}
		
		}
	}
	public void Shoot(Vector3 GunPoint,Quaternion PointerRotation){
		if (CanShoot && AS.CheckCanShootPrimary()){

			Color CtoChange = ES.GetColor(PCS.GetPlayerNum());
			CtoChange.r += .025f;
			CtoChange.g += .025f;
			CtoChange.b += .025f;

			AS.ConsumePrimaryAmmo();
			GameObject TmpBullet = OPProjectile.FetchObject();
		
			TmpBullet.GetComponent<LazerProjectileScript>().ShootLine(chargeLevel, CtoChange, transform.position, transform.up, PCS.GetPlayerNum(), BulletNumber);
			
				
				//if (chargeLevel > 2){
			//		diff = new Vector2( hit2.point.x - hit.point.x, hit2.point.y - hit.point.y);
		//			hit3 = Physics2D.Raycast (hit2.point, Vector3.Reflect(diff.normalized, hit2.normal), Mathf.Infinity, Layer);
	//				TmpBullet.GetComponent<LazerProjectileScript>().ShootLine(chargeLevel, CtoChange, transform.position, hit.point, hit2.point, hit3.point);
//				}else{
//					TmpBullet.GetComponent<LazerProjectileScript>().ShootLine(chargeLevel, CtoChange, transform.position, hit.point, hit2.point);
//				}


			//TmpBullet.SetActive(true);
			//TmpBullet.layer = 17 + PCS.GetPlayerNum();
			//TmpBullet.GetComponent<HitScript>().BulletDamage = chargeLevel;
			//TmpBullet.GetComponent<HitScript>().SetBulletNumber(BulletNumber);
			BulletNumber++;
			//TmpBullet.GetComponent<HitScript>().SetOwner(PCS.GetPlayerNum());

			TmpBullet.GetComponentInChildren<LineRenderer>().SetColors(CtoChange, CtoChange);

			SPS.PlayLazerShoot();
			CanShoot = false;
			ShotTimer = 0;
			chargeLevel = 1;
			chargeAmount = 0;
			
			particlesSystem.startColor = ES.GetColor(PCS.GetPlayerNum());
			
			particlesOBJ.transform.localScale = new Vector2(1f,1f);
			particlesSystem.emissionRate = 25f;
			particlesOBJ.SetActive(false);
			firstPress = true;
		}	
	}
	void OnDestroy(){
		Destroy(particlesOBJ);
	}
}
