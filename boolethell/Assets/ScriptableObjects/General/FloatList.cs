using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class FloatList : ScriptableObject
{
    [SerializeField] private List<FloatVariable> variables;
    private float sum;

    public float GetSum()
    {
        Recount();
        return sum;
    }

    public void AddVariable(FloatVariable variable)
    {
        if (!variables.Contains(variable))
            variables.Add(variable);
    }

    public void RemoveVariable(FloatVariable variable)
    {
        if (variables.Contains(variable))
            variables.Remove(variable);
    }

    private void Recount()
    {
        sum = 0f;
        for (int i = variables.Count - 1; i >= 0; i--)
            sum += variables[i].Value;
    }
}
