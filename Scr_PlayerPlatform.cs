using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerPlatform : MonoBehaviour {


    
    private Vector3 PlatformsOrigin, PlatformsDestination, PlayerToPlatformDistance;
    public bool PlayerOnButton, TravelingToDestination;
    private float PlatformSpeed, MovementStartTime, PlatformDistanceToTravel;
    public GameObject PlayerOnPlatform,PlatformsHighPoint;
    

    // Use this for initialization
    void Start()
    {
        PlatformsDestination = PlatformsHighPoint.transform.position;
        PlatformsOrigin = gameObject.transform.position;
        PlayerOnButton = false;
        TravelingToDestination = false;
        PlatformSpeed = (float)3.0f;
        PlayerOnPlatform = null;
    }

    // Update is called once per frame
    void Update()
    {

       

        if (PlayerOnButton == true)
        {
          
            if (transform.position != PlatformsDestination)
            {
              
                if (TravelingToDestination == false)
                {
                   
                    MovementStartTime = Time.time;
                    PlatformDistanceToTravel = Vector3.Distance(PlatformsOrigin, PlatformsDestination);
                    TravelingToDestination = true;
                    if (PlayerOnPlatform != null) { Debug.Log("4"); PlayerToPlatformDistance = PlayerOnPlatform.transform.position - transform.position; }
                }



                var distCovered = (Time.time - MovementStartTime) * PlatformSpeed * Time.deltaTime;
                var fracJourney = distCovered / PlatformDistanceToTravel;





                if (PlayerOnPlatform != null)
                {
                   
                    Vector3 playerDistance = (PlatformsDestination + PlayerToPlatformDistance);
                    Vector3 PlayerDestination = new Vector3(PlayerOnPlatform.transform.position.x, playerDistance.y, PlayerOnPlatform.transform.position.z);
                    PlayerOnPlatform.transform.position = Vector3.Lerp(PlayerOnPlatform.transform.position, PlayerDestination, fracJourney);
                }
              transform.position = Vector3.Lerp(transform.position, PlatformsDestination, fracJourney);


            }

            if (transform.position == PlatformsDestination)
            {
                TravelingToDestination = false;
            }

        }





        if (PlayerOnButton == false)
        {

            if (transform.position != PlatformsOrigin)
            {
                if (TravelingToDestination == false)
                {
                    MovementStartTime = Time.time;
                    PlatformDistanceToTravel = Vector3.Distance(PlatformsDestination, PlatformsOrigin);
                    TravelingToDestination = true;


                    if (PlayerOnPlatform != null)
                    {
                        PlayerToPlatformDistance = PlayerOnPlatform.transform.position - transform.position;
                    }
                }
                var distCovered = (Time.time - MovementStartTime) * PlatformSpeed * Time.deltaTime;
                var fracJourney = distCovered / PlatformDistanceToTravel;







                if (PlayerOnPlatform != null)
                {
                    Vector3 playerDistance = (PlatformsOrigin + PlayerToPlatformDistance);
                    Vector3 PlayerOrigin = new Vector3(PlayerOnPlatform.transform.position.x, playerDistance.y, PlayerOnPlatform.transform.position.z);
                    PlayerOnPlatform.transform.position = Vector3.Lerp(PlayerOnPlatform.transform.position, PlayerOrigin, fracJourney * Time.deltaTime);
                }



                transform.position = Vector3.Lerp(transform.position, PlatformsOrigin, fracJourney);


            }

            if (transform.position == PlatformsOrigin)
            {
                TravelingToDestination = false;
            }
        }
    }

     void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Minion" && (PlayerOnPlatform==null ))
        {
            PlayerOnPlatform = collision.gameObject;
            if ((PlayerOnPlatform.transform.position.y - transform.position.y) > 2)
            {

                PlayerOnPlatform = null;
            }
            
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Minion")
        {
            PlayerOnPlatform = null;
        }
    }

}
