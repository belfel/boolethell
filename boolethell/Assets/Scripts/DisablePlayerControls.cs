using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPauseControlsManager : MonoBehaviour
{
    [SerializeField] private BoolVariable playerPauseControls;

    public void LockControls()
    {
        playerPauseControls.SetValue(true);
    }

    public void UnlockControls()
    {
        playerPauseControls.SetValue(false);
    }
}
