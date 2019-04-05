using UnityEngine;
using System.Collections;

public class SwordSwinger : MonoBehaviour
{
    [SerializeField, Range(0f, 180f)]
    private float _startAngle = 15f;

    [SerializeField, Range(0f, 180f)]
    private float _endAngle = 165f;

    [SerializeField, Range(0f, 90f)]
    private float _elbowAngle = 75f;

    [SerializeField, Range(0.01f, 2f)]
    private float _attackDuration = 1.0f;

    public float _timer;
    public bool _attacking = false;

    private Quaternion _startRotation, _endRotation;

    public Sword sword;

    // Use this for initialization
    void Start()
    {
        StartCoroutine (InitialDelay() );
    }

    public void StartAttack()
    {
        _timer = 0f;

        _startRotation = Quaternion.Euler(x: 0f,
                                           y: _startAngle + 90f,
                                           z: _elbowAngle );
        _endRotation = Quaternion.Euler( x: 0f,
                                         y: _endAngle + 90f,
                                         z: _elbowAngle);

        transform.rotation = _startRotation;
        _attacking = true;
        sword.StartAttack();
    }

    public void EndAttack()
    {
        transform.rotation = _endRotation;
        _attacking = false;
        StartCoroutine(RepeatAttackDelay());
        sword.EndAttack();
    }

    // Update is called once per frame
    void Update()
    {
        if ( _attacking )
        {
            _timer += Time.deltaTime;

            if ( _timer > _attackDuration )
            {
                EndAttack();
            }
            else
            {
                transform.rotation = Quaternion.Lerp( a: _startRotation,
                                                      b: _endRotation,
                                                      t: SmoothTime( _timer / _attackDuration ) );
            }
        }
    }

    private float SmoothTime(float t)
    {
        return t * t * t * (t * (6f * t - 15f) + 10f);
    }

    private IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(1.0f);
        StartAttack();
    }

    private IEnumerator RepeatAttackDelay()
    {
        yield return new WaitForSeconds(2.0f);
        StartAttack();
    }
}
