using UnityEngine;
using System.Collections;

public class HitboxTester : MonoBehaviour, IDamageReceiver
{
    public Material idle;

    public Material[] hits;

    public new MeshRenderer renderer;

    [Range(0, 5)]
    public int hitcount = 0;

    private int hitsReceived = 0;

    public bool cooldown = false;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if ( hitsReceived > 0 && !cooldown )
        {
            // Debug.Log(name + " counted total of " + hitReceived);

            cooldown = true;

            if (hitcount < hits.Length)
            {
                renderer.material = hits[hitcount];
            }
            hitcount++;

            hitsReceived = 0;

            StartCoroutine(DelayIdle(hitcount));
            StartCoroutine(HitCooldown());
        }
    }

    public void TakeDamage(int damage)
    {
        if ( !cooldown )
            hitsReceived++;
    }

    IEnumerator DelayIdle(int count)
    {
        yield return new WaitForSeconds(2f);

        if (count == hitcount)
        {
            renderer.material = idle;
            hitcount = 0;
        }

        yield return null;
    }

    IEnumerator HitCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        cooldown = false;
    }
}
