using UnityEngine;
using System.Collections;

public class AudioCtrl : MonoBehaviour {
	public const int SE_KICK 	= 0;
	public const int SE_HAT_BAD 	= 1;
	public const int SE_HAT_GOOD	= 2;
	public const int SE_HAT_GREAT 	= 3;
	public const int SE_HAT_EXCELLENT = 4;
	public const int SE_SNARE = 5;
	public const int SE_LV_UP = 6;


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
