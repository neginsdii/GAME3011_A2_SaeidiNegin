using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChestScript : MonoBehaviour
{
    Outline outline;
	public Mode mode;

    void Start()
    {
        outline = GetComponent<Outline>();
		outline.enabled = false;
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			outline.enabled = true;
			StartCoroutine(LoadLockPickingScnene());
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			outline.enabled = false;
	
		}
	}

	IEnumerator LoadLockPickingScnene()
	{
		yield return new WaitForSeconds(3.0f);
		outline.enabled = false;
		LockState.GameMode = (int)mode;
		SceneManager.LoadScene("LockPickingScene"+(int)mode);
	}
}

public enum Mode
	{
		Novice,
		Casual,
		Master

	}