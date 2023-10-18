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
    public AttackType SelectedWeapon;
    public Weapon.Weapon ActiveWeapon;
    public Animator Animator;
    public Transform TargetTransform;
    public Vector3 TargetPosition { get; set; }

    private Vector3 targetPosition;
    public float StopDistance;
    [HideInInspector]
    public NavMeshAgent Agent;
    //public Character Character;
    public NPCStateMachine StateMachine;
    protected Character.Character chara;

    protected void Awake()
    {
        StateMachine = new NPCStateMachine();
        chara = GetComponent<Character.Character>();

        Agent = GetComponent<NavMeshAgent>();
        Agent.speed = chara.Stats.StatList[StatsEnum.Speed].Value/10;
        Agent.stoppingDistance = StopDistance;
        
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (ActiveWeapon != null) ActiveWeapon.OnEquip(chara);
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
