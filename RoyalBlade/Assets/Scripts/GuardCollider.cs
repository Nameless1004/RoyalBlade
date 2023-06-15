using System;
using UnityEngine;

public class GuardCollider : MonoBehaviour
{
    private BoxCollider2D _collider;
    public BoxCollider2D Collider { get { return _collider; } }
    public Action<Collider2D> OnTriggerStay;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Bounds bounds = _collider.bounds;
        var hit = Physics2D.OverlapBox(bounds.center, bounds.size, 0, LayerMask.GetMask("Obstacle"));

        if(hit != null)
        {
            this.OnTriggerStay?.Invoke(hit);
        }
    }
}