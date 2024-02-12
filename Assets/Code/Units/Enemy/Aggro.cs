using System;
using System.Collections;
using UnityEngine;

public class Aggro : MonoBehaviour
{
    [SerializeField]
    private TriggerObserver TriggerObserver;
    [SerializeField]
    private Follow _follow;
    [SerializeField]
    private float CoolDown;

    private Coroutine _aggrooCoroutine;
    private bool _hasAggroTArget;

    private void Start()
    {
        TriggerObserver.TriggerEnter += TriggerEnter;
        TriggerObserver.TriggerExit += TriggerExit;

        SwitchFollowOff();
    }

    public void Restart()
    {
        TriggerObserver.TriggerEnter += TriggerEnter;
        TriggerObserver.TriggerExit += TriggerExit;

        StopAggroCoroutine();
    }

    private void TriggerEnter(Collider obj)
    {
        if (!_hasAggroTArget)
        {
            _hasAggroTArget = true;
            StopAggroCoroutine();

            SwitchFollowOn();
        }
    }

    private void TriggerExit(Collider obj)
    {
        if (_hasAggroTArget)
        {
            _hasAggroTArget = false;

            _aggrooCoroutine = StartCoroutine(SwitchFollowOfAfterCollDown());
        }
    }

    private void StopAggroCoroutine()
    {
        if (_aggrooCoroutine != null)
        {
            StopCoroutine(_aggrooCoroutine);
            _aggrooCoroutine = null;
        }
    }

    private IEnumerator SwitchFollowOfAfterCollDown()
    {
        yield return new WaitForSeconds(CoolDown);

        SwitchFollowOff();
    }

    private void SwitchFollowOn() =>
        _follow.enabled = true;

    private void SwitchFollowOff() =>
        _follow.enabled = false;
}
