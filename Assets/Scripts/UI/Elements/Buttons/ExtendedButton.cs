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

[ExecuteInEditMode]
public class ExtendedButton : UIElement
{
    #region [ OBJECTS ]

    protected Button button { get { return gameObject.GetOrAddComponent<Button>(); } }

    #endregion

    #region [ PROPERTIES ]

    [Header("Button Properties")]
    [SerializeField] Graphic targetGraphic;
    [SerializeField] ColorBlock colours = new ColorBlock
    {
        normalColor = Color.white,
        highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1.0000000f),
        pressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 1.0000000f),
        selectedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1.0000000f),
        disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f),
        colorMultiplier = 1.0f,
        fadeDuration = 0.1f
    };
    [SerializeField] UnityEvent onClick;

    #endregion

    #region [ COROUTINES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    protected override void Awake()
    {
        base.Awake();
        if (Application.isPlaying)
        {
            UpdateButtonProperties();
            AddListeners();
        }
    }

    protected override void Update()
    {
        base.Update();
#if UNITY_EDITOR
        UpdateButtonProperties();
#endif
    }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public void SetLabel(string text)
    {
        TMP_Text[] textChildren = button.gameObject.GetComponentsInChildren<TMP_Text>();
        if (textChildren.Length > 0)
        {
            textChildren[0].text = text;
        }
    }

    protected virtual void UpdateButtonProperties()
    {
        if (targetGraphic == null)
        {
            targetGraphic = gameObject.GetComponent<Graphic>();
        }
        button.targetGraphic = targetGraphic;
        button.colors = colours;
    }

    protected virtual void AddListeners()
    {
        if (onClick != null)
        {
            button.onClick.AddListener(onClick.Invoke);
        }
    }
}