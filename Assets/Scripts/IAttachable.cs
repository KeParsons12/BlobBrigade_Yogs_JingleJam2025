using UnityEngine;

public interface IAttachable
{
    Transform Transform { get; }
    bool IsAttached { get; }
    bool CanBeAttached { get; }

    // Called when this object is attached
    void OnAttach();

    // Called when this object is detached
    void OnDetach();

    // Called externally to enable a temporary attachable window
    void EnableAttachWindow(float duration);
}
