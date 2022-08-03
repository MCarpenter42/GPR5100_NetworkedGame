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

public class GameManager : Core
{
    #region [ OBJECTS ]

    DefaultPool DefaultPool;
    [SerializeField] GameObject[] prefabs;

    private static GameManager instance = null;
    public static Controls controlsInstance;
    public static UIHandler UIHandler { get { return FindObjectOfType<UIHandler>(); } }
    public static RoomLoader RoomLoader { get { return FindObjectOfType<RoomLoader>(); } }
    public static PlayerManager PlayerManager { get { return FindObjectOfType<PlayerManager>(); } }

    public static GameObject WorldSpace { get { return GameObject.FindGameObjectWithTag("WorldSpace"); } }
    public static PlayerController ClientPlayer;

    #endregion

    #region [ PROPERTIES ]

    public static bool onLoad = true;
    public static bool goToStartScreen = true;

    public static bool isServer = false;
    public static bool inRoom = false;
    public static bool isCursorLocked = false;

    public static bool exitEventStarted = false;

    #endregion

    #region [ COROUTINES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ SINGLETON CONTROL ]

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameManager inst = FindObjectOfType<GameManager>();
                if (inst == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();

                    instance.Init();

                    // Prevents game manager from being destroyed on loading of a new scene
                    DontDestroyOnLoad(obj);

                    Debug.Log(obj.name);
                }
            }
            return instance;
        }
    }

    // Initialiser function, serves a similar purpose to a constructor
    private void Init()
    {
        //Setup();
    }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    void Awake()
    {
        if (Application.isPlaying)
        {
            if (onLoad)
            {
                Setup();
            }
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnModeChange;
#endif
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (UIHandler != null)
        {
            UIHandler.InputsUI();
        }
    }

    void FixedUpdate()
    {

    }

    private void OnApplicationQuit()
    {
        if (!exitEventStarted)
        {
            OnExitEvent();
        }
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        RoomHandler.RoomListUpdate(roomList);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        GameManager.inRoom = true;
        if (GameManager.isServer)
        {
            PhotonNetwork.LoadLevel("3_Gameplay");
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 0)
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

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        ClientPlayer = null;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        PlayerManager.RemovePlayer(otherPlayer.NickName);
    }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    private void Setup()
    {
        onLoad = false;
        SetupPrefabPool();
    }

    private void SetupPrefabPool()
    {
        DefaultPool = PhotonNetwork.PrefabPool as DefaultPool;
        if (prefabs.Length > 0)
        {
            foreach (GameObject prefab in prefabs)
            {
                if (!DefaultPool.ResourceCache.ContainsKey(prefab.name))
                {
                    DefaultPool.ResourceCache.Add(prefab.name, prefab);
                }
            }
        }
    }

    public static void OnExitEvent()
    {
        Disconnect();
    }

#if UNITY_EDITOR
    private void OnModeChange(PlayModeStateChange stateChange)
    {
        if (stateChange == PlayModeStateChange.ExitingPlayMode)
        {
            OnExitEvent();
        }
    }
#endif

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

}
