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

public class Core : MonoBehaviourPunCallbacks
{
    #region [ OBJECTS ]

    public static Controls Controls
    {
        get
        {
            if (GameManager.controlsInstance == null)
            {
                GameManager.controlsInstance = new Controls();
            }
            return GameManager.controlsInstance;
        }
    }

    #endregion

    #region [ ENUMERATION TYPES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ GAME STATE ]

    public static void LockCursor(bool lockCsr)
    {
        GameManager.isCursorLocked = lockCsr;
        if (lockCsr)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public static void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Disconnecting from Photon server...");
            PhotonNetwork.Disconnect();
        }
    }

    public void ExitGame()
    {
        GameManager.OnExitEvent();
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

    #endregion

    #region [ INPUT HANDLING ]

    public static bool GetInput(ControlInput input)
    {
        return Input.GetKey(input.Key);
    }

    public static bool GetInputDown(ControlInput input)
    {
        return Input.GetKeyDown(input.Key);
    }

    public static bool GetInputUp(ControlInput input)
    {
        return Input.GetKeyUp(input.Key);
    }

    #endregion
}
