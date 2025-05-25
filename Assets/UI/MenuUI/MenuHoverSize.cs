using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuHoverSize : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Vector3 targetScale;
    private Coroutine scaleCoroutine;
    public float scaleMultiplier = 1.1f;      // 10% larger
    public float scaleSpeed = 5f;             // Speed of the animation

    private void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartScaleCoroutine(originalScale * scaleMultiplier);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartScaleCoroutine(originalScale);
    }

    private void StartScaleCoroutine(Vector3 newScale)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);

        scaleCoroutine = StartCoroutine(ScaleTo(newScale));
    }

    private IEnumerator ScaleTo(Vector3 destination)
    {
        while (Vector3.Distance(transform.localScale, destination) > 0.001f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, destination, Time.unscaledDeltaTime * scaleSpeed);
            yield return null;
        }

        transform.localScale = destination; // Snap to target to avoid overshooting
    }
}
