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

public class RoomHandler : Core
{
    #region [ OBJECTS ]



    #endregion

    #region [ PROPERTIES ]

    private static List<RoomInfo> rooms = new List<RoomInfo>();
    private static List<RoomInfo> roomsOpen = new List<RoomInfo>();

    #endregion

    #region [ COROUTINES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        rooms = roomList;
        roomsOpen.Clear();
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].IsOpen)
            {
                roomsOpen.Add(rooms[i]);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (GameManager.isServer)
        {
            PhotonNetwork.LoadLevel("3_Gameplay");
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                PhotonNetwork.LoadLevel("3_Gameplay");
            }
            else
            {
                PhotonNetwork.LoadLevel("2b_WaitingRoom");
            }
        }
        PhotonNetwork.NickName = "<>";
    }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public static void CreateRoom(string roomName, int playerCap, string password)
    {
        RoomOptions opt = new RoomOptions();
        if (roomName.IsEmptyOrNullOrWhiteSpace())
        {
            roomName = "Room_" + Ext_String.RandomString(8);
        }
        if (playerCap < 2)
        {
            playerCap = 2;
        }
        opt.MaxPlayers = (byte)playerCap;
        if (!password.ValidateString())
        {
            password = null;
        }

        PhotonNetwork.CreateRoom(roomName, opt);
    }

    public static bool JoinRoom(string roomName, string password)
    {
        return JoinRoom(roomName, password, false);
    }
    
    public static bool JoinRoom(string roomName, string password, bool joiningRandom)
    {
        if (RoomExistsWithName(roomName))
        {
            if (!RoomIsFull(roomName))
            {
                PhotonNetwork.JoinRoom(roomName);
                return true;
            }
            else
            {
                if (!joiningRandom)
                {
                    DebugLogging.Error("#PlayerError_RoomFull");
                }
                return false;
            }
        }
        else
        {
            CreateRoom(roomName, 20, password);
            return true;
        }
    }

    public static void JoinRoomRandom()
    {
        bool successful = false;
        if (roomsOpen.Count > 0)
        {
            for (int i = 0; i < 50; i++)
            {
                int n = Random.Range(0, roomsOpen.Count);
                string roomName = roomsOpen[n].Name;
                if (JoinRoom(roomName, "", true))
                {
                    successful = true;
                    break;
                }
            }
        }
        if (!successful)
        {
            DebugLogging.Error("#PlayerError_CantJoinRandom");
        }
    }

    public static RoomInfo GetRoomInfo(string roomName)
    {
        foreach (RoomInfo room in rooms)
        {
            if (roomName == room.Name)
            {
                return room;
            }
        }
        return null;
    }

    public static bool RoomExistsWithName(string roomName)
    {
        return GetRoomInfo(roomName) != null;
    }

    public static bool RoomIsFull(string roomName)
    {
        RoomInfo room = GetRoomInfo(roomName);
        if (room != null && room.IsOpen && room.PlayerCount < room.MaxPlayers)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
