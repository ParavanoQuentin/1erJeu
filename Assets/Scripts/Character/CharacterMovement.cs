using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    private PlayerInput inputs;
    private InputAction moveAction;
    private GameManager manager;
    private InputAction actionKey; // Action pour "Espace"
    private Animator anim;

    private Vector2 velocity = Vector2.zero;
    private float speed = 5f;
    private int direction = 0;

    private int multiplicateur_dash = 4;
    private bool isDashing = false; // Indique si le personnage est en train de dasher

    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.GetInstance();
        if (manager == null)
        {
            Debug.LogError("GameManager instance is null!");
            return;
        }

        inputs = manager.GetInput();
        if (inputs == null)
        {
            Debug.LogError("PlayerInput is null! Check GameManager.GetInput().");
            return;
        }

        moveAction = inputs.actions.FindAction("Move");
        if (moveAction == null)
        {
            Debug.LogError("Move action not found! Check Input Actions asset.");
        }

        actionKey = inputs.actions.FindAction("Roulade");
        if (actionKey == null)
        {
            Debug.LogError("Roulade action not found! Check Input Actions asset.");
        }
        else
        {
            actionKey.performed += OnActionKeyPressed;
        }

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component is missing on this GameObject!");
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isDashing)
        {
            // Ignore le mouvement normal pendant le dash
            return;
        }

        if (moveAction == null)
        {
            Debug.LogWarning("Move action is null. Skipping movement.");
            return;
        }

        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        velocity = moveValue * speed;

        direction = AnimationDeplacement(velocity);

        if (anim != null)
        {
            anim.SetInteger("direction", direction);
        }

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

    private void OnActionKeyPressed(InputAction.CallbackContext context)
    {
        if (!isDashing) // Empêche plusieurs dashs en même temps
        {
            if (velocity == Vector2.zero)
            {
                Debug.LogWarning("Dash attempted but no movement detected.");
                return;
            }
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;

        int dashFrames = 30; // Nombre de frames pour le dash
        Vector3 dashVelocity = new Vector3(velocity.x, velocity.y, 0).normalized * multiplicateur_dash;

        for (int i = 0; i < dashFrames; i++)
        {
            if (dashVelocity == Vector3.zero)
            {
                Debug.LogWarning("Dash velocity is zero. Aborting dash.");
                break;
            }

            // Ajoute le déplacement du dash pour cette frame
            transform.position += dashVelocity * Time.fixedDeltaTime;

            // Attend la fin de la frame actuelle avant de continuer
            yield return null;
        }

        isDashing = false;
    }
}