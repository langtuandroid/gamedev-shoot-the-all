using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Sounds
{
	public class SAAudioManager : MonoBehaviour
	{
		public static SAAudioManager instance;
		[SerializeField] private AudioMixerGroup mixerGroup;
		[SerializeField] private SASound[] sounds;

		void Awake()
		{
			if (instance != null) Destroy(gameObject);
			else
			{
				instance = this;
				DontDestroyOnLoad(gameObject);
			}

			foreach (SASound s in sounds)
			{
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;
				s.source.loop = s.loop;
				s.source.outputAudioMixerGroup = mixerGroup;
			}
		}

		public void Play(string sound)
		{
			if (PlayerPrefs.GetInt("Audio" , 0) == 1) return;
			SASound s = Array.Find(sounds, item => item.name == sound);
			if (s == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}

			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

			s.source.Play();
		}
		public void Pause(string sound)
		{
			SASound s = Array.Find(sounds, item => item.name == sound);
			if (s == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}

			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

			s.source.Pause();
		}

	}
}
