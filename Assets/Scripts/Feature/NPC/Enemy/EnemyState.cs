public class EnemyState
{
    protected NPC.Enemy.Enemy enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(NPC.Enemy.Enemy enemy, EnemyStateMachine enemyStateMachine)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }

}
