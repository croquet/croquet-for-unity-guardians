using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonPressNameChooser : SessionNameChooser
{
    // public reference to the text to update
    public TMPro.TMP_Text sessionNameText;
    public TMPro.TMP_InputField sessionIPText;

    private static int SessionNameValue
    {
        get {
            return _sessionNameValue;
        }
        set {
            _sessionNameValue = Mathf.Clamp(value, 0, 100);
            if (OnSessionNameChange != null)
                OnSessionNameChange(_sessionNameValue);

            PlayerPrefs.SetInt("sessionNameValue", _sessionNameValue);
            PlayerPrefs.Save();
        }
    }

    private static string SessionIP
    {
        get {
            return _sessionIPValue;
        }
        set
        {
            _sessionIPValue = value;
            if (OnSessionIPChange != null)
                OnSessionIPChange(_sessionIPValue);

            Debug.Log($"Player Prefs 'sessionIP' set to {_sessionIPValue}");
            PlayerPrefs.SetString("sessionIP", _sessionIPValue);
            PlayerPrefs.Save();
        }
    }

    private static int _sessionNameValue = 0;
    public delegate void OnSessionNameChangeDelegate(int newVal);
    public static event OnSessionNameChangeDelegate OnSessionNameChange;

    private static string _sessionIPValue = "";
    public delegate void OnSessionIPChangeDelegate(string newIP);
    public static event OnSessionIPChangeDelegate OnSessionIPChange;

    void Start()
    {
        OnSessionNameChange += SessionNameChangeHandler;
        OnSessionIPChange += SessionIPChangeHandler;

        // recover the session name and IP from save data
        SessionNameValue = PlayerPrefs.GetInt("sessionNameValue");
        SessionIP = PlayerPrefs.GetString("sessionIP");
    }

    private void SessionNameChangeHandler(int newVal)
    {
        Debug.Log("Session name Change handler");
        sessionNameText.text = newVal.ToString();
    }

    private void SessionIPChangeHandler(string newIP)
    {

        Debug.Log($"Session IP Change handler newIP is: {newIP}");
        sessionIPText.text = newIP;
    }

    public void setSessionIP(string newIP)
    {
        Debug.Log($"Session IP is being set to:{newIP}");
        SessionIP = newIP;
    }

    // allow increment and decrement
    public static void increment(int amount)
    {
        SessionNameValue+=amount;
    }

    public static void decrement(int amount)
    {
        SessionNameValue-=amount;
    }
}
