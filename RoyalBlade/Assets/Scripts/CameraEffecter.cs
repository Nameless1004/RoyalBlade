using Cinemachine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffecter : Singleton<CameraEffecter>
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private VolumeProfile _volumeProfile;


    #region  CameraShake
    [Header("--- Camera Shake ---")]

    private IEnumerator _cameraShakeCoroutine;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    [SerializeField]
    private int _cameraShakeDuration;
    [SerializeField]
    private float _cameraShakeIntensity;
    #endregion

    #region  Chromatic Aberration
    [Header("ChromaticAbberation"), Space(10)]
    [SerializeField, Range(0f, 1f)]
    private float _caDuration;
    [SerializeField, Range(0f, 1f)]
    private float _caMaxIntensity;
    private ChromaticAberration _chromaticAberration;
    #endregion

    #region Vignette
    [Header("Vignette")]
    [SerializeField]
    private Color _vignetteCol;
    [SerializeField]
    private float _vignetteDuration;
    [SerializeField]
    private float _vignetteMaxIntensity;
    [SerializeField, Range(0f, 1f)]
    private float _vignetteSmoothness;
    private Vignette _vignette;
    #endregion

    private void Start()
    {
        Instance.CameraInitialize(Camera.main, FindObjectOfType<CinemachineVirtualCamera>());
    }

    public void CameraInitialize(Camera mainCam, CinemachineVirtualCamera virtualCamera)
    {
        _mainCamera = mainCam;
        _virtualCamera = virtualCamera;

        _volumeProfile = _mainCamera?.GetComponent<Volume>().profile;
        Debug.Assert(_volumeProfile != null, "VolumeProfile is Null", _volumeProfile);
        _cinemachineBasicMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _chromaticAberration = GetVolumeComponent<ChromaticAberration>(_volumeProfile);
        _vignette = GetVolumeComponent<Vignette>(_volumeProfile);
    }

    T GetVolumeComponent<T>(VolumeProfile profile) where T : VolumeComponent
    {
        // Profile이 없다면 null 리턴
        if (profile is null) return null;

        if (_volumeProfile.TryGet<T>(out T comp) == false)
        {
            Debug.Assert(true, $"Invalid VolumeComponent : {nameof(T)}");
            return null;
        }

        return comp;
    }

    #region Screen Shake Method

    /// <summary>
    /// 
    /// </summary>
    /// <param name="duration">MilliSecond</param>
    /// <param name="intensity"></param>
    public void PlayScreenShake(int duration, float intensity)
    {
        _cameraShakeDuration = duration;
        _cameraShakeIntensity = intensity;
        ShakeScreenEffectTask().Forget();
    }

    float prevIntensity;
    async UniTaskVoid ShakeScreenEffectTask()
    {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = prevIntensity = _cameraShakeIntensity;
        await UniTask.Delay(_cameraShakeDuration, true);
        if(prevIntensity == _cameraShakeIntensity)
        {
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }
    #endregion

    #region ChromaticAbberation Method
    public void PlayChromaticAbberation(float duration = 0f, float intensity = 0f)
    {
        ChromaticAberrationEffectTask(duration, intensity).Forget();
    }

    async UniTaskVoid ChromaticAberrationEffectTask(float duration, float intensity)
    {
        float t = 0f;
        float effectDuration = duration == 0f ? _caDuration : duration;
        float changeIntensity = 0;
        float caIntensity = intensity == 0f ? _caMaxIntensity : intensity;
        // 깜빡여야함  -1 ~ 1
        while (t - 0.1f < effectDuration)
        {
            t += Time.unscaledDeltaTime;
            float sinVal = Mathf.Sin(Mathf.Lerp(0, Mathf.PI, t / effectDuration));
            changeIntensity = Mathf.Clamp(sinVal, 0, caIntensity);
            _chromaticAberration.intensity.value = changeIntensity;
            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }
        _chromaticAberration.intensity.value = 0;
    }
    #endregion

    #region Vignette Shake Method
    public void PlayScreenBlink()
    {
        VignetteEffectTask().Forget();
    }

    public void PlayScreenBlink(Color color, float duration, float intencity)
    {

        VignetteEffectTask(color, duration, intencity).Forget();
    }

    async UniTaskVoid VignetteEffectTask(UnityEngine.Color col, float dur, float inten)
    {
        float t = 0f;
        float currentIntensity = 0f;
        float middle = dur * 0.5f;

        _vignette.intensity.value = 0f;
        _vignette.smoothness.value = _vignetteSmoothness;
        _vignette.color.value = col;

        while (t - 0.1f < dur)
        {
            t += Time.unscaledDeltaTime;
            if (t < middle)
            {
                float v = Mathf.Lerp(0, inten, t / middle);
                currentIntensity = v;
            }
            else
            {
                float v = Mathf.Lerp(inten, 0, (t - middle) / middle);
                currentIntensity = v;
            }
            _vignette.intensity.value = currentIntensity;
            // float sinVal = Mathf.Sin(Mathf.Lerp(0, Mathf.PI, t / duration));
            // currentIntensity = Mathf.Clamp(sinVal, 0, maxIntensity);
            // _vignette.intensity.value = currentIntensity;
            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }
        _vignette.intensity.value = 0f;

    }

    async UniTaskVoid VignetteEffectTask()
    {
        float t = 0f;
        float currentIntensity = 0f;
        float middle = _vignetteDuration * 0.5f;

        _vignette.intensity.value = 0f;
        _vignette.smoothness.value = _vignetteSmoothness;
        _vignette.color.value = _vignetteCol;

        while (t - 0.1f < _vignetteDuration)
        {
            t += Time.deltaTime;
            if (t < middle)
            {
                float v = Mathf.Lerp(0, _vignetteMaxIntensity, t / middle);
                currentIntensity = v;
            }
            else
            {
                float v = Mathf.Lerp(_vignetteMaxIntensity, 0, (t - middle) / middle);
                currentIntensity = v;
            }
            _vignette.intensity.value = currentIntensity;
            // float sinVal = Mathf.Sin(Mathf.Lerp(0, Mathf.PI, t / duration));
            // currentIntensity = Mathf.Clamp(sinVal, 0, maxIntensity);
            // _vignette.intensity.value = currentIntensity;
            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }
        _vignette.intensity.value = 0f;

    }
    #endregion

}