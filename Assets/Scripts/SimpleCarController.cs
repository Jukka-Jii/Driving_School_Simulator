using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleCarController : MonoBehaviour
{

	private float m_horizontalInput;
	private float m_verticalInput;
	private float m_steeringAngle;
	private Rigidbody rigidbody;
	public float KPH;

	public Text speedLimit;

	public WheelCollider frontDriverW, frontPassengerW;
	public WheelCollider rearDriverW, rearPassengerW;
	public Transform frontDriverT, frontPassengerT;
	public Transform rearDriverT, rearPassengerT;
	public float maxSteerAngle = 30;
	public float motorForce = 50;

	public bool isBrake = false;
	public float brakeTorque = 15000f;

	public void Start()
	{
		getObjects();
		speedLimit.text = "";
	}

	public void GetInput()
	{
		m_horizontalInput = Input.GetAxis("Horizontal");
		m_verticalInput = Input.GetAxis("Vertical");
	}

	private void Steer()
	{
		m_steeringAngle = maxSteerAngle * m_horizontalInput;
		frontDriverW.steerAngle = m_steeringAngle;
		frontPassengerW.steerAngle = m_steeringAngle;
	}

	private void Accelerate()
	{
		frontDriverW.motorTorque = m_verticalInput * motorForce;
		frontPassengerW.motorTorque = m_verticalInput * motorForce;
		KPH = rigidbody.velocity.magnitude * 3.6f;
	}

	void HandBrake ()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			isBrake = true;
		} else
		{
			isBrake = false;
		}
		if (isBrake == true)
		{
			rearDriverW.brakeTorque = brakeTorque;
			rearPassengerW.brakeTorque = brakeTorque;
			frontDriverW.brakeTorque = brakeTorque;
			frontPassengerW.brakeTorque = brakeTorque;
			rearDriverW.motorTorque = 0;
			rearPassengerW.motorTorque = 0;
			frontDriverW.motorTorque = 0;
			frontPassengerW.motorTorque = 0;
		} else
		{
			rearDriverW.brakeTorque = 0;
			rearPassengerW.brakeTorque = 0;
			frontDriverW.brakeTorque = 0;
			frontPassengerW.brakeTorque = 0;
		}
	}

	private void UpdateWheelPoses()
	{
		UpdateWheelPose(frontDriverW, frontDriverT);
		UpdateWheelPose(frontPassengerW, frontPassengerT);
		UpdateWheelPose(rearDriverW, rearDriverT);
		UpdateWheelPose(rearPassengerW, rearPassengerT);
	}

	private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
	{
		Vector3 _pos = _transform.position;
		Quaternion _quat = _transform.rotation;

		_collider.GetWorldPose(out _pos, out _quat);

		_transform.position = _pos;
		_transform.rotation = _quat;
	}

	private void FixedUpdate()
	{
		GetInput();
		Steer();
		Accelerate();
		UpdateWheelPoses();
		ExceedingSpeedLimit();
		HandBrake();
	}

	private void getObjects()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	void ExceedingSpeedLimit ()
	{
		if (KPH > 40)
		{
			speedLimit.text = "Speed Limit Exceeded!";
			Debug.Log(speedLimit);
		} else
		{
			speedLimit.text = "";
		}
	}
}
