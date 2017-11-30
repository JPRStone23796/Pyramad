using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SCR_MenuManager : MonoBehaviour {



    public Button PlayButton_Menu1, QuitButton_Menu1, Players3_Menu2, Players5_Menu2;
    public string PlayerInputControl,PlayerAnalogControl,PlayerBackControl,GameLevel;
    public int currentButton, currentMenu;
    private double AnalogHeldDown;
    public Animator anim, anim2, playersIcon, playerNoSelect;
    private AudioSource MenuAudioSource;
    public AudioClip AnalogMovementSound, InputButtonSound;
	
    
    // Use this for initialization
	void Start () {
        currentButton = 1;
        currentMenu = 1;
        AnalogHeldDown = 0;
        MenuAudioSource = GetComponent<AudioSource>();

        Players3_Menu2.gameObject.SetActive(false);
        Players5_Menu2.gameObject.SetActive(false);


        anim.SetBool("Slide", true);
        anim2.SetBool("Slide", true);



    }

    // Update is called once per frame
    void Update()
    {

        switch (currentButton)
        {
            case 1: PlayButton_Menu1.gameObject.GetComponentInChildren<Text>().color = Color.green; QuitButton_Menu1.gameObject.GetComponentInChildren<Text>().color = Color.white; break;
            case 2: QuitButton_Menu1.gameObject.GetComponentInChildren<Text>().color = Color.green; PlayButton_Menu1.gameObject.GetComponentInChildren<Text>().color = Color.white; break;
            case 3: Players3_Menu2.gameObject.GetComponentInChildren<Text>().color = Color.green; Players5_Menu2.gameObject.GetComponentInChildren<Text>().color = Color.white; break;
            case 4: Players5_Menu2.gameObject.GetComponentInChildren<Text>().color = Color.green; Players3_Menu2.gameObject.GetComponentInChildren<Text>().color = Color.white; break;

        }



        if (AnalogHeldDown > 0)
        {
            AnalogHeldDown -= Time.deltaTime;
        }

        if (Input.GetAxis(PlayerAnalogControl) > 0.1 && AnalogHeldDown <= 0)
        {
            if (currentMenu == 1 && currentButton < 2) { currentButton++; }
            if (currentMenu == 2 && currentButton < 4) { currentButton++; }

            if(!MenuAudioSource.isPlaying || MenuAudioSource.clip != AnalogMovementSound)
            {
                MenuAudioSource.PlayOneShot(AnalogMovementSound);
            }
            AnalogHeldDown = 0.2;
        }



        if (Input.GetAxis(PlayerAnalogControl) < -0.1 && AnalogHeldDown <= 0)
        {
            if (currentMenu == 1 && currentButton > 1) { currentButton--; }
            if (currentMenu == 2 && currentButton > 3) { currentButton--; }
            AnalogHeldDown = 0.2;
            if (!MenuAudioSource.isPlaying || MenuAudioSource.clip != AnalogMovementSound)
            {
                MenuAudioSource.PlayOneShot(AnalogMovementSound);
            }
        }



        if (Input.GetButtonDown(PlayerInputControl))
        {


            switch (currentButton)
            {
                case 1: PlayButton(); break;
                case 2: QuitButton(); break;
                case 3: LoadLevelEnvironment(3); break;
                case 4: LoadLevelEnvironment(5); break;

            }


            if (!MenuAudioSource.isPlaying || MenuAudioSource.clip != InputButtonSound)
            {
                MenuAudioSource.PlayOneShot(InputButtonSound);
            }
        }



        if (Input.GetButtonDown(PlayerBackControl) &&currentMenu==2)
        {
            
            Players3_Menu2.gameObject.SetActive(false);
            Players5_Menu2.gameObject.SetActive(false);
            PlayButton_Menu1.gameObject.SetActive(true);
            QuitButton_Menu1.gameObject.SetActive(true);
            currentMenu--;
            currentButton = 1;
             if (!MenuAudioSource.isPlaying || MenuAudioSource.clip != InputButtonSound)
            {
                MenuAudioSource.PlayOneShot(InputButtonSound);
            }
            anim.SetBool("Slide", true);
            anim2.SetBool("Slide", true);

            playersIcon.SetBool("Slide", false);
            playerNoSelect.SetBool("Slide", false);



        }
    }


    void PlayButton()
    {
        anim.SetBool("Slide", false);
        anim2.SetBool("Slide", false);


        Players3_Menu2.gameObject.SetActive(true);
        Players5_Menu2.gameObject.SetActive(true);



        playersIcon.SetBool("Slide", true);
        playerNoSelect.SetBool("Slide", true); 
        currentMenu++;
        currentButton = 3;
    }


    void QuitButton()
    {
        Application.Quit();
    }


    void LoadLevelEnvironment(int AmountOfPlayers)
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<SCR_GameManager>().StartMatchWithNumPlayers(AmountOfPlayers);
            
    }
}
