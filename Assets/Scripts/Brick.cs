using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {
	
	public Sprite[] hitSprites;
	public static int breakableCount = 0;
	public AudioClip crack;
	public GameObject smoke;

	private LevelManager levelManager;
	private int timesHit;
	private bool isBreakable;

	// Use this for initialization
	void Start () {
		isBreakable = (this.tag == "Breakable");
		//Keep track breakable bricks
		if(isBreakable) {
			breakableCount++;
		}

		levelManager = GameObject.FindObjectOfType<LevelManager> ();
		timesHit = 0;
	}

	void OnCollisionExit2D (Collision2D collision) {
		AudioSource.PlayClipAtPoint (crack, transform.position, 1f);
		if (isBreakable) {
			HandleHits ();
		}
	}

	void HandleHits () {
		timesHit++;
		int maxHits = hitSprites.Length + 1;
		if (timesHit >= maxHits) {
			breakableCount--;
			levelManager.BrickDestroyed ();
			PuffSmoke ();
			Destroy (gameObject);
		} else {
			LoadSprites ();
		}		
	}

	void PuffSmoke () {
		var smokeMain = smoke.GetComponentInChildren<ParticleSystem> ().main;
		smokeMain.startColor = gameObject.GetComponent<SpriteRenderer> ().color;
		Instantiate (smoke, transform.position, Quaternion.identity);		
	}

	void LoadSprites () {
		int spriteIndex = timesHit - 1;
		if (hitSprites [spriteIndex] != null) {
			this.GetComponent<SpriteRenderer> ().sprite = hitSprites [spriteIndex];
		}else
			Debug.LogError("Brick sprite missing!");
	}
}
