using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _cameraContainer;

    private float _rotateSemiAmount = 4;
    private float _shakeAmount;

    private Vector3 _startingLocalPos;

    void Start()
    {
        _cameraContainer = GameObject.Find("CameraContainer").transform;

    }


    void Update()
    {
        if (_shakeAmount > 0.01f)
        {
            Vector3 localPosition = _startingLocalPos;
            localPosition.x += _shakeAmount * Random.Range(3, 5);
            localPosition.y += _shakeAmount * Random.Range(3, 5);
            transform.localPosition = localPosition;
            _shakeAmount = 0.9f * _shakeAmount;
        }
    }

    public void SmallShake()
    {
        _shakeAmount = Mathf.Min(0.15f, _shakeAmount + 0.015f);
    }

    public void MediumShake()
    {
        _shakeAmount = Mathf.Min(0.2f, _shakeAmount + 0.02f);
    }

    public void RotateCameraToSide()
    {
        StartCoroutine(RotateCameraToSideRoutine());
    }

    public void RotateCameraToFront()
    {
        StartCoroutine(RotateCameraToFrontRoutine());
    }

    IEnumerator RotateCameraToSideRoutine()
    {
        int frames = 20;
        float increment = _rotateSemiAmount / (float)frames;

        for (int i = 0; i < frames; i++)
        {
            _cameraContainer.RotateAround(Vector3.zero, Vector3.up, increment);
            yield return null;
        }
        yield break;
    }

    IEnumerator RotateCameraToFrontRoutine()
    {
        int frames = 60;
        float increment = _rotateSemiAmount / (float)frames;

        for (int i = 0; i < frames; i++)
        {
            _cameraContainer.RotateAround(Vector3.zero, Vector3.up, -increment);
            yield return null;
        }
        _cameraContainer.localEulerAngles = new Vector3(0, 0, 0);
        yield break;
    }
}

