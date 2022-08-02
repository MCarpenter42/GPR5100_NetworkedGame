using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class Rotator : Core
{
	[SerializeField] Vector3 rotation;
	
    void Update()
    {
        float x = rotation.x * Time.deltaTime;
        float y = rotation.y * Time.deltaTime;
        float z = rotation.z * Time.deltaTime;
        transform.Rotate(x, y, z);
    }
}
