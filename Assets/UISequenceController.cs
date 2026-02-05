using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISequenceController : MonoBehaviour
{
    public enum TransitionType
    {
        Instant,
        Fade
    }

    [System.Serializable]
    public class SequenceStep
    {
        public GameObject target;

        public bool setActive = true;

        public TransitionType transition = TransitionType.Instant;

        [Tooltip("Used only if Transition = Fade")]
        public float fadeDuration = 0.5f;

        [Header("Auto Off")]
        public bool autoOff = false;

        [Tooltip("How long the object stays ON before turning OFF")]
        public float onDuration = 1f;

        [Tooltip("Delay after this step fully finishes")]
        public float delayAfter = 0.5f;
    }

    public List<SequenceStep> sequence = new List<SequenceStep>();
    public bool playOnStart = true;

    Coroutine runningSequence;

    void Start()
    {
        if (playOnStart)
            PlaySequence();
    }

    public void PlaySequence()
    {
        if (runningSequence != null)
            StopCoroutine(runningSequence);

        runningSequence = StartCoroutine(SequenceRoutine());
    }

    IEnumerator SequenceRoutine()
    {
        foreach (SequenceStep step in sequence)
        {
            if (step.target == null)
                continue;

            // TURN ON OR OFF
            if (step.transition == TransitionType.Instant)
            {
                step.target.SetActive(step.setActive);
            }
            else
            {
                yield return StartCoroutine(Fade(step.target, step.setActive, step.fadeDuration));
            }

            // AUTO OFF LOGIC
            if (step.setActive && step.autoOff)
            {
                yield return new WaitForSeconds(step.onDuration);

                if (step.transition == TransitionType.Instant)
                {
                    step.target.SetActive(false);
                }
                else
                {
                    yield return StartCoroutine(Fade(step.target, false, step.fadeDuration));
                }
            }

            if (step.delayAfter > 0f)
                yield return new WaitForSeconds(step.delayAfter);
        }
    }

    IEnumerator Fade(GameObject target, bool fadeIn, float duration)
    {
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = target.AddComponent<CanvasGroup>();

        target.SetActive(true);

        float startAlpha = canvasGroup.alpha;
        float endAlpha = fadeIn ? 1f : 0f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (!fadeIn)
            target.SetActive(false);
    }
}
