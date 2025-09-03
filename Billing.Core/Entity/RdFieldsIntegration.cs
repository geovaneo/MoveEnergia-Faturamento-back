namespace MoveEnergia.Billing.Core.Entity
{
    public class RdFieldsIntegration
    {
        public int Id { get; set; }
        public string IdRd { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool Callback { get; set; } = true;
        public bool OrigemRD { get; set; } = true; 
        public bool Required { get; set; } = false;
    }
}
