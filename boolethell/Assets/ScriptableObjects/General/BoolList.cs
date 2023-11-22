using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoolList : ScriptableObject
{
    [SerializeField] private List<BoolVariable> variables;
    private bool sum;

    public bool IsAnyTrue()
    {
        Recheck();
        return sum;
    }

    public void AddVariable(BoolVariable variable)
    {
        if (!variables.Contains(variable))
            variables.Add(variable);
    }

    public void RemoveVariable(BoolVariable variable)
    {
        if (variables.Contains(variable))
            variables.Remove(variable);
    }

    private void Recheck()
    {
        for (int i = variables.Count - 1; i >= 0; i--)
            if (variables[i].Value)
            {
                sum = true;
                return;
            }
        sum = false;
    }
}
