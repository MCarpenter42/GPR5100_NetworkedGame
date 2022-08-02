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

	[SerializeField] GameObject serverCam;
	[SerializeField] GameObject loadingCam;
	[SerializeField] UIElement nameSetFrame;
	private UIHandler_Room roomUI;

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

		roomUI = FindObjectOfType<UIHandler_Room>();
		if (roomUI != null)
        {
			roomUI.serverUI.SetActive(GameManager.isServer);
			roomUI.playerUI.SetActive(!GameManager.isServer);
		}
    }

    void Start()
    {
		if (!GameManager.isServer)
		{
			roomUI.ShowEscMenu(true);
			roomUI.escMenu.SetActiveFrame(nameSetFrame);
		}
	}

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

}
