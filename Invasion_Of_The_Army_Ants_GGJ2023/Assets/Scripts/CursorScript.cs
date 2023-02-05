using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    [SerializeField]
    private Image cursorShadow;
    [SerializeField]
    private Image cursor;
    [SerializeField]
    private float distanceFromMiddle = 5.0f;
    [SerializeField]
    private GameObject tower;

    private bool holding = false;
    private Vector2 startPos;

    [SerializeField]
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        startPos = GetComponent<RectTransform>().position;
        cursorShadow.enabled = false;
        cursor.enabled = false;
        Debug.Log(startPos);
    }

    // Update is called once per frame
    void Update()
    {
        cursorShadow.rectTransform.localPosition = Input.mousePosition + offset;
        
        if (Vector2.Distance(cursorShadow.rectTransform.position,Vector2.zero) < distanceFromMiddle || Vector2.Distance(cursorShadow.rectTransform.position, startPos) < 2.0f)
        {
            cursor.color = new Color(0.7f,0.7f,0.7f);
            cursorShadow.rectTransform.localScale = Vector3.one * 0.93f;
        }
        else
        {
            cursor.color = Color.white;
            cursorShadow.rectTransform.localScale = Vector3.one;
            if (Input.GetMouseButtonDown(0))
            {
                holding = false;
                cursorShadow.enabled = false;
                cursor.enabled = false;
                Instantiate(tower, Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10.0f), Quaternion.identity);
            }
        }
    }

    public void PickUp()
    {
        holding = true;
        cursorShadow.enabled = true;
        cursor.enabled = true;
    }
}
