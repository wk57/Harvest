using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Dependences")]
    [SerializeField] private InputMappingModel mapingModel;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private LayerMask groundMask;

    private bool IsTransitioning;
    private bool InAction;

    private IPlayerState CurrentState;
    private InputAction move, push;

    void Awake()
    {
        move = mapingModel.GetAction(InputMappingModel.PlayerActions.MOVE);
    }

    private void OnEnable()
    {
        move.Enable();
    }

    void Start()
    {
        CurrentState = new IdleState();
        CurrentState.EnterState(this);
    }

    private void Update()
    {
        CurrentState?.UpdateState(this);
        StatesHandling();
    }

    void StatesHandling()
    {
        if (!IsTransitioning && !InAction && move.ReadValue<Vector2>() != Vector2.zero)
        {
            InAction = true;
            TransitionToState(new MovingState(move, characterController,groundMask));
        }
        else if (!IsTransitioning && !InAction && move.ReadValue<Vector2>() == Vector2.zero)
        {
            TransitionToState(new IdleState());
        }
    }

    public void SetPlayerInTrap()
    {
        Debug.Log("In Trap");
        InAction = true;
        TransitionToState(new InTrapState());
    }

    void ActionFinished()
    {
        InAction = false;
    }

    public void TransitionToState(IPlayerState newState)
    {
        IsTransitioning = true;
        CurrentState?.ExitState(this);
        CurrentState.OnAction -= ActionFinished;
        CurrentState = newState;
        CurrentState.OnAction += ActionFinished;
        CurrentState.EnterState(this);
        IsTransitioning = false;
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.3f);
    }
}
