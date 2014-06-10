using UnityEngine;
using System.Collections;

public class LastPlayerSighting : MonoBehaviour
{
		public Vector3 position = new Vector3 (1000f, 1000f, 1000f);
		public Vector3 resetPosition = new Vector3 (1000f, 1000f, 1000f);
		public float lightHighIntensity = 0.25f;
		public float lightLowIntesity = 0f;

		public float fadeSpeed = 7f;
		public float musicFadeSpeed = 1f;

		private AlarmLight alarm;
		private Light mainLight;
		private AudioSource panicAudio;
		private AudioSource[] sirens;
		private bool playingAlarm = false;
		 

		void Awake ()
		{
				alarm = GameObject.FindGameObjectWithTag (Tags.alarm).GetComponent<AlarmLight> ();
				mainLight = GameObject.FindGameObjectWithTag (Tags.mainLight).light;
				panicAudio = transform.Find ("secondaryMusic").audio;
				GameObject[] sirenGameObjects = GameObject.FindGameObjectsWithTag (Tags.siren);
				sirens = new AudioSource[sirenGameObjects.Length];

				for (int i = 0; i < sirenGameObjects.Length; i++) {
						sirens [i] = sirenGameObjects [i].audio;
				}
		}

		void Update ()
		{
				SwithAlarms ();
				MusicFading ();
		}

		void SwithAlarms ()
		{
				alarm.alarmOn = (position != resetPosition);
				
				float newIntensity;

				if (alarm.alarmOn) {
						newIntensity = lightLowIntesity;
				} else {
						newIntensity = lightHighIntensity;
				}
				mainLight.intensity = Mathf.Lerp (mainLight.intensity, newIntensity, fadeSpeed * Time.deltaTime);

				if (alarm.alarmOn) {
						for (int i = 0; i < sirens.Length; i++) {
								if (!playingAlarm) {
										playingAlarm = true;
										sirens [i].Play ();
								}
						}
				} else {
						for (int i = 0; i < sirens.Length; i++) {
								if (playingAlarm) {
										playingAlarm = false;
										sirens [i].Stop ();
								}
						}
				}
		}

		void MusicFading ()
		{
				if (position != resetPosition) {
						audio.volume = Mathf.Lerp (audio.volume, 0f, musicFadeSpeed * Time.deltaTime);
						panicAudio.volume = Mathf.Lerp (panicAudio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);

				} else {
						audio.volume = Mathf.Lerp (audio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
						panicAudio.volume = Mathf.Lerp (panicAudio.volume, 0f, musicFadeSpeed * Time.deltaTime);

				}
		}
}
