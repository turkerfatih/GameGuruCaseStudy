using System.Collections;
using Case2;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float SuccessRotationSpeed=30;
    public CinemachineVirtualCamera PlayCamera;
    public CinemachineVirtualCamera SuccessCamera;

    private CinemachineOrbitalTransposer orbitalTransposer;
    private readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    private float angle;
    private bool rotate;
    private void Start()
    {
        orbitalTransposer = SuccessCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void OnEnable()
    {
        EventBus.OnGameSuccess += OnSuccess;
        EventBus.OnContinue += OnContinueNewLevel;
    }

    private void OnDisable()
    {
        EventBus.OnGameSuccess += OnSuccess;
    }

    private void OnSuccess()
    {
        PlayCamera.gameObject.SetActive(false);
        StartCoroutine(Rotate());
    }

    private void OnContinueNewLevel()
    {
        PlayCamera.gameObject.SetActive(true);
        rotate = false;
    }

    IEnumerator Rotate()
    {
        rotate = true;
        angle = orbitalTransposer.m_Heading.m_Bias;
        while (rotate)
        {
            angle += SuccessRotationSpeed * Time.deltaTime;
            var val = Mathf.Repeat(angle, 360);
            var ratio=Mathf.InverseLerp(0, 360, val);
            orbitalTransposer.m_Heading.m_Bias = Mathf.Lerp(-180, 180, ratio);
            yield return waitForEndOfFrame;
        }
    }
}
