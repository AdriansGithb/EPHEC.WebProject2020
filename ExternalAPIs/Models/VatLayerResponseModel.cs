namespace MyLibrary.Helpers
{
    public class VatLayerResponseModel
    {
        public bool valid { get; set; }
        public string database { get; set; }
        public bool format_valid { get; set; }
        public string query { get; set; }
        public string country_code { get; set; }
        public string vat_number { get; set; }
        public string company_name { get; set; }
        public string company_address { get; set; }
    }
}
