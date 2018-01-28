using EntityModels.Entitys;
using Models.DTOModels.CertificateOfSecondaryEducation;
using Models.DTOModels.CertificateOfTesting;

namespace Models.DTOModels.EntrantModels
{
    public class EntrantExpandDto : EntrantDto
    {
        public CertificateOfTestingDto CertificateOfTesting { get; set; }
        public CertificateOfSecondaryEducationDto CertificateOfSecondaryEducation { get; set; }

        public EntrantExpandDto(EntrantEntity entrant) : base(entrant)
        {
            CertificateOfTesting = new CertificateOfTestingDto(entrant.CertificateOfTesting);
            CertificateOfSecondaryEducation =
                new CertificateOfSecondaryEducationDto(entrant.CertificateOfSecondaryEducation);
        }
    }
}
