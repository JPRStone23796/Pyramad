using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EndZone : MonoBehaviour {


    public int CurrentPlayersInZone;


    public int ConsumePlayersInZoneForScore()
    {
        int TempPlayerCount = CurrentPlayersInZone;
        CurrentPlayersInZone = 0;
        return TempPlayerCount;

    }
	// Use this for initialization
	void Start () {
        CurrentPlayersInZone = 0;
	}

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Minion")
        {
            CurrentPlayersInZone++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Minion")
        {
            CurrentPlayersInZone--;
        }
    }
}
