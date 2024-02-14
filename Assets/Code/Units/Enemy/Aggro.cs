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

        SwitchFollow(false);
    }

    public void End()
    {
        StopAggroCoroutine();
        SwitchFollow(false); 
        _hasAggroTArget = false;
    }

    public void Restart()
    {
        TriggerObserver.TriggerEnter += TriggerEnter;
        TriggerObserver.TriggerExit += TriggerExit;

        
    }

    private void TriggerEnter(Collider obj)
    {
        if (!_hasAggroTArget)
        {
            _hasAggroTArget = true;
            StopAggroCoroutine();

            SwitchFollow(true);
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

        SwitchFollow(false);
    }

    private void SwitchFollow(bool value) =>
        _follow.enabled = value;

    //private void SwitchFollowOff() =>
    //    _follow.enabled = false;
}
