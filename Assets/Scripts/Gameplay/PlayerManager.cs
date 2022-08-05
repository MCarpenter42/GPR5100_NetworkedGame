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

    private SpawnPoint[] spawnPoints;

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
        spawnPoints = FindObjectsOfType<SpawnPoint>();
    }

    void Start()
    {
        if (GameManager.UIHandler.HUD != null && GameManager.UIHandler.HUD.healthBar != null)
        {
            GameManager.UIHandler.HUD.ShowHealthBar(false);
            GameManager.UIHandler.HUD.ShowWeaponElevation(false);
            //GameManager.UIHandler.HUD.ShowWeaponCooldown(false);
            GameManager.UIHandler.HUD.ShowRespawnTimer(false);
        }
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

        Vector3 randomSpawnPos = GetSpawnPoint();

        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPos, Quaternion.identity);
        PlayerController client = playerObj.GetComponent<PlayerController>();
        client.CameraActive(true);
        GameManager.RoomLoader.loadingCam.SetActive(false);

        AddPlayer(PhotonNetwork.NickName, client);

        GameManager.UIHandler.HUD.ShowHealthBar(true);
        GameManager.UIHandler.HUD.ShowWeaponElevation(true);
        //GameManager.UIHandler.HUD.ShowWeaponCooldown(true);
        GameManager.UIHandler.HUD.healthBar.DoUpdate(client.currentHealth, client.maxHealth, true, 1);

        GameManager.UIHandler.blackScreen.SetActive(false);
    }

    public Vector3 GetSpawnPoint()
    {
        Vector3 point = new Vector3(2.0f, 0.5f, 0.0f);
        if (spawnPoints.Length > 0)
        {
            List<SpawnPoint> available = spawnPoints.ToList();
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                int r = Random.Range(1, available.Count) - 1;
                if (available[r].IsValid(5.0f))
                {
                    point = available[r].pos;
                }
                else
                {
                    available.RemoveAt(r);
                }
            }
        }
        point.y -= 0.2f;
        return point;
    }

    [PunRPC]
    public void AddPlayer(string name, PlayerController controller)
    {
        if (players.ContainsKey(name))
        {
            players[name] = controller;
        }
        else
        {
            controller.playerName = name;
            players.Add(name, controller);
        }
    }

    [PunRPC]
    public void RemovePlayer(string name)
    {
        players.Remove(name);
    }
}
