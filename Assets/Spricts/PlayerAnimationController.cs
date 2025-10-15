using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    private PlayerJumpFloat jumpScript;

    [Header("Quad 模型旋轉根節點")]
    [SerializeField] private Transform modelTransform; // Quad 本身

    [Header("旋轉時間 (秒)")]
    [SerializeField] private float rotateDuration = 0.25f;

    private bool isRotating360 = false;
    private int lastFacing = 1; // 1 = 右, -1 = 左

    void Start()
    {
        anim = GetComponent<Animator>();
        jumpScript = GetComponent<PlayerJumpFloat>();

        if (modelTransform == null)
            modelTransform = transform;
    }

    public void UpdateAnimation(float horizontalInput)
    {
        if (jumpScript == null) return;

        bool isGrounded = jumpScript.IsGrounded;
        bool isCharging = jumpScript.IsJumpHeld && isGrounded;
        float horizontalSpeed = Mathf.Abs(horizontalInput);

        // 動畫控制
        anim.SetBool("isJumping", !isGrounded);
        anim.SetBool("isCharging", isCharging);
        anim.SetBool("isWalking", isGrounded && horizontalSpeed > 0.1f);
        anim.SetBool("isIdle", isGrounded && horizontalSpeed <= 0.1f && !isCharging);

        // 左右翻轉 (scale)
        int currentFacing = horizontalInput > 0.01f ? 1 : horizontalInput < -0.01f ? -1 : lastFacing;
        HandleFacingDirection(currentFacing);

        // 如果方向改變才旋轉
        if (!isRotating360 && currentFacing != lastFacing)
        {
            StartCoroutine(Rotate360());
        }

        lastFacing = currentFacing;
    }

    void HandleFacingDirection(int facing)
    {
        modelTransform.localScale = new Vector3(facing == 1 ? -1 : 1, 1, 1);
    }

    private IEnumerator Rotate360()
    {
        isRotating360 = true;

        float elapsed = 0f;
        float startY = modelTransform.eulerAngles.y;
        float endY = startY + 360f;

        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotateDuration);
            float currentY = Mathf.Lerp(startY, endY, t);
            modelTransform.rotation = Quaternion.Euler(0f, currentY, 0f);
            yield return null;
        }

        modelTransform.rotation = Quaternion.Euler(0f, endY, 0f);
        isRotating360 = false;
    }
}

