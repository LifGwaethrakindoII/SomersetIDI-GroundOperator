using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APUAirplane : MonoBehaviour {

    public AudioSource APU_Start_A380;
    public AudioClip APU_START;
    public AudioClip APU_LOOP;
    public float volumeAPU;

    //public float durationAPU;


    public void ActiveAPU()
    {
        StarAPU();
        StartCoroutine(LoopAPU());
    }

    public void StarAPU()
    {
        APU_Start_A380.PlayOneShot(APU_START, volumeAPU);
    }

    IEnumerator LoopAPU()
    {
        while (APU_Start_A380.isPlaying)
        {
            yield return null;
        }
        APU_Start_A380.clip = APU_LOOP;
        APU_Start_A380.loop = true;
        
    }

}
