using UnityEngine;
using System.Collections;

public class MusicScript : MonoBehaviour {
	public AudioClip MainSong, MenuMusic;
	AudioSource AS;
	int currentClip;
	// Use this for initialization
	void Start () {
		currentClip = 0;
		AS = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void PlaySong(){
		if (currentClip == 1){
			AS.clip = MainSong;
			AS.Play ();
			currentClip = 0;
		}
	}
	void PlayMenu(){
		if (currentClip == 0){
			AS.clip = MenuMusic;
			AS.Play ();
			currentClip = 1;
		}
	}
	void OnLevelWasLoaded(int level){
		if (level > 3) {
			PlaySong ();
		} else {
			PlayMenu();
		}
	}

}
