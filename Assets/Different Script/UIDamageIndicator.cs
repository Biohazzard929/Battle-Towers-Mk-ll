using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDamageIndicator : MonoBehaviour
{
    public TMP_Text damageText;
    public float moveSpeed;
    public float lifetime = 2f;

    private RectTransform transformRect;

    // Start is called before the first frame update
    void Start()
    {
        transformRect = GetComponent<RectTransform>();
        StartCoroutine(MoveAndFade());
    }

    public void SetLifetime(float newLifetime)
    {
        lifetime = newLifetime;
    }

    private IEnumerator MoveAndFade()
    {
        float elapsedTime = 0f;

        while (elapsedTime < lifetime)
        {
            // Move the indicator upwards
            transformRect.anchoredPosition += new Vector2(0f, moveSpeed * Time.deltaTime);

            // Fade out the indicator
            if (elapsedTime > lifetime - 1f) // Adjust fade duration as needed
            {
                float fadeAmount = 1 - ((elapsedTime - (lifetime - 1f)) / 1f);
                damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, fadeAmount);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
