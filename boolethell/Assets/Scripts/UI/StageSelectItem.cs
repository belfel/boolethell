using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectItem : MonoBehaviour
{
    [SerializeField] private UnlockManager.EItem item;
    private Type type;
    private Button button;

    private void Awake()
    {
        type = UnlockManager.instance.GetItemType(item);
        button = gameObject.GetComponent<Button>();
        if (!button)
            Debug.LogError("No button object found");
    }

    public Type GetItemType()
    {
        return type;
    }

    public Button GetButton()
    {
        return button;
    }
}
