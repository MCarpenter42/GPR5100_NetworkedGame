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

public class Nameplate : Core
{
	#region [ OBJECTS ]

	[Header("Components")]
	[SerializeField] TextMesh nametag;
	[SerializeField] GameObject healthBar;

	#endregion

	#region [ PROPERTIES ]

	private Vector3 healthBarBaseSize;
    private bool active = true;
    private bool disabled = false;
	
	#endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

	#region [ BUILT-IN UNITY FUNCTIONS ]

    void Awake()
    {
        healthBarBaseSize = healthBar.transform.localScale;
    }

    void Start()
    {
        Show(false);
        if (GameManager.ClientPlayer != null)
        {
            if (transform.parent.gameObject == GameManager.ClientPlayer.gameObject)
            {
                disabled = true;
            }
        }
    }

    void Update()
    {
        if (active)
        {
            Vector3 lookDir = GameManager.ClientPlayer.transform.position - transform.position;
            lookDir.y = 0.0f;
            transform.eulerAngles = Vector3.RotateTowards(transform.forward, lookDir, 100, 100);
        }
    }

	#endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */
	
    public void SetText(string text)
    {
        nametag.text = text;
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        float delta = (float)currentHealth / (float)maxHealth;
        Vector3 newSize = healthBarBaseSize;
        newSize.x *= delta;
        Vector3 offset = Vector3.zero;
        offset.x -= (healthBarBaseSize.x - newSize.x) * 0.5f;
    }

    public void Show(bool show)
    {
        if (disabled)
        {
            Transform[] children = transform.GetChildren();
            foreach (Transform child in children)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            active = show;
            Transform[] children = transform.GetChildren();
            foreach (Transform child in children)
            {
                child.gameObject.SetActive(show);
            }
        }
    }
}
