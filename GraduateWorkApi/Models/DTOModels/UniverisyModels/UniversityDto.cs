using EntityModels.Abstractions;

namespace Models.DTOModels.UniverisyModels
{
    public class UniversityDto: IUniversity
    {
        public string FullName { get; set; }
        public string LevelOfAccreditation { get; set; }

        public UniversityDto(IUniversity university)
        {
            FullName = university.FullName;
            LevelOfAccreditation = university.LevelOfAccreditation;
        }
    }
}
