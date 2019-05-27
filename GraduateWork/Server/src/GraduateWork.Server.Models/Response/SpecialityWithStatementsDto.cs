using System.Collections.Generic;

namespace GraduateWork.Server.Models.Response
{
    public class SpecialityWithStatementsDto : SpecialityDto
    {
        public List<StatementDto> Statements { get; set; }
    }
}
