using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using PlayFab;
using PlayFab.ClientModels;

public class Manager : MonoBehaviour
{
    string[] textsArray,left,right,specialfinal;
    string filePath,fileName,filePath1,fileName1,filePath2,fileName2,specialfilename,specialpath,specialtext;
    public TMP_Text contentWindow,lefttext,righttext;
    public int control;
    public GameObject gameover, main;
    public bool login;
    public int next1,chealth,ccoins,chunger,specialnumber;
    public string keywords;
    public string s;
    public GetTitleDataResult result;
    public bool timerrunning;   
    public float time;

    public Graphic coinimage, healthimage, hungerimage;

    public int hunger, coins, health;

    // Start is called before the first frame update
    void Start()
    {
        keywords = null;
        login = false;
        hunger= coins = health = 3;
        main.SetActive(true);
        control =0;
        fileName = "File.txt";
        filePath = Application.dataPath + "/" + fileName;
        fileName1 = "Left.txt";
        filePath1 = Application.dataPath + "/" + fileName1;
        fileName2 = "Right.txt";
        filePath2 = Application.dataPath + "/" + fileName2;
        specialfilename = "Special.txt";
        specialpath = Application.dataPath + "/" + specialfilename;
        readfromfile();
       // Login();
    }


    public void gettitledata(int i)
    {
        void ServerGetTitleData()
        {
            PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnTitleDataRecieved, OnError);
        }

        void OnTitleDataRecieved(GetTitleDataResult result)
        {
            if (result.Data == null)
            {
                Debug.Log("Error");
            }
            else
            {
                s = i.ToString();
                Debug.Log(result.Data[s]);
                string[] splitArray = result.Data[s].Split(char.Parse(","));
                next1 = int.Parse(splitArray[0]);
                ccoins = int.Parse(splitArray[1]);
                chunger = int.Parse(splitArray[2]);
                chealth = int.Parse(splitArray[3]);
                if(splitArray[4] != null)
                {
                    keywords = keywords + splitArray[4];
                    Debug.Log(keywords);
                }
                updatestats(ccoins,chunger,chealth);
                settext(next1);
            }
        }
        ServerGetTitleData();
    }
    

    public void Left()
    {
        gettitledata(control);
        //Debug.Log("Left Pressed");
        stoptimer();
    }

    public void Right()
    {
        gettitledata(control+50);
        //Debug.Log("Right Pressed");
        stoptimer();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest{
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        //ServerGetTitleData();
        Debug.Log("Login Success");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Login Error");
    }

    // Update is called once per frame
    void Update()
    {
        check();
        runtimer();
    }

    public void runtimer()
    {
        if(timerrunning == false)
        {
            time = 0;
            timerrunning = true;
        }
        if(timerrunning == true)
        {
            time = time + Time.deltaTime;
            //
        }
    }

    public void stoptimer()
    {
        Debug.Log(time);
        WriteClientPlayerEventRequest request = new WriteClientPlayerEventRequest();
        request.EventName = "AnswerPressed";
        request.CustomTags = new Dictionary<string, string>();
        request.CustomTags.Add(control.ToString(), time.ToString());

        PlayFabClientAPI.WritePlayerEvent(request, OnSuccess, OnError);

        void OnSuccess(WriteEventResponse obj)
        {
            Debug.Log("Event Recorded");
        }

        void OnError(PlayFabError obj)
        {
            Debug.Log("Event Failed");
            Debug.Log(obj.ErrorMessage);
        }

        timerrunning = false;
    }    

    //public void SaveIntoJson()
    //{
    //    string timedata = JsonUtility.ToJson(time);
    //    System.IO.File.WriteAllText(Application.persistentDataPath + "/TimeData.json", timedata);
    //}

    public void updatestats(int i,int j,int k)
    {
        coins = coins + i;
        hunger = hunger + j;
        health = health + k;
    }
    public void readfromfile()
    {
        textsArray = File.ReadAllLines(filePath);
        left = File.ReadAllLines(filePath1);
        right = File.ReadAllLines(filePath2);
        specialfinal = File.ReadAllLines(specialpath);
        contentWindow.SetText(textsArray[control]);
        lefttext.SetText(left[control]);
        righttext.SetText(right[control]);
        //control = 1;
    }

    public void check()
    {
        if(health == 1)
        {
            healthimage.color = new Color(255f, 0f, 0f, 255f);
        }
        if(health == 2 || health == 3 || health == 4)
        {
            healthimage.color = new Color(255f, 173f, 0f, 255f);
        }
        if(health == 5)
        {
            healthimage.color = new Color(0f, 255f, 0f, 255f);
        }
        /////////
        if (coins == 1)
        {
            coinimage.color = new Color(255f, 0f, 0f, 255f);
        }
        if (coins == 2 || coins == 3 || coins == 4)
        {
            coinimage.color = new Color(255f, 173f, 0f, 255f);
        }
        if (coins == 5)
        {
            coinimage.color = new Color(0f, 255f, 0f, 255f);
        }
        ////
         if (hunger == 1)
        {
            hungerimage.color = new Color(255f, 0f, 0f, 255f);
        }
        if (hunger == 2 || hunger == 3 || hunger == 4)
        {
            hungerimage.color = new Color(255f, 173f, 0f, 255f);
        }
        if (hunger == 5)
        {
            hungerimage.color = new Color(0f, 255f, 0f, 255f);
        }
        if (hunger == 0 || health == 0 || coins == 0)
        {
            end();
        }
    }

    public void settext( int i)
    {
        contentWindow.SetText(textsArray[i]);
        lefttext.SetText(left[i]);
        righttext.SetText(right[i]);
        special(i);
    }

    public void special(int i)
    {
        if (keywords.Contains("dog"))
        {

        }
        control = i;
    }

    public void end()
    {
        main.SetActive(false);
        gameover.SetActive(true);
    }
}
