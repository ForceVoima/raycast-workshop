using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
    public float _weaponLength = 2f;

    [SerializeField]
    private bool _attackRunning = false;

    private Vector3 _handle, _tip;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        Debug.DrawLine( start: transform.position,
                        end: transform.position + transform.up * 2f * _weaponLength,
                        color: Color.red,
                        duration: 1.5f );
        */
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_attackRunning)
        {
            DetectHits();
        }
    }

    public void DetectHits()
    {
        _handle = transform.position;
        _tip = transform.position + transform.up * _weaponLength;

        Debug.DrawLine(start: _handle,
                        end: _tip,
                        color: Color.green,
                        duration: 1.5f);
    }

    // Replace this solution!
    public void OnTriggerEnter(Collider other)
    {
        SendDamage( other );
    }

    private void SendDamage( RaycastHit hit )
    {
        SendDamage( hit.collider );
    }

    private void SendDamage( Collider other )
    {
        IDamageReceiver receiver = other.transform.GetComponent<IDamageReceiver>();

        if (receiver != null)
            receiver.TakeDamage(10);
    }

    public void StartAttack()
    {
        _attackRunning = true;
    }

    public void EndAttack()
    {
        _attackRunning = false;
    }
}
