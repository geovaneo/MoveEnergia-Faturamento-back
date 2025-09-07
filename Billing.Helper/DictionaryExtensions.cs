namespace MoveEnergia.Billing.Helper
{
    public static class DictionaryExtensions
    {
        public static string GetValue(Dictionary<string, string> dict, string fieldName)
        {
            dict.TryGetValue(fieldName, out var value);
            return value ?? string.Empty;
        }

        public static string GetValueByFieldId(Dictionary<string, string> dict, string fieldId)
        {
            return dict.TryGetValue(fieldId, out var value) ? value : string.Empty;
        }

        public static decimal GetDecimalValueByFieldId(Dictionary<string, string> dict, string fieldId)
        {
            return decimal.TryParse(GetValueByFieldId(dict, fieldId), out var result) ? result : 0;
        }

        public static DateTime? GetDateValueByFieldId(Dictionary<string, string> dict, string fieldId)
        {
            return DateTime.TryParse(GetValueByFieldId(dict, fieldId), out var result) ? result : (DateTime?)null;
        }

        public static bool GetBooleanValueByFieldId(Dictionary<string, string> dict, string fieldId)
        {
            return string.Equals(GetValueByFieldId(dict, fieldId), "Sim", StringComparison.OrdinalIgnoreCase);
        }
    }
}
