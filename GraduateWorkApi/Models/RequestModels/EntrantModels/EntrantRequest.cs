using System;
using EntityModels.Abstractions;

namespace Models.RequestModels.EntrantModels
{
    public class EntrantRequest :IEntrant
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BDay { get; set; }

        //TODO Add Lists for certificate and etc.
    }
}
