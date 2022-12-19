using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supercargo;

public class BrakeAirplane : MonoBehaviour
{

    [SerializeField] private float _brakeForce;               /// <summary>Brake Force.</summary>
    [SerializeField] private float _volumeChangeDuration;     /// <summary>Volume Change's Duration.</summary>
    private Airplane _airplane;                               /// <summary>Airplane component.</summary>

    /// <summary>Gets brakeForce property.</summary>
    public float brakeForce { get { return _brakeForce; } }

    /// <summary>Gets volumeChangeDuration property.</summary>
    public float volumeChangeDuration { get { return _volumeChangeDuration; } }

    /// <summary>Gets and Sets airplane Component.</summary>
    public Airplane airplane
    { 
        get
        {
            if(_airplane == null)
            {
                _airplane = GetComponent<Airplane>();
            }
            return _airplane;
        }
    }
    
    public void StopAirplane()
    {
        airplane.frontLeftWheel.brakeTorque = brakeForce;
        airplane.frontRightWheel.brakeTorque = brakeForce;
        StartCoroutine(TurnOffEngines(0.0f, GetAllIndices()));
        airplane.APUAirplane.ActiveAPU();
        airplane.Reset();
        //airplane.frontWheelsSystem.transform.localEulerAngles = Vector3.zero;
    }

    public void ReleaseBrakes()
    {
        airplane.frontLeftWheel.brakeTorque = 0.0f;
        airplane.frontRightWheel.brakeTorque = 0.0f;
    }

    private int[] GetAllIndices()
    {
        int[] result = new int[airplane.soundEngines.Length];

        for(int i = 0; i < airplane.soundEngines.Length; i++)
        {
            result[i] = i;
        } 

        return result;
    }

    private IEnumerator TurnOffEngines(float DestinyVolumeEngine, params int[] _indices)
    {
        float n = 0.0f;
        float[] initialVolumes = new float[_indices.Length];

        foreach(int index in _indices)
        {
            initialVolumes[index] = airplane.soundEngines[index].volume;
        }

        while(n < 1.0f)
        {
            foreach (int index in _indices)
            {
                airplane.soundEngines[index].volume = Mathf.Lerp(initialVolumes[index], DestinyVolumeEngine, n);
            }

            n += (Time.deltaTime / volumeChangeDuration);
            yield return null;
        }

        foreach(int index in _indices)
        {
            airplane.soundEngines[index].volume = DestinyVolumeEngine;
        }
    }
}
