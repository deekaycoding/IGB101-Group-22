using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Walk up to an interactable (a sample or the gate lever); a floating key-prompt appears
// above it, hold the interact key and a ring fills to confirm, then the interactable acts
// (the player plays its animation and the effect runs).
public class PlayerInteractor : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Q;
    public float range = 2.8f;
    public float holdTime = 0.5f;
    public float grabDelay = 0.8f;
    public Animator anim;

    [Header("Floating prompt")]
    public Transform promptRoot;   // world-space prompt, positioned above the target
    public Image ringFill;         // radial fill that shows hold progress
    public float heightAbove = 1.4f;

    [Header("Locked hint popup")]
    public GameObject lockedHint;  // shown briefly when a locked interactable is used early
    public float lockedHintTime = 2.2f;

    private GameManager gm;
    private bool busy;
    private float hold;
    private float hintTimer;

    public GameManager Manager { get { return gm; } }

    void Start()
    {
        var g = GameObject.FindGameObjectWithTag("GameManager");
        if (g != null) gm = g.GetComponent<GameManager>();
        HidePrompt();
        if (lockedHint != null) lockedHint.SetActive(false);
    }

    void Update()
    {
        if (hintTimer > 0f)
        {
            hintTimer -= Time.deltaTime;
            if (hintTimer <= 0f && lockedHint != null) lockedHint.SetActive(false);
        }

        if (Time.timeScale == 0f) { HidePrompt(); return; }
        var target = busy ? null : FindNearest();
        if (target == null) { HidePrompt(); hold = 0f; return; }

        ShowPromptAbove(target.Anchor);

        if (Input.GetKey(interactKey)) hold += Time.deltaTime;
        else hold = 0f;
        if (ringFill != null) ringFill.fillAmount = Mathf.Clamp01(hold / holdTime);

        if (hold >= holdTime) { hold = 0f; target.Interact(this); }
    }

    void ShowPromptAbove(Transform t)
    {
        if (promptRoot == null || t == null) return;
        if (!promptRoot.gameObject.activeSelf) promptRoot.gameObject.SetActive(true);
        promptRoot.position = t.position + Vector3.up * heightAbove;
        var cam = Camera.main;
        if (cam != null) promptRoot.rotation = cam.transform.rotation; // billboard to camera
    }

    void HidePrompt()
    {
        if (promptRoot != null && promptRoot.gameObject.activeSelf) promptRoot.gameObject.SetActive(false);
        if (ringFill != null) ringFill.fillAmount = 0f;
    }

    IInteractable FindNearest()
    {
        IInteractable best = null;
        float bestDist = range;
        foreach (var it in Interactables.All)
        {
            if (it == null || it.Anchor == null) continue;
            float d = Vector3.Distance(transform.position, it.Anchor.position);
            if (d <= bestDist) { bestDist = d; best = it; }
        }
        return best;
    }

    // Plays an action animation, waits for it to land, then runs the effect. Blocks other
    // interactions while it runs. The delay lets each action match its own animation timing.
    public void DoAction(string trigger, System.Action effect)
    {
        DoAction(trigger, grabDelay, effect);
    }

    public void DoAction(string trigger, float delay, System.Action effect)
    {
        StartCoroutine(ActionRoutine(trigger, delay, effect));
    }

    IEnumerator ActionRoutine(string trigger, float delay, System.Action effect)
    {
        busy = true;
        HidePrompt();
        if (anim != null && !string.IsNullOrEmpty(trigger)) anim.SetTrigger(trigger);
        yield return new WaitForSeconds(delay);
        if (effect != null) effect();
        busy = false;
    }

    public void ShowLockedHint()
    {
        if (lockedHint != null) { lockedHint.SetActive(true); hintTimer = lockedHintTime; }
    }
}
