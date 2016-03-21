using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
	public float health;
	public GameObject HitParticles, InvincibleBoarder;
	SoundPlayerScript SPS;
	public ParticleSystem DeathPaticles;
	public Transform DeathTrans;
	public SpriteRenderer HitBoarder;
	public GameObject CorpseObject;
	int[] bulletNumbers;
	RoundHandlerScript RHS;
	PlayerControlScript PCS;
	Color BoarderColor;
	bool Invincible;
	Color PlayerColor;
	CameraControllerScript CCS;
	EndGameStatsScript Stats;
	EternalScript ES;
	// Use this for initialization
	void Start () {
		CCS = GameObject.Find ("Main Camera").GetComponent<CameraControllerScript>();
		PCS = GetComponent<PlayerControlScript> ();
		PlayerColor = GetComponent<SpriteRenderer> ().color;
		RHS = GameObject.Find ("WinTextObj").GetComponent<RoundHandlerScript> ();
		BoarderColor = HitBoarder.color;
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		Stats = GameObject.Find ("TotalStatsHolder").GetComponent<EndGameStatsScript>();
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		Invincible = true;
		bulletNumbers = new int[4];
		for (int x = 0; x < 4; x++) {
			bulletNumbers[x] = -1;
		}
		Invoke ("RemoveIncincible", 2f);
	}
	void RemoveIncincible(){
		Invincible = false;
		InvincibleBoarder.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
	
	}
	// checks health numbers for death
	void HealthCheck(int PlayerNum){
		if (health <= 0){
			DeathTrans.position = transform.position;
			DeathPaticles.Emit(25);
			SPS.playDeath();
			
			Stats.IncrementDeaths (PCS.GetPlayerNum());
			CCS.CancelDeathAnimations();
			if (PlayerNum != PCS.GetPlayerNum()){
				if (RHS.UpdateScore(PlayerNum)){
					CCS.PlayDeathAnimations(transform.position,ES.GetColor(PlayerNum) , true);
				}else {
					CCS.PlayDeathAnimations(transform.position, ES.GetColor(PlayerNum));
				}
			}else {
				RHS.UpdateScore(PlayerNum, true);
				Stats.IncrementSuicides(PlayerNum);
				CCS.PlayDeathAnimations(transform.position, ES.GetColor(PlayerNum));
			}
			Component[] DeathParticlesArray;
			CorpseObject.transform.position = transform.position;
			DeathParticlesArray = CorpseObject.GetComponentsInChildren<DeathAnimationScript> ();
			foreach(DeathAnimationScript Current in DeathParticlesArray){
				Current.PlayDeathAnim(PlayerColor);
			}

			Component[] arrows = GetComponentsInChildren<WallStickScript>();
			foreach (WallStickScript ws in arrows){
				ws.transform.parent = null;
			}
			Destroy(gameObject);
		}
	}
	public void AddHealth(int Amount){
		health += Amount;
	}
	void OnTriggerEnter2D(Collider2D other){
		if ((other.tag == "Grenade" || other.tag == "Bullet") && !Invincible){
			HitScript HS = other.GetComponent<HitScript>();
			if ( other.tag == "Grenade" || ((!Invincible) && (!(HS.GetOwner() == PCS.GetPlayerNum()))) && !HS.DamageInactive){
				if (HS.IsKnife){
					HS.DamageInactive = true;
				}
				SPS.playHit();
				if (HS.GetBulletNumber() > bulletNumbers[HS.GetOwner()]){
					bulletNumbers[HS.GetOwner()] = HS.GetBulletNumber();
					Stats.IncrementHits(HS.GetOwner());
				}
				Stats.IncrementDamageDone(HS.GetOwner(), HS.GetDamage());
				health = health - HS.GetDamage();
				StopAllCoroutines (); 
				StartCoroutine(lerpHit());
				HealthCheck(HS.GetOwner());
				HS.Kill(transform.position, true);
			}else if (!(HS.GetOwner() == PCS.GetPlayerNum())){
				HS.Kill(transform.position, false);
			}
		}
	}
	IEnumerator lerpHit(){
		Color tmp = BoarderColor;
		while (HitBoarder.color.a < 1){
			tmp.a += .2f;
			HitBoarder.color = tmp;
			yield return new WaitForSeconds(.015f);
		}
		while (HitBoarder.color.a > 0){
			tmp.a -= .05f;
			HitBoarder.color = tmp;
			yield return new WaitForSeconds(.015f);
		}
		
	}
}
