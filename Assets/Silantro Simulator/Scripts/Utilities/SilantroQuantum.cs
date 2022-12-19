using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[ExecuteInEditMode]
public class SilantroQuantum : MonoBehaviour {
	#if UNITY_EDITOR
	[Header("Parent")]
	public Transform Instance;
	private Transform localInstance;
	// Use this for initialization
	void Start () {
		localInstance = this.gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Instance != null && localInstance.root.gameObject == Instance.root.gameObject) {
			if (this == Instance) {
				return;
			}
			//
			transform.localScale = new Vector3( -Instance.transform.localScale.x, Instance.transform.localScale.y, Instance.transform.localScale.z );
			transform.localPosition = Instance.transform.localPosition;
			transform.localPosition = new Vector3( -transform.localPosition.x, transform.localPosition.y, transform.localPosition.z );

			transform.localRotation = new Quaternion( -Instance.transform.localRotation.x,
				Instance.transform.localRotation.y,
				Instance.transform.localRotation.z,
				Instance.transform.localRotation.w * -1.0f);
			
		}
	}
	#endif
}
