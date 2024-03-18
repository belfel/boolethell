using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float maxAlphaLength;
    [SerializeField] private float fadeLength;
    private Color baseColor;

    private void Awake()
    {
        baseColor = image.color;
    }

    public void Fire()
    {
        StartCoroutine(HitScreenEffectRoutine());
    }

    private IEnumerator HitScreenEffectRoutine()
    {
        image.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);
        yield return new WaitForSeconds(maxAlphaLength);

        float timer = 0f;
        while (timer < fadeLength) 
        {
            float alpha = (fadeLength - timer) / fadeLength;
            image.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
