namespace Metin2_v2.Models
{
    public class ItemModel
    {
        public int id { get; set; }
        public int owner_id { get; set; }
        public string? name { get; set; }
        public ushort vnum { get; set; }
        public string? locale_name { get; set; }
        public int price_cheque { get; set; }
        public int limitvalue0 { get; set; }

    }
}
