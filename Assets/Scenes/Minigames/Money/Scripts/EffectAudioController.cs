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
        // ����� �� �б� (������ �⺻ 1.0f)
        float saved = PlayerPrefs.GetFloat(PREF_KEY, 1.0f);
        saved = Mathf.Clamp01(saved);

        // �����̴� & ����� �ʱ�ȭ
        if (EffectAudioSlider != null) EffectAudioSlider.value = saved;
        if (EffectAudioSource != null) EffectAudioSource.volume = saved;

        // ������ ���
        if (EffectAudioSlider != null)
            EffectAudioSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnDestroy()
    {
        // ������ ����(�޸� ���� ����)
        if (EffectAudioSlider != null)
            EffectAudioSlider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    private void OnSliderChanged(float v)
    {
        float vol = Mathf.Clamp01(v);
        if (EffectAudioSource != null) EffectAudioSource.volume = vol;

        // ��� ���� (���ϸ� ���� ���� ���� �ֱ�� �����ص� ��)
        PlayerPrefs.SetFloat(PREF_KEY, vol);
        PlayerPrefs.Save();
    }

    // ����: �� ���� �� �� �� �� ����(������ġ)
    void OnApplicationQuit()
    {
        if (EffectAudioSlider != null)
        {
            PlayerPrefs.SetFloat(PREF_KEY, Mathf.Clamp01(EffectAudioSlider.value));
            PlayerPrefs.Save();
        }
    }
}

