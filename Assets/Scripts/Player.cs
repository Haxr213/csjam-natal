using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 moveInput;
    void Update()
    {
        Move(moveInput);
    }

    public void ReadMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Move(Vector2 direction)
    {
        Vector3 movement = new Vector3(direction.x, direction.y, 0f);
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
