using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StringVariable : ScriptableObject
{
    public string Text;

    public void SetValue(string text)
    {
        Text = text;
    }

    public void SetValue(StringVariable text)
    {
        Text = text.Text;
    }

    public void Add(string text)
    {
        Text += text;
    }

    public void Add(StringVariable text)
    {
        Text += text.Text;
    }
}
