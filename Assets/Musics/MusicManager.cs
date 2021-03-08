using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum MusicType { Master, Music, Effects }

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
    [SerializeField] MusicStatSO musicStats;

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
        LoadToUI(masterVolSlider, musicStats.master.volume, MixerGroup.MasterVolume);
        LoadToUI(musicVolSlider, musicStats.music.volume, MixerGroup.MusicVolume);
        LoadToUI(effectsVolSlider, musicStats.effects.volume, MixerGroup.EffectsVolume);
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

    public void VolumeMaster(float value) => NewValue(MixerGroup.MasterVolume, ref musicStats.master.volume, value);
    public void VolumeBackground(float value) => NewValue(MixerGroup.MusicVolume, ref musicStats.music.volume, value);
    public void VolumeEffects(float value) => NewValue(MixerGroup.EffectsVolume, ref musicStats.effects.volume, value);
    #endregion

    #region Выбор Snapshots
    public void ToMenuSnapshot() => ToSnapshot(menu);
    public void ToNormalSnapshot() => ToSnapshot(normal);
    public void ToPauseSnapshot() => ToSnapshot(pause);
    public void ToSnapshot(AudioMixerSnapshot snapshot, float timeTransition = 1f) => snapshot.TransitionTo(timeTransition);
    #endregion

    #endregion
}
