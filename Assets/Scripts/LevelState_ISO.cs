using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="LevelState_ISO", menuName = "Data / LevelState_ISO")]
public class LevelState_ISO : ScriptableObject
{
    public List<bool> levelState = new List<bool>();
}
