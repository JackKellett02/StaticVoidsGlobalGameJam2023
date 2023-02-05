using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterPlantFollowMouse : MonoBehaviour
{
    [SerializeField]
    private GameObject plantSprite = null;

    private static GameObject sPlant = null;
    private static Color targetColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        sPlant = plantSprite;
        plantSprite.SetActive(false);
        targetColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10.0f);
        plantSprite.GetComponent<SpriteRenderer>().color = targetColor;
    }

    public static void ShowPlant(bool showPlant)
    {
        sPlant.SetActive(showPlant);
    }

    public static void SetTargetColour(Color a_color)
    {
        targetColor = a_color;
    }

    public static void SetSize(Vector3 scale)
    {
        scale *= 5;
        sPlant.transform.localScale = scale;
    }
}
