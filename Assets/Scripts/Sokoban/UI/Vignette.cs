using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Vignette : MonoBehaviour
{
    public enum VignetteState
    {
        NONE = 0,
        OPENING = 1,
        CLOSING = 2
    }

    [System.Serializable]
    public class Configuration
    {
        public Vector2 circlePosition;
        public Vector2 squarePosition;
        public Vector2 squareSize;
        public Vector2 dimensions;
    }
    [SerializeField] List<Configuration> configurations = new List<Configuration>();
    [SerializeField] Material vignetteMaterial;

    [SerializeField] float vignetteCounter;
    [SerializeField] float vignetteCounterMax;
    [SerializeField] AnimationCurve vignetteSpeed;

    Action beforeOpen;
    Action afterOpen;
    VignetteState vignetteState;
    int savedIndex;

    private void Awake()
    {
        vignetteMaterial.SetFloat("_Size", 100);
    }

    public void Update()
    {
        if (vignetteState == VignetteState.OPENING && vignetteCounter < vignetteCounterMax)
        {
            //Open quickly at start
            //Slow at end
            vignetteCounter += Time.unscaledDeltaTime;
            float percentage = vignetteCounter / vignetteCounterMax;
            float sizeValue = vignetteSpeed.Evaluate(percentage) * 100;
            vignetteMaterial.SetFloat("_Size", sizeValue);
            if (vignetteCounter >= vignetteCounterMax)
            {
                vignetteCounter = 0;
                vignetteMaterial.SetFloat("_Size", 100);
                afterOpen?.Invoke();
                vignetteState = VignetteState.NONE;
            }
        }
        else if (vignetteState == VignetteState.CLOSING && vignetteCounter < vignetteCounterMax)
        {
            vignetteCounter += Time.unscaledDeltaTime;
            float percentage = 1 - vignetteCounter / vignetteCounterMax;
            float sizeValue = vignetteSpeed.Evaluate(percentage) * 100;
            vignetteMaterial.SetFloat("_Size", sizeValue);
            if (vignetteCounter >= vignetteCounterMax)
            {
                beforeOpen?.Invoke();
                vignetteCounter = 0;
                vignetteState = VignetteState.OPENING;
                vignetteMaterial.SetVector("_CirclePosition", configurations[savedIndex].circlePosition);
                vignetteMaterial.SetVector("_SquarePosition", configurations[savedIndex].squarePosition);
                vignetteMaterial.SetVector("_SquareSize", configurations[savedIndex].squareSize);
                GetComponent<RectTransform>().sizeDelta = configurations[savedIndex].dimensions;
            }
        }
    }

    public void CloseAndOpen(Action beforeOpen, Action afterOpen, int index = 0)
    {
        if(vignetteState != VignetteState.NONE) { return; }
        Time.timeScale = 0;
        this.beforeOpen = beforeOpen;
        this.afterOpen = afterOpen;
        savedIndex = index;
        vignetteState = VignetteState.CLOSING;
    }
    public void Open(Action afterOpen, int index = 1)
    {
        Time.timeScale = 0;
        vignetteState = VignetteState.OPENING;
        vignetteMaterial.SetFloat("_Size", 0);
        this.afterOpen = afterOpen;
    }
    public void FadeToBlack(Action onFinish)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.OnComplete(onFinish.Invoke);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
}