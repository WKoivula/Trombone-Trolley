using System.Collections;
using UnityEngine;

public class NoteGlowOnHit : MonoBehaviour
{
    [Header("Shader property settings")]
    public string rimPowerProperty = "_RimPower";
    public string rimColorProperty = "_RimColor";
    public float maxRimPower = 100f;
    public float glowDuration = 0.2f;

    private Renderer targetRenderer;
    private Material matInstance;
    private bool isPlaying = false;

    void Awake()
    {
        targetRenderer = GetComponent<Renderer>();
        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();

        if (targetRenderer != null)
        {
            matInstance = targetRenderer.material;
            if (matInstance.HasProperty(rimPowerProperty))
                matInstance.SetFloat(rimPowerProperty, 0f);
        }
    }

    public void SetRimColor(Color color)
    {
        if (matInstance == null)
            return;

        if (matInstance.HasProperty(rimColorProperty))
            matInstance.SetColor(rimColorProperty, color);
    }

    public void PlayGlowAndDespawn(Vector3 stopPosition)
    {
        if (!isPlaying && matInstance != null)
            StartCoroutine(GlowRoutine(stopPosition));
    }

    private IEnumerator GlowRoutine(Vector3 stopPos)
    {
        isPlaying = true;

        float t = 0f;
        Vector3 startPos = transform.position;
        float startRim = 0f;

        while (t < glowDuration)
        {
            t += Time.deltaTime;
            float x = Mathf.Clamp01(t / glowDuration);

            float moveCurve = 1f - Mathf.Pow(1f - x, 3f);

            float glowCurve = x * x;

            transform.position = Vector3.Lerp(startPos, stopPos, moveCurve);

            if (matInstance.HasProperty(rimPowerProperty))
            {
                float rim = Mathf.Lerp(startRim, maxRimPower, glowCurve);
                matInstance.SetFloat(rimPowerProperty, rim);
            }

            yield return null;
        }

        transform.position = stopPos;
        Destroy(gameObject);
    }
}
