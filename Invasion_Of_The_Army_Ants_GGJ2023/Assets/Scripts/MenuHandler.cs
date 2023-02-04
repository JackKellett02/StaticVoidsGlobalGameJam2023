using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [SerializeField]
    List<GameObject> toSwitch = new List<GameObject>();
    [SerializeField]
    List<GameObject> toSet = new List<GameObject>();

    [SerializeField]
    private Image splashScreen;
    [SerializeField]
    private Image fadeScreen;
    [SerializeField]
    private SpringDynamics backGround;

    private bool splashScreenFade = false;
    private bool screenFading = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (splashScreenFade)
        {
            splashScreen.color = new Color(255, 255, 255, Mathf.Lerp(splashScreen.color.a,0,Time.deltaTime * 2.5f));
        }
        if (screenFading)
        {
            fadeScreen.color = new Color(0, 0, 0, Mathf.Lerp(fadeScreen.color.a, 1.0f, Time.deltaTime * 2.0f));
            if(fadeScreen.color.a >= 0.95f)
            {
                SceneManager.LoadScene(1);
            }
        }
    }

    public void MassSwitch()
    {
        StartCoroutine(WaitSwitch(0.2f));
    }
    
    public void ChangeScene()
    {
        StartCoroutine(ChangingScene());
    }

    IEnumerator StartScene()
    {
        splashScreen.color = new Color(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        MassSceneSet();
        backGround.AltSizeTeleport();
        backGround.SwitchSize();
        yield return new WaitForSeconds(4.0f);
        splashScreenFade = true;
        yield return new WaitForSeconds(1.5f);
        backGround.SwitchSize();
        yield return new WaitForSeconds(3.0f);
        MassSwitch();
    }

    IEnumerator ChangingScene()
    {
        yield return new WaitForSeconds(0.5f);
        screenFading = true;
    }

        IEnumerator WaitSwitch(float inputSeconds)
    {
        foreach (GameObject currentSwitch in toSwitch)
        {
            yield return new WaitForSeconds(inputSeconds);
            currentSwitch.GetComponent<SpringDynamics>().SwitchPos();
        }
        toSwitch.Reverse();
    }

    public void MassSceneSet()
    {
        foreach (GameObject currentSet in toSet)
        {
            currentSet.GetComponent<SpringDynamics>().AltPosTeleport();
            currentSet.GetComponent<SpringDynamics>().SwitchPos();
        }
        toSwitch.Reverse();
    }
}
