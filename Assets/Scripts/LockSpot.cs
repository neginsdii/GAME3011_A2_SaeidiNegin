using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSpot : MonoBehaviour
{
	public float time;
	public float maxtime;
	public GameObject lockObject;
	public AudioSource audioSource;
	public AudioClip[] audioClips;
	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}
	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.CompareTag("Pin"))
		{
			Debug.Log("Colliding with pin");
			time += Time.deltaTime;
			if(time>=maxtime)
			{
				if (!LockState.isSpotFound)
				{
					audioSource.PlayOneShot(audioClips[1], 0.4f);

				}
				LockState.isSpotFound = true;
				lockObject.GetComponent<Outline>().enabled = true;
			}
		}

	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Pin"))
		{
			Debug.Log("exit collision with pin");
		
			lockObject.GetComponent<Outline>().enabled = false;
			time = 0;
			if(LockState.isLockMoved)
			{
				LockState.lsLockBroken = true;
				Time.timeScale = 0;
				//audioSource.Play();
				audioSource.PlayOneShot(audioClips[0], 0.6f);

			}
		}
	}
}
