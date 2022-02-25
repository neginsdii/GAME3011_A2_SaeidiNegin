using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
	public TextMeshProUGUI playerSkill;
	[Header("Sound properties")]
	public AudioSource audioSource;
	public AudioClip[] audioClips;
	public GameObject backButton;
	private void Start()
	{
		GenerateLockSpot();
		this.gameObject.GetComponent<Outline>().enabled = false;
		audioSource = GetComponent<AudioSource>();
		backButton.GetComponent<Button>().onClick.AddListener(OnBackClicked);
		PlayerSkillLevel();
	}
	private void Update()
	{
		if (!LockState.lsLockBroken && !LockState.isUnlocked && !LockState.isTimeOver)
		{
			RotatePin();
			RotateScrewDriver();
		}
		if (LockState.lsLockBroken || LockState.isUnlocked || LockState.isTimeOver)
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
		backButton.SetActive(true);
		if (LockState.isTimeOver)
		{
			text.SetText("Time is over!");

		}
		if (LockState.isUnlocked)
		{
			text.SetText("The lock is open!");
		}
		if (LockState.lsLockBroken)
		{
			text.SetText("The lock is broken!");
		}
	}

	private void OnBackClicked()
	{
		if (LockState.isUnlocked)
		{
			ChangeSkill(LockState.GameMode);
		}
		ResetGame();
		SceneManager.LoadScene("Main");
	}

	private void ResetGame()
	{
		 LockState.isSpotFound = false;
		 LockState.isUnlocked = false;
		 LockState.lsLockBroken = false;
		 LockState.isLockMoved = false;
		 LockState.isTimeOver = false;
		 LockState.PinIsMoved = false;
		Time.timeScale = 1;
		text.SetText("");
	}

	private void PlayerSkillLevel()
	{
		if(LockState.GameMode == 0)
		{
			SetLevelText(LockState.NovicePinRotationSpeed);
			PinTurnSpeed = LockState.NovicePinRotationSpeed;
		}
		else if (LockState.GameMode == 1)
		{
			SetLevelText(LockState.CasualPinRotationSpeed);
			PinTurnSpeed = LockState.CasualPinRotationSpeed;
		}
		else if (LockState.GameMode == 2)
		{
			SetLevelText(LockState.MasterPinRotationSpeed);
			PinTurnSpeed = LockState.MasterPinRotationSpeed;
		}
	}
	private void SetLevelText(float skill)
	{
		switch (skill)
		{
			case 30:
				playerSkill.SetText("Player Skill: Basic");
				break;
			case 28:
				playerSkill.SetText("Player Skill: Intermediate");
				break;
			case 25:
				playerSkill.SetText("Player Skill: Advanced");
				break;

		}
	}
	private void ChangeSkill( int gameMode)
	{
		switch (gameMode)
		{
			case 0:

				if (LockState.NovicePinRotationSpeed >=28)
				LockState.NovicePinRotationSpeed -= 3;
				else
					LockState.NovicePinRotationSpeed =25;
				break;
			case 1:
				if (LockState.CasualPinRotationSpeed >= 28)
					LockState.CasualPinRotationSpeed -= 3;
				else
					LockState.CasualPinRotationSpeed = 25;
				break;
			case 2:
				if (LockState.MasterPinRotationSpeed >= 28)
					LockState.MasterPinRotationSpeed -= 3;
				else
					LockState.MasterPinRotationSpeed = 25;
				break;

		}
	}
}
