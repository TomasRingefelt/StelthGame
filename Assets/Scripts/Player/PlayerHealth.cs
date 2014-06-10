using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
		public float playerHealth = 100f;
		public float resetAfterDeathTime = 5f;
		public AudioClip deathClip; 

		private Animator anim;
		private PlayerMovement playerMovement;
		private HashIDs hash;
		private SceneFadeInAndOut sceneFadeInAndOut;
		private LastPlayerSighting lastPlayerSighting;
		private float timer;
		private bool playerDead = false;


		void Awake ()
		{
				hash = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<HashIDs> ();
				if ((anim = GetComponent<Animator> ()) == null) {
						Debug.LogWarning ("Warning! Animator component not found", this);
				}
				if ((playerMovement = GetComponent<PlayerMovement> ()) == null) {
						Debug.LogWarning ("playerMovement can't be found in gameController! playerMovement component is null!", this);
				}
				
				if ((sceneFadeInAndOut = GameObject.FindGameObjectWithTag (Tags.fader).GetComponent<SceneFadeInAndOut> ()) == null) {
						Debug.LogWarning ("Warning! SceneFader component not found", this);
				}
				if ((lastPlayerSighting = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<LastPlayerSighting> ()) == null) {
						Debug.LogWarning ("Warning! LastPlayerSighting component not found", this);
				}
		}

		void Update ()
		{
				if (playerHealth <= 0f) {
						if (!playerDead) {
								PlayerDying ();
						} else {
								PlayerDead ();
								LevelReset ();
						}
				}
		}


		void PlayerDying ()
		{
				playerDead = true;
				anim.SetBool (hash.deadBool, true);
				AudioSource.PlayClipAtPoint (deathClip, transform.position);
		}

		void PlayerDead ()
		{
				if (anim.GetCurrentAnimatorStateInfo (0).nameHash == hash.dyingState) {
						anim.SetBool (hash.deadBool, false);
				}
				
				anim.SetFloat (hash.speedFloat, 0f);
				playerMovement.enabled = false;
				lastPlayerSighting.position = lastPlayerSighting.resetPosition;
				audio.Stop ();
		}

		void LevelReset ()
		{
				timer += Time.deltaTime;
				if (timer >= resetAfterDeathTime) {
						sceneFadeInAndOut.EndScene ();
				} 
		}

		public void TakeDamage (float amout)
		{
				if (amout >= 0f) {
						playerHealth -= amout;
				}
		}
}
