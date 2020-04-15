using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot.Data.Entities
{
    [Table("dialogflow_result")]
    public class DialogflowResultEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("response")]
        public string Response { get; set; }
        
        [Column("request")]
        public string Request { get; set; }
    }
}
