using MyBox;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhaseDisplayer : MonoBehaviour
{
    [Foldout("Main", true), SerializeField]
    private TMP_Text phaseText;
    [SerializeField]
    private float colorAnimSpeed = 2f;
    [SerializeField]
    private float progressAnimSpeed = 1f;

    [Foldout("Slider", true) ,SerializeField]
    private Slider phaseProgressSlider;
    [SerializeField]
    private Image phaseProgressSliderBG;
    [SerializeField]
    private Image phaseProgressSliderFill;

    [Foldout("Border", true), SerializeField]
    private Image border;

    private Fases currentPhase;
    private int phaseProgress = 0;

    Color32 borderColor;
    Color32 fillColor;

    private Coroutine colorRoutine;
    private Coroutine progressRoutine;


    [ContextMenu("Increment Progress")]
    public void IncrementProgress()
    {
        phaseProgress++;

        if (progressRoutine != null)
            StopCoroutine(progressRoutine);

        progressRoutine = StartCoroutine(AnimateProgress(phaseProgress));
    }

    [ContextMenu("Increment Phase")]
    public void IncrementPhase()
    {
        if (currentPhase < Fases.Amerika)
        {
            currentPhase++;
            SetPhase(currentPhase, 10);
        }
    }

    public void SetPhase(Fases phase, int phaseLength)
    {
        phaseProgressSlider.maxValue = phaseLength;

        phaseProgressSlider.value = 0;
        phaseProgress = 0;

        SetPhaseInfo(phase);

        if (progressRoutine != null)
            StopCoroutine(progressRoutine);

        progressRoutine = StartCoroutine(AnimateProgress(phaseProgress));
    }

    private void SetPhaseInfo(Fases phase)
    {
        string phaseName;

        switch (phase)
        {
            case Fases.Achterhoek:
                phaseName = "De Achterhoek";
                borderColor = new Color32(119, 127, 112, 255);
                fillColor = new Color32(96, 165, 174, 255);
                break;

            case Fases.Pheonix:
                phaseName = "De Overtocht";
                borderColor = new Color32(96, 165, 174, 255);
                fillColor = new Color32(145, 91, 81, 255);
                break;

            case Fases.Amerika:
                phaseName = "Amerika";
                borderColor = new Color32(145, 91, 81, 255);
                fillColor = new Color32(243, 65, 46, 255);
                break;

            default:
                return;
        }

        phaseText.text = phaseName;
        currentPhase = phase;

        if (colorRoutine != null)
            StopCoroutine(colorRoutine);

        colorRoutine = StartCoroutine(AnimateColors());
    }

    private IEnumerator AnimateColors()
    {
        float time = 0f;

        Color startBorder = border.color;
        Color startBG = phaseProgressSliderBG.color;
        Color startFill = phaseProgressSliderFill.color;

        while (time < colorAnimSpeed)
        {
            float t = time / colorAnimSpeed;

            border.color = Color.Lerp(startBorder, borderColor, t);
            phaseProgressSliderBG.color = Color.Lerp(startBG, borderColor, t);
            phaseProgressSliderFill.color = Color.Lerp(startFill, fillColor, t);

            time += Time.deltaTime;
            yield return null;
        }

        // Snap to final values (important!)
        border.color = borderColor;
        phaseProgressSliderBG.color = borderColor;
        phaseProgressSliderFill.color = fillColor;
    }

    private IEnumerator AnimateProgress(float targetValue)
    {
        float time = 0f;
        float startValue = phaseProgressSlider.value;

        while (time < progressAnimSpeed)
        {
            float t = time / progressAnimSpeed;

            phaseProgressSlider.value = Mathf.Lerp(startValue, targetValue, t);

            time += Time.deltaTime;
            yield return null;
        }

        phaseProgressSlider.value = targetValue;
    }
}
