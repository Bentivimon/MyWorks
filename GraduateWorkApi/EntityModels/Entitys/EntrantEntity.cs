using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels.Entitys
{
    public class EntrantEntity
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
    }
}
