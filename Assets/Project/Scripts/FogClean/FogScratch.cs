using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FogScratch : MonoBehaviour
{
    [Header("Setup")]
    public Texture2D baseTexture;
    public int brushRadius = 20;
    public float dragStepSize = 8f;
    [Range(0f, 1f)] public float clearThreshold = 0.8f;

    private Texture2D clonedTexture;
    private Image uiImage;
    private Color32[] pixels;
    private Color32[] originalPixels;
    private static Color32 Clear32 = new Color32(0, 0, 0, 0);

    private int width, height, totalPixels, clearedPixels;
    private bool hasLastPoint;
    private Vector2 lastLocalPoint;

    public float ScratchedPercentage => totalPixels == 0 ? 0f : (float)clearedPixels / totalPixels;

    void Awake()
    {
        uiImage = GetComponent<Image>();
        clonedTexture = Instantiate(baseTexture);

        width = clonedTexture.width;
        height = clonedTexture.height;
        totalPixels = width * height;

        originalPixels = clonedTexture.GetPixels32();
        pixels = new Color32[originalPixels.Length];
        Array.Copy(originalPixels, pixels, pixels.Length);

        uiImage.sprite = Sprite.Create(
            clonedTexture,
            new Rect(0, 0, width, height),
            Vector2.one * 0.5f
        );
    }

    void OnEnable()
    {
        ResetScratch();
    }

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            hasLastPoint = false;
        }

        if (mouse.leftButton.isPressed)
        {
            TryScratchAtScreenPoint(mouse.position.ReadValue());
        }
        else
        {
            hasLastPoint = false;
        }
    }

    public void ResetScratch() // yo dilly, use this shit to reset the Fag
    {
        Array.Copy(originalPixels, pixels, pixels.Length);
        clearedPixels = 0;
        hasLastPoint = false;

        clonedTexture.SetPixels32(pixels);
        clonedTexture.Apply(false, false);
    }

    private void TryScratchAtScreenPoint(Vector2 screenPoint)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                uiImage.rectTransform, screenPoint, null, out Vector2 localPoint))
        {
            return;
        }

        bool changed;

        if (hasLastPoint)
        {
            float dist = Vector2.Distance(lastLocalPoint, localPoint);
            int steps = Mathf.Max(1, Mathf.CeilToInt(dist / dragStepSize));

            changed = false;
            for (int s = 1; s <= steps; s++)
            {
                Vector2 lerped = Vector2.Lerp(lastLocalPoint, localPoint, (float)s / steps);
                changed |= ClearCircleAtLocalPoint(lerped);
            }
        }
        else
        {
            changed = ClearCircleAtLocalPoint(localPoint);
        }

        lastLocalPoint = localPoint;
        hasLastPoint = true;

        if (changed)
        {
            clonedTexture.SetPixels32(pixels);
            clonedTexture.Apply(false, false);
            CheckIfCleared();
        }
    }

    private void CheckIfCleared()
    {
        if (ScratchedPercentage >= clearThreshold)
        {
            gameObject.SetActive(false);
        }
    }

    private bool ClearCircleAtLocalPoint(Vector2 localPoint)
    {
        Vector2 normalized = Rect.PointToNormalized(uiImage.rectTransform.rect, localPoint);

        int centerX = Mathf.Clamp((int)(normalized.x * width), 0, width - 1);
        int centerY = Mathf.Clamp((int)(normalized.y * height), 0, height - 1);

        int minX = Mathf.Max(0, centerX - brushRadius);
        int maxX = Mathf.Min(width - 1, centerX + brushRadius);
        int minY = Mathf.Max(0, centerY - brushRadius);
        int maxY = Mathf.Min(height - 1, centerY + brushRadius);

        int radiusSqr = brushRadius * brushRadius;
        bool changedAny = false;

        for (int y = minY; y <= maxY; y++)
        {
            int dy = y - centerY;
            int rowOffset = y * width;

            for (int x = minX; x <= maxX; x++)
            {
                int dx = x - centerX;
                if (dx * dx + dy * dy > radiusSqr) continue;

                int index = rowOffset + x;
                if (pixels[index].a != 0)
                {
                    pixels[index] = Clear32;
                    clearedPixels++;
                    changedAny = true;
                }
            }
        }

        return changedAny;
    }

}