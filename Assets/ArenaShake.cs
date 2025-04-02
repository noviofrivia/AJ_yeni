using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShakeExample : MonoBehaviour
{
    public float duration = 0.3f;
    public float strength = 0.1f;
    public int vibrato = 5;
    public float randomness = 90f;
    public bool snapping = false;
    public bool fadeOut = true;

    private Vector3 originalPosition;

    void Start()
    {
        // Shake the object's position
        // ShakeObject();
        originalPosition = transform.position;

        // You can also shake rotation
        // transform.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut);

        // Or scale
        // transform.DOShakeScale(duration, strength, vibrato, randomness, fadeOut);
    }

    // Call this method to trigger the shake
    public void ShakeObject()
    {
        transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut)
            .OnComplete(() => {
                // Reset position after shake completes
                transform.position = originalPosition;
            });
    }
}
