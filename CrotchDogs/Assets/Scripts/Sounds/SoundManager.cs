using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour 
{
		private const int NUM_OF_BITE_EFFECTS=2,NUM_OF_SWOOSH_EFFECTS=2;
		public static SoundManager Instance;
		public  AudioSource m_AudioSource;
		public  AudioSource m_AudioSourceMusic;
		public bool SFX_Mute=false, Mute=false;


		//AUDIO
		public List<AudioClip> BiteEffects;
		public List<AudioClip> SwooshEffects;


		//MUSIC
		public AudioClip TitleMusic;



		void Awake()
		{
			Instance = this;
		}

		private void PlaySound(AudioClip clip)
		{
				if(clip != null && !SFX_Mute && !Mute)
				{
						m_AudioSource.PlayOneShot(clip, 1.0f);	
				}
				else 
				{
						//Debug.Log("AudioManager: PlaySound() - * AudioClip == null *");	
				}
		}

		public static void PlayBite()
		{
				if (Instance != null)
				{
						int rand = Random.Range (0, Instance.BiteEffects.Count);
	
						if (Instance.BiteEffects[rand] != null) 
						{
								Instance.PlaySound (Instance.BiteEffects[rand]);
						} 
				}
		}

		public static void PlaySwoosh()
		{

			if (Instance != null)
			{
						int rand = Random.Range (0, Instance.SwooshEffects.Count);

						if (Instance.SwooshEffects[rand] != null) 
						{
								Instance.PlaySound (Instance.SwooshEffects[rand]);
						}
			}
		}

		public static void PlayMusic() 
		{
				if (Instance != null) {
						if (!Instance.Mute) {
								if (Instance.m_AudioSourceMusic.clip != Instance.TitleMusic) {
										Instance.m_AudioSourceMusic.clip = Instance.TitleMusic;
										Instance.m_AudioSourceMusic.Play ();
								}
						}

				}
		}

		public static void StopCurrentMusic() {
				if (Instance != null) 
				{
						Instance.m_AudioSourceMusic.Stop ();
						Instance.m_AudioSourceMusic.clip = null;
				}
		}
}
