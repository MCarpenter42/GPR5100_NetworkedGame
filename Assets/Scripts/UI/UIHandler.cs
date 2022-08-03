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

public class UIHandler : Core
{
    #region [ OBJECTS ]

    public static TMP_Text ErrorText
    {
        get
        {
            GameObject[] textObjs = GameObject.FindGameObjectsWithTag("ErrorText");
            for (int i = 0; i < textObjs.Length; i++)
            {
                if (textObjs[i].activeSelf)
                {
                    return textObjs[i].GetComponent<TMP_Text>();
                }
            }
            if (textObjs.Length > 0)
            {
                return textObjs[0].GetComponent<TMP_Text>();
            }
            else
            {
                return null;
            }
        }
    }

    [Header("Components")]
    public GameObject blackScreen;
    public GameObject serverUI;
    public GameObject playerUI;
    [HideInInspector] public HUD HUD;
    [HideInInspector] public Menu escMenu;

    #endregion

    #region [ PROPERTIES ]

    private bool usePlayerUI;

    #endregion

    #region [ COROUTINES ]

    protected Coroutine errorText = null;
    protected TMP_Text targetErrorText;

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    void Awake()
    {
        if (GameManager.inRoom)
        {
            usePlayerUI = !GameManager.isServer;
            playerUI.SetActive(usePlayerUI);
            HUD = playerUI.GetComponentsInChildren<HUD>().First();
            escMenu = playerUI.GetComponentsInChildren<Menu>().First();
            serverUI.SetActive(!usePlayerUI);
        }
        else if (GameManager.isCursorLocked)
        {
            LockCursor(false);
        }
    }

    void Start()
    {
        if (GameManager.inRoom)
        {
            escMenu.ShowMenu(true);
        }
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */
    
    public void InputsUI()
    {
        if (escMenu != null)
        {
            if (GetInputDown(Controls.General.EscMenu) && GameManager.PlayerManager.nameSet)
            {
                ToggleEscMenu();
            }
        }
    }

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public void ToggleEscMenu()
    {
        escMenu.ShowMenu(!escMenu.framesVisible);
    }

    public void ShowErrorText(string text)
    {
        ShowErrorText(text, 5.0f);
    }
    
    public void ShowErrorText(string text, float time)
    {
        if (errorText != null)
        {
            StopCoroutine(errorText);
        }
        if (ErrorText != null)
        {
            errorText = StartCoroutine(IShowErrorText(text, time));
        }
    }

    protected IEnumerator IShowErrorText(string text, float time)
    {
        ErrorText.gameObject.SetActive(true);
        ErrorText.text = text;
        Color clr = ErrorText.color;
        clr.a = 1.0f;
        ErrorText.color = clr;

        yield return new WaitForSeconds(time);

        float fadeTime = 0.4f;
        float timePassed = 0.0f;
        while (timePassed <= fadeTime)
        {
            if (ErrorText == null || !ErrorText.gameObject.activeSelf)
            {
                break;
            }
            yield return null;
            timePassed += Time.deltaTime;
            float delta = 1.0f - (timePassed / fadeTime);
            clr.a = delta;
            ErrorText.color = clr;
        }
        if (ErrorText != null || ErrorText.gameObject.activeSelf)
        {
            clr.a = 0.0f;
            ErrorText.color = clr;
        }
    }
}
