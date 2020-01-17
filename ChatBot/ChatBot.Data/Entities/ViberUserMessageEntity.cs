using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot.Data.Entities
{
    [Table("viber_user_messages")]
    public class ViberUserMessageEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        public ViberUserEntity User { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("message_type")]
        public string MessageType { get; set; }
    }
}
