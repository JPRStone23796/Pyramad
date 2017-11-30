using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Fans : MonoBehaviour
{


    private float WindStrength;
    // Use this for initialization
    void Start()
    {
        WindStrength = 550;
     

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Minion")
        {

            Rigidbody MinionRigidBody = other.gameObject.GetComponent<Rigidbody>();
            MinionRigidBody.AddForce(Vector3.up * WindStrength);
           
        }
    }


}
