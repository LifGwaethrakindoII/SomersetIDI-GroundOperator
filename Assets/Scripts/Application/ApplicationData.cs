using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

namespace Supercargo
{
public delegate void OnDataUpdated(ApplicationData _data);

[CreateAssetMenu(menuName = ASSET_PATH_ROOT + "Data")]
[XmlRoot(Namespace="Supercargo", IsNullable = true)]
public class ApplicationData : ScriptableObject
{
	public const string RESOURCES_DATA_ROOT = "Assets/Resources/Data/"; 	/// <summary>Resources' Path Root.</summary>
	public const string ASSET_PATH_ROOT = "Application/"; 					/// <summary>Application's Scriptable Object Path's Root.</summary>

	public static event OnDataUpdated onDataUpdated;

	//[SerializeField] private TextAsset _file; 							/// <summary>File to serialize the data to.</summary>
	/*[SerializeField][XmlArray("PatternCommands")]
	public PatternCommand[] _commands; 										/// <summary>Application's Commands.</summary>*/
	[SerializeField] private GameObject _A380; 	/// <summary>A380's Airplane Prefab.</summary>
	[SerializeField] private GameObject _A320; 	/// <summary>A320's Airplane.</summary>
	[SerializeField] private GameObject _Boeing787; 	/// <summary>Boeing 787's Airplane.</summary>
	[SerializeField][XmlElement("AirplaneStartingPoint")]
	public TransformData _startingPoint;
	[SerializeField][XmlArray("PatternClassifications")]
	public PatternClassification[] _classifications;
	[HideInInspector] public string _dataName;
#if UNITY_EDITOR
	[HideInInspector] public Color _color;
	[HideInInspector] public int _index;
	[HideInInspector] public bool _toggle;

	/// <summary>Gets and Sets color property.</summary>
	public Color color
	{
		get { return _color; }
		set { _color = value; }
	}

	/// <summary>Gets and Sets index property.</summary>
	public int index
	{
		get { return _index; }
		set { _index = value; }
	}

	/// <summary>Gets and Sets toggle property.</summary>
	public bool toggle
	{
		get { return _toggle; }
		set { _toggle = value; }
	}
#endif

	/// <summary>Gets and Sets file property.</summary>
	/*public TextAsset file
	{
		get { return _file; }
		set { _file = value; }
	}*/

	/// <summary>Gets and Sets commands property.</summary>
	//[SerializeField][XmlArray("Pattern_Commands"), XmlArrayItem("Pattern_Command")]
	/*public PatternCommand[] commands
	{
		get { return _commands; }
		private set { _commands = value; }
	}*/

	/// <summary>Gets and Sets startingPoint property.</summary>
	public TransformData startingPoint
	{
		get { return _startingPoint; }
		set { _startingPoint = value; }
	}

	/// <summary>Gets and Sets classifications property.</summary>
	public PatternClassification[] classifications
	{
		get { return _classifications; }
		set { _classifications = value; }
	}

	/// <summary>Gets A380 property.</summary>
	public GameObject A380 { get { return _A380; } }

	/// <summary>Gets A320 property.</summary>
	public GameObject A320 { get { return _A320; } }

	/// <summary>Gets Boeing787 property.</summary>
	public GameObject Boeing787 { get { return _Boeing787; } }

	/// <summary>Gets and Sets dataName property.</summary>
	public string dataName
	{
		get { return _dataName; }
		set { _dataName = value; }
	}

	/// <summary>Serializes ScriptableObject's Data into an XML File.</summary>
	/// <param name="_name">Name of the Serialized XML file.</param>
	public void SerializeData(string _name)
	{
		StringBuilder builder = new StringBuilder();

		builder.Append(RESOURCES_DATA_ROOT);
		builder.Append(_name);
		builder.Append(Extensions.EXTENSION_SUFIX_XML);

		this.Serialize(builder.ToString());
	}

	/// <summary>deserializes XML's Data into this ScriptableObject.</summary>
	/// <param name="_name">Name of the XML to deserialize.</param>
	public void DeserializeData(string _name)
	{
		StringBuilder builder = new StringBuilder();

		builder.Append(RESOURCES_DATA_ROOT);
		builder.Append(_name);
		builder.Append(Extensions.EXTENSION_SUFIX_XML);

		ApplicationData data = Extensions.Deserialize<ApplicationData>(builder.ToString());
		if(data != null)
		{
			//commands = data.commands;
			startingPoint = data.startingPoint;
			classifications = data.classifications;
		}
	}

	public void UpdateData()
	{
		if(onDataUpdated != null) onDataUpdated(this);
	}
}
}