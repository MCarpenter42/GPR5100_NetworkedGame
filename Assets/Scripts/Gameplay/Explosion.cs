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

public class Explosion : Core
{
    #region [ OBJECTS ]

    [SerializeField] GameObject visuals;

	#endregion

	#region [ PROPERTIES ]

	[HideInInspector] public int damage = 1;
    [HideInInspector] public float radius = 1.0f;

    #endregion

    #region [ COROUTINES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public virtual void Explode()
    {
        StartCoroutine(IExplode());
    }

    protected virtual IEnumerator IExplode()
    {
        Vector3 randRot = new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        visuals.transform.eulerAngles = randRot;

        float explosionDuration = 0.25f;
        float timePassed = 0.0f;
        while (timePassed <= explosionDuration)
        {
            yield return null;
            timePassed += Time.deltaTime;
            float delta = InterpDelta.CosSlowDown(timePassed / explosionDuration);
            visuals.transform.localScale = Vector3.one * radius * delta;
        }
        visuals.transform.localScale = Vector3.one * radius;
        Destroy(gameObject, 0.05f);
    }
}
