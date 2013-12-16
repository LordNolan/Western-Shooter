using UnityEngine;
using System.Collections;

public class CrabAI : MonoBehaviour 
{
    [HideInInspector]
    public enum AIState
    {
        Idle,
        Aggressive
    }
    
    public double viewRange = 6.0;
	public double firingRange = 4.0;
    public AIState currentState = AIState.Idle;
    public AudioSource alertSound; 
        
    GameObject player;
    EnemyFireWeapon weapon;
    
    bool alerted = false;
    float alertAliveTime = 1.0f;
    float currentAliveTime = 0;
    
    void Start()
    {
        weapon = GetComponent<EnemyFireWeapon>();
    }
    
    void Update()
    {
        if (player == null) player = (GameObject)GameObject.FindWithTag("Player");
        
        if (player != null && !GlobalParams.InNonPlayingState() && GlobalParams.IsMobAIDelayComplete() && !GetComponent<Hitpoints>().mobDead) 
        {
            switch (currentState)
            {
                case AIState.Idle:
                    LookForPlayer();
                    break;
                case AIState.Aggressive:
                    StartMoving();
                    FireAtPlayer();
                    break;
            }
        } 
        
        // to reset alert sprite (hide it)
        if (alerted) {
            if ((currentAliveTime += Time.deltaTime) > alertAliveTime) {
                alerted = false;
                currentAliveTime = 0;
                transform.FindChild("alert").localScale = new Vector3(.8f,0,1);
            }
        }
    }
    
    void LookForPlayer()
    {
        // look for player via raycast view length
        if (!GlobalParams.InNonPlayingState() && CanSeePlayer(player.transform.position)) 
        {
            currentState = AIState.Aggressive;
            alertSound.Play();
            PlayAlertSpriteAnimation();
        }
    }
    
    void PlayAlertSpriteAnimation()
    {
        iTween.ScaleTo(transform.FindChild("alert").gameObject, iTween.Hash("y", .8f, "easeType", "easeOutElastic", "time", .5));
        alerted = true;
    }
    
    void StartMoving()
    {
        GetComponent<EnemyMovement>().canMove = true;
    }
    
    void FireAtPlayer()
    {
        if (CanSeePlayer(player.transform.position) && WithinFiringRange(player.transform.position))
            weapon.FireAtPlayer();
    }
    
    bool CanSeePlayer(Vector3 player)
    {
        // within range to care
        if (Vector3.Distance(player, transform.position) < viewRange)
        {
            // do we have line of sight
            return true;
        }
        return false;
    }
    
    bool WithinFiringRange(Vector3 player)
    {
        return (Vector3.Distance(player, transform.position) < firingRange);
    }
}
