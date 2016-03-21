using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AmmoScript : MonoBehaviour {
	public int PrimaryMagazineSize;
	public int PrimaryCurrentAmmo;
	public int SecondaryCurrentAmmo;
	PlayerControlScript PCS;
	public ParticleSystem PSShell, PSFlash;
	public Transform ShellTrans;
	public GameObject SecondaryOne, SecondaryTwo;
	EndGameStatsScript Stats;
	float Delay;
	SpriteRenderer SpriteColor;
	Color CurrentColor;
	EternalScript ES;
	SoundPlayerScript SPS;
	// Use this for initialization
	void Start () {
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		PCS = GetComponent<PlayerControlScript> ();
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		SpriteColor = GetComponent<SpriteRenderer> ();
		CurrentColor = ES.GetColor(PCS.GetPlayerNum());
		SpriteColor.color = CurrentColor;
		
		Stats = GameObject.Find ("TotalStatsHolder").GetComponent<EndGameStatsScript>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

	public void SetPrimaryMagazineSize(int Size){
		PrimaryMagazineSize = Size;
		Reload ();
	}
	public bool CheckCanShootPrimary(){
		return ((PrimaryCurrentAmmo-1) >= 0);
	}
	public void SetReloadTime(float DelayTime){
		Delay = DelayTime;
	}
	public void Reload(){
		SpriteColor.color = CurrentColor;
		PrimaryCurrentAmmo = PrimaryMagazineSize;
		
	}
	public void ReloadSingle(){
		SpriteColor.color = CurrentColor;
		PrimaryCurrentAmmo++;
		
	}
	public void ConsumePrimaryAmmo(bool flash = true){
		PrimaryCurrentAmmo--;
		ShellTrans.position = transform.position;
		ShellTrans.rotation = transform.rotation;
		if (flash) {
			PSShell.Emit (1);
			PSFlash.Emit (15);
		}
		Stats.IncrementShots (PCS.GetPlayerNum ());
		if (PrimaryCurrentAmmo <= 0){
			SpriteColor.color = Color.gray;
			SPS.playReload();

			Invoke ("Reload", Delay);
		}
	}
	public void ConsumePrimaryAmmoNoReload(){
		PrimaryCurrentAmmo--;
		
		Stats.IncrementShots (PCS.GetPlayerNum ());
		//PSFlash.Emit(15);
		if (PrimaryCurrentAmmo <= 0){
			SpriteColor.color = Color.gray;
		}
	}
	public void SetSecondaryMagazineSize(int Size){
		SecondaryCurrentAmmo = Size;
		if (Size == 1) {
			ConsumeSecondaryAmmo();
			SecondaryCurrentAmmo = Size;
		}
	}
	public bool CheckCanShootSecondary(){
		return ((SecondaryCurrentAmmo-1) >= 0);
	}
	public void ConsumeSecondaryAmmo(){
		SecondaryCurrentAmmo--;
		if (SecondaryOne.activeSelf) {
			SecondaryOne.gameObject.SetActive(false);
		} else {
			SecondaryTwo.gameObject.SetActive(false);
		}
	}
	public void ReloadSec(){
		if (SecondaryCurrentAmmo < 2){
			SecondaryCurrentAmmo++;
			if (!SecondaryTwo.activeSelf) {
				SecondaryTwo.gameObject.SetActive(true);
			} else {
				SecondaryOne.gameObject.SetActive(true);
			}
		}
	}
	public void DestroySecondaryAmmo(){
		if (SecondaryOne != null) {
			Destroy (SecondaryOne.gameObject);
		} 
		if (SecondaryTwo != null){
			Destroy (SecondaryTwo.gameObject);
		}
	}
	void OnDestroy(){
		DestroySecondaryAmmo ();
	}
}
