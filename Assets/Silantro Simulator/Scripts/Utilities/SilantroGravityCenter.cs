//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class SilantroGravityCenter : MonoBehaviour {
	//Get Parent Rigidbody
	 GameObject Parent = null;
	// Use this for initialization
	void Start () {
		Parent = gameObject.transform.root.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//Update center of mass.
		Rigidbody rigidBody = Parent.GetComponent<Rigidbody>();
		if ( null != rigidBody )
		{
			rigidBody.centerOfMass = gameObject.transform.localPosition;
		}

		//Debug draw.
		Debug.DrawLine( gameObject.transform.position - ( gameObject.transform.up * 1.0f ), gameObject.transform.position + ( gameObject.transform.up * 1.0f ), Color.cyan );
		Debug.DrawLine( gameObject.transform.position - ( gameObject.transform.right * 1.0f ), gameObject.transform.position + ( gameObject.transform.right * 1.0f ), Color.cyan );

	}

	//
	//Draw phere at cOG point
	public void OnDrawGizmos() 
	{
		//Draw sphere at cg position.
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere (transform.position, 0.1f);
	}
}
