using UnityEngine;
using System.Collections;

public class Raycast2DBounce : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    public int reflections = 5;
    private int reflectionCounter = 0;

    public Vector2[] points;
    public Vector2[] directions;

    private Vector3 mousePoint;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.positionCount = reflections;
        
        points = new Vector2[reflections];
        directions = new Vector2[reflections];
    }

    void Update()
    {
        mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        mousePoint.z = 0f;
        
        points[0] = transform.position;
        directions[0] = mousePoint - transform.position;
        directions[0].Normalize();

        reflectionCounter = 1;

        while (reflectionCounter < reflections)
        {
            RaycastDirection(reflectionCounter);
            reflectionCounter++;
        }

        for (int i = 0; i < reflections; i++)
        {
            _lineRenderer.SetPosition(i, points[i]);
        }
    }

    private void RaycastDirection(int index)
    {
        Vector2 from = new Vector2(points[index - 1].x, points[index - 1].y);
        Vector2 dir = new Vector2(directions[index - 1].x, directions[index - 1].y);

        RaycastHit2D hit = Physics2D.Raycast(from, dir);

        if (hit)
        {
            dir = dir - Vector2.Dot(dir, hit.normal) * 2f * hit.normal;
            dir.Normalize();
            points[index] = hit.point + dir * 0.1f;
            directions[index] = dir;
        }
    }
}
