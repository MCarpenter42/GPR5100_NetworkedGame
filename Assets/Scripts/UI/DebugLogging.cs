using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;
using Photon;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;

using NeoCambion;
using NeoCambion.Collections;
using NeoCambion.Encryption;
using NeoCambion.Interpolation;
using NeoCambion.Maths;
using NeoCambion.Unity;

public class DebugLogging : Core
{
    public static List<string> debugLog = new List<string>();

    public static string logStart;
    public static string logEnd;

    private static string logLineHeader;
    private static string logLineContent;
    private static string stack;

    public static string editorDirectory { get { return Application.dataPath + "/OutputLogs"; } }

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    void Start()
    {
        NewLog();
    }

    public override void OnEnable()
    {
        StartLogging();
    }

    public override void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public void StartLogging()
    {
        Application.logMessageReceived += Log;
        NewLog();
    }

    public static void NewLog()
    {
        debugLog.Clear();
        logStart = System.DateTime.Now.ToString();
        logEnd = "";
        debugLog.Add(">> LOGGING STARTED: " + logStart + " <<");
        debugLog.Add("");
        debugLog.Add("");
        //GameManager.Instance.OnLog();
    }

    public static void Log(string logString, string stackTrace, LogType type)
    {
        stack = stackTrace;
        string typeString = type.ToString();

        WriteToLog(logString, typeString);
    }

    public static void Error(string logString)
    {
        stack = "";
        WriteToLog(logString, "Error (Custom)");
    }
    
    public static void Command(string logString)
    {
        stack = "";
        WriteToLog(logString, "Console Command");
    }

    public static void SystemMessage(string logString)
    {
        stack = "";
        WriteToLog(logString, "System Message");
    }

    public static void WriteToLog(string logString, string logType)
    {
        string timestamp = System.DateTime.Now.ToString().Substring(11);

        logLineHeader = timestamp + " | " + logType;
        debugLog.Add(logLineHeader);
        logLineContent = ">>> " + logString;
        debugLog.Add(logLineContent);
        debugLog.Add(stack);
        debugLog.Add("");

        if (logString.StartsWith("#"))
        {
            TaggedMessageHandler(logString.Substring(1));
        }
        //GameManager.Instance.OnLog();
    }

    public static void TaggedMessageHandler(string logString)
    {
        string substr = logString.Split('_')[0];
        int n = logString.IndexOf('_') + 1;
        switch (substr)
        {
            case "PlayerError":
                PlayerError(logString.Substring(n));
                break;

            default:
                break;
        }
    }

    private static void PlayerError(string logString)
    {
        Debug.LogError("PLAYER ERROR: " + logString);
    }
}