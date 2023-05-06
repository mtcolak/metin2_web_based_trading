using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;

namespace Metin2_v2.Models
{
    public class PlayerModel
    {
        [Key]
        public int id { get; set; }
        public int account_id { get; set; }
        [StringLength(24)]
        public string name { get; set; }
        public int job { get; set; }
        public int map_index { get; set; }
        public short hp { get; set; }
        public short mp { get; set; }
        public short stamina { get; set; }
        public int random_hp { get; set; }
        public int random_sp { get; set; }
        public int playtime { get; set; }
        public int level { get; set; }
        public short st { get; set; }
        public short ht { get; set; }
        public short dx { get; set; }
        public short iq { get; set; }
        public int exp { get; set; }
        public long gold { get; set; }

        [StringLength(15)]
        public string ip { get; set; }
        public int skill_group { get; set; }
        public int alignment { get; set; }
        public DateTime last_play { get; set; }
        public int horse_level { get; set; }
        public long cheque { get; set; }
        public long offline_shop_money { get; set; }
        public long offline_shop_cheque { get; set; }
        public int sifre { get; set; }
        public int kilitdurum { get; set; }
        public int kral { get; set; }

    }


}
