using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCast2D : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    [SerializeField]
    private Vector2 size = new Vector2(1f, 2f);

    [SerializeField, Range(-90f, 90f)]
    private float _angle = 0f;

    [SerializeField]
    private Transform boxPosition;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        var point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        point.z = 0f;

        Vector2 direction = point - transform.position;
        _angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        TryRaycast(transform.position, point);

        UpdateLaser(transform.position, point);
    }

    private void TryRaycast(Vector2 from, Vector2 to)
    {
        RaycastHit2D hit = Physics2D.CapsuleCast(from, size, CapsuleDirection2D.Vertical, _angle, to - from);

        if (hit)
        {
            Debug.Log("Raycast hit in: " + hit.transform.name);

            if (boxPosition != null)
            {
                boxPosition.position = hit.centroid;
                Quaternion rotation = boxPosition.rotation;
                rotation.eulerAngles = new Vector3(0f, 0f, _angle);
                boxPosition.rotation = rotation;
            }
        }
    }

    private void UpdateLaser(Vector2 start, Vector2 end)
    {
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }
}
