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

public class RoomHandlerButton : ExtendedButton
{
    #region [ OBJECTS ]

    private enum RmBtnType { Create, Join, JoinRandom };
    [SerializeField] RmBtnType type = RmBtnType.Join;

    [Header("Target Objects")]
    [SerializeField] RoomHandler roomHandler;
    [SerializeField] TMP_InputField inputRoomName;
    [SerializeField] TMP_InputField inputPlayerCap;
    [SerializeField] TMP_InputField inputPassword;

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    protected override void AddListeners()
    {
        base.AddListeners();
        switch (type)
        {
            case RmBtnType.Create:
                button.onClick.AddListener(CreateRoom);
                break;

            default:
            case RmBtnType.Join:
                button.onClick.AddListener(JoinRoom);
                break;

            case RmBtnType.JoinRandom:
                button.onClick.AddListener(JoinRoomRandom);
                break;
        }
    }

    public void CreateRoom()
    {
        if (inputRoomName == null || inputPlayerCap == null || inputPassword == null)
        {
            Debug.LogError("Missing necessary component!");
        }
        else
        {
            string roomName = inputRoomName.text;
            int playerCap = 0;
            try
            {
                playerCap = int.Parse(inputRoomName.text);
            }
            catch
            {
                playerCap = 10;
            }
            string password = inputRoomName.text;
            if (!RoomHandler.RoomExistsWithName(roomName))
            {
                if (password.ValidateString())
                {
                    RoomHandler.CreateRoom(roomName, playerCap, password);
                }
                else
                {
                    DebugLogging.Error("#PlayerError_InvalidPassword");
                }
            }
            else
            {
                DebugLogging.Error("#PlayerError_RoomNameTaken");
            }
        }
    }

    public void JoinRoom()
    {
        if (inputRoomName == null || inputPassword == null)
        {
            Debug.LogError("Missing necessary component!");
        }
        else
        {
            string roomName = inputRoomName.text;
            string password = inputRoomName.text;
            RoomHandler.JoinRoom(roomName, password);
        }
    }

    public void JoinRoomRandom()
    {
        RoomHandler.JoinRoomRandom();
    }
}
