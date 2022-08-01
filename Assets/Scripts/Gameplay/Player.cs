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

public class Player : Core
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
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] float rotFactor = 5.0f;
    [Range(-50.0f, 0.0f)]
    [SerializeField] float minLookAngle = -30.0f;
    [Range(0.0f, 50.0f)]
    [SerializeField] float maxLookAngle = 30.0f;

    [HideInInspector] public bool isFocusPlayer = false;

    [HideInInspector] public Vector3 direction = Vector3.zero;

    private float camPitch = 0.0f;
    private float camYaw = 0.0f;
    private Vector3 camOffset = new Vector3();

    #endregion

    #region [ COROUTINES ]



    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    void Awake()
    {
        camOffset = cam.position - pitchPivot.position;
    }

    void Start()
    {
        LockCursor(true);
    }

    void Update()
    {
        GetDirection();
        Rotate();
        CameraDist();
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
        body.eulerAngles = bodyRot;
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
