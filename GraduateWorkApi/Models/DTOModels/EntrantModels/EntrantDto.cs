using System;
using EntityModels.Abstractions;
using EntityModels.Entitys;

namespace Models.DTOModels.EntrantModels
{
    public class EntrantDto : IEntrant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BDay { get; set; }

        public EntrantDto(EntrantEntity entrant)
        {
            Id = entrant.Id;
            Name = entrant.Name;
            Surname = entrant.Surname;
            BDay = entrant.BDay;
        }
    }
}
