using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingState : IPlayerState
{
    private InputAction move;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private Vector3 groundCheck;
    private LayerMask groundMask;

    public event Action OnAction;

    private const float GRAVITY = -9.81f;
    private float rotationSpeed = 10f;
    private float moveSpeed = 5f;
    private float groundCheckRadius = 0.3f;

    private bool isGrounded;

    public MovingState(InputAction playerInput, CharacterController characterController, LayerMask groundMask)
    {
        this.move = playerInput;
        this.characterController = characterController;
        this.groundMask = groundMask;
    }

    public void EnterState(PlayerStateMachine player) { }

    public void UpdateState(PlayerStateMachine player)
    {
        playerGravityHandling();

        playerMovementHandling();
    }

    public void ExitState(PlayerStateMachine player) { }

    private void playerGravityHandling()
    {
        groundCheck = new(characterController.transform.position.x, characterController.transform.position.y - 1, characterController.transform.position.z);
        
        isGrounded = Physics.CheckSphere(groundCheck, groundCheckRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += GRAVITY * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    private void playerMovementHandling()
    {
        moveDirection = new Vector3(move.ReadValue<Vector2>().x, 0, move.ReadValue<Vector2>().y);

        if (moveDirection.magnitude < 0.2f)
        {
            OnAction.Invoke();
        }

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        characterController.transform.rotation = Quaternion.Slerp(characterController.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
