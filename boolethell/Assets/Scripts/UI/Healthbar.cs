using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private GameObject greenBar;
    [SerializeField] private GameObject redBar;
    [SerializeField] private bool updatePosition = true;
    [SerializeField] private bool dissapearOnDeath = true;
    [SerializeField] private Vector3 positionOffset = new Vector3(0f, 1f, 0f);
    public FloatVariable currentHp;
    public FloatVariable maxHp;
    public Vector3Variable targetPosition;

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        if (updatePosition)
            gameObject.transform.position = targetPosition.Value + positionOffset;
    }

    public void Refresh()
    {
        if (dissapearOnDeath && currentHp.Value <= 0f)
            Destroy(gameObject);

        float percentHp = Mathf.Max(currentHp.Value, 0f) / maxHp.Value;
        float greenBarOffset = (-0.5f) + 0.5f * percentHp;

        greenBar.transform.localPosition = new Vector3(greenBarOffset, 0, 0);
        greenBar.transform.localScale = new Vector3(percentHp, 1, 1);
    }
}
