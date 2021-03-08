using UnityEngine;

[CreateAssetMenu(fileName = "MusicOption")]
public class MusicOptionSO : ScriptableObject
{
    public MusicType type;
    public bool play;
    [Range(0, 1)] public float volume;
}
