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

public class Menu : UIElement
{
    #region [ OBJECTS ]

    [SerializeField] UIElement[] menuFrames;

    #endregion

    #region [ PROPERTIES ]

    private int activeFrame = 0;

    #endregion

    #region [ COROUTINES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    protected override void Awake()
    {
        SetActiveFrame(activeFrame);
    }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public void SetActiveFrame(int index)
    {
        if (index.InBounds(menuFrames))
        {
            for (int i = 0; i < menuFrames.Length; i++)
            {
                if (i == index)
                {
                    menuFrames[i].SetShown(true);
                }
                else
                {
                    menuFrames[i].SetShown(false);
                }
            }
        }
    }
}
