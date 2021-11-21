//----------------------------------------------------------------
// Small treasure hunt game for Christmas 2021.
// Copyright (c) Josefine Klintberg.
//----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>Class <c>TreasureHuntLogic</c> describes the game logic of the clue-game.
/// </summary>
public class TreasureHuntLogic : MonoBehaviour
{
    /// <value>Property <c>errorString</c> represents the string telling the user that
    /// Christmas is not here yet or that the entered passcode was incorrect.</value>
    public GameObject errorString;

    /// <value>Property <c>startString</c> represents the start/continue text.</value>
    public GameObject startString;

    /// <value>Property <c>background1</c> represents the fartest background.</value>
    public GameObject background1;

    /// <value>Property <c>background1</c> represents the closest background.</value>
    public GameObject background2;

    /// <value>Property <c>background1</c> represents the animated decorations.</value>
    public GameObject ornaments;

    /// <value>Property <c>background1</c> represents the title and clue text.</value>
    public GameObject title;

    /// <value>Property <c>background1</c> represents the pressable button.</value>
    public GameObject button;

    /// <value>Property <c>background1</c> represents the inputfield for passcodes.</value>
    public GameObject passCode;

    // Internal members, stores the player progress and the clues with passcodes.
    private bool executeStartup = false;
    private int currLevel = 0;

    // TODO : The real passCodes and clues should be read from file.
    private string[ ] passCodes = new string[]{
        "1", 
        "2", 
        "3", 
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "10",
        "11",
        "12",
        "13",
        "14",
        "15",
        "16",
        "17",
        "18",
        "19",
        "20",
        "21",
        "22",
        "23",
        "---",
    };

    private string[ ] clues = new string[]{
        "1 clue goes like this...", 
        "2 clue goes like this...",
        "3 clue goes like this...", 
        "4 clue goes like this...",
        "5 clue goes like this...", 
        "6 clue goes like this...",
        "7 clue goes like this...", 
        "8 clue goes like this...",
        "9 clue goes like this...", 
        "10 clue goes like this...",
        "11 clue goes like this...", 
        "12 clue goes like this...",
        "13 clue goes like this...", 
        "14 clue goes like this...",
        "15 clue goes like this...", 
        "16 clue goes like this...",
        "17 clue goes like this...", 
        "18 clue goes like this...",
        "19 clue goes like this...", 
        "20 clue goes like this...", 
        "21 clue goes like this...", 
        "22 clue goes like this...",
        "23 clue goes like this...", 
        "24 clue goes like this...", 
    }; 


    // Execute startup procedure and check for previous
    // player progress such that one is not needed to
    // re-enter previously correctly entered passcodes.
    public void Awake()
    {
        if(!executeStartup){
            Debug.Log("Checking for progress...");
            int level = PlayerPrefs.GetInt("level", 0);
            currLevel = level;

            // Debug, test to start on a different level.
            // currLevel = 2;
            if(currLevel != 0){
                setTextTM(ref startString, "CONTINUE");
            }else{
                Debug.Log("No valid progress found...");
                setTextTM(ref startString, "START");
            }
            executeStartup = true;
        }
    }

    /// <summary>
    /// Starts the treasure hunt by animating over to the "clue-view"
    /// while disabling no longer needed objects and enabling the
    /// ones for starting guessing for clues. Only possible between
    /// the 24th and 31th of December.
    /// </summary>
    /// <returns>None.</returns>
    public void startTreasureHunt() {
        int month = int.Parse(System.DateTime.Now.ToString("MM"));
        int day = int.Parse(System.DateTime.Now.ToString("dd"));

        // Debug, test to start the game on valid date.
        // month = 12;
        // day = 24;

        if(month != 12 && day < 24) {
            errorString.SetActive(true);
            setTextTM(ref errorString, "BEGINS ON CHRISTMAS EVE...");
        }else{
            ornaments.SetActive(false);
            title.SetActive(false);
            button.SetActive(false);
            Animation anim = background1.GetComponent<Animation>();
            anim.Play("backgroundSlide");
            Animation anim2 = background2.GetComponent<Animation>();
            anim2.Play("backgroundSlide");
            
            StartCoroutine(waitForAnimationThenStart());
        }
    }

    /// <summary>
    /// Waits for the animation to the "clue view" to be finished, then
    /// sets the current clue text in the title text and enables
    /// the passcode input field and moves the error string to be
    /// set next to the passcode input field.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator waitForAnimationThenStart() {
        yield return new WaitForSeconds(3);

        setCurrentClue();

        errorString.transform.position -= new Vector3(0, 3.5f, 0);
    }

    /// <summary>
    /// Sets the current clue text in the title text and enables
    /// the passcode input field.
    /// </summary>
    /// <returns>None.</returns>
    public void setCurrentClue() {
        title.SetActive(true);
        setTextTM(ref title, clues[currLevel]);
        title.GetComponent<Animation>().enabled = false;
        passCode.SetActive(true);
    }

    /// <summary>
    /// Checks the passcode entered by a user, will update clues if correct, otherwise display error msg.
    /// </summary>
    /// <param name="passcode">Passcode entered by user.</param>
    /// <returns>None.</returns>
    public void enterPasscode(string passcode) {
        if(currLevel <= passCodes.Length && currLevel <= clues.Length && passcode == passCodes[currLevel]){
            errorString.SetActive(false);

            // Update level and set the next clue.
            currLevel += 1;

            // Final clue, will lead to gift!
            if(currLevel == 23){
                passCode.SetActive(false);
            }else{
                setCurrentClue();
                passCode.GetComponent<TMP_InputField>().text = "Passcode...";   // Reset input field.
            }
        }else{
            errorString.SetActive(true);
            setTextTM(ref errorString, "Incorrect passcode");
        }
    }

    /// <summary>
    /// Sets the text on a TextMeshPro object.
    /// </summary>
    /// <param name="textObj">TextMeshPro object.</param>
    /// <param name="newText">Text to set.</param>
    /// <returns>None.</returns>
    public void setTextTM(ref GameObject textObj, string newText) {
        textObj.GetComponent<TMP_Text>().text = newText;
    }

    /// <summary>
    /// Gets the text on a TextMeshPro object.
    /// </summary>
    /// <param name="textObj">TextMeshPro object.</param>
    /// <returns>String: The text on textObj.</returns>
    public string getTextTM(ref GameObject textObj) {
        return textObj.GetComponent<TMP_Text>().text;
    }

    // Save player progress when leaving the app such that we
    // keep track of correctly entered passcodes.
    void OnApplicationQuit()
    {
        // Enable when testing is done, for now don't save player progress.
        // PlayerPrefs.SetInt("level", currLevel);
    }
}
