using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private BoxCollider2D _collider;

    private bool _isGrounded = true;
    public bool IsGrounded { get => _isGrounded; }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int a = 1 << collision.gameObject.layer;
        if ((a & LayerMask.GetMask("Ground")) != 0)
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        int a = 1 << collision.gameObject.layer;
        if ((a & LayerMask.GetMask("Ground")) != 0)
        {
            _isGrounded = false;
        }
    }

    private bool CheckGrounded()
    {
        Bounds bounds = _collider.bounds;
        Collider2D hit = Physics2D.OverlapBox(bounds.center, bounds.size, 0f, LayerMask.GetMask("Ground"));
        if (hit != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}