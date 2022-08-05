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

[ExecuteInEditMode]
public class ValueTracker : UIElement
{
	#region [ ENUMERATION TYPES ]

    private enum Visual { Scale, Rotate, TimeText };
    private enum ScaleDir { X, Y, Both };
    private enum ScaleAnchor { BottomLeft, Centre, TopRight };
    private enum ScaleAnim { None, Slide, Segment };
    private enum RotAnim { None, Slide };

    #endregion

    #region [ OBJECTS ]

    [Header("Components")]
    [SerializeField] RectTransform targetRect;
    [SerializeField] Graphic targetGraphic;
    [SerializeField] TMP_Text label;
    [SerializeField] TMP_Text label_secondary;

	#endregion

	#region [ PROPERTIES ]

    [Header("Tracker Properties")]
    [SerializeField] Visual visualRepres;
    [SerializeField] RectTransform scaleFrom;
    [SerializeField] bool startFull = true;
    [SerializeField] bool maxInLabel = true;

    private Vector2 anchor;
    private Vector2 defaultSize;
    private Vector2 sizeRange = Vector2.zero;
    private float sourceMax;
    private float sourceValue;

    [Header("Scale Settings")]
    [Range(0.0f, 1.0f)]
    [SerializeField] float minScale = 0.0f;
    [SerializeField] ScaleDir scaleDirection;
    [SerializeField] ScaleAnchor scaleAnchor;

    [Header("Rotation Settings")]
    [Range(0.0f, 360.0f)]
    [SerializeField] float minAngle;
    [Range(0.0f, 360.0f)]
    [SerializeField] float maxAngle;
    [SerializeField] RotDirection positiveRotDir;
    [Range(0.0f, 1.0f)]
    [SerializeField] float rotAnchorX = 0.5f;
    [Range(0.0f, 1.0f)]
    [SerializeField] float rotAnchorY = 0.5f;

    [Header("Animation Settings")]
    [SerializeField] Color defaultColour = Color.white;
    [SerializeField] Color[] flashColours;
    [SerializeField] bool doColourFlash = true;
    [SerializeField] ScaleAnim scaleAnim;
    [SerializeField] RotAnim rotAnim;
    [SerializeField] float animDuration = 0.5f;

    #endregion

    #region [ COROUTINES ]

    private Coroutine updateAnim = null;

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    protected override void Awake()
    {
        base.Awake();
        if (targetGraphic != null)
        {
            targetGraphic.color = defaultColour;
        }
        SetAnchor();
        if (Application.isPlaying)
        {
            OnAwakeProperties();
        }
    }

    void OnValidate()
    {
        if (visualRepres != Visual.TimeText)
        {
            if (targetGraphic != null)
            {
                targetGraphic.color = defaultColour;
            }
            SetAnchor();
            targetRect.sizeDelta = GetDefaultSize();
        }
    }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    private Vector2 GetDefaultSize()
    {
        if (scaleFrom != null)
        {
            defaultSize = new Vector2(scaleFrom.rect.width, scaleFrom.rect.height);
        }
        return defaultSize;
    }
    
    private void SetAnchor()
    {
        if (visualRepres == Visual.Scale)
        {
            if (scaleAnchor == ScaleAnchor.BottomLeft)
            {
                anchor = Vector2.zero;
            }
            else if (scaleAnchor == ScaleAnchor.TopRight)
            {
                anchor = Vector2.one;
            }
            else
            {
                anchor = new Vector2(0.5f, 0.5f);
            }
        }
        else if (visualRepres == Visual.Rotate)
        {
            anchor = new Vector2(rotAnchorX, rotAnchorY);
        }

        if (visualRepres != Visual.TimeText)
        {
            targetRect.anchorMin = anchor;
            targetRect.anchorMax = anchor;
            targetRect.pivot = anchor;
            targetRect.anchoredPosition = Vector2.zero;
        }
    }

    private void OnAwakeProperties()
    {
        Vector2 size = new Vector2();
        Vector3 rot = new Vector3();
        if (visualRepres != Visual.TimeText)
        {
            size = GetDefaultSize();
            rot = targetRect.eulerAngles;
        }

        if (visualRepres == Visual.Scale)
        {
            if (scaleDirection == ScaleDir.X)
            {
                sizeRange.x = defaultSize.x - (defaultSize.x * minScale);
                sizeRange.y = 0.0f;
            }
            else if (scaleDirection == ScaleDir.Y)
            {
                sizeRange.x = 0.0f;
                sizeRange.y = defaultSize.y - (defaultSize.y * minScale);
            }
            else
            {
                sizeRange.x = defaultSize.x - (defaultSize.x * minScale);
                sizeRange.y = defaultSize.y - (defaultSize.y * minScale);
            }
            if (!startFull)
            {
                size = defaultSize - new Vector2(defaultSize.x * sizeRange.x, defaultSize.y * sizeRange.y);
            }
            targetRect.sizeDelta = size;
        }
        else if (visualRepres == Visual.Rotate)
        {
            minAngle *= -1.0f;
            maxAngle *= -1.0f;

            if (startFull)
            {
                rot.z = maxAngle;
            }
            else
            {
                rot.z = minAngle;
            }
            targetRect.eulerAngles = rot;
        }
    }

    #region [ DO UPDATE ]

    public void DoUpdate(int sourceValue)
    {
        DoUpdate(sourceValue, true);
    }

    public void DoUpdate(int sourceValue, int sourceMax)
    {
        DoUpdate(sourceValue, sourceMax, true);
    }
    
    public void DoUpdate(int sourceValue, bool doAnim)
    {
        this.sourceValue = (float)sourceValue;
        if (doAnim)
        {
            UpdateAnim();
        }
        else
        {
            UpdateNoAnim();
        }
        if (label != null)
        {
            if (maxInLabel)
            {
                label.text = sourceValue + " / " + (int)sourceMax;
            }
            else
            {
                label.text = sourceValue.ToString();
            }
        }
    }

    public void DoUpdate(int sourceValue, int sourceMax, bool doAnim)
    {
        this.sourceValue = (float)sourceValue;
        this.sourceMax = (float)sourceMax;
        if (doAnim)
        {
            UpdateAnim();
        }
        else
        {
            UpdateNoAnim();
        }
        if (label != null)
        {
            if (maxInLabel)
            {
                label.text = sourceValue + " / " + sourceMax;
            }
            else
            {
                label.text = sourceValue.ToString();
            }
        }
    }
    
    public void DoUpdate(float sourceValue)
    {
        DoUpdate(sourceValue, true);
    }

    public void DoUpdate(float sourceValue, float sourceMax)
    {
        DoUpdate(sourceValue, sourceMax, true);
    }
    
    public void DoUpdate(float sourceValue, bool doAnim)
    {
        this.sourceValue = sourceValue;
        if (doAnim)
        {
            UpdateAnim();
        }
        else
        {
            UpdateNoAnim();
        }
        if (label != null)
        {
            if (maxInLabel)
            {
                label.text = (int)sourceValue + " / " + (int)sourceMax;
            }
            else
            {
                label.text = ((int)sourceValue).ToString();
            }
        }
    }

    public void DoUpdate(float sourceValue, float sourceMax, bool doAnim)
    {
        this.sourceValue = sourceValue;
        this.sourceMax = sourceMax;
        if (doAnim)
        {
            UpdateAnim();
        }
        else
        {
            UpdateNoAnim();
        }
        if (label != null)
        {
            if (maxInLabel)
            {
                label.text = (int)sourceValue + " / " + (int)sourceMax;
            }
            else
            {
                label.text = ((int)sourceValue).ToString();
            }
        }
    }
    
    public void DoUpdate(int sourceValue, long flashColourIndex)
    {
        DoUpdate(sourceValue, true, flashColourIndex);
    }

    public void DoUpdate(int sourceValue, int sourceMax, long flashColourIndex)
    {
        DoUpdate(sourceValue, sourceMax, true, flashColourIndex);
    }
    
    public void DoUpdate(int sourceValue, bool doAnim, long flashColourIndex)
    {
        this.sourceValue = sourceValue;
        if (doAnim)
        {
            UpdateAnim((int)flashColourIndex);
        }
        else
        {
            UpdateNoAnim();
        }
        if (label != null)
        {
            if (maxInLabel)
            {
                label.text = sourceValue + " / " + (int)sourceMax;
            }
            else
            {
                label.text = sourceValue.ToString();
            }
        }
    }

    public void DoUpdate(int sourceValue, int sourceMax, bool doAnim, long flashColourIndex)
    {
        this.sourceValue = sourceValue;
        this.sourceMax = sourceMax;
        if (doAnim)
        {
            UpdateAnim((int)flashColourIndex);
        }
        else
        {
            UpdateNoAnim();
        }
        if (label != null)
        {
            if (maxInLabel)
            {
                label.text = sourceValue + " / " + sourceMax;
            }
            else
            {
                label.text = sourceValue.ToString();
            }
        }
    }
    
    public void DoUpdate(float sourceValue, long flashColourIndex)
    {
        DoUpdate(sourceValue, true, flashColourIndex);
    }

    public void DoUpdate(float sourceValue, float sourceMax, long flashColourIndex)
    {
        DoUpdate(sourceValue, sourceMax, true, flashColourIndex);
    }
    
    public void DoUpdate(float sourceValue, bool doAnim, long flashColourIndex)
    {
        this.sourceValue = sourceValue;
        if (doAnim)
        {
            UpdateAnim((int)flashColourIndex);
        }
        else
        {
            UpdateNoAnim();
        }
        if (label != null)
        {
            if (maxInLabel)
            {
                label.text = (int)sourceValue + " / " + (int)sourceMax;
            }
            else
            {
                label.text = ((int)sourceValue).ToString();
            }
        }
    }

    public void DoUpdate(float sourceValue, float sourceMax, bool doAnim, long flashColourIndex)
    {
        this.sourceValue = sourceValue;
        this.sourceMax = sourceMax;
        if (doAnim)
        {
            UpdateAnim((int)flashColourIndex);
        }
        else
        {
            UpdateNoAnim();
        }
        if (label != null)
        {
            if (maxInLabel)
            {
                label.text = (int)sourceValue + " / " + (int)sourceMax;
            }
            else
            {
                label.text = ((int)sourceValue).ToString();
            }
        }
    }

    #endregion

    #region [ ANIMATION ]

    public void UpdateAnim()
    {
        UpdateAnim(0);
    }
    
    public void UpdateAnim(float animDuration)
    {
        UpdateAnim(animDuration, 0);
    }
    
    public void UpdateAnim(int flashColourIndex)
    {
        updateAnim.Stop();
        if (visualRepres == Visual.Scale)
        {
            updateAnim = StartCoroutine(IUpdateAnim_Scale(animDuration, flashColourIndex));
        }
        else if (visualRepres == Visual.Rotate)
        {
            updateAnim = StartCoroutine(IUpdateAnim_Rotate(animDuration, flashColourIndex));
        }
        else
        {
            UpdateNoAnim_TimeText();
        }
    }
    
    public void UpdateAnim(float animDuration, int flashColourIndex)
    {
        updateAnim.Stop();
        if (visualRepres == Visual.Scale)
        {
            updateAnim = StartCoroutine(IUpdateAnim_Scale(animDuration, flashColourIndex));
        }
        else if (visualRepres == Visual.Rotate)
        {
            updateAnim = StartCoroutine(IUpdateAnim_Rotate(animDuration, flashColourIndex));
        }
        else
        {
            UpdateNoAnim_TimeText();
        }
    }

    private IEnumerator IUpdateAnim_Scale(float animDuration, int flashColourIndex)
    {
        Vector2 sizeStart = targetRect.sizeDelta;
        Vector2 sizeEnd = defaultSize - (sizeRange * (1.0f - (sourceValue / sourceMax)));

        Color flash = Color.white;
        if (flashColourIndex.InBounds(flashColours))
        {
            flash = flashColours[flashColourIndex];
        }
        else if (flashColours.Length > 0)
        {
            flash = flashColours[flashColourIndex];
        }
        if (doColourFlash)
        {
            ColourFlash(targetGraphic, defaultColour, flash, animDuration);
        }

        if (scaleAnim == ScaleAnim.Slide)
        {
            float timePassed = 0.0f;
            while (timePassed <= animDuration)
            {
                yield return null;
                timePassed += Time.deltaTime;
                float delta = timePassed / animDuration;
                targetRect.sizeDelta = Vector2.Lerp(sizeStart, sizeEnd, delta);
            }
        }
        else
        {
            yield return new WaitForEndOfFrame();
        }
        targetRect.sizeDelta = sizeEnd;
    }

    private IEnumerator IUpdateAnim_Rotate(float animDuration, int flashColourIndex)
    {
        float rotStart = targetRect.eulerAngles.z.WrapClamp(-360.0f, 0.0f);
        float rotAngle;
        float rotDiff = maxAngle - minAngle;
        if (positiveRotDir == RotDirection.Clockwise)
        {
            if (rotDiff <= 0.0f)
            {
                rotAngle = rotDiff;
            }
            else
            {
                rotAngle = rotDiff - 360.0f;
            }
        }
        else
        {
            if (rotDiff >= 0.0f)
            {
                rotAngle = rotDiff;
            }
            else
            {
                rotAngle = rotDiff + 360.0f;
            }
        }
        float rotEnd = rotStart + rotAngle;

        Color flash = Color.white;
        if (flashColourIndex.InBounds(flashColours))
        {
            flash = flashColours[flashColourIndex];
        }
        else if (flashColours.Length > 0)
        {
            flash = flashColours[flashColourIndex];
        }
        if (doColourFlash)
        {
            ColourFlash(targetGraphic, defaultColour, flash, animDuration);
        }

        if (rotAnim == RotAnim.Slide)
        {
            float timePassed = 0.0f;
            while (timePassed <= animDuration)
            {
                yield return null;
                timePassed += Time.deltaTime;
                float delta = timePassed / animDuration;
                float rotZ = Mathf.Lerp(rotStart, rotEnd, delta);
                targetRect.eulerAngles = targetRect.eulerAngles.SetAxis(Axis.Z, rotZ);
            }
        }
        else
        {
            yield return new WaitForEndOfFrame();
        }
        targetRect.eulerAngles = targetRect.eulerAngles.SetAxis(Axis.Z, rotEnd);
    }

    private void UpdateNoAnim()
    {
        if (visualRepres == Visual.Scale)
        {
            UpdateNoAnim_Scale();
        }
        else if (visualRepres == Visual.Rotate)
        {
            UpdateNoAnim_Rotate();
        }
        else
        {
            UpdateNoAnim_TimeText();
        }
    }
    
    private void UpdateNoAnim_Scale()
    {
        Vector2 sizeStart = targetRect.sizeDelta;
        Vector2 sizeEnd = defaultSize - (sizeRange * (1.0f - (sourceValue / sourceMax)));
        targetRect.sizeDelta = sizeEnd;
    }
    
    private void UpdateNoAnim_Rotate()
    {
        float rotAngle;
        float rotDiff = maxAngle - minAngle;
        if (positiveRotDir == RotDirection.Clockwise)
        {
            if (rotDiff <= 0.0f)
            {
                rotAngle = rotDiff;
            }
            else
            {
                rotAngle = rotDiff - 360.0f;
            }
        }
        else
        {
            if (rotDiff >= 0.0f)
            {
                rotAngle = rotDiff;
            }
            else
            {
                rotAngle = rotDiff + 360.0f;
            }
        }
        float rotEnd = minAngle + rotAngle * (sourceValue/sourceMax);
        targetRect.eulerAngles = targetRect.eulerAngles.SetAxis(Axis.Z, rotEnd);
    }

    private void UpdateNoAnim_TimeText()
    {
        string[] components = sourceValue.StopwatchTime();
        label.text = components[1];
        label_secondary.text = components[2];
    }

    #endregion
}
