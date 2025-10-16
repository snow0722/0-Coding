using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CollidedFall : MonoBehaviour
{
    private bool isFalling = false;
    private Quaternion originalRotation;
    private AudioSource audioSource;

    [Header("角度與動畫設定")]
    [Tooltip("倒下角度")]
    public float fallAngle = -90f;

    [Tooltip("倒下動畫時間（時間越短倒下越快）")]
    public float fallDuration = 0.3f;

    [Tooltip("倒下後等待多久再站起")]
    public float recoverDelay = 2f;

    [Tooltip("站起動畫時間（時間越短站起越快）")]
    public float recoverDuration = 0.3f;

    [Header("音效設定")]
    [Tooltip("倒下時播放的音效")]
    public AudioClip fallSound;

    [Tooltip("立起時播放的音效")]
    public AudioClip standSound;

    [Header("3D音效參數")]
    [Tooltip("距離小於這個值時音量維持最大")]
    public float minDistance = 3f;

    [Tooltip("超過這距離後音量會逐漸減弱到無聲")]
    public float maxDistance = 20f;

    [Tooltip("聲音衰減模式")]
    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

    void Start()
    {
        originalRotation = transform.rotation;

        // 取得或建立 AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // 設定為 3D 音效
        audioSource.spatialBlend = 1.0f;   // ✅ 完全 3D 聲音
        audioSource.playOnAwake = false;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = rolloffMode;

        // 一開始倒下（不改動你的原始邏輯）
        float angleX = fallAngle;
        transform.rotation = Quaternion.Euler(angleX, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isFalling) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // 原本的方向判定方式（不改動）
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
        Quaternion targetRotation = Quaternion.Euler(angleX, transform.eulerAngles.y, transform.eulerAngles.z);

        // 播放倒下音效
        PlaySound(fallSound);

        // 慢慢倒下
        yield return StartCoroutine(RotateOverTime(transform.rotation, targetRotation, fallDuration));

        // 等待回復
        yield return new WaitForSeconds(recoverDelay);

        // 播放立起音效
        PlaySound(standSound);

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
            StartCoroutine(FallAndRecover(forward));
    }

    public void StandUp()
    {
        if (!isFalling)
        {
            PlaySound(standSound);
            StartCoroutine(RotateOverTime(transform.rotation, originalRotation, recoverDuration));
        }
    }

    public void FallDown(bool forward = true)
    {
        if (!isFalling)
        {
            float angleX = forward ? fallAngle : -fallAngle;
            Quaternion targetRotation = Quaternion.Euler(angleX, transform.eulerAngles.y, transform.eulerAngles.z);
            PlaySound(fallSound);
            StartCoroutine(RotateOverTime(transform.rotation, targetRotation, fallDuration));
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f); // 小幅隨機音高
            audioSource.volume = Random.Range(0.9f, 1f);    // 小幅隨機音量
            audioSource.PlayOneShot(clip);
        }
    }
}