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
        originalRotation = transform.rotation;
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
}
