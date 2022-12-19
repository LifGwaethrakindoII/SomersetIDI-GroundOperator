using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeChecker : MonoBehaviour
{
	[SerializeField] private float projection; 	/// <summary>Normal's Projection.</summary>
	[SerializeField] private Vector3 _up; 		/// <summary>Up's Vector.</summary>
	private Vector3 _forward; 					/// <summary>Forward's Vector.</summary>
	private Vector3 _normal; 					/// <summary>Normal Vector resultant of the summatory of right, up and forward.</summary>
	private RaycastHit hitInfo; 				/// <summary>Raycast Hit's Information.</summary>
	private bool _isGrounded; 					/// <summary>Is this Transform on ground?.</summary>

	/// <summary>Gets and Sets up property.</summary>
	public Vector3 up
	{
		get { return _up; }
		set { _up = value; }
	}

	/// <summary>Gets and Sets forward property.</summary>
	public Vector3 forward
	{
		get { return _forward; }
		set { _forward = value; }
	}

	/// <summary>Gets and Sets normal property.</summary>
	public Vector3 normal
	{
		get { return _normal; }
		set { _normal = value; }
	}

	/// <summary>Gets and Sets isGrounded property.</summary>
	public bool isGrounded
	{
		get { return _isGrounded; }
		set { _isGrounded = value; }
	}

	private void OnDrawGizmos()
	{
		if(!Application.isPlaying) CalculateNormals();
		//transform.DrawNormals(normal, projection);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, forward * projection);
	}

	private void Update()
	{
		CalculateNormals();
	}

	public void UpdateHitInformation(RaycastHit _hit)
	{
		hitInfo = _hit;
	}

	private void CalculateNormals()
	{
		forward = isGrounded ? Vector3.Cross(transform.right, up) : transform.forward;
		normal = (transform.right + up + forward).normalized;
	}
}
