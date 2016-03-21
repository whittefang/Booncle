using UnityEngine;
using System.Collections;

public class SoundPlayerScript : MonoBehaviour {

	public AudioClip Hit;
	public AudioClip Shoot;
	public AudioClip Reload;
	public AudioClip Explosion;
	public AudioClip GrenadeToss;
	public AudioClip Interceptor;
	public AudioClip Death;
	public AudioClip Select;
	public AudioClip Step;
	public AudioClip Boomerang;
	public AudioClip ShieldUp;
	public AudioClip ShieldDown;
	public AudioClip ButtonMove;
	public AudioClip ButtonBack;
	public AudioClip ButtonLevelTransition;
	public AudioClip LazerShoot;	
	public AudioClip LazerCharge1;	
	public AudioClip LazerCharge2;
	public AudioClip LazerCharge3;
	AudioSource AS;
	// Use this for initialization
	void Start () {
		AS = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void playHit(){
		AS.Stop ();
		AS.PlayOneShot (Hit);
	}
	public void playShoot(){
		AS.Stop ();
		AS.PlayOneShot (Shoot);
	}
	public void playReload(){
		AS.Stop ();
		AS.PlayOneShot (Reload);
	}
	public void playExplosion(){
		AS.Stop ();
		AS.PlayOneShot (Explosion);
	}
	public void playGrenadeToss(){
		AS.Stop ();
		AS.PlayOneShot (GrenadeToss);
	}
	public void playInterceptor(){
		AS.Stop ();
		AS.PlayOneShot (Interceptor);
	}
	public void playDeath(){
		AS.Stop ();
		AS.PlayOneShot (Death);
	}
	public void playSelect(){
		AS.Stop ();
		AS.PlayOneShot (Select);
	}
	public void playStep(){
		AS.Stop ();
		AS.PlayOneShot (Step);
	}
	public void PlayButtonMove(){
		AS.Stop ();
		AS.PlayOneShot (ButtonMove);
	}
	public void PlayButtonBack(){
		AS.Stop ();
		AS.PlayOneShot (ButtonBack);
	}
	public void PlayButtonLevelTransition(){
		AS.Stop ();
		AS.PlayOneShot (ButtonLevelTransition);
	}
	public void PlayBoomerang(){
		AS.Stop ();
		AS.PlayOneShot (Boomerang);
	}
	public void PlayShieldUp(){
		AS.Stop ();
		AS.PlayOneShot (ShieldUp);
	}
	public void PlayShieldDown(){
		AS.Stop ();
		AS.PlayOneShot (ShieldDown);
	}
	public void PlayLazerShoot(){
		AS.Stop ();
		AS.PlayOneShot (LazerShoot);
	}
	public void PlayLazerCharge(int level){
		AS.Stop ();
		switch (level) {
		case 0:
			AS.PlayOneShot(LazerCharge1);
			break;
		case 1:
			AS.PlayOneShot(LazerCharge2);
			break;
		default:
			AS.PlayOneShot(LazerCharge3);
			break;
		}
	}
}
