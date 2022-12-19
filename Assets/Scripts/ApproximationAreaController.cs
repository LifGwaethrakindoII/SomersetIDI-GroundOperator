using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Supercargo
{
public delegate void OnChocksInstalled(bool _installed);

public class ApproximationAreaController : MonoBehaviour
{
	public static OnChocksInstalled onChocksInstalled;

	[SerializeField] private ApplicationData data; 	/// <summary>Description.</summary>
	[SerializeField] private Airplane _airplane; 				/// <summary>Debuggable Airplane.</summary>
	[SerializeField] private User user; 	/// <summary>User.</summary>
	[Space(5f)]
	[Header("Chocks Configurations:")]
	[SerializeField] private GameObject _chock; 				/// <summary>Chock's Prefab.</summary>
	[SerializeField] private float _minInstallationDuration; 	/// <summary>Minimum Installation's duration.</summary>
	[SerializeField] private float _maxInstallationDuration; 	/// <summary>Maximum Installation's duration.</summary>
	private Coroutine chockInstallation;

	/// <summary>Gets and Sets airplane property.</summary>
	public Airplane airplane
	{
		get { return _airplane; }
		set { _airplane = value; }
	}

	/// <summary>Gets chock property.</summary>
	public GameObject chock { get { return _chock; } }

	/// <summary>Gets minInstallationDuration property.</summary>
	public float minInstallationDuration { get { return _minInstallationDuration; } }

	/// <summary>Gets maxInstallationDuration property.</summary>
	public float maxInstallationDuration { get { return _maxInstallationDuration; } }

#if UNITY_EDITOR
	private void OnGUI()
	{
		if(airplane != null)
		{
			float defaultOffset = 20.0f;
			float width = 400.0f;
			float height = 30.0f;
			Rect positionRect = new Rect(defaultOffset, defaultOffset, width, height);
			GUI.Label(positionRect, "User on sight: " + airplane.pilotSight.targetAtSight.ToString());
			positionRect.y += height;
			GUI.Label(positionRect, "Airplane's current Command: " + airplane.airplaneDijkstraEngine.command.ToString());
		}
	}
#endif

	private void OnEnable()
	{
		LandingZone.onAirplaneLanded += OnAirplaneLanded;
	}

	private void OnDisable()
	{
		LandingZone.onAirplaneLanded -= OnAirplaneLanded;
	}

	private void Awake()
	{
		data.DeserializeData(data.dataName);

		switch(PlayerPrefs.GetString("Airplane"))
		{
			case "A380":
			airplane = Instantiate(data.A380.GetComponent<Airplane>()) as Airplane;
			break;

			case "A320":
			airplane = Instantiate(data.A320.GetComponent<Airplane>()) as Airplane;
			break;

			case "Boeing787":
			airplane = Instantiate(data.Boeing787.GetComponent<Airplane>()) as Airplane;
			break;
		}

		if(airplane != null) data.startingPoint.UpdateTransform(airplane.transform);
		airplane.pilotSight.target = user;
		airplane.gameObject.SetActive(false);
	}

	private void Start()
	{
		//InstallChocksTo(airplane);
		airplane.gameObject.SetActive(true);
	}

	private void InstallChocksTo(Airplane _airplane)
	{
		this.StartCoroutine(InstallChocks(_airplane), ref chockInstallation);
	}

	private IEnumerator InstallChocks(Airplane _airplane)
	{
		int count = _airplane.chocksPoints.Length;
		GameObject[] chocks = new GameObject[count];
		Vector3[] spawnPoints = _airplane.GetSpawnPoints(_airplane.transform.position);
		float[] durations = new float[count];
		float[] times = new float[count];

		for(int i = 0; i < count; i++)
		{
			chocks[i] = Instantiate(chock, spawnPoints[i], Quaternion.identity);
			durations[i] = Random.Range(minInstallationDuration, maxInstallationDuration);
		}

		while(AllTimesTicking(times) && _airplane.airplaneDijkstraEngine.command == Command.Stop)
		{
			for(int i = 0; i < count; i++)
			{
				float n = times[i];
				chocks[i].transform.position = Vector3.Slerp(spawnPoints[i], airplane.transform.TransformPoint(airplane.chocksPoints[i]), n);
				n += (Time.deltaTime / durations[i]);
				times[i] = n;
			}

			yield return null;
		}

		for(int i = 0; i < count; i++)
		{
			chocks[i].transform.position = airplane.transform.TransformPoint(airplane.chocksPoints[i]);	
		}

		if(onChocksInstalled != null) onChocksInstalled(_airplane.airplaneDijkstraEngine.command == Command.Stop);
	}

	private void OnAirplaneLanded(Airplane _airplane)
	{
		InstallChocksTo(_airplane);
	}

	private bool AllTimesTicking(float[] _times)
	{
		foreach(float time in _times)
		{
			if(time < 1.0f) return true;
		}

		return false;
	}
}
}