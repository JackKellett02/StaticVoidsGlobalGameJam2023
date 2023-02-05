using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleScript : MonoBehaviour
{
    [SerializeField]
    private float appleLifetime = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(AppleLifetime());
    }

    private IEnumerator AppleLifetime()
    {
        yield return new WaitForSeconds(appleLifetime);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Ant")
        {
            //Early out.
            return;
        }

        //Get the ants health.
        AntAIScript ant = other.GetComponent<AntAIScript>();
        if (ant == null)
        {
            Debug.LogError("Ant does not have correct controller script.");
            return;
        }

        //Give it as much damage as possible.
        ant.DamageAnt(float.MaxValue);
    }
}
