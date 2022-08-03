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
    [Header("Weapon Properties")]
	[SerializeField] Transform firePoint;

    [Header("Primary Fire")]
    [SerializeField] Projectile primaryProjectile;
    [SerializeField] float primaryFireCooldown = 0.5f;

    [Header("Secondary Fire")]
    [SerializeField] Projectile secondaryProjectile;
    [SerializeField] bool enableSecondaryFire = false;
    [SerializeField] float secondaryFireCooldown = 0.5f;
    public bool hasSecondaryFire
    {
        get
        {
            if (secondaryProjectile == null)
            {
                return false;
            }
            else
            {
                return enableSecondaryFire;
            }
        }
    }

    [HideInInspector] public bool onCooldown = false;

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
            Projectile prj = InstantiateRPC(primaryProjectile, firePoint.position, Quaternion.identity);
            prj.gameObject.transform.eulerAngles = firePoint.eulerAngles;
            prj.Fire();
            cooldown = StartCoroutine(Cooldown(primaryFireCooldown));
        }
    }
    
    public void SecondaryFire()
    {
        if (!onCooldown)
        {
            Projectile prj = InstantiateRPC(secondaryProjectile, firePoint.position, Quaternion.identity);
            prj.gameObject.transform.eulerAngles = firePoint.eulerAngles;
            prj.Fire();
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
