using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EntityModels.Abstractions;

namespace EntityModels.Entitys
{
    public class EntrantEntity : IEntrant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BDay { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public UserEntity User { get; set; }

        public CertificateOfTestingEntity CertificateOfTesting { get; set; }
        public CertificateOfSecondaryEducationEntity CertificateOfSecondaryEducation { get; set; }
        public List<StatementEntity> Statements { get; set; }

        public EntrantEntity(IEntrant entrant)
        {
            Name = entrant.Name;
            Surname = entrant.Surname;
            BDay = entrant.BDay;
        }
    }
}
