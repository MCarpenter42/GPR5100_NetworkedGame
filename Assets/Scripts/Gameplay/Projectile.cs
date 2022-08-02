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

public class Projectile : Core
{
    #region [ OBJECTS ]

    [Header("Components")]
    [SerializeField] ProjectilePropulsion propulsion;
    public Rigidbody rb { get { return gameObject.GetComponent<Rigidbody>(); } }

    #endregion

    #region [ PROPERTIES ]

    [Header("Properties")]
    [SerializeField] float primingDelay = 0.25f;
    [SerializeField] float lifeTime = 5.0f;
    [SerializeField] Vector3 propulsionForce;
    private bool primed = false;
    [SerializeField] bool useCustomGravity;
    [Range(0.0f, 20.0f)]
    [SerializeField] float customGravity = 0.0f;

    [Header("Effects")]
    [Range(1, 10)]
    [SerializeField] int damage = 1;
    [SerializeField] bool explode;
    [SerializeField] Explosion explosion;
    [Range(0.0f, 10.0f)]
    [SerializeField] float explosionRadius = 1.0f;

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    void Awake()
    {
        if (lifeTime <= primingDelay)
        {
            lifeTime = primingDelay + 2.0f;
        }
        propulsion.force = propulsionForce;
        rb.useGravity = !useCustomGravity;
        StartCoroutine(Prime());
        StartCoroutine(Lifetime());
    }

    private void FixedUpdate()
    {
        if (useCustomGravity)
        {
            rb.AddForce(new Vector3(0.0f, -customGravity, 0.0f), ForceMode.Acceleration);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (primed)
        {
            OnHit(collision);
            Despawn(0.05f);
        }
    }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    private IEnumerator Prime()
    {
        yield return new WaitForSeconds(primingDelay);
        primed = true;
    }
    
    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        Despawn(0.0f);
    }

    private void Despawn(float delay)
    {
        Destroy(gameObject, delay);
    }

    private void OnHit(Collision collision)
    {
        if (explode)
        {
            Explosion xpl = Instantiate(explosion, GameManager.WorldSpace.transform, true);
            ContactPoint hit = collision.GetContact(0);
            xpl.transform.position = hit.point;
            xpl.damage = damage;
            xpl.radius = explosionRadius;
            xpl.Explode();
        }
        else
        {
            if (collision.gameObject.CompareTag("Player"))
            {

            }
        }
    }
}
