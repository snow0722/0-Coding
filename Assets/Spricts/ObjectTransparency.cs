using UnityEngine;

public class ObjectTransparency : MonoBehaviour
{
    [Header("目標角色")]
    public Transform target;

    [Header("透明度設定")]
    [Tooltip("最透明的透明度")]
    [Range(0f, 1f)]
    public float transparentAlpha = 0.3f;

    [Header("最大 Z 軸距離")]
    [Tooltip("超過此值則完全不透明")]
    [Range(0.1f, 10f)]
    public float maxDistance = 5f;

    [Header("啟動距離")]
    [Tooltip("實際距離小於此值才啟動透明")]
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
        float worldDistance = Vector3.Distance(target.position, transform.position);

        //如果角色的Z大於物件的Z(在物件後方) 並 兩者距離小於啟動距離
        if (target.position.z > transform.position.z && worldDistance < activationDistance)
        {
            //兩者Z相減的絕對值
            float zDistance = Mathf.Abs(transform.position.z - target.position.z);
            //越靠近越透明
            float t = Mathf.InverseLerp(0f, maxDistance, zDistance);
            targetAlpha = Mathf.Lerp(transparentAlpha, originalAlpha, t);
        }

        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);

        Color color = objRenderer.material.color;
        color.a = currentAlpha;
        objRenderer.material.color = color;
    }
}