using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
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

        Destroy(this.gameObject);
    }
}
