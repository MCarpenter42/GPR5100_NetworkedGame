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
    [HideInInspector] public Dictionary<string, PlayerController> players = new Dictionary<string, PlayerController>();

    [SerializeField] TMP_InputField inputName;

    #endregion

    #region [ PROPERTIES ]

    [HideInInspector] public bool nameSet = false;

    #endregion

    #region [ COROUTINES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    void Awake()
    {
        
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

    public void TrySetName()
    {
        string nameInput = inputName.text;
        if (nameInput.ValidateString(true))
        {
            if (NameAvailable(nameInput))
            {
                PhotonNetwork.NickName = nameInput;
                nameSet = true;
                inputName.text = "";
                GameManager.UIHandler.escMenu.activeFrame = 0;
                GameManager.UIHandler.ToggleEscMenu();
                SpawnPlayer();
            }
            else
            {
                DebugLogging.Error("#PlayerError_NicknameTaken");
            }
        }
        else
        {
            DebugLogging.Error("#PlayerError_InvalidNickame");
        }
    }

    public bool NameAvailable(string name)
    {
        Player[] currentPlayers = PhotonNetwork.PlayerList;
        foreach (Player plr in currentPlayers)
        {
            if (name == plr.NickName)
            {
                return false;
            }
        }
        return true;
    }

    public void SpawnPlayer()
    {
        GameManager.UIHandler.blackScreen.SetActive(true);

        Vector3 randomSpawnPos = new Vector3(0.0f, 0.5f, 0.0f);
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPos, Quaternion.identity);
        PlayerController client = playerObj.GetComponent<PlayerController>();
        client.SetAsClient(true);
        GameManager.RoomLoader.loadingCam.SetActive(false);

        AddPlayer(PhotonNetwork.NickName, client);

        GameManager.UIHandler.blackScreen.SetActive(false);
    }

    [PunRPC]
    public void AddPlayer(string name, PlayerController controller)
    {
        controller.playerName = name;
        players.Add(name, controller);
    }

    public void RemovePlayer(string name)
    {
        players.Remove(name);
    }
}
