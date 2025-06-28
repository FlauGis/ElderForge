using UnityEngine;
using UnityEngine.UI;

public class ImageAppearance : MonoBehaviour
{
    public Image image; 
    public float minValue = 0f;
    public float maxValue = 100f;

    [Range(0f, 100f)]
    public float value = 0f;

    public float maxHeight = 500f;
    public Vector2 imageAnchor = new Vector2(0.5f, 0f);

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = image.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, maxHeight);
    }

    void Update()
    {
        float normalizedValue = Mathf.InverseLerp(minValue, maxValue, value);

        float newHeight = Mathf.Lerp(0f, maxHeight, normalizedValue);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);

        rectTransform.anchorMin = new Vector2(imageAnchor.x, imageAnchor.y);
        rectTransform.anchorMax = new Vector2(imageAnchor.x, imageAnchor.y);

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, (normalizedValue - 1) * newHeight / 2);
    }
}
