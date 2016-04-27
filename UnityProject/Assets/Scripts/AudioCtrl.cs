using UnityEngine;
using System.Collections;

public class AudioCtrl : MonoBehaviour {
	public const int SE_KICK 	= 0;
	public const int SE_HAT 	= 1;

	public AudioSource[] _audioSourceArray;
	public AudioClip[] _seArray;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < _audioSourceArray.Length; i++) {
			_audioSourceArray [i].clip = _seArray [i];
		}
	}

	public void PlaySE(int pSeNumber) {
		_audioSourceArray[pSeNumber].Play ();
	}
}
