using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

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
            Projectile proj = Instantiate(primaryProjectile, firePoint, false);
            proj.transform.position = firePoint.position;
            proj.transform.eulerAngles = firePoint.eulerAngles;
            proj.gameObject.transform.SetParent(GameManager.WorldSpace.transform);
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
