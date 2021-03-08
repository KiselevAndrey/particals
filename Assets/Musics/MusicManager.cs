using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [Header("Музыка")]
    [SerializeField] ClipListSO clipList;

    [Header("UI elements")]
    [SerializeField] Toggle musicToggle;
    [SerializeField] Slider masterVolSlider;
    [SerializeField] Slider musicVolSlider;
    [SerializeField] Slider effectsVolSlider;

    [Header("Snapshots")]
    [SerializeField] AudioMixerSnapshot normal;
    [SerializeField] AudioMixerSnapshot pause;
    [SerializeField] AudioMixerSnapshot menu;

    [Header("Данные доп")]
    [SerializeField] AudioMixerGroup mixer;
    [SerializeField] GameStatSO gameStats;

    AudioSource _audioSource;
    float _musicLen;

    #region Awake Start
    private void Awake()
    {
        normal.TransitionTo(1f);
        _audioSource = GetComponent<AudioSource>();

        Singleton();
    }

    void Singleton()
    {
        MusicManager[] musics = FindObjectsOfType<MusicManager>();
        for (int i = 0; i < musics.Length; i++)
        {
            if (musics[i].gameObject != gameObject)
            {
                Destroy(gameObject);
                break;
            }
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        LoadMusicOptions();

        StartCoroutine(NextTrek());
    }
    #endregion

    #region NextClip
    IEnumerator NextTrek()
    {
        while (true)
        {
            NextClip();
            
            yield return new WaitForSecondsRealtime(_musicLen - 0.1f);
        }
    }

    void NextClip()
    {
        _audioSource.clip = clipList.NextClip(ref _musicLen);
        _audioSource.Play();
    }
    #endregion

    #region Изменение настроек звука
    void LoadMusicOptions()
    {
        // установка тоглов музыки и эффектов
        //LoadToUI(musicToggle, gameStats.musicPlay, MixerGroup.VolumeBackGround);

        // установка параметров slider
        LoadToUI(masterVolSlider, gameStats.masterVolume, MixerGroup.MasterVolume);
        LoadToUI(musicVolSlider, gameStats.musicVolume, MixerGroup.MusicVolume);
        LoadToUI(effectsVolSlider, gameStats.effectsVolume, MixerGroup.EffectsVolume);
    }

    #region Toggle
    /// <summary>
    /// установка значения на сам UI element и выстановление параметров в mixerGroup
    /// </summary>
    void LoadToUI(Toggle toggle, bool value, string mixerGroup)
    {
        toggle.isOn = value;    // кнопка самого toggle
        SetFloatMixer(mixerGroup, value);
    }

    /// <summary>
    /// Сохраниение нового значения и установка его в mixerGroup
    /// </summary>
    void NewValue(string mixerGroup, ref bool oldValue, bool newValue)
    {
        SetFloatMixer(mixerGroup, newValue);
        oldValue = newValue;
    }

    /// <summary>
    /// Вкл/выкл звука нужного mixerGroup
    /// </summary>
    void SetFloatMixer(string mixerGroup, bool value) => mixer.audioMixer.SetFloat(mixerGroup, value ? 1 : -80);
    #endregion

    #region Sliders
    /// <summary>
    /// установка значения на сам UI element и выстановление параметров в mixerGroup
    /// </summary>
    void LoadToUI(Slider slider, float value, string mixerGroup)
    {
        slider.value = value;    // кнопка самого toggle
        SetFloatMixer(mixerGroup, value);
    }

    /// <summary>
    /// Сохраниение нового значения и установка его в mixerGroup
    /// </summary>
    void NewValue(string mixerGroup, ref float oldValue, float newValue)
    {
        SetFloatMixer(mixerGroup, newValue);
        oldValue = newValue;
    }

    /// <summary>
    /// Вкл/выкл звука нужного mixerGroup
    /// </summary>
    void SetFloatMixer(string mixerGroup, float value) => mixer.audioMixer.SetFloat(mixerGroup, Mathf.Lerp(-50, 0, value));
    #endregion

    #region UIActions
    //public void SwitchMusic(bool value) => NewValue(MixerGroup.VolumeBackGround, ref gameStats.musicPlay, value);
    //public void SwitchEffects(bool value) => NewValue(MixerGroup.VolumeEffects, ref gameStats.effectsPlay, value);

    public void VolumeMaster(float value) => NewValue(MixerGroup.MasterVolume, ref gameStats.masterVolume, value);
    public void VolumeBackground(float value) => NewValue(MixerGroup.MusicVolume, ref gameStats.musicVolume, value);
    public void VolumeEffects(float value) => NewValue(MixerGroup.EffectsVolume, ref gameStats.effectsVolume, value);
    #endregion

    #region Выбор Snapshots
    public void ToMenuSnapshot() => ToSnapshot(menu);
    public void ToNormalSnapshot() => ToSnapshot(normal);
    public void ToPauseSnapshot() => ToSnapshot(pause);
    public void ToSnapshot(AudioMixerSnapshot snapshot, float timeTransition = 1f) => snapshot.TransitionTo(timeTransition);
    #endregion

    #endregion
}
