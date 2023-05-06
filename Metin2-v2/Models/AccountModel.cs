using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Metin2_v2.Models
{
    public class AccountModel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("login")]
        [StringLength(30)]
        public string Login { get; set; }

        [Column("password")]
        [StringLength(45)]
        public string Password { get; set; }

        [Column("real_name")]
        [StringLength(16)]
        public string RealName { get; set; }

        [Column("social_id")]
        [StringLength(7)]
        public string SocialId { get; set; }

        [Column("email")]
        [StringLength(64)]
        public string Email { get; set; }

        [Column("phone1")]
        [StringLength(16)]
        public string Phone1 { get; set; }

        [Column("phone2")]
        [StringLength(16)]
        public string Phone2 { get; set; }

        [Column("address")]
        [StringLength(128)]
        public string Address { get; set; }

        [Column("zipcode")]
        [StringLength(7)]
        public string Zipcode { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        [Column("question1")]
        [StringLength(48)]
        public string Question1 { get; set; }

        [Column("answer1")]
        [StringLength(48)]
        public string Answer1 { get; set; }

        [Column("question2")]
        [StringLength(48)]
        public string Question2 { get; set; }

        [Column("answer2")]
        [StringLength(48)]
        public string Answer2 { get; set; }

        [Column("is_testor")]
        public bool IsTestor { get; set; }

        [Column("status")]
        [StringLength(8)]
        public string Status { get; set; }

        [Column("securitycode")]
        [StringLength(192)]
        public string SecurityCode { get; set; }

        [Column("newsletter")]
        public bool Newsletter { get; set; }

        [Column("empire")]
        public byte Empire { get; set; }

        [Column("name_checked")]
        public bool NameChecked { get; set; }

        [Column("availDt")]
        public DateTime AvailDt { get; set; }

        [Column("mileage")]
        public int Mileage { get; set; }

        [Column("cash")]
        public int Cash { get; set; }

        [Column("gold_expire")]
        public DateTime GoldExpire { get; set; }

        [Column("silver_expire")]
        public DateTime SilverExpire { get; set; }

        [Column("safebox_expire")]
        public DateTime SafeboxExpire { get; set; }

        [Column("autoloot_expire")]
        public DateTime AutolootExpire { get; set; }

        [Column("fish_mind_expire")]
        public DateTime FishMindExpire { get; set; }

        [Column("marriage_fast_expire")]
        public DateTime MarriageFastExpire { get; set; }

        [Column("money_drop_rate_expire")]
        public DateTime MoneyDropRateExpire { get; set; }

        [Column("ttl_cash")]
        public int TtlCash { get; set; }

        [Column("ttl_mileage")]
        public int TtlMileage { get; set; }

        [Column("channel_company")]
        [StringLength(30)]
        public string ChannelCompany { get; set; }

        [Column("coins")]
        public int Coins { get; set; }

        [Column("web_admin")]
        public int WebAdmin { get; set; }

        [Column("web_ip")]
        [StringLength(15)]
        public string WebIp { get; set; }

        [Column("web_aktiviert")]
        [StringLength(32)]
        public string WebAktiviert { get; set; }

        [Column("server")]
        public int Server { get; set; }

        [Column("reason")]
        [StringLength(256)]
        public string Reason { get; set; }

        [Column("ysifre")]
        [StringLength(255)]
        public string Ysifre { get; set; }

        [Column("yemail")]
        [StringLength(255)]
        public string Yemail { get; set; }

        [Column("ylogin")]
        [StringLength(255)]
        public string Ylogin { get; set; }

        [Column("tkod")]
        [StringLength(10)]
        public string Tkod { get; set; }

        [Column("ypass")]
        [StringLength(255)]
        public string Ypass { get; set; }

        [Column("ban_neden")]
        [StringLength(255)]
        public string BanNeden { get; set; }

        [Column("durum")]
        public int Durum { get; set; }

        [Column("davet")]
        public int Davet { get; set; }

        [Column("davet_durum")]
        public int DavetDurum { get; set; }

        [Column("ip")]
        [StringLength(40)]
        public string Ip { get; set; }

        [Column("soru")]
        public int Soru { get; set; }

        [Column("cevap")]
        [StringLength(255)]
        public string Cevap { get; set; }

        [Column("foto")]
        [StringLength(255)]
        public string Foto { get; set; }

        [Column("depo")]
        [StringLength(255)]
        public string Depo { get; set; }

        [Column("em_coins")]
        public int EmCoins { get; set; }

        [Column("fastepin_bakiye")]
        [StringLength(25)]
        public string FastepinBakiye { get; set; }
    }

}
