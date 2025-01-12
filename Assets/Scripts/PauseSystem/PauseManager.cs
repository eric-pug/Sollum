/*
    Script Added by Aurora Russell
	10/08/2023
	// PAUSE MANAGER ENABLES PAUSE UI, UPDATES LOCATION INFO //
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{

    // SCRIPT TO BE PLACED ON EMPTY GAME OBJECT CALLED "PauseManager"

    // ALLOWS DEVELOPER TO SELECT KEY FROM LIST
    [Header("Pause Key")]
    public KeyCode PauseKey = KeyCode.Escape;

    [Header("UI Elements")]
    [Tooltip("Pause UI")]
    public GameObject PauseUI;
    [Tooltip("Image of Location")]
    public Image locationSprite;
    [Tooltip("Name of Location")]
    public Text locationName;
    [Tooltip("Current Objective Text")]
    public Text currentObjective;

    // SYSTEMS TO BE FROZEN WHILE DIALOGUE IS OPEN
    [HideInInspector]
    public playerMovement playerMovement;
    //[HideInInspector]
    //public WhipManager whipManager;
    //[HideInInspector]
    //public EnemyAttack enemyAttack;

    // THIS IS WHERE THE START SCENE NAME NEEDS TO BE ENTERED
    [Header("Scene Change")]
    [Tooltip("Main Menu")]
    public string levelToLoad;

    // HOLDS LIST OF LOCATIONS NAMES AND SPRITES
    [Header("Location Library")]
    [Tooltip("Invisible/Placeholder sprite for when no location info")]
    public Sprite invisSprite;
    public LocationLibrary locationLibrary;
    [HideInInspector]
    public List<string> locationNames;
    public GameObject hideOtherUI;

    // BOOLEANS
    private bool pauseActive = false;
    private bool isTyping = false;
    private bool cancelTyping = false;
    [HideInInspector]
    public bool dialogueOpen = false;
    [HideInInspector]
    public bool gamePaused = false;

    // STORES DIALOGUE
    private Queue<string> inputStream = new Queue<string>();

    void Start()
    {
        // ACCESS PLAYER MOVEMENT, WHIP ATTACK, ENEMY ATTACK FOR FREEZE MOVEMENT
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<playerMovement>();
        //whipManager = GameObject.Find("WhipManager").GetComponent<WhipManager>();
        //enemyAttack = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAttack>();

        // UPDATES LIST OF LOCATIONS FROM LOCATION LIBRARY
        foreach (LocationLibrary.SpriteInfo info in locationLibrary.locationSpriteList)
        {
            locationNames.Add(info.name);
        }
        locationSprite.sprite = invisSprite;
    }
    void Update()
    {
        // ENABLES PAUSE UI
        if (Input.GetKeyDown(PauseKey) && dialogueOpen == false && pauseActive == false)
        {
            Debug.Log("PAUSE KEY PRESSED");
            ActivatePause();
        }
        // DISABLES PAUSE UI
        else if (Input.GetKeyDown(PauseKey) && pauseActive == true)
        {
            ContinueButton();
        }
    }

    // HAULT PLAYER MOVEMENT, WHIP ATTACK, ENEMY ATTACKS
    private void FreezePlayer()
    {
        gamePaused = true;
        playerMovement.freezeMovement = true;
        //whipManager.pauseOpen = true;
        //enemyAttack.freezeAttack = true;
    }
    // RESTORE PLAYER MOVEMENT, WHIP ATTACK, ENEMY ATTACKS
    private void UnFreezePlayer()
    {
        gamePaused = false;
        playerMovement.freezeMovement = false;
        //whipManager.pauseOpen = false;
        //enemyAttack.freezeAttack = false;
    }

    public void NewLocation(Queue<string> objective)
    {
        // CLEARS THE LOCATION
        locationSprite.sprite = invisSprite;
        // STORES LOCATION FROM LOCATION TRIGGER
        inputStream = objective;
        // PRINTS FIRST LINE OF OBJECTIVE
        PrintObjective();
    }
    public void AdvanceText()
    {
        PrintObjective();
    }

    private void PrintObjective()
    {
        if (!isTyping)
        {
            if (inputStream.Peek().Contains("EndQueue"))
            {
                // ENDS OBJECTIVE
                if (!isTyping)
                {
                    inputStream.Dequeue();
                    EndObjective();
                    Debug.Log("END OBJECTIVE");
                }
                else
                {
                    cancelTyping = true;
                }
            }
            // READS LOCATION NAME
            else if (inputStream.Peek().Contains("[LOCATION="))
            {
                // SETS SPEAKER NAME TO NAME UI
                string name = inputStream.Peek();
                name = inputStream.Dequeue().Substring(name.IndexOf('=') + 1, name.IndexOf(']') - (name.IndexOf('=') + 1));
                locationName.text = name;
                PrintObjective(); // print the rest of this line
            }
            // READS LOCATION SPRITE
            else if (inputStream.Peek().Contains("[SPRITE="))
            {
                string part = inputStream.Peek();
                string spriteName = inputStream.Dequeue().Substring(part.IndexOf('=') + 1, part.IndexOf(']') - (part.IndexOf('=') + 1));
                if (spriteName != "")
                {
                    // SETS SPEAKER SPRITE TO THE SPRITE UI
                    locationSprite.sprite = locationLibrary.locationSpriteList[locationNames.IndexOf(spriteName)].sprite;
                }
                else
                {
                    // RETURNS SPEAKER SPRITE TO INVISIBLE SPRITE
                    locationSprite.sprite = invisSprite;
                }
                // PRINTS THE REST OF OBJECTIVE
                PrintObjective();
            }
            else
            {
                // PRINTS OBJECTIVE
                currentObjective.text = inputStream.Dequeue();
            }
        }
    }

    // ENDS TYPING
    public void EndObjective()
    {
        currentObjective.text = "";
        locationName.text = "";
        inputStream.Clear();
        cancelTyping = false;
        isTyping = false;
        inputStream.Clear();
    }

    // PAUSE GAME
    public void ActivatePause()
    {
        Debug.Log("PAUSE GAME");
        FreezePlayer();
        if (hideOtherUI != null)
            hideOtherUI.SetActive(false);
        PauseUI.SetActive(true);
        pauseActive = true;
    }
    // UNPAUSE GAME
    public void ContinueButton()
    {
        Debug.Log("UNPAUSE GAME");
        if (hideOtherUI != null)
            hideOtherUI.SetActive(true);
        PauseUI.SetActive(false);
        UnFreezePlayer();
        pauseActive = false;
    }

    public void SaveButton()
    {
        // Access save system
        GameManager.instance.SaveGame(); // SAVE GAME
    }
    // SCENE CHANGE TO START MENU
    public void MainMenuButton()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
