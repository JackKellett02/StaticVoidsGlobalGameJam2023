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
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        cursorShadow.rectTransform.position = Input.mousePosition + offset;
        
        if (Input.GetMouseButtonDown(0))
        {
            cursor.color = new Color(0.7f,0.7f,0.7f);
            cursorShadow.rectTransform.localScale = Vector3.one * 0.93f;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cursor.color = Color.white;
            cursorShadow.rectTransform.localScale = Vector3.one;
        }
    }
}
