using UnityEngine;
using System.Collections;

public class ColorMorphingScript : MonoBehaviour {
	public Color C;
	public float red, blue, green, colorSpeed, colorUpper, colorLower;
	int stage;
	// Use this for initialization
	void Start () {
		C.r = red; C.b = blue; C.g = green;
		stage = 0;
	}
	
	// Update is called once per frame
	void Update () {


		switch (stage){
		case 0:
			if (C.b < colorUpper) {
				C.b += colorSpeed;
			}else{
				stage = 1;
			}
			break;
		case 1:
			if (C.r > colorLower) {
				C.r -= colorSpeed;
			}else{
				stage = 2;
			}
			break;
		case 2:
			if (C.g < colorUpper){
				C.g += colorSpeed;
			}else{
				stage = 3;
			}
			break;
		case 3:
			if (C.b > colorLower){
				C.b -= colorSpeed;
			}else{
				stage = 4;
			}
			break;
		case 4:
			if (C.r < colorUpper){
				C.r += colorSpeed;
			}else{
				stage = 5;
			}
			break;
		case 5:
			if (C.g > colorLower){
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
