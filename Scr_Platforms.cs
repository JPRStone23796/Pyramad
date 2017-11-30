using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Platforms : MonoBehaviour {

    public GameObject ThisPlatformsButton;
    public Scr_PlayerPlatform ThisPlatformsButtonScr;




    private Vector3 PlatformsOrigin, PlatformsDestination, PlayerToPlatformDistance;
    public bool PlayerOnButton, TravelingToDestination;
    private float PlatformSpeed, MovementStartTime, PlatformDistanceToTravel;
    public GameObject PlayerOnPlatform, PlatformsHighPoint;
    public AudioSource GameAudioSource;
    public AudioClip ButtonTriggeredSound,PlatformTriggeredSound;

    // Use this for initialization
    void Start()
    {
        PlatformsDestination = PlatformsHighPoint.transform.position;
        PlatformsOrigin = transform.position;
        PlayerOnButton = false;
        TravelingToDestination = false;
        PlatformSpeed = (float)4.0f;
        PlayerOnPlatform = null;
        ThisPlatformsButtonScr = ThisPlatformsButton.GetComponent<Scr_PlayerPlatform>();
        GameAudioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {



        if (PlayerOnButton == true)
        {

            if (transform.position != PlatformsDestination && (PlayerOnPlatform != null && (((PlayerOnPlatform.transform.position.y - transform.position.y) > 0) && (PlayerOnPlatform.transform.position.y - transform.position.y) < 1)))
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
                ThisPlatformsButtonScr.PlayerOnButton = true;
                if (!GameAudioSource.isPlaying)
                {
                    GameAudioSource.PlayOneShot(PlatformTriggeredSound);
                }

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Minion")
        {
            PlayerOnButton = true;
            PlayerOnPlatform = collision.gameObject;

            if (!GameAudioSource.isPlaying)
            {
                GameAudioSource.PlayOneShot(ButtonTriggeredSound);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Minion")
        {
            PlayerOnPlatform = null;
            PlayerOnButton = false;
            ThisPlatformsButtonScr.PlayerOnButton = false;
            ThisPlatformsButtonScr.TravelingToDestination = false;

        }
    }

}
