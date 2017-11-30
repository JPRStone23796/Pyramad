using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_BallController : MonoBehaviour {


    //----------------------------------------------------
    // Component References
    //----------------------------------------------------

    Rigidbody ThisRigidBody;
    public ParticleSystem ThisParticleSystem;

    //----------------------------------------------------
    // External References
    //----------------------------------------------------

    public SCR_GroundPoundIcon GroundPoundIcon;

    //----------------------------------------------------
    // Player Assignment
    //----------------------------------------------------

   public enum EPlayerNumber
    {
        EPN_One,
        EPN_Two,
        EPN_Three,
        EPN_Four,
        EPN_Five
    };

    public EPlayerNumber ThisPlayerNumber = EPlayerNumber.EPN_One;

    public void AssignPlayerInput()
    {
        AssignPlayerInputHorizontal();
        AssignPlayerInputVertical();
        AssignPlayerInputJump();
    }




    //----------------------------------------------------
    // Ball Type Assignment
    //----------------------------------------------------


    //----------------------------------------------------
    // Input capture
    //----------------------------------------------------

    struct BallInput
    {
        public string Horizontal;
        public string Vertical;
        public string Jump;
    }

    BallInput ThisBallInput;
    
    void AssignPlayerInputHorizontal()
    {
        string BaseString = "L_XAxis_";

        ThisBallInput.Horizontal = BaseString + (int)(ThisPlayerNumber + 1);

    }

    void AssignPlayerInputVertical()
    {
        string BaseString = "L_YAxis_";

        ThisBallInput.Vertical = BaseString + (int)(ThisPlayerNumber + 1);
    }

    void AssignPlayerInputJump()
    {
        string BaseString = "A_";

        ThisBallInput.Jump = BaseString + (int)(ThisPlayerNumber + 1);
    }

    void GetJumpInput()
    {
        bool bJumpPressedThisFrame = Input.GetButtonDown(ThisBallInput.Jump);
        if (TimeLastGroundPounded > 0) { TimeLastGroundPounded -= Time.deltaTime; }
        if (bJumpPressedThisFrame && bJumpAvailable)
        {
            RequestJump();
        }
        if (bJumpPressedThisFrame && bJumpAvailable==false)
        {
            
           
            if (bGroundPound==false)
            {
                AttemptGroundPound();
            }
           
        }

        if (bGroundPound)
        {
            CarryOutGroundPound();
        }
    }

    void GetPhysicsInputs()
    {
        Vector2 InputAxisValues = new Vector2();
        float HorizontalAxisValue = Input.GetAxis(ThisBallInput.Horizontal);

        if (Mathf.Abs(HorizontalAxisValue) > 0.1f)
        {
            InputAxisValues.x = HorizontalAxisValue;

        }

        float VerticalAxisValue = Input.GetAxis(ThisBallInput.Vertical);

        if (Mathf.Abs(VerticalAxisValue) > 0.1f)
        {
            InputAxisValues.y = VerticalAxisValue;
        }

        ApplyMotionForce(InputAxisValues);
    }

    //----------------------------------------------------
    // Ball Motions
    //----------------------------------------------------

    public float BallMotionForce = 15.0f;
    private float TimeLastJumped = 0.0f;
    private float TimeLastGroundPounded = 0.0f;

    void ApplyMotionForce(Vector2 InputAxisValues)
    {

        Vector3 MotionForce = new Vector3();
        MotionForce +=  new Vector3(0,0,1) * BallMotionForce * -InputAxisValues.y;
        MotionForce += new Vector3(1, 0, 0)  * BallMotionForce * InputAxisValues.x;

        ThisRigidBody.AddForce(MotionForce, ForceMode.Acceleration);
    }

    bool bJumpAvailable = true;
    public float JumpImpulseForce = 40.0f;

    void RequestJump()
    {
        if (TimeLastJumped + 0.5f < Time.time) { bJumpAvailable = true; }
        if (bJumpAvailable)
        {
            bJumpAvailable = false;
            TimeLastJumped = Time.time;
            ThisRigidBody.AddForce(new Vector3(0,1,0) * JumpImpulseForce, ForceMode.Impulse);
        }
       
    }

    //----------------------------------------------------
    // Collision Management
    //----------------------------------------------------

    public float BossContactImpulseForce = 4000.0f;
    private float TimeLastShuntedByBoss = 9999.0f;

    public float GetTimeLastShuntedByBoss()
    {
        return TimeLastShuntedByBoss;
    }

    public void SetTimeLastShuntedByBoss(float CurrentTime)
    {
        TimeLastShuntedByBoss = CurrentTime;
    }


    void OnCollisionEnter(Collision collision)
    {

        if(collision.contacts[0].normal.y > 0.6) { bJumpAvailable = true; }

        if(this.gameObject.tag=="Boss")
        {
            if(collision.gameObject.tag=="Minion" )
            {
                SCR_BallController MinionBallController = collision.gameObject.GetComponent<SCR_BallController>();

                if(MinionBallController.GetTimeLastShuntedByBoss() < 0.2f) { return; }

                Rigidbody MinionsRigidBody = collision.gameObject.GetComponent<Rigidbody>();

                Vector3 ForceDirection = this.transform.localPosition - new Vector3(0, this.transform.position.y, 0);


                MinionsRigidBody.AddForce(ForceDirection * BossContactImpulseForce);

                MinionBallController.SetTimeLastShuntedByBoss(Time.time);
                if(!AudioManager.isPlaying)
                {
                    AudioManager.PlayOneShot(CollisionSoundEffect);
                }
            }
        }
    }

    //----------------------------------------------------
    // Ground Pound Mechanic
    //----------------------------------------------------

    private float DownwardImpulse = 1400.0f;
    private bool bGroundPound = false;

    void AttemptGroundPound()
    {
        if (this.gameObject.tag == "Boss" && TimeLastGroundPounded<=0)
        {
            Vector3 DownwardVector = Vector3.down;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, DownwardVector, out hit, 10000, 1))
            {
                var CurrentDistanceFromTheGround = Vector3.Distance(transform.position, hit.point);
                if (CurrentDistanceFromTheGround > 1.2)
                {
                    ThisRigidBody.AddForce(new Vector3(0, -1, 0) * DownwardImpulse, ForceMode.Impulse);
                    bGroundPound = true;
                    TimeLastGroundPounded = 5.0f;
                }
                
            }
        }
    }

    private float GroundPoundStrikeStrength = 55000.0f;
    void CarryOutGroundPound()
    {

        if(bGroundPound)
        {
         
            Vector3 DownwardVector = Vector3.down;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, DownwardVector, out hit, 10000, 1))
            {
               
                var CurrentDistanceFromTheGround = Vector3.Distance(transform.position, hit.point);
                if (CurrentDistanceFromTheGround < 0.7)
                {
                    var hitColliders = Physics.OverlapSphere(transform.position, 6.0f);

                    for (int i=0; i < hitColliders.Length;i++)
                    {

                        if (hitColliders[i].transform.tag == "Minion")
                        {
                            Vector3 CurrentDistanceFromMinion = -(transform.position - hitColliders[i].transform.position);
                            float DistanceFromStrikePoint = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                            Rigidbody MinionRigidBodyStruck = hitColliders[i].transform.gameObject.GetComponent<Rigidbody>();
                            float GroundPoundForceScalar = Mathf.Clamp((6.0f - DistanceFromStrikePoint)/6.0f,0,1);                        
                            MinionRigidBodyStruck.AddForce(GroundPoundForceScalar * GroundPoundStrikeStrength * CurrentDistanceFromMinion.normalized);
                        }
                    }    
                    bGroundPound = false;
                    if (!AudioManager.isPlaying)
                    {
                        AudioManager.PlayOneShot(GroundPoundSoundEffect);
                    }
                    ThisParticleSystem.Emit(1);
                    GroundPoundIcon.ConsumeGroundPound(5.0f);
                }
            }
        }
    }



    //----------------------------------------------------
    // Environmental Interaction
    //----------------------------------------------------

    public AudioSource AudioManager;
    public AudioClip CollisionSoundEffect;
    public AudioClip GroundPoundSoundEffect;


    //----------------------------------------------------
    // Menu Interaction
    //----------------------------------------------------



    //----------------------------------------------------
    // Mono develop interface
    //----------------------------------------------------


    // Use this for initialization
    void Awake()
    {
        ThisRigidBody = GetComponent<Rigidbody>();
        AudioManager = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {

        GetJumpInput();
       



    }

    void FixedUpdate()
    {
        GetPhysicsInputs();
    }
}
