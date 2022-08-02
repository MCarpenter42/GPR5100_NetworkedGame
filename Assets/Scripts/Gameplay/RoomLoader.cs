using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class RoomLoader : Core
{
	#region [ OBJECTS ]

	public GameObject serverCam;
	public GameObject loadingCam;
	[SerializeField] UIElement nameSetFrame;

	#endregion

	#region [ PROPERTIES ]



	#endregion

	#region [ COROUTINES ]



	#endregion

	/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

	#region [ BUILT-IN UNITY FUNCTIONS ]

	void Awake()
    {
		serverCam.SetActive(GameManager.isServer);
		loadingCam.SetActive(!GameManager.isServer);

		if (GameManager.UIHandler != null)
        {
			GameManager.UIHandler.serverUI.SetActive(GameManager.isServer);
			GameManager.UIHandler.playerUI.SetActive(!GameManager.isServer);
		}
    }

    void Start()
    {
		if (!GameManager.isServer)
		{
			GameManager.UIHandler.escMenu.ShowMenu(true);
			GameManager.UIHandler.escMenu.SetActiveFrame(nameSetFrame);
		}
	}

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

}
