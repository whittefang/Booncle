using UnityEngine;
using System.Collections;

public class CameraColorScript : MonoBehaviour {
	ColorMorphingScript CMS;
	Camera Cam;
	// Use this for initialization
	void Start () {
		CMS = GetComponent<ColorMorphingScript> ();
		Cam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		Cam.backgroundColor = CMS.GetColor ();
	}
}
