using System.Collections;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Supercargo
{
[System.Flags]
public enum OrientationSemantics 																					/// <summary>Orientation Semantics.</summary>
{
	None = 0, 																										/// <summary>None Orientation.</summary>
	Middle = 1, 																									/// <summary>Middle Orientation.</summary>
	Left = 2, 																										/// <summary>Left Orientation.</summary>
	Right = 4, 																										/// <summary>Right Orientation.</summary>
	Up = 8, 																										/// <summary>Up Orientation.</summary>
	Down = 16, 																										/// <summary>Down Orientation.</summary>
	Forward = 32, 																									/// <summary>Forward Orientation.</summary>
	Backward = 64, 																									/// <summary>Backward Orientation.</summary>

	LeftAndUp = Left | Up, 																							/// <summary>Left and Up Orientation.</summary>
	LeftAndDown = Left | Down, 																						/// <summary>Left and Down Orientation.</summary>
	RightAndUp = Right | Up, 																						/// <summary>right and Up Orientation.</summary>
	RightAndDown = Right | Down, 																					/// <summary>right and Down Orientation.</summary>
	LeftAndUpAndForward = LeftAndUp | Forward, 																		/// <summary>Left and Up and Forward Orientation.</summary>
	LeftAndUpAndBackward = LeftAndUp | Backward, 																	/// <summary>Left and Up and Backward.</summary>
	LeftAndDownAndForward = LeftAndDown | Forward, 																	/// <summary>Left and Down and Forward Orientation.</summary>
	LeftAndDownAndBackward = LeftAndDown | Backward, 																/// <summary>Left and Down and Backward.</summary>
	RightAndUpAndForward = RightAndUp | Forward, 																	/// <summary>Right and Up and Forward Orientation.</summary>
	RightAndUpAndBackward = RightAndUp | Backward, 																	/// <summary>Right and Up and Backward.</summary>
	RightAndDownAndForward = RightAndDown | Forward, 																/// <summary>Right and Down and Forward Orientation.</summary>
	RightAndDownAndBackward = RightAndDown | Backward 																/// <summary>Right and Down and Backward.</summary>
}

[System.Serializable]
[XmlRoot(Namespace="Supercargo", IsNullable = false)]
public struct PatternData
{
	[SerializeField][XmlElement("Head_Orientation")] public OrientationSemantics headOrientation; 					/// <summary>Head's Orientation.</summary>
	[SerializeField][XmlElement("Left_Hand_Orientation")] public OrientationSemantics leftHandOrientation; 			/// <summary>LeftHand's Orientation.</summary>
	[SerializeField][XmlElement("Left_Paddle_Orientation")] public OrientationSemantics leftPaddleOrientation; 		/// <summary>Left Paddle's Orientation.</summary>
	[SerializeField][XmlElement("Right_Hand_Orientation")] public OrientationSemantics rightHandOrientation; 		/// <summary>Right Hand's Orientation.</summary>
	[SerializeField][XmlElement("Right_Paddle_Orientation")] public OrientationSemantics rightPaddleOrientation; 	/// <summary>Right Paddle's Orientation.</summary>

	/// <summary>Equals operator.</summary>
	public static bool operator == (PatternData a, PatternData b)
	{
		return (a.headOrientation.HasFlag(b.headOrientation)
		&& a.leftHandOrientation.HasFlag(b.leftHandOrientation)
		&& a.leftPaddleOrientation.HasFlag(b.leftPaddleOrientation)
		&& a.rightHandOrientation.HasFlag(b.rightHandOrientation)
		&& a.rightPaddleOrientation.HasFlag(b.rightPaddleOrientation));
	}

	/// <summary>Not Equal operator.</summary>
	public static bool operator != (PatternData a, PatternData b)
	{
		return (!a.headOrientation.HasFlag(b.headOrientation)
		|| !a.leftHandOrientation.HasFlag(b.leftHandOrientation)
		|| !a.leftPaddleOrientation.HasFlag(b.leftPaddleOrientation)
		|| !a.rightHandOrientation.HasFlag(b.rightHandOrientation)
		|| !a.rightPaddleOrientation.HasFlag(b.rightPaddleOrientation));
	}

	public PatternData(OrientationSemantics _headOrientation, OrientationSemantics _leftHandOrientation, OrientationSemantics _leftPaddleOrientation, OrientationSemantics _rightHandOrientation, OrientationSemantics _rightPaddleOrientation)
	{
		this.headOrientation = _headOrientation;
		this.leftHandOrientation = _leftHandOrientation;
		this.leftPaddleOrientation = _leftPaddleOrientation;
		this.rightHandOrientation = _rightHandOrientation;
		this.rightPaddleOrientation = _rightPaddleOrientation;
	}

	/// <summary>Compares PatterData against other PatternData.</summary>
	/// <param name="_data">PatternData to compare.</param>
	/// <returns>True if this PatternData's values has the same bit flags than the PatterData to compare. False Otherwise.</returns>
	public bool Equals(PatternData _data)
	{
		return (headOrientation.HasFlag(_data.headOrientation)
		&& leftHandOrientation.HasFlag(_data.leftHandOrientation)
		&& leftPaddleOrientation.HasFlag(_data.leftPaddleOrientation)
		&& rightHandOrientation.HasFlag(_data.rightHandOrientation)
		&& rightPaddleOrientation.HasFlag(_data.rightPaddleOrientation));
	}

	/// <returns>String representing all Orientation semantics of the User's joints.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Head Orientation: ");
		builder.AppendLine(headOrientation.ToString());
		builder.Append("Left Hand Orientation: ");
		builder.AppendLine(leftHandOrientation.ToString());
		builder.Append("Left Paddle Orientation: ");
		builder.AppendLine(leftPaddleOrientation.ToString());
		builder.Append("Right Hand Orientation: ");
		builder.AppendLine(rightHandOrientation.ToString());
		builder.Append("Right Paddle Orientation: ");
		builder.AppendLine(rightPaddleOrientation.ToString());

		return builder.ToString();
	}
}
}