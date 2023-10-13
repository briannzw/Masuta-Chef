using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public enum AttackType
    {
        Melee,
        Ranged,
        Utility,
        Joker
    }
    public AttackType selectedWeapon;
    public Animator Animator;
    public Transform TargetTransform;
    public float MoveSpeed;
    public Vector3 TargetPosition { get; set; }

    private Vector3 targetPosition;
    public float StopDistance;
    [HideInInspector]
    public NavMeshAgent Agent;
    //public Character Character;
    public NPCStateMachine StateMachine;
    public NPCMoveState NPCMoveState { get; set; }
    public NPCAttackState NPCAttackState { get; set; }

    protected void Awake()
    {
        StateMachine = new NPCStateMachine();
        NPCMoveState = new NPCMoveState(this, StateMachine);
        NPCAttackState = new NPCAttackState(this, StateMachine);
        Agent = GetComponent<NavMeshAgent>();
        Agent.speed = MoveSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        StateMachine.CurrentState.FrameUpdate();
    }

    public void Attack()
    {

    }
}
