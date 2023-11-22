using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoGUI : MonoBehaviour
{
    [SerializeField] TextMesh textMesh;
    [SerializeField] StringVariable text;

    public void UpdateText()
    {
        textMesh.text = text.Text;
    }
}
