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

    [SerializeField]
    private int towerCost = 10;

    private bool holding = false;
    private Vector2 startPos;

    [SerializeField]
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        startPos = GetComponent<RectTransform>().position;
        ShooterPlantFollowMouse.ShowPlant(false);
        cursorShadow.enabled = false;
        cursor.enabled = false;
        Debug.Log(startPos);
    }

    // Update is called once per frame
    void Update()
    {
        //cursorShadow.rectTransform.position = Input.mousePosition + offset;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10.0f);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        if (Vector2.Distance(mousePos2D, Vector2.zero) < distanceFromMiddle || Vector2.Distance(mousePos2D, startPos) < 2.0f)
        {
            ShooterPlantFollowMouse.SetTargetColour(new Color(0.7f, 0.7f, 0.7f));
            ShooterPlantFollowMouse.SetSize(Vector3.one * 0.93f);
        }
        else
        {
            ShooterPlantFollowMouse.SetTargetColour(Color.white);

            ShooterPlantFollowMouse.SetSize(Vector3.one);
            if (Input.GetMouseButtonDown(0) && holding)
            {
                holding = false;
                cursorShadow.enabled = false;
                cursor.enabled = false;
                Instantiate(tower, mousePos, Quaternion.identity);

                ShooterPlantFollowMouse.ShowPlant(false);
            }
        }
    }

    public void PickUp()
    {
        if (GameManagerScript.GetMoneyCount() >= towerCost)
        {
            holding = true;
            ShooterPlantFollowMouse.ShowPlant(true);
            GameManagerScript.ReduceMoneyPlz(towerCost);
            //cursorShadow.enabled = true;
            //cursor.enabled = true;
        }
    }
}
