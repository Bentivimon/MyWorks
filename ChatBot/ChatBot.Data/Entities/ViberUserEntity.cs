using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot.Data.Entities
{
    [Table("viber_users")]
    public class ViberUserEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("viber_id")]
        public string ViberId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("avatar")]
        public string Avatart { get; set; }

        [Column("country")]
        public string Country { get; set; }

        [Column("language")]
        public string Language { get; set; }

        [Column("session_id")]
        public string SessionId { get; set; }

        [Column("is_subscribed")]
        public bool IsSubscribed { get; set; }

        [Column("created_timestamp")]
        public long CreatedTimestamp { get; set; }
         
        [Column("updated_timestamp")]
        public long UpdatedTimestamp { get; set; }
    }
}
