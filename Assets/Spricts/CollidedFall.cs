using UnityEngine;
using System.Collections;

public class CollidedFall : MonoBehaviour
{
    private bool isFalling = false;
    private Quaternion originalRotation;

    [Tooltip("倒下角度")]
    public float fallAngle = -90f;         // 倒下角度

    [Tooltip("倒下動畫時間，時間越短，倒下速度越快")]
    public float fallDuration = 0.3f;     // 倒下動畫時間

    [Tooltip("倒下多久")]
    public float recoverDelay = 2f;       // 等待回復時間

    [Tooltip("回復動畫時間，時間越短，站起速度越快")]
    public float recoverDuration = 0.3f;  // 回復動畫時間

    void Start()
    {
        // 記錄初始站立角度
        originalRotation = transform.rotation;
        
        // 開場倒下
        float angleX = fallAngle; // 或 -fallAngle，視模型正面方向
        transform.rotation = Quaternion.Euler(angleX, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isFalling) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            float characterZ = collision.transform.position.z;
            float boardZ = transform.position.z;

            bool fallForward = characterZ > boardZ;

            StartCoroutine(FallAndRecover(fallForward));
        }
    }

    IEnumerator FallAndRecover(bool forward)
    {
        isFalling = true;

        float angleX = forward ? fallAngle : -fallAngle;
        Quaternion targetRotation = Quaternion.Euler(angleX, 0f, 0f);

        // 慢慢倒下
        yield return StartCoroutine(RotateOverTime(transform.rotation, targetRotation, fallDuration));

        // 等待回復
        yield return new WaitForSeconds(recoverDelay);

        // 慢慢回正
        yield return StartCoroutine(RotateOverTime(transform.rotation, originalRotation, recoverDuration));

        isFalling = false;
    }

    IEnumerator RotateOverTime(Quaternion from, Quaternion to, float duration)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Slerp(from, to, t);
            yield return null;
        }
    }

    public void StandUpFromTrigger(bool forward = true)
    {
        if (!isFalling)
        {
            StartCoroutine(FallAndRecover(forward));
        }
    }

    // 立起
    public void StandUp()
    {
        if (!isFalling)
            StartCoroutine(RotateOverTime(transform.rotation, originalRotation, recoverDuration));
    }

    // 倒下
    public void FallDown(bool forward = true)
    {
        if (!isFalling)
        {
            float angleX = forward ? fallAngle : -fallAngle;
            Quaternion targetRotation = Quaternion.Euler(angleX, transform.eulerAngles.y, transform.eulerAngles.z);
            StartCoroutine(RotateOverTime(transform.rotation, targetRotation, fallDuration));
        }
    }
}

