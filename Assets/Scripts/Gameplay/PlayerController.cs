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

public class PlayerController : Core
{
    #region [ OBJECTS ]

    [Header("Components")]
    [SerializeField] Transform body;
    [SerializeField] Transform cam;
    [SerializeField] Transform pitchPivot;
    [SerializeField] MeshRenderer[] modelElements;
    public Nameplate nameplate;

    [HideInInspector] public Rigidbody rb { get { return gameObject.GetOrAddComponent<Rigidbody>(); } }
    [HideInInspector] public Collider coll { get { return gameObject.GetOrAddComponent<Collider>(); } }
    private PhotonView view { get { return gameObject.GetComponent<PhotonView>(); } }

    #endregion

    #region [ PROPERTIES ]

    #region < Player Properties >

    [Header("Player Properties")]
    public Color colour = Color.white;
    [HideInInspector] public string playerName;
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] float rotFactor = 5.0f;
    [Range(-30.0f, 30.0f)]
    [SerializeField] float minLookAngle = -15.0f;
    [Range(30.0f, 90.0f)]
    [SerializeField] float maxLookAngle = 30.0f;

    [HideInInspector] public Vector3 direction = Vector3.zero;
    [HideInInspector] public float dirRot = 0.0f;

    private float camPitch = 0.0f;
    private float camYaw = 0.0f;
    private Vector3 camOffset = new Vector3();

    #endregion

    #region < Combat >

    [Header("Combat")]
    [Range(1, 40)]
    public int maxHealth = 10;
    [HideInInspector] public int currentHealth;
    [SerializeField] Weapon[] weapons = new Weapon[1];
    public int activeWeapon = 0;

    [HideInInspector] public bool alive = true;
    [HideInInspector] public bool damageCooldown = false;
    [HideInInspector] public bool canBeDamaged = true;
    [HideInInspector] public bool canBeHealed = true;
    [HideInInspector] public bool canShoot = true;

    [HideInInspector] public int kills = 0;

    #endregion

    #endregion

    #region [ COROUTINES ]

    private Coroutine DamageCooldown = null;

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    void Awake()
    {
        currentHealth = maxHealth;
        camOffset = cam.localPosition;
        if (nameplate != null)
        {
            nameplate.SetText(playerName);
            nameplate.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    void Start()
    {
        if (view.IsMine)
        {
            LockCursor(true);
        }
    }

    void Update()
    {
        if (view.IsMine)
        {
            Inputs();
        }
    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            Move();
        }
    }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */
    
    public void Inputs()
    {
        if (alive)
        {
            if (!GameManager.UIHandler.escMenu.framesVisible)
            {
                GetDirection();
                Rotate();
                Shooting();
                CameraDist();
            }
        }
    }

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */
    
    private void GetDirection()
    {
        Vector3 dir = Vector3.zero;
        if (GetInput(Controls.Movement.Forward))
        {
            dir.z += 1.0f;
        }
        if (GetInput(Controls.Movement.Right))
        {
            dir.x += 1.0f;
        }
        if (GetInput(Controls.Movement.Backward))
        {
            dir.z -= 1.0f;
        }
        if (GetInput(Controls.Movement.Left))
        {
            dir.x -= 1.0f;
        }
        if (dir.magnitude > 0.0f)
        {
            dir = dir.normalized;
        }
        direction = dir * 100.0f;
    }

    private void Move()
    {
        rb.AddRelativeForce(direction * moveSpeed * Time.deltaTime, ForceMode.Force);
    }

    private void Rotate()
    {
        Vector3 bodyRot = body.eulerAngles;
        bodyRot.y = bodyRot.y.WrapClamp(0.0f, 360.0f);

        if (GameManager.isCursorLocked)
        {
            if (GameManager.isCursorLocked)
            {
                camPitch += rotFactor * Input.GetAxis("Mouse Y");
                camYaw += rotFactor * Input.GetAxis("Mouse X");
            }

            // Clamp pitch
            camPitch = Mathf.Clamp(camPitch, minLookAngle, maxLookAngle);

            // Wrap yaw
            camYaw = camYaw.WrapClamp(0.0f, 360.0f);

            transform.eulerAngles = new Vector3(0.0f, camYaw, 0.0f);
            pitchPivot.localEulerAngles = new Vector3(-camPitch, 0.0f, 0.0f);
            float displayAngle = pitchPivot.localEulerAngles.x;
            if (displayAngle != 0.0f)
            {
                displayAngle = 360.0f - displayAngle;
            }
            GameManager.UIHandler.HUD.weaponElevation.DoUpdate(displayAngle, 90.0f, false);
        }

        if (direction.magnitude > 0.0f)
        {
            float yRotLocal = 0.0f;

            if (direction.magnitude > 0.0f)
            {
                if (direction.x > 0.0f)
                {
                    if (direction.z > 0.0f)
                    {
                        yRotLocal = 45.0f;
                    }
                    else if (direction.z < 0.0f)
                    {
                        yRotLocal = 135.0f;
                    }
                    else
                    {
                        yRotLocal = 90.0f;
                    }
                }
                else if (direction.x < 0.0f)
                {
                    if (direction.z > 0.0f)
                    {
                        yRotLocal = 315.0f;
                    }
                    else if (direction.z < 0.0f)
                    {
                        yRotLocal = 225.0f;
                    }
                    else
                    {
                        yRotLocal = 270.0f;
                    }
                }
                else
                {
                    if (direction.z < 0.0f)
                    {
                        yRotLocal = 180.0f;
                    }
                }
            }
            {/*else if (rb.velocity.magnitude > 0.1f)
            {
                Debug.Log(rb.velocity.magnitude);
                yRotLocal = rb.velocity.AngleFromAxis(DualAxis.XZ, true).WrapClamp(0.0f, 360.0f);
                {
                *//*Vector3 vel = transform.InverseTransformDirection(rb.velocity);
                if (vel.x > 0.0f)
                {
                    if (vel.z > 0.0f)
                    {
                        yRotLocal = 45.0f;
                    }
                    else if (vel.z < 0.0f)
                    {
                        yRotLocal = 135.0f;
                    }
                    else
                    {
                        yRotLocal = 90.0f;
                    }
                }
                else if (vel.x < 0.0f)
                {
                    if (vel.z > 0.0f)
                    {
                        yRotLocal = 315.0f;
                    }
                    else if (vel.z < 0.0f)
                    {
                        yRotLocal = 225.0f;
                    }
                    else
                    {
                        yRotLocal = 270.0f;
                    }
                }
                else
                {
                    if (vel.z < 0.0f)
                    {
                        yRotLocal = 180.0f;
                    }
                }*//*
                }
            }*/}
            
            if (body.localEulerAngles.y != yRotLocal)
            {
                float angle = (yRotLocal - body.localEulerAngles.y).WrapClamp(-180.0f, 180.0f);
                float bodyRotScale = 270.0f * Time.deltaTime;
                angle = Mathf.Clamp(angle, -bodyRotScale, bodyRotScale);
                bodyRot.y += angle;
            }
        }

        body.eulerAngles = bodyRot;
    }

    private void Shooting()
    {
        if (canShoot)
        {
            if (GetInput(Controls.Shooting.PrimaryFire) || (GetInput(Controls.Shooting.SecondaryFire) && !weapons[activeWeapon].hasSecondaryFire))
            {
                view.RPC("PrimaryFire", RpcTarget.All);
            }
            else if (GetInput(Controls.Shooting.SecondaryFire))
            {
                view.RPC("SecondaryFire", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void PrimaryFire()
    {
        Weapon active = weapons[activeWeapon];
        if (!active.onCooldown)
        {
            Projectile prj = Instantiate(active.primaryProjectile, active.firePoint.position, Quaternion.identity);
            prj.gameObject.transform.eulerAngles = active.firePoint.eulerAngles;
            prj.Fire();
            active.Cooldown(active.primaryFireCooldown);
        }
    }

    [PunRPC]
    public void SecondaryFire()
    {
        Weapon active = weapons[activeWeapon];
        if (!active.onCooldown)
        {
            Projectile prj = Instantiate(active.secondaryProjectile, active.firePoint.position, Quaternion.identity);
            prj.gameObject.transform.eulerAngles = active.firePoint.eulerAngles;
            prj.Fire();
            active.Cooldown(active.secondaryFireCooldown);
        }
    }

    public void CameraActive(bool active)
    {
        cam.gameObject.SetActive(active);
    }

    private void CameraDist()
    {
        Vector3 dir = pitchPivot.transform.TransformDirection(camOffset.normalized);
        dir.y = camOffset.normalized.y;
        float dist = camOffset.magnitude;
        if (Physics.Raycast(pitchPivot.transform.position, dir, out RaycastHit hit, dist + 0.25f))
        {
            Vector3 hitOffset = pitchPivot.transform.InverseTransformPoint(hit.point);
            cam.transform.localPosition = hitOffset.normalized * (hitOffset.magnitude - 0.25f);
        }
        else
        {
            cam.transform.localPosition = camOffset;
        }
    }

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public void Damage(int damage)
    {
        if (canBeDamaged && !damageCooldown)
        {
            if (currentHealth >= damage)
            {
                currentHealth -= damage;
            }
            else
            {
                currentHealth = 0;
            }
            OnDamaged(damage);
        }
    }
    
    public void Heal(int healing)
    {
        if (canBeHealed)
        {
            if (currentHealth <= (maxHealth - healing))
            {
                currentHealth += healing;
            }
            else
            {
                currentHealth = maxHealth;
            }
            OnHealed(healing);
        }
    }

    private void OnDamaged(int damage)
    {
        StartCoroutine(IDamageCooldown());
        if (view.IsMine)
        {
            GameManager.UIHandler.HUD.healthBar.DoUpdate(currentHealth, maxHealth, 0);
            SyncHealth(currentHealth);

            if (currentHealth == 0)
            {
                view.RPC("OnDeath", RpcTarget.All);
            }
        }
        OnHealthChange();
    }

    private void OnHealed(int healing)
    {
        if (view.IsMine)
        {
            GameManager.UIHandler.HUD.healthBar.DoUpdate(currentHealth, maxHealth, 1);
            SyncHealth(currentHealth);
        }
        OnHealthChange();
    }

    private void OnHealthChange()
    {
        nameplate.UpdateHealthBar(currentHealth, maxHealth);
    }

    private IEnumerator IDamageCooldown()
    {
        damageCooldown = true;
        int waitCount = (int)(0.1f / Time.fixedDeltaTime);
        for (int i = 0; i < waitCount; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        damageCooldown = false;
    }

    [PunRPC]
    private void SyncHealth(int health)
    {
        currentHealth = health;
    }

    [PunRPC]
    private void OnDeath()
    {
        alive = false;
        PhysicalPresence(false);
        if (view.IsMine)
        {
            HUDElements(false, false, false, true);
        }
        else
        {
            nameplate.Show(false);
        }

        Respawn(10.0f);
    }

    public void Respawn(float respawnTimer)
    {
        StartCoroutine(IRespawn(respawnTimer));
    }

    private IEnumerator IRespawn(float respawnTimer)
    {
        float timePassed = 0.0f;
        while (timePassed <= respawnTimer)
        {
            yield return null;
            timePassed += Time.deltaTime;
            GameManager.UIHandler.HUD.respawnTimer.DoUpdate(respawnTimer - timePassed);
        }

        GameManager.UIHandler.blackScreen.SetActive(true);

        alive = true;
        transform.position = GameManager.PlayerManager.GetSpawnPoint();
        if (view.IsMine)
        {
            HUDElements(true, true, true, false);
        }
        PhysicalPresence(true);
        Heal(maxHealth);
        if (!view.IsMine)
        {
            nameplate.Show(false);
        }

        GameManager.UIHandler.blackScreen.SetActive(false);
    }

    private void PhysicalPresence(bool active)
    {
        foreach (MeshRenderer rndr in modelElements)
        {
            rndr.enabled = active;
        }
        weapons[activeWeapon].Show(active);
        coll.enabled = active;
        rb.useGravity = active;
    }

    private void HUDElements(bool healthBar, bool elevation, bool fireCooldown, bool respawnTimer)
    {
        GameManager.UIHandler.HUD.ShowHealthBar(healthBar);
        GameManager.UIHandler.HUD.ShowWeaponElevation(elevation);
        //GameManager.UIHandler.HUD.ShowWeaponCooldown(fireCooldown);
        GameManager.UIHandler.HUD.ShowRespawnTimer(respawnTimer);
    }
}
