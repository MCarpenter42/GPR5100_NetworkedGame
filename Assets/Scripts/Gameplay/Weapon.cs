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

public class Weapon : Core
{
    #region [ OBJECTS ]

	[SerializeField] Transform firePoint;
    [SerializeField] Projectile primaryProjectile;
    [SerializeField] Projectile secondaryProjectile;

    #endregion

    #region [ PROPERTIES ]

    [HideInInspector] public bool onCooldown = false;
    [SerializeField] float primaryFireCooldown = 0.5f;
    public bool hasSecondaryFire { get { return secondaryProjectile != null; } }
    [SerializeField] float secondaryFireCooldown = 0.5f;

    #endregion

    #region [ COROUTINES ]

    private Coroutine cooldown = null;
	
	#endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

	#region [ BUILT-IN UNITY FUNCTIONS ]

    void Awake()
    {
        if (primaryProjectile == null)
        {
            Debug.LogError("Missing projectile prefab!");
        }
    }

	#endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */
	
    public void PrimaryFire()
    {
        if (!onCooldown)
        {
            //PhotonNetwork.InstantiateRoomObject(primaryProjectile.name, firePoint.transform.position, firePoint.rotation);
            InstantiateRPC(primaryProjectile, firePoint.transform.position, firePoint.rotation);
            cooldown = StartCoroutine(Cooldown(primaryFireCooldown));
        }
    }
    
    public void SecondaryFire()
    {
        if (!onCooldown)
        {
            cooldown = StartCoroutine(Cooldown(secondaryFireCooldown));
        }
    }

    private IEnumerator Cooldown(float cd)
    {
        onCooldown = true;
        yield return new WaitForSeconds(cd);
        onCooldown = false;
    }
}
