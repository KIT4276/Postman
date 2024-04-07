using System.Collections;
using TMPro;
using UnityEngine;

public class SaveTriggerUI : MonoBehaviour
{
    [SerializeField] private SaveTrigger _saveTrigger;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _fadeTime = 6;

    private float _timer;

    private void Start() =>
        _saveTrigger.PlayerEnter += ShowUI;

    private void ShowUI()
    {
        _text.alpha = 1;
        StartCoroutine(HideUI());
    }

    private IEnumerator HideUI()
    {
        _timer = _fadeTime;

        while (_timer > 0)
        {
            _timer -= Time.deltaTime;

            _text.alpha = (1f / _fadeTime) * _timer;

            yield return null;
        }

        _text.alpha = 0;
    }
}
