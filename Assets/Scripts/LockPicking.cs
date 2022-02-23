using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


	[Header("Pin properties")]
	[SerializeField]
	private GameObject ScrewDriver;
	[SerializeField]
	private float SDTurnSpeed;
	[SerializeField]
	private float SDAngle = 0.0f;
	private void Update()
	{
		
			RotatePin();
		RotateScrewDriver();
	}

	private void RotatePin()
	{
		if (Input.GetMouseButton(0))
		{
			PinAngle += Input.GetAxis("Mouse X") * PinTurnSpeed * Time.deltaTime;
			pin.transform.rotation = Quaternion.Euler(Mathf.Clamp(pin.transform.rotation.x + PinAngle,-90,90), 0, 0);
		}

	}

	private void RotateScrewDriver()
	{
		if(!Input.GetMouseButton(0))
		{
			SDAngle += Input.GetAxis("Horizontal") * SDTurnSpeed * Time.deltaTime;
			ScrewDriver.transform.rotation = Quaternion.Euler((ScrewDriver.transform.rotation.x + SDAngle), 0, 0);

		}
	}
}
