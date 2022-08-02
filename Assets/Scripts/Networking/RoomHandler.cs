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



    #endregion

    #region [ COROUTINES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    public static void RoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if ((!room.IsOpen || !room.IsVisible || room.RemovedFromList) && rooms.ContainsKey(room.Name))
            {
                rooms.Remove(room.Name);
            }
            else
            {
                if (rooms.ContainsKey(room.Name))
                {
                    rooms[room.Name] = room;
                }
                else
                {
                    rooms.Add(room.Name, room);
                }
            }
        }
        //Debug.Log(rooms.Count);

        foreach (KeyValuePair<string, RoomInfo> kvp in rooms)
        {
            if (roomsOpen.ContainsKey(kvp.Key))
            {
                roomsOpen[kvp.Key] = kvp.Value;
            }
            else
            {
                roomsOpen.Add(kvp.Key, kvp.Value);
            }
        }

        foreach (KeyValuePair<string, RoomInfo> kvp in roomsOpen)
        {
            if (!rooms.ContainsKey(kvp.Key))
            {
                roomsOpen.Remove(kvp.Key);
            }
        }
    }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public static void CreateRoom(string roomName, int playerCap, string password)
    {
        if (roomName.IsEmptyOrNullOrWhiteSpace())
        {
            roomName = "Room_" + Ext_String.RandomString(8);
        }
        if (playerCap < 2)
        {
            playerCap = 2;
        }
        if (!password.ValidateString())
        {
            password = null;
        }
        RoomOptions rOpt = new RoomOptions();
        rOpt.IsOpen = true;
        rOpt.IsVisible = true;
        rOpt.MaxPlayers = (byte)playerCap;
        PhotonNetwork.CreateRoom(roomName, rOpt);
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
                List<string> keys = new List<string>();
                foreach(KeyValuePair<string, RoomInfo> kvp in rooms)
                {
                    keys.Add(kvp.Key);
                }
                int n = Random.Range(0, keys.Count);
                string roomName = roomsOpen[keys[n]].Name;
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
        if (rooms.ContainsKey(roomName))
        {
            return rooms[roomName];
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
