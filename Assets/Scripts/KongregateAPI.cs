﻿using UnityEngine;
using System;
using System.Collections;

/*
//////////////////////////////////////////////////////////////////////////////
//                                                                          //
//                  SUBMIT STATISTICS WITH                                  //
//                                                                          //
//  Application.ExternalCall("kongregate.stats.submit","MatchesMade",1);    //
//                                                                          //
//////////////////////////////////////////////////////////////////////////////
*/

public class KongregateAPI : MonoBehaviour
{

    public bool isKongregate = false;
    public string username = "Guest";
    public int userID = 0;
    public string gameAuthToken = "";

    private static KongregateAPI s_instance = null;
    public static KongregateAPI instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject go = new GameObject("KongregateAPI");
                s_instance = go.AddComponent<KongregateAPI>();
            }
            return s_instance;
        }
    }

    public void InitialStart()
    {

    }

    void Awake()
    {

        if(s_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        
		s_instance = this;

        DontDestroyOnLoad(gameObject);

        Application.ExternalEval(
            "if(typeof(kongregateUnitySupport) != 'undefined'){" +
            " kongregateUnitySupport.initAPI('KongregateAPI', 'OnKongregateAPILoaded');" +
            "}"
        );
    }

    void OnKongregateAPILoaded(string userInfoString)
    {
        isKongregate = true;

        string[] splitUserInfo = userInfoString.Split("|"[0]);
        userID = int.Parse(splitUserInfo[0]);
        username = splitUserInfo[1];
        gameAuthToken = splitUserInfo[2];
    }

    public void SubmitStats(string stat, int score)
    {
        Application.ExternalCall("kongregate.stats.submit", stat, score);
    }
}


