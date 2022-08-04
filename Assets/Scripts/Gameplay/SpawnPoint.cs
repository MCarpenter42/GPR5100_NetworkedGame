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

public class SpawnPoint : Core
{
    #region [ OBJECTS ]

    private MeshRenderer rndr { get { return gameObject.GetComponent<MeshRenderer>(); } }
    private MeshRenderer[] childRndr { get { return gameObject.GetComponentsInChildren<MeshRenderer>(); } }

    public Vector3 pos { get { return gameObject.transform.position; } }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    void Awake()
    {
        rndr.enabled = false;
        foreach (MeshRenderer rndr in childRndr)
        {
            rndr.enabled = false;
        }
    }

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public bool IsValid(float playerBlockDistance)
    {
        foreach (KeyValuePair<string, PlayerController> kvp in GameManager.PlayerManager.players)
        {
            GameObject playerObj = kvp.Value.gameObject;
            float dist = (playerObj.transform.position - transform.position).magnitude;
            if (dist <= playerBlockDistance)
            {
                return false;
            }
        }
        return true;
    }
}
