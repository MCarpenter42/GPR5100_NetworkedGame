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
    [Header("Components")]
    public Transform firePoint;
	public MeshRenderer[] modelElements;
    public PhotonView pView { get { return gameObject.GetOrAddComponent<PhotonView>(); } }

    [Header("Primary Fire")]
    public Projectile primaryProjectile;
    public float primaryFireCooldown = 0.5f;

    [Header("Secondary Fire")]
    public Projectile secondaryProjectile;
    public bool enableSecondaryFire = false;
    public float secondaryFireCooldown = 0.5f;
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

    public Coroutine cooldown = null;
	
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

    /*[PunRPC]
    public void PrimaryFire()
    {
        if (!onCooldown)
        {
            Projectile prj = Instantiate(primaryProjectile, firePoint.position, Quaternion.identity);
            prj.gameObject.transform.eulerAngles = firePoint.eulerAngles;
            prj.Fire();
            cooldown = StartCoroutine(Cooldown(primaryFireCooldown));
        }
    }*/

    /*[PunRPC]
    public void SecondaryFire()
    {
        if (!onCooldown)
        {
            Projectile prj = Instantiate(secondaryProjectile, firePoint.position, Quaternion.identity);
            prj.gameObject.transform.eulerAngles = firePoint.eulerAngles;
            prj.Fire();
            cooldown = StartCoroutine(Cooldown(secondaryFireCooldown));
        }
    }*/

    public void Cooldown(float cd)
    {
        cooldown = StartCoroutine(ICooldown(cd));
    }

    private IEnumerator ICooldown(float cd)
    {
        onCooldown = true;
        yield return new WaitForSeconds(cd);
        onCooldown = false;
    }

    public void Show(bool show)
    {
        foreach (MeshRenderer rndr in modelElements)
        {
            rndr.enabled = show;
        }
    }
}
