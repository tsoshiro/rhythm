using UnityEngine;
using System.Collections;

public class AudioCtrl : MonoBehaviour {
	public AudioSource[] _audioSourceArray;
	public AudioClip[] _seArray;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < _audioSourceArray.Length; i++) {
			_audioSourceArray [i].clip = _seArray [i];
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayKick() {
		_audioSourceArray[0].Play ();
	}

	public void PlayHat() {
		_audioSourceArray[1].Play ();
	}
}
