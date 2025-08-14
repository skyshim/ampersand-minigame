using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private const string PREF_KEY = "BGM_VOLUME";

    [Header("UI / Audio")]
    public Slider volumeSlider;   // Canvas�� Slider
    public AudioSource bgmAudio;  // BGM�� ���� ������Ʈ�� AudioSource

    void Awake()
    {
        // ����� �� �б� (������ �⺻ 1.0f)
        float saved = PlayerPrefs.GetFloat(PREF_KEY, 1.0f);
        saved = Mathf.Clamp01(saved);

        // �����̴� & ����� �ʱ�ȭ
        if (volumeSlider != null) volumeSlider.value = saved;
        if (bgmAudio != null) bgmAudio.volume = saved;

        // ������ ���
        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnDestroy()
    {
        // ������ ����(�޸� ���� ����)
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    private void OnSliderChanged(float v)
    {
        float vol = Mathf.Clamp01(v);
        if (bgmAudio != null) bgmAudio.volume = vol;

        // ��� ���� (���ϸ� ���� ���� ���� �ֱ�� �����ص� ��)
        PlayerPrefs.SetFloat(PREF_KEY, vol);
        PlayerPrefs.Save();
    }

    // ����: �� ���� �� �� �� �� ����(������ġ)
    void OnApplicationQuit()
    {
        if (volumeSlider != null)
        {
            PlayerPrefs.SetFloat(PREF_KEY, Mathf.Clamp01(volumeSlider.value));
            PlayerPrefs.Save();
        }
    }
}