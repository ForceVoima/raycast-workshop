using UnityEngine;

public class Raycast2D : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    void Start ()
    {
        _lineRenderer = GetComponent<LineRenderer>();
	}

	void Update ()
    {
        var point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        point.z = 0f;

        TryRaycast(transform.position, point);
	}

    private void TryRaycast(Vector2 from, Vector2 to)
    {
        RaycastHit2D hit = Physics2D.Raycast(from, to - from);

        if (hit)
        {
            Debug.Log("Raycast hit in: " + hit.transform.name);

            UpdateLaser(from, hit.point);
        }
    }

    private void UpdateLaser(Vector2 start, Vector2 end)
    {
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }
}
