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



    #endregion

    #region [ ENUMERATION TYPES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ GAME STATE ]

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
}
