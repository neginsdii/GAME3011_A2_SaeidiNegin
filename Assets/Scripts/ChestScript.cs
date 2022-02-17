using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    Outline outline;
    void Start()
    {
        outline = GetComponent<Outline>();
		outline.enabled = false;
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Key"))
		{
			outline.enabled = true;
		
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Key"))
		{
			outline.enabled = false;
	
		}
	}
}
