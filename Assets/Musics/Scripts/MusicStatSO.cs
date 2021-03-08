using UnityEngine;

public enum MusicType { Master, Music, Effects }

[CreateAssetMenu(fileName = "MusicStats")]
public class MusicStatSO : ScriptableObject
{
    public MusicOptionSO master;
    public MusicOptionSO music;
    public MusicOptionSO effects;
}
