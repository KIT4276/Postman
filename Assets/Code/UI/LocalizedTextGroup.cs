using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizedTextGroup : MonoBehaviour
{
    private readonly List<TMP_Text> _texts = new();
    private readonly List<string> _sourceTexts = new();

    private ILocalizationService _localizationService;

    public void Init(ILocalizationService localizationService)
    {
        if (_localizationService != null)
            _localizationService.LanguageChanged -= UpdateTexts;

        _localizationService = localizationService;

        _texts.Clear();
        _sourceTexts.Clear();

        foreach (TMP_Text text in GetComponentsInChildren<TMP_Text>(true))
        {
            if (!_localizationService.HasTranslation(text.text))
                continue;

            _texts.Add(text);
            _sourceTexts.Add(text.text);
        }

        _localizationService.LanguageChanged += UpdateTexts;
        UpdateTexts(_localizationService.CurrentLanguage);
    }

    private void OnDestroy()
    {
        if (_localizationService != null)
            _localizationService.LanguageChanged -= UpdateTexts;
    }

    private void UpdateTexts(string _)
    {
        for (int i = 0; i < _texts.Count; i++)
        {
            if (_texts[i] == null)
                continue;

            _texts[i].text = _localizationService.Translate(_sourceTexts[i]);
        }
    }
}
