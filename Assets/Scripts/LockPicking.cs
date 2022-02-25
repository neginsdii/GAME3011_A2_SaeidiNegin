using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LockPicking : MonoBehaviour
{
	[Header ("Pin properties")]
    [SerializeField]
    private GameObject pin;
	[SerializeField]
	private float PinTurnSpeed;
	private Vector3 mousePosition;
	[SerializeField]
	private float PinAngle = 0.0f;
	[SerializeField]
	private GameObject pinTip;

	[Header("Screw Driver properties")]
	[SerializeField]
	private GameObject ScrewDriver;
	[SerializeField]
	private float SDTurnSpeed;
	[SerializeField]
	private float SDAngle = 0.0f;

	[Header("Lock properties")]
	[SerializeField]
	private float DesiredAngle;
	[SerializeField]
	private GameObject lockSpot;
	[SerializeField]
	private GameObject LockObject;
	[SerializeField]
	private float minDesiredAngle;
	[SerializeField]
	private float maxDesiredAngle;
	[SerializeField]
	private float sweetSpotRotation;
	public TextMeshProUGUI text;
	[Header("Sound properties")]
	public AudioSource audioSource;
	public AudioClip[] audioClips;
	private void Start()
	{
		GenerateLockSpot();
		this.gameObject.GetComponent<Outline>().enabled = false;
		audioSource = GetComponent<AudioSource>();
	}
	private void Update()
	{
		if (!LockState.lsLockBroken && !LockState.isUnlocked && !LockState.isTimeOver)
		{
			RotatePin();
			RotateScrewDriver();
		}
		CheckUpdate();
	}

	private void RotatePin()
	{
		
		Vector3 mousePosition = Input.mousePosition;
		Vector3 pinPosition = Camera.main.WorldToScreenPoint(pin.transform.position);
		
		if (Input.GetMouseButton(0) )
		{
			if (mousePosition.x - 5 < pinPosition.x)
			{
				PinAngle += PinTurnSpeed * Time.deltaTime;
			
			}
			else if (mousePosition.x + 5 > pinPosition.x)
			{
				PinAngle -= PinTurnSpeed * Time.deltaTime;
			}
			PinAngle = Mathf.Clamp(PinAngle, minDesiredAngle, maxDesiredAngle);
			pin.transform.rotation = Quaternion.Euler(0, 0, (PinAngle));

			if(!audioSource.isPlaying)
			audioSource.PlayOneShot(audioClips[0], 0.1f);

			LockState.PinIsMoved = true;

		}
	}

	private void RotateScrewDriver()
	{

		if (LockState.isSpotFound )
		{
			Vector3 mousePosition = Input.mousePosition;
			Vector3 pinTipPosition = Camera.main.WorldToScreenPoint(pinTip.transform.position);
			float inp = Input.GetAxis("Horizontal");
			SDAngle -= inp * SDTurnSpeed * Time.deltaTime;
			SDAngle = Mathf.Clamp(SDAngle, -90, 90);

			ScrewDriver.transform.rotation = Quaternion.Euler(0, 0, (ScrewDriver.transform.rotation.z + SDAngle));
			LockObject.transform.rotation = Quaternion.Euler(0, LockObject.transform.rotation.y, (LockObject.transform.rotation.z + SDAngle));
			
			if (inp != 0)
			{
				LockState.isLockMoved = true;
				minDesiredAngle = -180;
				maxDesiredAngle = 180;
			}
			if (SDAngle == sweetSpotRotation)
			{
				Debug.Log("unlocked");
				LockState.isUnlocked = true;
				LockState.isSpotFound = true;
				audioSource.PlayOneShot(audioClips[0], 0.6f);
				Time.timeScale = 0;
			}
			
		}
		
	}

	private void GenerateLockSpot()
	{
		float angle = Random.Range(-90 , 90);
		if (angle > 0)
		{
			angle = Random.Range(30, 80);
			sweetSpotRotation = 90;
		}
		if (angle < 0)
		{
			angle = Random.Range(-80, -30);
			sweetSpotRotation = -90;
		}
		Debug.Log(angle);
		lockSpot.transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	private void CheckUpdate()
	{
		if (LockState.isTimeOver)
			text.SetText("Time is over!");
		if (LockState.isUnlocked)
			text.SetText("The lock is open!");
		if (LockState.lsLockBroken)
			text.SetText("The lock is broken!");
	}
}
