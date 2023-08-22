using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingBar : LoadingProgressDisplay
{
    public bool activateTestAnimation;
    public bool smoothing;

    private Slider slider;
    private float lerpTargetValue;
    private float t = 0.0f;

    private TMPro.TMP_Text msgTxt;

    void Awake()
    {
        // if a long-lived LoadingProgressDisplay has come across from another scene, delete this one
        LoadingProgressDisplay[] loadingObjs = FindObjectsOfType<LoadingProgressDisplay>(true);
        if (loadingObjs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // be ready in case Start() in some client wants to set a value here
        slider = GetComponentInChildren<Slider>();
        msgTxt = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        if (activateTestAnimation)
        {
            StartCoroutine("UpdateTestProgress");
        }
    }

    private void Update()
    {
        if (smoothing)
        {
            slider.value = Mathf.Lerp(slider.value, lerpTargetValue, t);
            t += 0.2f * Time.deltaTime;
        }
    }

    public override void Show()
    {
        this.gameObject.SetActive(true);
    }

    public override void Hide()
    {
        this.gameObject.SetActive(false);
        slider.value = lerpTargetValue = 0; // reset in case we get reused
    }

    IEnumerator UpdateTestProgress()
    {
        float testProgress = 0.0f;
        for (; ; ) { //forever
            testProgress += UnityEngine.Random.Range(0.02f, 0.3f);
            SetProgress(testProgress % 1.0f, $"Loading... ({testProgress % 1.0f * 100:#0.0}%)");
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.9f, 2.0f));
        }
    }

    /// <summary>
    /// Set the current progress of the loading bar.
    /// This is the proper public interface to update the progress.
    /// </summary>
    /// <param name="progress">a normalized value representing current loading bar progress</param>
    public override void SetProgress(float progress)
    {
        if (smoothing)
        {
            t = 0.0f;
            // CroquetLogger.Log($"Loading Bar (Smoothed) Progress Updated to {progress}");
            lerpTargetValue = progress;
        } else
        {
            // CroquetLogger.Log($"Loading Bar (Non-Smoothed) Progress Updated to {progress}");
            slider.value = lerpTargetValue = progress;
        }
    }

    /// <summary>
    /// Set the current progress of the loading bar with a message.
    /// This is the proper public interface to update the progress.
    /// </summary>
    /// <param name="progress">a normalized value representing current loading bar progress</param>
    /// <param name="msg">A message to display on the loading bar</param>
    public override void SetProgress(float progress, string msg) {
        SetMessage(msg);
        SetProgress(progress);
    }

    /// <summary>
    /// Sets the loading message.
    /// This is the proper public interface to update the message.
    /// </summary>
    /// <param name="msg">the string to display</param>
    public override void SetMessage(string msg)
    {
        msgTxt.text = msg;
    }


}
