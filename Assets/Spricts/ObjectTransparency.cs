using UnityEngine;

public class ObjectTransparency : MonoBehaviour
{
    [Header("目標角色")]
    public Transform target;

    [Header("透明度設定")]
    [Tooltip("最透明的透明度")]
    [Range(0f, 1f)]
    public float transparentAlpha = 0.3f;

    [Header("最大距離 (XZ平面)")]
    [Tooltip("超過此值則完全不透明")]
    [Range(0.1f, 10f)]
    public float maxDistance = 5f;

    [Header("啟動距離")]
    [Tooltip("距離小於此值才啟動透明")]
    [Range(0.1f, 10f)]
    public float activationDistance = 1f;

    [Header("透明度變化速度")]
    [Tooltip("越大越快")]
    [Range(0.1f, 20f)]
    public float fadeSpeed = 5f;

    private float originalAlpha = 1f;
    private float currentAlpha;
    private Renderer objRenderer;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        currentAlpha = originalAlpha;

        if (target == null)
        {
            Debug.LogWarning("目標角色呢！");
        }
    }

    void Update()
    {
        if (target == null || objRenderer == null) return;

        float targetAlpha = originalAlpha;

        // 取得 XZ 平面距離（忽略 Y 高度）
        Vector3 targetPosXZ = new Vector3(target.position.x, 0f, target.position.z);
        Vector3 objectPosXZ = new Vector3(transform.position.x, 0f, transform.position.z);
        float flatDistance = Vector3.Distance(targetPosXZ, objectPosXZ);

        // 如果角色的Z大於物件的Z(在物件後方) 並且XZ平面距離小於啟動距離
        if (target.position.z > transform.position.z && flatDistance < activationDistance)
        {
            float zDistance = Mathf.Abs(transform.position.z - target.position.z);
            float t = Mathf.InverseLerp(0f, maxDistance, zDistance);
            targetAlpha = Mathf.Lerp(transparentAlpha, originalAlpha, t);
        }

        // 平滑變化透明度
        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);

        Color color = objRenderer.material.color;
        color.a = currentAlpha;
        objRenderer.material.color = color;
    }
}