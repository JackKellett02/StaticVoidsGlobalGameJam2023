using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeInt : MonoBehaviour
{
    [SerializeField]
    private float reactionForce;
    
    public void ChangeInt(int newNum)
    {
        GetComponent<Text>().text = newNum.ToString();
        GetComponent<SpringDynamics>().React(reactionForce);
    }
}
