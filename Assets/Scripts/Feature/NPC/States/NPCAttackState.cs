using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackState : NPCState
{
    private float timer;
    public NPCAttackState(NPC npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        npc.Agent.SetDestination(npc.TargetPosition);
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            if (npc.GetComponent<NPC>().selectedWeapon == NPC.AttackType.Ranged)
            {
                npc.GetComponent<ShootBall>().Shoot();
                timer = npc.GetComponent<ShootBall>().ShootInterval;
            }

            if(npc.GetComponent<NPC>().selectedWeapon == NPC.AttackType.Melee)
            {
                npc.GetComponent<MeleeAttack>().Attack();
                timer = npc.GetComponent<MeleeAttack>().AttackInterval;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
