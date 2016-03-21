using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerColorSetScript : MonoBehaviour {
	public Image preview1, preview2;
	public Slider RedSlider, BlueSlider, GreenSlider;
	public Color CurrentColor;
	public int PlayerNum;
	EternalScript ES;

	// Use this for initialization
	void Start () {
		ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
		preview1 = GetComponent<Image> ();
		CurrentColor = Color.white;
	}

	void OnLevelWasLoaded(int level) {
		if (level == 2) {
			ES = GameObject.Find ("EternalHolder").GetComponent<EternalScript> ();
			CurrentColor = ES.GetColor(PlayerNum);
			RedSlider.value = CurrentColor.r;
			GreenSlider.value = CurrentColor.g;
			BlueSlider.value = CurrentColor.b;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		CurrentColor.r = RedSlider.value;
		CurrentColor.b = BlueSlider.value;
		CurrentColor.g = GreenSlider.value;
		preview1.color = CurrentColor;
		preview2.color = CurrentColor;

		ES.setColor(CurrentColor, PlayerNum);

	}
}
