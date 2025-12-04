using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Vairables
    [SerializeField] private float moveSpeed = 10f; // Represents how fast the player can move

    private Vector2 moveInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveInput = GetInputs();
    }

    private void FixedUpdate()
    {
        HandleMovement(moveInput);
        HandleRotation();
    }

    private Vector2 GetInputs()
    {
        // Player inputs wasd and arrow keys
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        return new Vector2 (horizontalInput, verticalInput);
    }

    private void HandleMovement(Vector2 input)
    {
        // Apply the move input
        rb.linearVelocity = input.normalized * moveSpeed;
    }

    private void HandleRotation()
    {
        // Calculate the mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Calculate the direction from mousePos to player
        Vector3 pointerDirection = mousePos - transform.position;
        // float in degrees
        float rotationAng = Mathf.Atan2(pointerDirection.y, pointerDirection.x) * Mathf.Rad2Deg;

        // only rotate if mouse moved
        if (pointerDirection.sqrMagnitude > 0.01f)
            rb.SetRotation(rotationAng);
    }
}
