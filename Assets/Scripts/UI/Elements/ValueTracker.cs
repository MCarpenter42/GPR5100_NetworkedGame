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
public class ValueTracker : UIElement
{
	#region [ OBJECTS ]

    private enum Track { PlayerHealth };
    private enum Visual { Scale, Rotate };
    private enum ScaleAnchor { BottomLeft, Centre, TopRight };

	#endregion

	#region [ PROPERTIES ]

    [Header("Tracker Properties")]
    [SerializeField] Track valueToTrack;
    [SerializeField] Visual visualRepres;

    [Header("Scale Settings")]
    [SerializeField] Axis scaleAxis;
    [SerializeField] ScaleAnchor scaleAnchor;
    private Vector2 scaleAnchorPoint;

    [Header("RotationSettings")]
    [SerializeField] RotDirection rotDirection;
    [Range(0.0f, 360.0f)]
    [SerializeField] float maxAngle;
    private Vector2 rotAnchorPoint;

    #endregion

    #region [ COROUTINES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    protected override void Awake()
    {
        base.Awake();
        DisallowZ();
    }

    protected override void Update()
    {
		if (Application.isPlaying)
        {
            base.Update();
        }
        else
        {
            DisallowZ();
        }
    }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    private void DisallowZ()
    {
        if (scaleAxis == Axis.Z)
        {
            scaleAxis = Axis.X;
        }
    }


}
