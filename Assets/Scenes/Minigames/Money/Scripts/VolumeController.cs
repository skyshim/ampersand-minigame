using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private const string PREF_KEY = "BGM_VOLUME";

    [Header("UI / Audio")]
    public Slider volumeSlider;   // Canvas의 Slider
    public AudioSource bgmAudio;  // BGM이 붙은 오브젝트의 AudioSource

    void Awake()
    {
        // 저장된 값 읽기 (없으면 기본 1.0f)
        float saved = PlayerPrefs.GetFloat(PREF_KEY, 1.0f);
        saved = Mathf.Clamp01(saved);

        // 슬라이더 & 오디오 초기화
        if (volumeSlider != null) volumeSlider.value = saved;
        if (bgmAudio != null) bgmAudio.volume = saved;

        // 리스너 등록
        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnDestroy()
    {
        // 리스너 해제(메모리 누수 방지)
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    private void OnSliderChanged(float v)
    {
        float vol = Mathf.Clamp01(v);
        if (bgmAudio != null) bgmAudio.volume = vol;

        // 즉시 저장 (원하면 성능 위해 일정 주기로 저장해도 됨)
        PlayerPrefs.SetFloat(PREF_KEY, vol);
        PlayerPrefs.Save();
    }

    // 선택: 앱 종료 시 한 번 더 저장(안전장치)
    void OnApplicationQuit()
    {
        if (volumeSlider != null)
        {
            PlayerPrefs.SetFloat(PREF_KEY, Mathf.Clamp01(volumeSlider.value));
            PlayerPrefs.Save();
        }
    }
}