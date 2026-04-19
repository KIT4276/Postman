using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class LocalizationService : ILocalizationService
{
    private const string DefaultLanguage = "en";

    private readonly Dictionary<string, LocalizedEntry> _entries = new();
    private readonly Dictionary<string, string> _lookup = new();
    private readonly HashSet<string> _supportedLanguages = new()
    {
        "ru",
        "en"
    };

    public event Action<string> LanguageChanged;

    public string CurrentLanguage { get; private set; } = DefaultLanguage;

    public LocalizationService()
    {
        Register("start_new_game", "Start new game", "\u041d\u043e\u0432\u0430\u044f \u0438\u0433\u0440\u0430");
        Register("continue_game", "Continue game", "\u041f\u0440\u043e\u0434\u043e\u043b\u0436\u0438\u0442\u044c");
        Register("ok", "OK", "\u041f\u043e\u043d\u044f\u0442\u043d\u043e");
        Register("enemies", "Enemies:", "\u0412\u0440\u0430\u0433\u043e\u0432:");
        Register("delivered", "Delivered parcels:", "\u0414\u043e\u0441\u0442\u0430\u0432\u043b\u0435\u043d\u043e \u043f\u043e\u0441\u044b\u043b\u043e\u043a:");
        Register(
            "post_hint",
            "Go to the post office behind you and pick up the parcel\nThe destination address is marked on the map",
            "\u0417\u0430\u0439\u0434\u0438 \u043d\u0430 \u043f\u043e\u0447\u0442\u0443 \u0441\u0437\u0430\u0434\u0438 \u0442\u0435\u0431\u044f \u0438 \u0437\u0430\u0431\u0435\u0440\u0438 \u043f\u043e\u0441\u044b\u043b\u043a\u0443\n\u0410\u0434\u0440\u0435\u0441 \u043d\u0430\u0437\u043d\u0430\u0447\u0435\u043d\u0438\u044f \u043e\u0442\u043c\u0435\u0447\u0435\u043d \u043d\u0430 \u043a\u0430\u0440\u0442\u0435");
    }

    public void InitializeFromSystemLanguage()
    {
        SetLanguage(Application.systemLanguage == SystemLanguage.Russian ? "ru" : DefaultLanguage);
    }

    public void ApplySdkLanguage(string sdkLanguageCode)
    {
        SetLanguage(sdkLanguageCode);
    }

    public void SetLanguage(string languageCode)
    {
        string normalized = NormalizeLanguage(languageCode);
        if (!_supportedLanguages.Contains(normalized))
            normalized = DefaultLanguage;

        if (CurrentLanguage == normalized)
            return;

        CurrentLanguage = normalized;
        LanguageChanged?.Invoke(CurrentLanguage);
        Debug.Log($"[LocalizationService] Language set to {CurrentLanguage}");
    }

    public bool HasTranslation(string sourceText)
    {
        if (string.IsNullOrWhiteSpace(sourceText))
            return false;

        return _lookup.ContainsKey(NormalizeText(sourceText));
    }

    public string Translate(string sourceText)
    {
        if (string.IsNullOrWhiteSpace(sourceText))
            return sourceText;

        string normalizedSource = NormalizeText(sourceText);
        if (!_lookup.TryGetValue(normalizedSource, out string key))
            return sourceText;

        LocalizedEntry entry = _entries[key];
        return CurrentLanguage == "ru" ? entry.Russian : entry.English;
    }

    private void Register(string key, string english, string russian)
    {
        _entries[key] = new LocalizedEntry(english, russian);
        _lookup[NormalizeText(english)] = key;
        _lookup[NormalizeText(russian)] = key;
    }

    private static string NormalizeLanguage(string languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
            return DefaultLanguage;

        string normalized = languageCode.Trim().ToLowerInvariant();
        string[] separators = { "-", "_" };
        string[] parts = normalized.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        return parts.Length > 0 ? parts[0] : DefaultLanguage;
    }

    private static string NormalizeText(string sourceText)
    {
        string normalized = sourceText
            .Replace("\r\n", "\n")
            .Replace('\r', '\n')
            .Trim();

        normalized = Regex.Replace(normalized, "\\s+", " ");
        return normalized;
    }

    private readonly struct LocalizedEntry
    {
        public LocalizedEntry(string english, string russian)
        {
            English = english;
            Russian = russian;
        }

        public string English { get; }
        public string Russian { get; }
    }
}
