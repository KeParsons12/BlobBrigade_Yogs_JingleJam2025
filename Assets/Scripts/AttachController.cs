using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attaching:
///     If objects that are attachable are inside the trigger
///     then allow player to attach the closest object
///     or
///     click on the object they would like to attach
/// 
/// Detaching:
///     If player decides to detach an object then the object at index 0 will be remove 
///     or 
///     the click on attached object will be removed
/// </summary>
public class AttachController : MonoBehaviour
{
    [SerializeField] private int maxAttachableObjects;
    [SerializeField] private float attachRadius = 1.5f;

    // FOR NOW SERIALIZEFIELD remove later and make it private
    private List<IAttachable> attachedObjects = new List<IAttachable>();
    private List<IAttachable> objectsInRange = new List<IAttachable>();

    private void Update()
    {
        // EXAMPLE INPUT
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryAttachClosest();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DetachObject(0);
        }
    }

    private void TryAttachClosest()
    {
        if (attachedObjects.Count >= maxAttachableObjects || objectsInRange.Count == 0)
            return;

        IAttachable closest = GetClosestObject();
        AttachObject(closest);
    }

    private IAttachable GetClosestObject()
    {
        IAttachable closest = null;
        float shortestDist = Mathf.Infinity;

        foreach (IAttachable obj in objectsInRange)
        {
            float dist = Vector2.Distance(transform.position, obj.Transform.position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                closest = obj;
            }
        }

        return closest;
    }

    private void AttachObject(IAttachable objToAttach)
    {
        if (!objToAttach.CanBeAttached) return; // check if the object can be attached (Is object in attachable state)
        if (objToAttach.IsAttached) return; // check if the object is already attached
        if (attachedObjects.Contains(objToAttach)) return; // check if the object is already in our list of attached objects
        if (attachedObjects.Count >= maxAttachableObjects) return; // check if the list is full

        // add the object to the attached list
        attachedObjects.Add(objToAttach);

        // parent the object to the player
        objToAttach.Transform.SetParent(this.transform);

        // call the objects IAttachable onAttach()
        objToAttach.OnAttach();

        UpdateAttachedObjectPositions();
    }

    private void DetachObject(IAttachable objToAttach)
    {
        if (!attachedObjects.Contains(objToAttach)) return;

        // remove the object from the list
        attachedObjects.Remove(objToAttach);
        // remove the object from the parent
        objToAttach.Transform.SetParent(null);
        // call the OnDetach on the IAttachable
        objToAttach.OnDetach();

        UpdateAttachedObjectPositions();
    }

    private void DetachObject(int index)
    {
        if (attachedObjects.Count == 0 || index < 0 || index >= attachedObjects.Count) return;

        // cache a reference to the attach object
        IAttachable objToAttach = attachedObjects[index];
        // remove the object from the list at index
        attachedObjects.RemoveAt(index);
        // remove the object from the parent
        objToAttach.Transform.SetParent(null);
        // call the OnDetach on the IAttachable
        objToAttach.OnDetach();

        UpdateAttachedObjectPositions();
    }

    private void UpdateAttachedObjectPositions()
    {
        float step = 360f / attachedObjects.Count;

        for (int i = 0; i < attachedObjects.Count; i++)
        {
            float angle = step * i * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 1.5f; // attach radius
            attachedObjects[i].Transform.localPosition = offset;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IAttachable attachable = other.GetComponent<IAttachable>();
        if (attachable != null && !objectsInRange.Contains(attachable) && attachable.IsAttached == false)
        {
            objectsInRange.Add(attachable);
            Debug.Log("Object is in range...");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IAttachable attachable = other.GetComponent<IAttachable>();
        if (attachable != null)
        {
            objectsInRange.Remove(attachable);
            Debug.Log("Object is out of range...");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IAttachable attachable = collision.gameObject.GetComponent<IAttachable>();
        if (attachable != null)
        {
            attachable.EnableAttachWindow(3f);
        }    
    }
}
