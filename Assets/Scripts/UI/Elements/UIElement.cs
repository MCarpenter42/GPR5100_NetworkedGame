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

public class UIElement : Core, IPointerEnterHandler, IPointerExitHandler
{
    #region [ OBJECTS ]

    

    #endregion

    #region [ PROPERTIES ]

    public RectTransform rTransform { get { return gameObject.GetComponent<RectTransform>(); } }

    [Header("UI Element Properties")]
    public UnityEvent onShow;
    public UnityEvent onHide;

    #endregion

    #region [ COROUTINES ]

    [HideInInspector] public Coroutine colourTransition = null;
    [HideInInspector] public Coroutine colourFlash = null;

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    #region [ BUILT-IN UNITY FUNCTIONS ]

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public virtual void OnPointerEnter(PointerEventData pointerEventData)
    {

    }
    
    public virtual void OnPointerExit(PointerEventData pointerEventData)
    {

    }

    #endregion

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public void SetShown(bool show)
    {
        if (show)
        {
            gameObject.SetActive(true);
            onShow.Invoke();
        }
        else
        {
            onHide.Invoke();
            gameObject.SetActive(false);
        }
    }

    public Coroutine ColourTransition(Graphic graphic, Color clrEnd, float duration)
    {
        return ColourTransition(graphic, graphic.color, clrEnd, duration, false);
    }
    
    public Coroutine ColourTransition(Graphic graphic, Color clrEnd, float duration, bool realTime)
    {
        return ColourTransition(graphic, graphic.color, clrEnd, duration, realTime);
    }
    
    public Coroutine ColourTransition(Graphic graphic, Color clrStart, Color clrEnd, float duration)
    {
        return ColourTransition(graphic, clrStart, clrEnd, duration, false);
    }
    
    public Coroutine ColourTransition(Graphic graphic, Color clrStart, Color clrEnd, float duration, bool realTime)
    {
        if (colourTransition != null)
        {
            StopCoroutine(colourTransition);
        }
        colourTransition = StartCoroutine(IColourTransition(graphic, clrStart, clrEnd, duration, realTime));
        return colourTransition;
    }

    public IEnumerator IColourTransition(Graphic graphic, Color clrStart, Color clrEnd, float duration, bool realTime)
    {
        float timePassed = 0.0f;
        while (timePassed <= duration)
        {
            yield return null;
            timePassed += ITime.DeltaTime(realTime);
            float delta = timePassed / duration;
            graphic.color = Color.Lerp(clrStart, clrEnd, delta);
        }
        graphic.color = clrEnd;
    }

    public Coroutine ColourFlash(Graphic graphic, Color clrPeak, float duration)
    {
        if (colourTransition != null)
        {
            StopCoroutine(colourTransition);
        }
        if (colourFlash != null)
        {
            StopCoroutine(colourFlash);
        }
        colourFlash = StartCoroutine(IColourFlash(graphic, graphic.color, clrPeak, duration, false));
        return colourFlash;
    }
    
    public Coroutine ColourFlash(Graphic graphic, Color clrPeak, float duration, bool realTime)
    {
        if (colourTransition != null)
        {
            StopCoroutine(colourTransition);
        }
        if (colourFlash != null)
        {
            StopCoroutine(colourFlash);
        }
        colourFlash = StartCoroutine(IColourFlash(graphic, graphic.color, clrPeak, duration, realTime));
        return colourFlash;
    }
    
    public Coroutine ColourFlash(Graphic graphic, Color clrBase, Color clrPeak, float duration)
    {
        if (colourTransition != null)
        {
            StopCoroutine(colourTransition);
        }
        if (colourFlash != null)
        {
            StopCoroutine(colourFlash);
        }
        colourFlash = StartCoroutine(IColourFlash(graphic, clrBase, clrPeak, duration, false));
        return colourFlash;
    }
    
    public Coroutine ColourFlash(Graphic graphic, Color clrBase, Color clrPeak, float duration, bool realTime)
    {
        if (colourTransition != null)
        {
            StopCoroutine(colourTransition);
        }
        if (colourFlash != null)
        {
            StopCoroutine(colourFlash);
        }
        colourFlash = StartCoroutine(IColourFlash(graphic, clrBase, clrPeak, duration, realTime));
        return colourFlash;
    }

    public IEnumerator IColourFlash(Graphic graphic, Color clrBase, Color clrPeak, float duration, bool realTime)
    {
        float halfDur = duration / 2.0f;
        ColourTransition(graphic, clrBase, clrPeak, halfDur, realTime);
        yield return ITime.Wait(halfDur, realTime);
        ColourTransition(graphic, clrPeak, clrBase, halfDur, realTime);
    }
}
