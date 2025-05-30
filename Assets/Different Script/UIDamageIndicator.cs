using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDamageIndicator : MonoBehaviour
{
    public TMP_Text damageText;

    public float moveSpeed;

    public float lifetime = 3f;

    private RectTransform transformRect;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);

        transformRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        transformRect.anchoredPosition += new Vector2(0f, -moveSpeed * Time.deltaTime);
    }
}
