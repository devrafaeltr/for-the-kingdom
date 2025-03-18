using UnityEngine.Localization.Settings;

namespace FTKingdom
{
    [System.Serializable]
    public static class LocalizationHelper
    {
        private const string localizationTableName = "GameLocalization";
        public static string GetLocalizedText(string key)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(localizationTableName, key);
        }
    }
}