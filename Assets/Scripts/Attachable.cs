using System.Collections;
using UnityEngine;

public class Attachable : MonoBehaviour, IAttachable
{
    public Transform Transform => this.transform;

    public bool IsAttached { get => isAttached; }
    private bool isAttached = false;

    public bool CanBeAttached { get => canBeAttached; }
    private bool canBeAttached = false;
    private Coroutine attachWindowRoutine;

    private LayerMask originalLayer;

    // USED FOR TESTING
    private SpriteRenderer sprite => this.gameObject.GetComponent<SpriteRenderer>();

    public void OnAttach()
    {
        isAttached = true;
        canBeAttached = false;

        originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Attached");
    }

    public void OnDetach()
    {
        isAttached = false;
        canBeAttached = true;
        
        gameObject.layer = originalLayer;
    }

    public void EnableAttachWindow(float duration)
    {
        // Prevent stacking multiple Timers
        if (attachWindowRoutine != null)
            StopCoroutine(attachWindowRoutine);
        
        attachWindowRoutine = StartCoroutine(AttachWindowTimer(duration));
    }

    private IEnumerator AttachWindowTimer(float duration)
    {
        canBeAttached = true;
        sprite.color = Color.red;
        yield return new WaitForSeconds(duration);
        canBeAttached = false;
        sprite.color = Color.white;
    }
}
