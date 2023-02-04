using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIChangeInt : MonoBehaviour
{
    [SerializeField]
    private float reactionForce;
    
    public void ChangeInt(int newNum)
    {
        GetComponent<TextMeshProUGUI>().text = newNum.ToString();
        GetComponent<SpringDynamics>().React(reactionForce);
    }
}
