using System;

public interface ILocalizationService
{
    event Action<string> LanguageChanged;

    string CurrentLanguage { get; }

    void InitializeFromSystemLanguage();
    void ApplySdkLanguage(string sdkLanguageCode);
    void SetLanguage(string languageCode);
    bool HasTranslation(string sourceText);
    string Translate(string sourceText);
}
