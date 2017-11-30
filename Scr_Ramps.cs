using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Ramps : MonoBehaviour {
    




    private Vector3 PlatformsOrigin, PlatformsDestination, PlayerToPlatformDistance;
    public bool PlayerOnButton, TravelingToDestination;
    private float PlatformSpeed, MovementStartTime, PlatformDistanceToTravel;
    public float  TimerOutOfUse;
    public GameObject PlayerOnPlatform, PlatformsHighPoint;
    public AudioSource GameAudioSource;
    public AudioClip RampTriggeredNoise;
    // Use this for initialization
    void Start()
    {
        PlatformsDestination = PlatformsHighPoint.transform.position;
        PlatformsOrigin = transform.position;
        PlayerOnButton = false;
        TravelingToDestination = false;
        PlatformSpeed = (float)4.0f;
        PlayerOnPlatform = null;
        TimerOutOfUse = 0.0f;
        GameAudioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
       
    }

    // Update is called once per frame
    void Update()
    {

        if (TimerOutOfUse >= 0 && (transform.position == PlatformsDestination)) { TimerOutOfUse -= Time.deltaTime; }

        if (PlayerOnButton == true )
        {

            if (transform.position != PlatformsDestination)
            {
                if (TravelingToDestination == false)
                {
                    MovementStartTime = Time.time;
                    PlatformDistanceToTravel = Vector3.Distance(PlatformsOrigin, PlatformsDestination);
                    TravelingToDestination = true;
                    if (PlayerOnPlatform != null) { PlayerToPlatformDistance = PlayerOnPlatform.transform.position - transform.position; }
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
                PlayerOnButton = false;
                

            }

        }





        if (PlayerOnButton == false && TimerOutOfUse<0)
        {

            if (transform.position != PlatformsOrigin)
            {
                if (TravelingToDestination == false)
                {
                    MovementStartTime = Time.time;
                    PlatformDistanceToTravel = Vector3.Distance(PlatformsDestination, PlatformsOrigin);
                    TravelingToDestination = true;
                }
                var distCovered = (Time.time - MovementStartTime) * PlatformSpeed * Time.deltaTime;
                var fracJourney = distCovered / PlatformDistanceToTravel;

                transform.position = Vector3.Lerp(transform.position, PlatformsOrigin, fracJourney);
            }

            if (transform.position == PlatformsOrigin)
            {
                TravelingToDestination = false;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
       
        if (collision.gameObject.tag == "Minion" && TravelingToDestination == false && (transform.position != PlatformsDestination))
        {
           
            PlayerOnButton = true;
            PlayerOnPlatform = collision.gameObject;
            TimerOutOfUse = 2.0f;
            if(!GameAudioSource.isPlaying)
            {
             GameAudioSource.PlayOneShot(RampTriggeredNoise);
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

