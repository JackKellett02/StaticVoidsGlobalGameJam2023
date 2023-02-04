using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AntAIScript : MonoBehaviour
{
    #region Variables to assign via the unity inspector.
    [SerializeField]
    private HealthScript health = new HealthScript();

    [SerializeField]
    private float antDmg = 5.0f;

    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float updateTargetCooldown = 1.0f;

    [SerializeField]
    [Range(0.0f, 20.0f)]
    private float antSpeed = 1.0f;

    [SerializeField]
    private Rigidbody antRigidbody = null;

    [SerializeField]
    private Transform antSpriteTransform = null;
    #endregion

    #region Private Variable Declarations.
    private Vector3 targetPos = Vector3.zero;
    private bool updateTarget = true;

    private bool canMove = true;
    private bool killedByDefencePoint = false;
    #endregion

    #region Event Handling.
    public static event EventHandler<AntInfo> antKilled;
    #endregion

    #region private functions.

    // Start is called before the first frame update
    void Start()
    {
        //Set up health script.
        health.OnStart();

        //Add ondeath actions.
        health.AddDeathActions(DeleteAnt);

        //Setup movement stuff.
        updateTarget = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Find the target position.
        if (updateTarget)
        {
            targetPos = FindClosestDefencePoint();
            StartCoroutine(UpdateTargetCooldownTimer());
        }

        if (canMove)
        {
            //Rotate the sprite towards the target.
            antSpriteTransform.up = (targetPos - this.transform.position).normalized;

            //Set the velocity.
            antRigidbody.velocity = (antSpriteTransform.up * antSpeed);
        }
    }

    private void OnValidate()
    {
        health.OnValidate();
        if (antDmg < 0)
        {
            antDmg = 0;
        }
    }

    //Movement stuff.
    private Vector3 FindClosestDefencePoint()
    {
        //Initialise return variable.
        Vector3 returnVal = Vector3.zero;

        //Get the defence points.
        GameObject[] defencePoints = GameObject.FindGameObjectsWithTag("DefencePoint");

        //Find the smallest distance.
        Vector3 antPos = this.transform.position;
        float smallestDistance = float.MaxValue;
        for (int i = 0; i < defencePoints.Length; i++)
        {
            //Get the distance between the ant and that defence point.
            float newDistance = (defencePoints[i].transform.position - antPos).magnitude;
            if (newDistance < smallestDistance)
            {
                smallestDistance = newDistance;
                returnVal = defencePoints[i].transform.position;
            }
        }


        //Return it.
        return returnVal;
    }

    private IEnumerator UpdateTargetCooldownTimer()
    {
        updateTarget = false;
        yield return new WaitForSeconds(updateTargetCooldown);
        updateTarget = true;
    }

    //Death stuff.
    private IEnumerator DestructionTimer(float a_animationLength)
    {
        yield return new WaitForSeconds(a_animationLength);
        Destroy(this.gameObject);
    }

    private void DeleteAnt()
    {
        Debug.Log("Ant Deleted");
        //Give currency to the user.

        //Invoke death event.
        AntInfo info = new AntInfo();
        info.m_killedByDefencePoint = killedByDefencePoint;
        antKilled?.Invoke(this, info);

        //Play Death Animation.

        //Put the death animation length in the coroutine.
        StartCoroutine(DestructionTimer(0.0f));
    }
    #endregion

    #region Public Access Functions.
    public void DamageAnt(float a_damage, bool a_isDefencePoint = false)
    {
        killedByDefencePoint = a_isDefencePoint;
        health.DamageEntity(a_damage);
    }

    public float GetAntDamage()
    {
        return antDmg;
    }

    #endregion

    #region structs.
    public class AntInfo : EventArgs
    {
        public bool m_killedByDefencePoint;
    }
    #endregion
}
