using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    private PlayerInput inputs;
    private InputAction moveAction;
    private GameManager manager;
    private Animator anim;

    private Vector2 velocity = Vector2.zero;
    [SerializeField] private float speed = 5f;
    private int direction = 0;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.GetInstance();
        inputs = manager.GetInput();
        moveAction = inputs.actions.FindAction("Move");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        velocity = moveValue * speed;

        direction = AnimationDeplacement(velocity);

        anim.SetInteger("direction", direction);

        transform.position += new Vector3(velocity.x * Time.fixedDeltaTime, velocity.y * Time.fixedDeltaTime, 0);
    }

    private int AnimationDeplacement(Vector2 velocity)
    {
        if (velocity.x < 0 && Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.y))
        {
            return 4;
        }
        if (velocity.x > 0 && Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.y))
        {
            return 6;
        }
        if (velocity.y < 0 && Mathf.Abs(velocity.x) < Mathf.Abs(velocity.y))
        {
            return 2;
        }
        if (velocity.y > 0 && Mathf.Abs(velocity.x) < Mathf.Abs(velocity.y))
        {
            return 8;
        }

        return 0;
    }
}
