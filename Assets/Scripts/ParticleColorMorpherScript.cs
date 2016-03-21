using UnityEngine;
using System.Collections;

public class ParticleColorMorpherScript: MonoBehaviour {
	public Color C;
	public float colorSpeed, colorVarience, currentMorphAmount;
	int stage;
	ParticleSystem PS;
	// Use this for initialization
	void Start () {
		PS = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		ColorMorph ();
	}
	void ColorMorph(){
		switch (stage){
		case 0:
			if (currentMorphAmount < colorVarience) {
				C.g += colorSpeed;
				C.r += colorSpeed;
				C.b += colorSpeed;
				currentMorphAmount += colorSpeed;
			}else{
				stage = 1;
			}
			break;
		case 1:
			if (currentMorphAmount > .01) {
				C.g -= colorSpeed;
				C.r -= colorSpeed;
				C.b -= colorSpeed;
				currentMorphAmount -= colorSpeed;
			}else{
				stage = 0;
			}
			break;
		}
		PS.startColor = C;
	}
	public void SetColor(Color NewColor){
		C = NewColor;
	}
}
