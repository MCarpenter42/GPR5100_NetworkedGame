using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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

public class ServerConnection : Core
{
    [SerializeField] Menu frameHandler;
    [SerializeField] UnityEvent onConnected;

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    void Start()
    {
        if (GameManager.goToStartScreen)
        {
            GameManager.goToStartScreen = false;
            SceneManager.LoadScene("1_StartScreen");
        }
    }

    #endregion

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        onConnected.Invoke();
    }

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void GoToRoomMenu()
    {
        SceneManager.LoadScene("2_RoomSelect");
    }
}
