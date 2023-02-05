using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIBarScale : MonoBehaviour
{
    public float maxBarValue;
    public float currentBarValue;
    private RectTransform thisRect;
    private Vector2 startSize;
    private float storedX;
    [SerializeField]
    private float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        thisRect = GetComponent<RectTransform>();
        startSize = thisRect.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        float currentX = Mathf.Lerp(0, startSize.x, currentBarValue / maxBarValue);
        thisRect.sizeDelta = Vector2.Lerp(thisRect.sizeDelta, new Vector2(currentX, startSize.y),Time.deltaTime * speed);
    }
}
