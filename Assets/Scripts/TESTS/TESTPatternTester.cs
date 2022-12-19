using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Supercargo;
using System.Text;

public class TESTPatternTester : MonoBehaviour
{
	[SerializeField] private User user; 		/// <summary>User.</summary>
	[SerializeField] private Text feedback; 	/// <summary>Description.</summary>

	void Update()
	{
		if(user != null)
		{
			if(user.leftHand.paddle != null && user.rightHand.paddle != null)
			{
				StringBuilder builder = new StringBuilder();

				builder.Append("Right Controller Orientation: ");
				builder.AppendLine(GetOrientation(user.torax, user.rightHand.transform.position, new Vector3(0.3f, 0.3f, 0.3f)).ToString());
				builder.Append("Left Controller Orientation: ");
				builder.AppendLine(GetOrientation(user.torax, user.leftHand.transform.position, new Vector3(0.3f, 0.3f, 0.3f)).ToString());
				builder.Append("Right Paddle Orientation");
				builder.AppendLine(GetOrientation(user.rightHand.transform, user.rightHand.GetRelativePaddlePoint()).ToString());
				builder.Append("Left Paddle Orientation");
				builder.AppendLine(GetOrientation(user.leftHand.transform, user.leftHand.GetRelativePaddlePoint()).ToString());

				feedback.text = builder.ToString();
			}
			else feedback.text = "User needs to pick both paddles";

			transform.LookAt(user.transform);
		}
	}

	private OrientationSemantics GetOrientation(Transform _centralJoint, Vector3 _childJointPosition, Vector3? _distanceThreshold = null)
	{
		OrientationSemantics orientation = OrientationSemantics.Middle;
		Vector3 relativePoint = _centralJoint.InverseTransformPoint(_childJointPosition);
		if(!_distanceThreshold.HasValue) _distanceThreshold = new Vector3(0.0f, 0.0f);

		orientation |= relativePoint.x > _distanceThreshold.Value.x ? OrientationSemantics.Right : relativePoint.x < -_distanceThreshold.Value.x ? OrientationSemantics.Left : OrientationSemantics.Middle;
		orientation |= relativePoint.y > _distanceThreshold.Value.y ? OrientationSemantics.Up : relativePoint.y < -_distanceThreshold.Value.y ? OrientationSemantics.Down : OrientationSemantics.Middle;
		orientation |= relativePoint.z > _distanceThreshold.Value.z ? OrientationSemantics. Forward : relativePoint.z < -_distanceThreshold.Value.z ? OrientationSemantics.Backward : OrientationSemantics.Middle;

		return orientation;
	}
}