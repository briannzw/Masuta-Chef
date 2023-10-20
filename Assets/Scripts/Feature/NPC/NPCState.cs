using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NPCState
{
    protected NPC.NPC npc;
    protected NPCStateMachine npcStateMachine;

    public NPCState(NPC.NPC npc, NPCStateMachine npcStateMachine)
    {
        this.npc = npc;
        this.npcStateMachine = npcStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
}
