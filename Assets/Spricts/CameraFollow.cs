using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [Header("要跟隨的目標 (必須指定)")]
    public Transform target;

    [Header("平滑時間 (越小越快)")]
    public float smoothTime = 0.12f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("[SmoothFollow] 沒有指定目標，攝影機不會移動。");
            return;
        }

        // 目標的 x，保持攝影機現有的 y 和 z
        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, transform.position.z);

        // 平滑過渡
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}