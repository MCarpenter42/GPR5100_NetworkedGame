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

public class PlayerManager : Core
{
    #region [ OBJECTS ]

    [SerializeField] GameObject playerPrefab;
    [HideInInspector] public Dictionary<string, PlayerItem> players = new Dictionary<string, PlayerItem>();

    #endregion

    #region [ PROPERTIES ]



    #endregion

    #region [ COROUTINES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    void Awake()
    {
        Player[] currentPlayers = PhotonNetwork.PlayerList;
        string nameCheck = PhotonNetwork.NickName;
        int nameMatches = 0;
        foreach (Player plr in currentPlayers)
        {
            if (nameCheck == plr.NickName)
            {
                nameMatches++;
            }
        }
        if (nameMatches > 0)
        {
            string newName = "";
            int lastPos = nameCheck.LastIndexOf('_');
            if (lastPos > 0)
            {
                string substrA = nameCheck.Substring(0, lastPos + 1);
                string substrB = nameCheck.Substring(lastPos + 1);
                int toInt = 0;
                if (int.TryParse(substrB, out toInt))
                {
                    newName = substrA + (toInt + 1);
                }
                else
                {
                    newName = nameCheck + "_1";
                }
            }
            else
            {
                newName = nameCheck + "_1";
            }
            PhotonNetwork.NickName = newName;
        }
    }

    void Start()
    {
        
    }
	
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

	#endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */
	
}

public class PlayerItem
{
    public string name = "";
    public PlayerController controller;
    public int score = 0;

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public PlayerItem(string name, PlayerController controller)
    {
        this.name = name;
        this.controller = controller;
    }
}
