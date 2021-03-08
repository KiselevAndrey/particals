using UnityEngine;

[CreateAssetMenu(fileName = "GameStatSO")]
public class GameStatSO : ScriptableObject
{
    public float record;
    public bool tutorialEnding;


    [Header("Параметры музыки")]
    public bool musicPlay;
    public bool effectsPlay;
    [Range(0, 1)] public float masterVolume;
    [Range(0, 1)] public float musicVolume;
    [Range(0, 1)] public float effectsVolume;


    public void CheckRecord(float value) => record = Mathf.Max(record, value);

    public void EndTutorial() => tutorialEnding = true;
}
