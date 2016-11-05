using System;

namespace SnsLite
{
    public class Company : SnsUser
    {
        public string Code { get; set; }
        public string Url { get; set; }
        public int Capital { get; set; }
        public string Currency { get; set; }
        public string Owner { get; set; }
        public string Phone { get; set; }
        public string Region { get; set; }
        public string Trade { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string EconomicNature { get; set; }
        public DateTime? LicenseDate { get; set; }
        public string TaxId { get; set; }
        public string BankQuality { get; set; }
        public int StaffCount { get; set; }
        public string TotalAssets { get; set; }
        public string Acreage { get; set; }
    }
}
