using UnityEngine;

public class PlayerJumpFloat : MonoBehaviour
{
    [Header("跳躍設定")]
    [SerializeField][Tooltip("小跳力道")] private float jumpForce = 12f;
    [SerializeField][Tooltip("大跳力道（需長按才觸發）")] private float bigJumpForce = 20f;
    [SerializeField][Tooltip("空中推升力道（上飄用）")] private float floatBoostForce = 10f;
    [SerializeField][Tooltip("長按超過此秒數才觸發大跳")] private float floatDelay = 0.3f;

    [Header("重力設定")]
    //[SerializeField] [Tooltip("一般下落重力")] private float normalGravity = 3f;
    [SerializeField][Tooltip("漂浮時的低重力")] private float floatGravity = 0.05f;

    [Header("地面偵測")]
    [SerializeField][Tooltip("地面偵測點")] private Transform groundCheck;
    [SerializeField][Tooltip("地面偵測半徑")] private float groundCheckRadius = 0.3f;
    [SerializeField][Tooltip("地面圖層")] private LayerMask groundLayer;

    public Basket basketScript;  // 取得 Basket.cs

    // 狀態變數
    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isJumpHeld = false;
    private bool canJump = false;
    private bool isBigJump = false;
    private float jumpHoldTime = 0f;
    private bool wasGrounded = false; // 用來判斷「剛落地」

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.useGravity = true;
    }

    void Update()
    {
        GroundCheck();

        // 偵測「剛落地」時執行重置
        if (!wasGrounded && isGrounded)
        {
            ResetJumpStateOnLand(); // 落地時重設狀態
        }

        wasGrounded = isGrounded; // 更新上幀落地狀態

        // 按住跳躍期間累加時間
        if (isJumpHeld)
            jumpHoldTime += Time.deltaTime;

    }

    void FixedUpdate()
    {
        HandleFloatGravity(); // 控制漂浮時的低重力
    }

    // 玩家按下跳躍鍵
    public void HandleJumpInput()
    {
        if (isGrounded)
        {
            isJumpHeld = true;
            canJump = true;
            jumpHoldTime = 0f;
        }
        else
        {
            TryAirFloatBoost(); // 空中點按觸發上飄
        }
    }

    // 玩家放開跳躍鍵（決定跳躍種類）
    public void HandleJumpRelease()
    {
        if (!canJump) return;

        isJumpHeld = false;
        canJump = false;

        if (isGrounded)
        {
            DecideJumpType(); // 根據按住時間判斷大跳或小跳
        }
    }

    // 執行小跳（不需要籃子）
    private void DoSmallJump()
    {
        Vector3 vel = rb.velocity;
        vel.y = jumpForce;
        rb.velocity = vel;

        isBigJump = false;
    }

    // 執行大跳（需有籃子且長按）
    private void DoBigJumpAndFloat()
    {
        Vector3 vel = rb.velocity;
        vel.y = bigJumpForce;
        rb.velocity = vel;

        isBigJump = true;
    }

    // 判斷跳躍種類（是否為大跳）
    private void DecideJumpType()
    {
        if (basketScript != null && basketScript.basket && jumpHoldTime >= floatDelay)
        {
            DoBigJumpAndFloat();
        }
        else
        {
            DoSmallJump();
        }
    }

    // 漂浮邏輯：長按並在空中且大跳後才進入低重力
    private void HandleFloatGravity()
    {
        if (basketScript != null && basketScript.basket && isBigJump && !isGrounded && isJumpHeld)
        {
            // 法1:使用較小的重力達成漂浮效果
            //rb.AddForce(Vector3.down * (normalGravity * floatGravity), ForceMode.Acceleration);

            // 法2(重力較小，飛得高落下慢):禁用內建重力並施加自訂重力
            rb.useGravity = false;
            rb.AddForce(Vector3.down * floatGravity, ForceMode.Acceleration);
        }
        else
        {
            // 法1:一般重力
            // rb.AddForce(Vector3.down * normalGravity, ForceMode.Acceleration);
            // 法2(重力較小，飛得高落下慢):恢復內建重力
            rb.useGravity = true;
        }
    }

    // 空中上飄（點按跳躍鍵）
    private void TryAirFloatBoost()
    {
        if (basketScript == null || !basketScript.basket || isGrounded || !isBigJump) return;

        Vector3 vel = rb.velocity;
        vel.y = floatBoostForce;
        rb.velocity = vel;
    }

    // 落地時重設跳躍相關狀態
    private void ResetJumpStateOnLand()
    {
        isBigJump = false;
        isJumpHeld = false;
        canJump = false;
        jumpHoldTime = 0f;

        Debug.Log("[落地] 已重置跳躍狀態");
    }

    // 地面偵測
    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // 顯示偵測地面範圍
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    // 給動畫控制器讀取狀態
    public bool IsGrounded => isGrounded;
    public bool IsJumpHeld => isJumpHeld;
    public float HorizontalSpeed => Mathf.Abs(rb.velocity.x);
}
