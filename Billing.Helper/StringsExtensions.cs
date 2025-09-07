namespace MoveEnergia.Billing.Helper
{
    public static class StringsExtensions
    {
        public static string FormatToValueCPFCNPJ(string value)
        {
            string doc = value.Replace(".", "").Replace("-", "").Replace("/", "");

            return doc;
        }

        public static string FormatToName(string value)
        {
            var partes = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string primeiroNome = partes.First();

            return primeiroNome;
        }
        public static string FormatToSurName(string value)
        {
            var partes = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string ultimoNome = partes.Last();

            return ultimoNome;
        }
    }
}
