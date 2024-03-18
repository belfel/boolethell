using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlayerControls : MonoBehaviour
{
    public BoolVariable cutsceneControlsLock;

    private void OnEnable()
    {
        cutsceneControlsLock.Value = true;
    }

    private void OnDisable()
    {
        cutsceneControlsLock.Value = false;
    }
}
