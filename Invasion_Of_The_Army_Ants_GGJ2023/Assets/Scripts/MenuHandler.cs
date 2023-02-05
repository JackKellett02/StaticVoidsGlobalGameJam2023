using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{

    [SerializeField]
    private Image splashScreen;
    [SerializeField]
    private Image fadeScreen;

    private bool splashScreenFade = false;
    public bool screenFading = false;

    // Start is called before the first frame update
    void Start()
    {
        fadeScreen.color = Color.black;
        StartCoroutine(startGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (splashScreenFade && !screenFading)
        {
            splashScreen.color = new Color(0, 0, 0, Mathf.Lerp(splashScreen.color.a,0,Time.deltaTime * 2.5f));
        }
        if (screenFading)
        {
            fadeScreen.color = new Color(0, 0, 0, Mathf.Lerp(fadeScreen.color.a, 1.0f, Time.deltaTime * 2.0f));
        }
    }

    private IEnumerator startGame()
    {
        yield return new WaitForSeconds(1.0f);
        splashScreenFade = true;
    }
}
