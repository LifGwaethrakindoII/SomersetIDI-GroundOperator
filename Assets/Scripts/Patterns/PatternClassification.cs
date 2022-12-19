using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

namespace Supercargo
{
[System.Serializable]
[XmlRoot(Namespace = "Supercargo", IsNullable = true)]
public struct PatternClassification/* : IEnumerable<UserPatternWaypoints>*/
{
	[SerializeField][XmlElement("Command")]
	public Command command; 					/// <summary>Command Infered from the Pattern.</summary>
	[SerializeField][XmlArray("Waypoints"), XmlArrayItem("Waypoint")]
	public UserPatternWaypoints[] waypoints; 	/// <summary>Waypoints.</summary>

	/// <summary>Parameterless PatternClassification's constructor.</summary>
	//public PatternClassification() : this() { /*...*/ }

	/// <returns>Waypoints' Iterator.</returns>
	public IEnumerator<UserPatternWaypoints> GetEnumerator()
	{
		foreach(UserPatternWaypoints waypoint in waypoints)
		{
			yield return waypoint;
		}
	}

	/// <returns>Waypoints' Iterator as Object.</returns>
	//IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
}
}