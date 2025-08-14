using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    private const string PREF_KEY = "EFFECT_VOLUME";

    [Header("UI / Audio")]
    public Slider EffectAudioSlider;
    public AudioSource EffectAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        // 저장된 값 읽기 (없으면 기본 1.0f)
        float saved = PlayerPrefs.GetFloat(PREF_KEY, 1.0f);
        saved = Mathf.Clamp01(saved);

        // 슬라이더 & 오디오 초기화
        if (EffectAudioSlider != null) EffectAudioSlider.value = saved;
        if (EffectAudioSource != null) EffectAudioSource.volume = saved;

        // 리스너 등록
        if (EffectAudioSlider != null)
            EffectAudioSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnDestroy()
    {
        // 리스너 해제(메모리 누수 방지)
        if (EffectAudioSlider != null)
            EffectAudioSlider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    private void OnSliderChanged(float v)
    {
        float vol = Mathf.Clamp01(v);
        if (EffectAudioSource != null) EffectAudioSource.volume = vol;

        // 즉시 저장 (원하면 성능 위해 일정 주기로 저장해도 됨)
        PlayerPrefs.SetFloat(PREF_KEY, vol);
        PlayerPrefs.Save();
    }

    // 선택: 앱 종료 시 한 번 더 저장(안전장치)
    void OnApplicationQuit()
    {
        if (EffectAudioSlider != null)
        {
            PlayerPrefs.SetFloat(PREF_KEY, Mathf.Clamp01(EffectAudioSlider.value));
            PlayerPrefs.Save();
        }
    }
}

