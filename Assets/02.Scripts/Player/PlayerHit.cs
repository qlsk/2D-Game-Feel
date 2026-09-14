using UnityEngine;
using UnityEngine.UI;

public class PlayerHit : MonoBehaviour
{
    [Header("Hit Stop")] [SerializeField] private HitStop _hitStop;

    [SerializeField] private float _hitStopDuration = 0.06f;

    [Header("Camera Shake")] [SerializeField]
    private CameraShake _cameraShake;

    [SerializeField] private float _shakeDuration = 0.15f;

    [SerializeField] private float _shakeStrength = 0.12f;

    [SerializeField] private float _shakeFrequency = 25f;

    [SerializeField] private Toggle _hitStopToggle;

    [SerializeField] private Toggle _cameraShakeToggle;
    public void Hit()
    {
        if (_hitStopToggle.isOn)
        {
            _hitStop.Play(_hitStopDuration);
        }

        if (_cameraShakeToggle.isOn)
        {
            _cameraShake.Play(_shakeDuration, _shakeStrength, _shakeFrequency);
        }
    }
}