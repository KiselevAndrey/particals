using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStatSO")]
public class GameStatSO : ScriptableObject
{
    public float record;
    public bool tutorialEnding;

    public void CheckRecord(float value) => record = Mathf.Max(record, value);

    public void EndTutorial() => tutorialEnding = true;
}
