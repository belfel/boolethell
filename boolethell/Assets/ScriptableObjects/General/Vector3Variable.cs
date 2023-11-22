using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Vector3Variable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    public Vector3 Value;

    public void SetValue(float x, float y, float z)
    {
        Value = new Vector3(x, y, z);
    }

    public void SetValue(Vector3Variable value)
    {
        Value = value.Value;
    }

    public void ApplyChange(Vector3 amount)
    {
        Value += amount;
    }

    public void ApplyChange(Vector3Variable amount)
    {
        Value += amount.Value;
    }
}
