using System.Collections.Generic;
using UnityEngine;

// Gently pulses the portal's emission and point light so the gateway feels alive,
// without the scattered sparkle look. Animates instanced materials so it is safe at runtime.
public class PortalPulse : MonoBehaviour
{
    public Color color = new Color(0.25f, 0.7f, 1f);
    public float speed = 2f;
    public float minIntensity = 1.3f;
    public float maxIntensity = 2.7f;

    private Material[] mats;
    private Light glow;

    void Start()
    {
        var list = new List<Material>();
        foreach (var r in GetComponentsInChildren<Renderer>())
        {
            var m = r.material; // instanced copy, safe to animate
            m.EnableKeyword("_EMISSION");
            list.Add(m);
        }
        mats = list.ToArray();
        glow = GetComponentInChildren<Light>();
    }

    void Update()
    {
        float t = Mathf.Sin(Time.time * speed) * 0.5f + 0.5f; // 0..1
        float k = Mathf.Lerp(minIntensity, maxIntensity, t);
        if (mats != null)
            foreach (var m in mats) m.SetColor("_EmissionColor", color * k);
        if (glow != null) glow.intensity = k;
    }
}
