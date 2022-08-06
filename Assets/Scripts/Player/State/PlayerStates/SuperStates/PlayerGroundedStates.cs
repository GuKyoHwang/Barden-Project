using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerState 상속
public class PlayerGroundedStates : PlayerState
{
    // Input
    protected int xInput;
    protected int yInput;
    private bool isJumpInputted;
    private bool isGrabInputted;
    private bool isDashInputted;
    
    // Check
    protected bool isTouchingCeiling;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    
    public PlayerGroundedStates(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        // grounded 상태에 진입했다면 남은 점프 횟수와 대시 가능 상태 초기화
        player.JumpState.ResetJumpCount();
        player.DashState.ResetCanDash();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        xInput = player.InputHandler.InputXNormalize;
        yInput = player.InputHandler.InputYNormalize;
        isJumpInputted = player.InputHandler.IsInputJumpStarted;
        isGrabInputted = player.InputHandler.IsInputGrabStarted;
        isDashInputted = player.InputHandler.IsInputDashStarted;

        // grounded 상태에서 벗어나는 조건들

        // 웅크린 상태가 아닐 때(천장에 머리가 닿지 않았을 때) 스킬 키를 눌렀다면 attack 상태로
        if(player.InputHandler.IsInputAttackArray[(int)AttackInput.primary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if(player.InputHandler.IsInputAttackArray[(int)AttackInput.secondary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        // 어떤 지상 상태에서든 점프 키 입력 시 점프 상태로 바뀔 수 있음
        // 단 남은 점프 횟수가 0보다 클 경우(CanJump() return 조건)
        else if (isJumpInputted && !isTouchingCeiling && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        // grounded 상태에서, 지면에서 벗어나게 될 경우
        // coyote time 적용 후 inAir 상태로 변경
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        // 벽과 난간에 닿은 상태로 grab 키를 눌렀을 경우 wall Grab 상태로
        // 낮은 턱에서 grab 키를 누르면 난간에 매달리는 모션을 취하기 때문
        else if (isTouchingWall && isGrabInputted && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        // 대시 키를 눌렀으면서 천장에 닿지 않았으면서 대시를 할 수 있는 상태라면 dash 상태로
        else if (isDashInputted && player.DashState.CheckCanDash() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.DashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoCheck()
    {
        base.DoCheck();
        
        isGrounded = core.ColSenses.GetGround;
        isTouchingWall = core.ColSenses.GetWall;
        isTouchingLedge = core.ColSenses.GetLedgeHor;
        isTouchingCeiling = core.ColSenses.GetCeiling;
    }
}
