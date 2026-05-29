using System.Collections;
using UnityEngine;

// Marks a sample as collectible. The player's interactor finds the nearest interactable and
// calls Interact() when the hold fills; the sample plays the grab animation then collects.
// On collection it plays a short absorb effect (pop + light flare, then shrink away).
public class Collectible : MonoBehaviour, IInteractable
{
    private bool taken;

    public Transform Anchor { get { return transform; } }

    void OnEnable() { Interactables.All.Add(this); }
    void OnDisable() { Interactables.All.Remove(this); }

    public void Interact(PlayerInteractor interactor)
    {
        interactor.DoAction("Grab", () => Collect(interactor.Manager));
    }

    public void Collect(GameManager gm)
    {
        if (taken) return;
        taken = true;
        Interactables.All.Remove(this);
        if (gm != null) gm.currentPickups++;
        StartCoroutine(Absorb());
    }

    IEnumerator Absorb()
    {
        var spin = GetComponent<SampleSpin>();
        if (spin != null) spin.enabled = false;
        var light = GetComponent<Light>();
        Vector3 baseScale = transform.localScale;

        // quick pop + light flare
        float t = 0f;
        while (t < 0.12f)
        {
            t += Time.deltaTime;
            float k = t / 0.12f;
            transform.localScale = baseScale * (1f + 0.4f * k);
            if (light != null) light.intensity = 2.2f + 4f * k;
            yield return null;
        }

        // shrink away and fade the light
        Vector3 popScale = transform.localScale;
        float startIntensity = light != null ? light.intensity : 0f;
        t = 0f;
        while (t < 0.22f)
        {
            t += Time.deltaTime;
            float k = t / 0.22f;
            transform.localScale = Vector3.Lerp(popScale, Vector3.zero, k);
            if (light != null) light.intensity = Mathf.Lerp(startIntensity, 0f, k);
            yield return null;
        }

        Destroy(gameObject);
    }
}
