using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeFruit : MonoBehaviour
{
    [SerializeField] public float shakeDuration = 1f;
    [SerializeField] public float shakeAmount;
    [SerializeField] public float decreaseFactor = 1.0f;
    private Vector3 originalPosition;
    private bool isshaking = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isshaking)
        {
            ChangeWallPosition();
        }
    }

    public void ChangeWallPosition()
    {
        if (shakeDuration > 0)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            transform.position = originalPosition;
            isshaking = false;
        }
    }

    public void ShakeButton()
    {
        isshaking = true;
        shakeDuration = 1f;
    }
}
