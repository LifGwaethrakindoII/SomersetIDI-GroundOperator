using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml.Serialization;

namespace Supercargo
{
public static class Extensions
{
	public const string EXTENSION_SUFIX_XML = ".xml";

#region GizmosExtensions:
	public static void DrawNormals(this Transform _transform, float _projection = 1.0f)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(_transform.position, (_transform.right * _projection));
		Gizmos.color = Color.green;
		Gizmos.DrawRay(_transform.position, (_transform.up * _projection));
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(_transform.position, (_transform.forward * _projection));
	}

	public static void DrawNormals(this Transform _transform, Vector3 _normal, float _projection = 1.0f)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(_transform.position, (_normal * _projection));
		Gizmos.color = Color.green;
		Gizmos.DrawRay(_transform.position, (_normal * _projection));
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(_transform.position, (_normal * _projection));
	}

	public static void DrawNormals(this Transform _transform, Vector3 _right, Vector3 _up, Vector3 _forward, float _projection = 1.0f)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(_transform.position, (_right * _projection));
		Gizmos.color = Color.green;
		Gizmos.DrawRay(_transform.position, (_up * _projection));
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(_transform.position, (_forward * _projection));
	}
#endregion

	/// <summary>Calculates a component-wise Vector division.</summary>
	public static Vector3 Division(Vector3 a, Vector3 b)
	{
		return new Vector3
		(
			(a.x / b.x),
			(a.y / b.y),
			(a.z / b.z)
		);
	}

	public static void DispatchCoroutine(this MonoBehaviour _monoBehaviour, ref Coroutine _coroutine)
	{
		if(_coroutine != null)
		{
			_monoBehaviour.StopCoroutine(_coroutine);
			_coroutine = null;
		}
	}

	public static Coroutine StartCoroutine(this MonoBehaviour _monoBehaviour, IEnumerator _enumerator, ref Coroutine _coroutine)
	{
		if(_coroutine != null)
		{
			_monoBehaviour.StopCoroutine(_coroutine);
			_coroutine = null;
		}
		return _coroutine = _monoBehaviour.StartCoroutine(_enumerator);
	}

	public static void ChangeState<T>(this IFiniteStateMachine<T> _fsm, T _state)
	{
		_fsm.OnExit(_fsm.state);
		_fsm.previousState = _fsm.state;
		_fsm.state = _state;
		_fsm.OnEnter(_fsm.state);
	}

	public static int Clamp(int x, int min, int max)
	{
		return x < min ? min : x > max ? max : x; 
	}

	/*public static IEnumerator (this Transform _transform, Transform _other, Func<Transform, Vector3> positionFunction = null, Func<Transform, Quaternion> rotationFunction = null, Action on = null)
	{
		float t = 0.0f;

	}*/

	public static Vector3 GetOrientation(OrientationSemantics _orientation)
	{
		Vector3 orientation = Vector3.zero;

		if(_orientation.HasFlag(OrientationSemantics.Right)) orientation.x += 1.0f;
		if(_orientation.HasFlag(OrientationSemantics.Left)) orientation.x -= 1.0f;
		if(_orientation.HasFlag(OrientationSemantics.Up)) orientation.y += 1.0f;
		if(_orientation.HasFlag(OrientationSemantics.Down)) orientation.y -= 1.0f;
		if(_orientation.HasFlag(OrientationSemantics.Forward)) orientation.z += 1.0f;
		if(_orientation.HasFlag(OrientationSemantics.Backward)) orientation.z -= 1.0f;

		return orientation;
	}

#region OrientationSemanticsEnumOperations:
	/// <summary>Checks if OrientationSemantics enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the OrientationSemantics enumerator contains flag.</returns>
	public static bool HasFlag(this OrientationSemantics _enum, OrientationSemantics _flag){ return ((_enum & _flag) == _flag); }

	/// <summary>Checks if OrientationSemantics enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the OrientationSemantics enumerator contains all flags.</returns>
	public static bool HasFlags(this OrientationSemantics _enum, params OrientationSemantics[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}

		return true;
	}

	/// <summary>Adds Flag [if there is not] to OrientationSemantics enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the OrientationSemantics enumerator.</param>
	public static void AddFlag(ref OrientationSemantics _enum, OrientationSemantics _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }

	/// <summary>Adds Flags [if there are not] to OrientationSemantics enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the OrientationSemantics enumerator.</param>
	public static void AddFlags(ref OrientationSemantics _enum, params OrientationSemantics[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }

	/// <summary>Removes flag from OrientationSemantics enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from OrientationSemantics enumerator.</param>
	public static void RemoveFlag(ref OrientationSemantics _enum, OrientationSemantics _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }

	/// <summary>Removes flags from OrientationSemantics enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from OrientationSemantics enumerator.</param>
	public static void RemoveFlags(ref OrientationSemantics _enum, params OrientationSemantics[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }

	/// <summary>Toggles flag from OrientationSemantics enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from OrientationSemantics enumerator.</param>
	public static void ToggleFlag(ref OrientationSemantics _enum, OrientationSemantics _flag){ _enum ^= _flag; }

	/// <summary>Toggles flags from OrientationSemantics enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from OrientationSemantics enumerator.</param>
	public static void ToggleFlags(ref OrientationSemantics _enum, params OrientationSemantics[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }

	/// <summary>Removes all OrientationSemantics enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref OrientationSemantics _enum){ _enum = (OrientationSemantics)0; }
#endregion

	public static Vector3 GetOrientationDirection(this Transform _transform, OrientationSemantics _orientation)
	{
		Vector3 orientationDirection = Vector3.zero;

		if(_orientation.HasFlag(OrientationSemantics.Left)) orientationDirection += Vector3.left;
		if(_orientation.HasFlag(OrientationSemantics.Right)) orientationDirection += Vector3.right;
		if(_orientation.HasFlag(OrientationSemantics.Down)) orientationDirection += Vector3.down;
		if(_orientation.HasFlag(OrientationSemantics.Up)) orientationDirection += Vector3.up;

		return _transform.TransformDirection(orientationDirection);
	}

#region XMLSerialization:
	/// <summary>Serializes Object.</summary>
	/// <param name="_object">Object to Serialize.</param>
	/// <param name="_path">Path to serialize the Object as Xml.</param>
	public static void Serialize<T>(this T _object, string _path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(T), _path);

		using(StreamWriter writer = new StreamWriter(_path))
		{
			try { serializer.Serialize(writer.BaseStream, _object); }
			catch(Exception e) { Debug.LogError("Error while trying to save " + typeof(T).Name + ": " + e.Message); }
			finally { writer.Close(); }
		}
			
	}

#if UNITY_EDITOR
	public static void SerializeIntoTextAsset<T>(this T _object, TextAsset _file)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(T));

		using(StreamWriter writer = new StreamWriter(UnityEditor.AssetDatabase.GetAssetPath(_file)))
		{
			try { serializer.Serialize(writer.BaseStream, _object); }
			catch(Exception e) { Debug.LogError("Error while trying to save " + typeof(T).Name + ": " + e.Message); }
			finally { writer.Close(); }
		}
	}
#endif

	/// <summary>Deserializes object on given path as T.</summary>
	/// <param name="_path">Path to deserialize the object from.</param>
	/// <param name="_root">Optional Root. Null by default.</param>
	/// <returns>Deserialized Object [as T].</returns>
	public static T Deserialize<T>(string _path, string _root = null)
	{
		XmlSerializer serializer = string.IsNullOrEmpty(_root) ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), new XmlRootAttribute(_root));
		T _object = default(T);

		using(StreamReader reader = new StreamReader(_path))
		{
			try { _object = (T)serializer.Deserialize(reader.BaseStream); }
			catch(Exception e) { Debug.LogError("Error while trying to load " + typeof(T).Name + ": " + e.Message); }
			finally { reader.Close(); }
		}

		return _object;
	}

	public static T DeserializeTextAsset<T>(TextAsset _file, string _root = null)
	{
		XmlSerializer serializer = string.IsNullOrEmpty(_root) ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), new XmlRootAttribute(_root));
		T _object = default(T);

		using(StreamReader reader = new StreamReader(_file.ToString()))
		{
			try { _object = (T)serializer.Deserialize(reader); }
			catch(Exception e) { Debug.LogError("Error while trying to load " + typeof(T).Name + ": " + e.Message); }
			finally { reader.Close(); }
		}

		return _object;
	}
#endregion

}
}