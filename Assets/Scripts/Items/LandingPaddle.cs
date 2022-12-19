using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Supercargo
{
public class LandingPaddle : Pickable
{
	[SerializeField] private Vector3 _tipOffset; 	/// <summary>Paddle's Tip Offset.</summary>
#if UNITY_EDITOR
	[SerializeField] private float jointRadius; 	/// <summary>Joint's Radius.</summary>
#endif

	/// <summary>Gets tipOffset property.</summary>
	public Vector3 tipOffset { get { return _tipOffset; } }

	protected override void DrawOrientationNormals()
	{
		base.DrawOrientationNormals();
#if UNITY_EDITOR
		Gizmos.DrawWireSphere(GetOffsetTipPosition(), jointRadius);
#endif
	}

	public Vector3 GetOffsetTipPosition()
	{
		return transform.TransformPoint(tipOffset);
	}

	/// <summary>Callback invoked when a drop request is made by a VRHand.</summary>
	/// <param name="_hand">VRHand requesting the drop.</param>
	/*public override void OnDropRequest(VRHand _hand)
	{
	}*/
}
}