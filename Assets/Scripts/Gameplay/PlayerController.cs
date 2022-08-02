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



    #endregion

    #region [ PROPERTIES ]

    [Header("Components")]
    [SerializeField] Transform body;
    [SerializeField] Transform cam;
    [SerializeField] Transform pitchPivot;
    [SerializeField] Transform yawRotator;

    [HideInInspector] public Rigidbody rb { get { return gameObject.GetOrAddComponent<Rigidbody>(); } }

    [Header("Player Properties")]
    public Color colour = Color.white;
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] float rotFactor = 5.0f;
    [Range(-15.0f, 15.0f)]
    [SerializeField] float minLookAngle = -15.0f;
    [Range(15.0f, 45.0f)]
    [SerializeField] float maxLookAngle = 30.0f;

    public bool isFocusPlayer = false;

    [HideInInspector] public Vector3 direction = Vector3.zero;
    [HideInInspector] public float dirRot = 0.0f;

    private float camPitch = 0.0f;
    private float camYaw = 0.0f;
    private Vector3 camOffset = new Vector3();

    [Header("Combat")]
    [Range(1, 10)]
    [SerializeField] int maxHealth;
    [HideInInspector] public int currentHealth;
    [SerializeField] Weapon[] weapons = new Weapon[1];
    public int activeWeapon = 0;

    #endregion

    #region [ COROUTINES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    void Awake()
    {
        if (isFocusPlayer)
        {
            camOffset = cam.position - pitchPivot.position;
        }
        else
        {
            cam.gameObject.SetActive(false);
        }
        currentHealth = maxHealth;
    }

    void Start()
    {
        if (isFocusPlayer)
        {
            LockCursor(true);
        }
    }

    void Update()
    {
        if (isFocusPlayer)
        {
            GetDirection();
            Rotate();
            CameraDist();

            if (GetInput(Controls.Shooting.PrimaryFire))
            {
                weapons[activeWeapon].PrimaryFire();
            }
            else if (GetInput(Controls.Shooting.SecondaryFire))
            {
                if (weapons[activeWeapon].hasSecondaryFire)
                {
                    weapons[activeWeapon].PrimaryFire();
                }
                else
                {
                    weapons[activeWeapon].SecondaryFire();
                }
            }
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    #endregion

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
        }

        if (direction.magnitude > 0.0f)
        {
            float yRotLocal = 0.0f;
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
            if (body.localEulerAngles.y != yRotLocal)
            {
                float angle = (yRotLocal - body.localEulerAngles.y).WrapClamp(-180.0f, 180.0f);
                float bodyRotScale = 80.0f * Time.deltaTime;
                angle = Mathf.Clamp(angle, -bodyRotScale, bodyRotScale);
                bodyRot.y += angle;
            }
        }

        body.eulerAngles = bodyRot;

        /*if (direction.magnitude > 0.0f)
        {
            yawRotator.transform.LookAt(transform.TransformDirection(direction));
            float angle = Vector3.Angle(body.transform.forward, yawRotator.transform.forward);
            Debug.Log(angle);
            float bodyRotScale = 60.0f * Time.deltaTime;
            angle = Mathf.Clamp(angle, 0.0f, bodyRotScale);
            if (body.InverseTransformDirection(yawRotator.transform.forward).x < 0.0f)
            {
                angle *= -1.0f;
            }
            bodyRot.y += angle;
        }
        body.eulerAngles = bodyRot;*/
    }

    private void CameraDist()
    {
        Vector3 dir = camOffset.normalized;
        float dist = camOffset.magnitude;
        if (Physics.Raycast(pitchPivot.transform.position, pitchPivot.transform.TransformDirection(dir), out RaycastHit hit, dist + 0.25f))
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

}
