using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;

    private int xInput;

    private float velocityToSet;
    private bool setVelocity;
    private bool shouldCheckFlip;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        setVelocity = false;

        weapon.EnterWeapon();
    }

    public override void Exit()
    {
        base.Exit();

        weapon.ExitWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.inputHandler.normalizedInputX;
    
        if(shouldCheckFlip)
        {
            core.movement.CheckFlip(xInput);
        }

        if(setVelocity)
        {
            core.movement.SetVelocityX(velocityToSet * core.movement.facingDir);
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this);
    }

    // float velocity 값을 받아 공격 모션 중 플레이어의 Velocity 설정
    public void SetPlayerVelocity(float velocity)
    {
        core.movement.SetVelocityX(velocity * core.movement.facingDir);

        velocityToSet = velocity;
        setVelocity = true;
    }

    // boolean value 값을 받아 공격 모션 중 Flip 할 수 있는 지 체크하는 함수
    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }

    #region Animation Triggers

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

    #endregion
}
