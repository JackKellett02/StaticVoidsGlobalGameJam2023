using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthScript
{
    #region Variables to assign via the unity inspector.
    [SerializeField]
    private float baseHealth = 200;
    #endregion

    #region Private Variable Declarations.
    private float currentHealth;

    private List<Action> deathActions = new List<Action>();
    #endregion

    #region Private Funct6ions.
    private void OnDeath()
    {
        if (deathActions == null)
        {
            //Early out
            return;
        }
        //Deal with entity death.
        for (int i = 0; i < deathActions.Count; i++)
        {
            deathActions[i].Invoke();
        }
    }
    #endregion

    #region Public Access Functions.
    public void AddDeathActions(Action a_action)
    {
        if(deathActions == null)
        {
            deathActions = new List<Action>();
        }

        deathActions.Add(a_action);
    }

    public void DamageEntity(float a_damage)
    {
        currentHealth -= a_damage;
        if(currentHealth <= 0.0f)
        {
            //Early out.
            OnDeath();
            return;
        }

        //Clamp health.
        currentHealth = Mathf.Clamp(currentHealth, 0.0f, baseHealth);
    }

    public void HealEntity(float a_health)
    {
        currentHealth += a_health;
        currentHealth = Mathf.Clamp(currentHealth, 0.0f, baseHealth);
    }

    public void HealToMax()
    {
        currentHealth = baseHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetTotalHealth()
    {
        return baseHealth;
    }

    //Use these functions to interface with the corresponding monobehaviour functions.
    public void OnValidate()
    {
        if (baseHealth < 0)
        {
            baseHealth = 0;
        }
    }

    public void OnStart()
    {
        if (deathActions == null)
        {
            deathActions = new List<Action>();
        }
        HealToMax();
    }
    #endregion
}