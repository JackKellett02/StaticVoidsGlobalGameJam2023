using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencePointScript : MonoBehaviour
{
    #region Variables to assign via the unity inspector.
    [SerializeField]
    private HealthScript health = new HealthScript();

    [SerializeField]
    private bool isTree = false;
    #endregion

    #region Private variable Declarations.
    private static float currentSystemHealth;
    private static float totalSystemHealth;
    #endregion

    #region private Functions.
    // Start is called before the first frame update
    void Start()
    {
        health.OnStart();
        health.AddDeathActions(OnDeath);
        totalSystemHealth += health.GetTotalHealth();
        currentSystemHealth += health.GetCurrentHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Ant")
        {
            //Early out.
            return;
        }

        //Get the ants health.
        AntAIScript ant = other.GetComponent<AntAIScript>();
        if(ant == null)
        {
            Debug.LogError("Ant does not have correct controller script.");
            return;
        }

        //Give it as much damage as possible and indicate that it is the defence points killing the ant.
        ant.DamageAnt(float.MaxValue, true);

        //Apply damage to defence point.
        health.DamageEntity(ant.GetAntDamage());
        currentSystemHealth -= ant.GetAntDamage();
        currentSystemHealth = Mathf.Clamp(currentSystemHealth, 0.0f, currentSystemHealth);
    }

    private void OnValidate()
    {
        health.OnValidate();
    }

    private void OnDeath()
    {
        if (isTree)
        {
            //Signal game over.
            GameManagerScript.HandleGameOver();
        }
        Destroy(this.gameObject);
    }
    #endregion

    #region Public Access Functions.
    public static float GetSystemMaxHealth()
    {
        return totalSystemHealth;
    }

    public static float GetSystemCurrentHealth()
    {
        return currentSystemHealth;
    }
    #endregion
}
