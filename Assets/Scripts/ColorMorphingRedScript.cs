using UnityEngine;
using System.Collections;

public class ColorMorphingRedScript : MonoBehaviour {

	public Color C;
	public float red, blue, green, colorSpeed, colorUpper, colorLower;
	int stage;
	// Use this for initialization
	void Start () {
		C.r = 1; C.b = 0; C.g = .1f; C.a = 1;
		stage = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		
		switch (stage){
		case 0:
			if (C.g < colorUpper) {
				C.g += colorSpeed;
			}else{
				stage = 1;
			}
			break;
		case 1:
			if (C.g > colorLower) {
				C.g -= colorSpeed;
			}else{
				stage = 0;
			}
			break;
		}
		blue = C.b;
		red = C.r;
		green = C.g;
	}
	
	// funtion to return current color
	public Color GetColor(){
		return C;
	}
}
