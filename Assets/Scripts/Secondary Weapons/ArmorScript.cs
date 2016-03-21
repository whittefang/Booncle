using UnityEngine;
using System.Collections;

public class ArmorScript : MonoBehaviour {
	public int AdditionalHealth;
	// Use this for initialization
	void Start () {
		GetComponent<AmmoScript> ().DestroySecondaryAmmo ();

		AdditionalHealth = 2;
		GetComponent<HealthScript> ().AddHealth (AdditionalHealth);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
