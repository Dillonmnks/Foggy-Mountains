using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public float countDuration = 0.4f;
    public AnimationCurve countEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public float punchScale = 1.3f;
    public float punchDuration = 0.25f;
    public AnimationCurve punchEase = new AnimationCurve(
        new Keyframe(0, 0),
        new Keyframe(0.5f, 1.2f),
        new Keyframe(1, 1)
    );

    public Color flashColor = Color.white;
    public float flashDuration = 0.2f;

    public float rainbowDuration = 1.2f;
    public float rainbowCyclesPerSecond = 1.5f;

    private int displayedScore;
    private int targetScore;
    private Color baseColor;
    private Coroutine countRoutine;
    private Coroutine punchRoutine;
    private Coroutine flashRoutine;
    private Coroutine rainbowRoutine;

    void Awake()
    {
        baseColor = scoreText.color;
        UpdateText();
    }

    public void AddScore(int amount)
    {
        targetScore += amount;

        if (countRoutine != null) StopCoroutine(countRoutine);
        countRoutine = StartCoroutine(CountUpRoutine());

        if (punchRoutine != null) StopCoroutine(punchRoutine);
        punchRoutine = StartCoroutine(PunchRoutine());

        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    public void PlayMilestoneEffect()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        if (rainbowRoutine != null) StopCoroutine(rainbowRoutine);
        rainbowRoutine = StartCoroutine(RainbowRoutine());
    }

    private IEnumerator RainbowRoutine()
    {
        float t = 0f;
        while (t < rainbowDuration)
        {
            t += Time.deltaTime;
            float hue = Mathf.Repeat(t * rainbowCyclesPerSecond, 1f);
            scoreText.color = Color.HSVToRGB(hue, 1f, 1f);
            yield return null;
        }
        scoreText.color = baseColor;
    }

    private IEnumerator CountUpRoutine()
    {
        int startScore = displayedScore;
        float t = 0f;

        while (t < countDuration)
        {
            t += Time.deltaTime;
            float eased = countEase.Evaluate(Mathf.Clamp01(t / countDuration));
            displayedScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, eased));
            UpdateText();
            yield return null;
        }

        displayedScore = targetScore;
        UpdateText();
    }

    private IEnumerator PunchRoutine()
    {
        float t = 0f;
        while (t < punchDuration)
        {
            t += Time.deltaTime;
            float scale = 1f + (punchScale - 1f) * punchEase.Evaluate(t / punchDuration);
            scoreText.transform.localScale = Vector3.one * scale;
            yield return null;
        }
        scoreText.transform.localScale = Vector3.one;
    }

    private IEnumerator FlashRoutine()
    {
        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            scoreText.color = Color.Lerp(flashColor, baseColor, t / flashDuration);
            yield return null;
        }
        scoreText.color = baseColor;
    }

    private void UpdateText()
    {
        scoreText.text = displayedScore.ToString();
    }
}