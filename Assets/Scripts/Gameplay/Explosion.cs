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

	public int damage = 1;
    public float radius = 1.0f;

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

        foreach (KeyValuePair<string, PlayerController> playerItem in GameManager.PlayerManager.players)
        {
            PlayerController player = playerItem.Value;
            float dist = (player.transform.position - transform.position).magnitude;
            if (dist <= radius)
            {
                player.Damage(damage);
            }
        }

        float explosionDuration = 0.2f;
        float timePassed = 0.0f;
        while (timePassed <= explosionDuration)
        {
            yield return null;
            timePassed += Time.deltaTime;
            float deltaRadius = InterpDelta.CosSlowDown(timePassed / explosionDuration) * radius;
            visuals.transform.localScale = Vector3.one * deltaRadius * 2.0f;
        }
        visuals.transform.localScale = Vector3.one * radius * 2.0f;
        Destroy(gameObject, 0.05f);
    }
}
