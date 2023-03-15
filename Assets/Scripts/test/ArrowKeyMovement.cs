using UnityEngine;

public class ArrowKeyMovement : MonoBehaviour
{
    public float movement_speed = 4.0f;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 current_input = GetInput();
        print(current_input);
        rb.velocity = current_input * movement_speed;
    }

    private Vector2 GetInput()
    {
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(horizontal_input) > 0.0f)
        {
            vertical_input = 0.0f;
        }

        return new Vector2(horizontal_input, vertical_input);
    }
}