using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreParentRotation : MonoBehaviour
{
    [SerializeField] Vector3 rotation = Vector3.zero;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(rotation);
    }
}
