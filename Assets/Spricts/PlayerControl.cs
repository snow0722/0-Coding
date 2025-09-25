using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    [Tooltip("移動速度")]
    private float moveSpeed = 5f;

    private InputAction moveAction;

    // 跳躍動作
    private InputAction jumpAction;

    // 引用 PlayerJumpFloat：角色漂浮機制
    private PlayerJumpFloat jumpFloat;

    // 控制是否能移動
    private bool canMove = true;

    void Start()
    {
        moveAction = playerInput.actions.FindAction("Move");

        // 取得 Jump Action 並綁定輸入事件
        jumpAction = playerInput.actions.FindAction("Jump");
        jumpAction.performed += OnJumpPressed;
        jumpAction.canceled += OnJumpReleased;

        // 取得 PlayerJumpFloat 組件
        jumpFloat = GetComponent<PlayerJumpFloat>();
    }

    // 跳躍按下
    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        if (jumpFloat != null && canMove)
            jumpFloat.HandleJumpInput();
    }

    // 跳躍放開
    private void OnJumpReleased(InputAction.CallbackContext context)
    {
        if (jumpFloat != null && canMove)
            jumpFloat.HandleJumpRelease();
    }

    void Update()
    {
        if (!canMove) return; // 如果被停用，直接不跑移動

        var moveVector2 = moveAction.ReadValue<Vector2>();
        var direction = new Vector3(moveVector2.x, 0, moveVector2.y);
        var movement = direction * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    // 提供 Fungus 呼叫的方法
    public void SetMove(bool enable)
    {
        canMove = enable;

        if (!enable)
        {
            // 如果你有用 Rigidbody，這邊建議清掉速度
            // GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}

