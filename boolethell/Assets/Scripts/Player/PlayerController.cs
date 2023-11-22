using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerModel;
    [SerializeField] private float playerSpeed;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] private BoolList controlsDisabled;
    [SerializeField] private BoolList playerInvincible;
    [SerializeField] private BoolVariable dashInvincibility;
 
    private Vector3 dashDirection = Vector3.zero;
    private bool isDashing = false;
    private bool canDash = true;
    [SerializeField] private FloatVariable dashCooldown;
    [SerializeField] private FloatVariable dashDuration;
    [SerializeField] private FloatVariable dashDistance;
    [SerializeField] private FloatVariable dashInvincibilityDuration;

    void Update()
    {
        if (controlsDisabled.IsAnyTrue())
            return;

        RotateTowardsCursor();

        if (canDash && Input.GetKey(KeyCode.LeftShift))
            TryDash();
    }

    private void FixedUpdate()
    {
        if (controlsDisabled.IsAnyTrue())
            return;

        Move();
        if (isDashing)
            Dash();
    }

    private void RotateTowardsCursor()
    {
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        playerModel.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    private void Move()
    {
        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * Time.fixedDeltaTime * playerSpeed;
        rb.MovePosition((Vector2)transform.position + moveVector);
    }

    private void TryDash()
    {
        dashDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized;
        if (dashDirection == Vector3.zero) return;

        StartCoroutine(DashProcedure());
    }

    private void Dash()
    {
        float dashSpeed = dashDistance.Value / dashDuration.Value;
        Vector2 moveVector = dashDirection * Time.fixedDeltaTime * dashSpeed;
        rb.MovePosition((Vector2)transform.position + moveVector);
    }

    private IEnumerator DashProcedure()
    {
        float timer = 0f;
        isDashing = true;
        canDash = false;
        playerInvincible.AddVariable(dashInvincibility);

        for (; ;)
        {
            timer += Time.deltaTime;

            if (timer >= dashDuration.Value)
                isDashing = false;

            if (timer >= dashInvincibilityDuration.Value)
                playerInvincible.RemoveVariable(dashInvincibility);

            if (timer >= dashCooldown.Value)
            {
                canDash = true;
                break;
            }

            yield return null;
        }
    }
}
