using UnityEngine;

public class HitboxSpawner : MonoBehaviour
{
    [Range(0.5f, 5f)]
    public float width = 5f;

    [Range(1f, 10f)]
    public float length = 10f;

    [Range(0.05f, 0.5f)]
    public float boxSize = 0.20f;

    public Vector3 offset = new Vector3(1f, 1f, 0f);

    private int columns = 5;
    private int rows = 5;

    private Vector3 boxPos = new Vector3();
    private Vector3 boxScale = new Vector3(0.5f, 2f, 0.5f);

    public GameObject hitboxTester;

    public int totalBoxes = 0;

    private void Start()
    {
        columns = (int)(width / boxSize);
        rows = (int)(length/ boxSize);

        boxScale.x = boxSize * 0.9f;
        boxScale.y = 2f;
        boxScale.z = boxSize * 0.9f;

        boxPos.y = offset.y;

        for (int j = 0; j < rows; j++)
        {
            boxPos.z = offset.z + ((j * 1f) / (rows * 1f) * length) - length/2f;

            for (int i = 0; i < columns; i++)
            {
                boxPos.x = offset.x + ((i * 1f) / (columns * 1f) * width);
                SpawnBox();
            }
        }
    }

    private void SpawnBox()
    {
        totalBoxes++;

        GameObject go = Instantiate(hitboxTester, boxPos, transform.rotation, transform);
        go.name = hitboxTester.name + " " + totalBoxes;
        go.transform.localScale = boxScale;
    }
}
