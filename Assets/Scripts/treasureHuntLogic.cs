using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class treasureHuntLogic : MonoBehaviour
{
    public GameObject notYetString = null;
    private List<string> scenesInBuild;
    private bool executeStartup = false;
    private int currLvl = 0;
    private string[ ] passCodes = new string[]{"John", "Amanda", "Chris", "Amber"}; 

    // Make global
    public static treasureHuntLogic Instance {
        get;
        set;
    }

    public void Awake()
    {
        if(!executeStartup){
            scenesInBuild = new List<string>();
            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                int lastSlash = scenePath.LastIndexOf("/");
                scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
            }

            Debug.Log("Checking for progress...");
            string prevLvl = PlayerPrefs.GetString("level");
            string passLvl = PlayerPrefs.GetString("passLevel");
            if(prevLvl != "" && scenesInBuild.Contains(prevLvl)){
                loadScene(prevLvl);
                if(passLvl != ""){
                    currLvl = int.Parse(passLvl);
                }
            }else{
                Debug.Log("No valid progress found...");
            }
            executeStartup = true;
        }

        GameObject[] objs = GameObject.FindGameObjectsWithTag("logic");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void startHunt(string levelName) {
        int month = int.Parse(System.DateTime.Now.ToString("MM"));
        int day = int.Parse(System.DateTime.Now.ToString("dd"));
        if(month != 12 && day <= 24) {
            Debug.Log(month);
            Debug.Log(day);
            Debug.Log("NO NO NO");
            notYetString.SetActive(true);
        }else{
            loadScene(levelName);
        }
    }

    public void loadScene(string levelName) {
        SceneManager.LoadScene(levelName);
    }

    public void enterPasscode(string password) {
        if(password == passCodes[currLvl]){
            Debug.Log("YES");
        }else{
            Debug.Log("NO");
        }
    }

    void OnApplicationQuit()
    {
        if(notYetString){
            notYetString.SetActive(false);
        }
        PlayerPrefs.SetString("level", SceneManager.GetActiveScene().name);
    }
}
