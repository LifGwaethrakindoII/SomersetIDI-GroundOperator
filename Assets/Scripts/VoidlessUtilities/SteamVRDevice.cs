using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace VoidlessUtilities.VR
{
[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SteamVRDevice : MonoBehaviour
{
	private SteamVR_TrackedObject _trackedObject; 	/// <summary>SteamVR_TrackedObject's component.</summary>
	private SteamVR_Controller.Device _device; 		/// <summary>TrackedObject's Device.</summary>

	/// <summary>Gets and Sets trackedObject Component.</summary>
	public SteamVR_TrackedObject trackedObject
	{ 
		get
		{
			if(_trackedObject == null)
			{
				_trackedObject = GetComponent<SteamVR_TrackedObject>();
			}
			return _trackedObject;
		}
	}

	/// <summary>Gets and Sets device Component.</summary>
	public SteamVR_Controller.Device device
	{ 
		get
		{
			//if(_device == null)
			{
				_device = SteamVR_Controller.Input((int)trackedObject.index);
			}
			return _device;
		}
	}

	/// <summary>Updates Device's Index and its device reference.</summary>
	public void UpdateDevice(SteamVR_TrackedObject.EIndex _index)
	{
		trackedObject.index = _index;
		_device = SteamVR_Controller.Input((int)trackedObject.index);
	}

	//public static implicit operator SteamVR_Controller.Device(SteamVRDevice _device) { return _device.device; }
}
}