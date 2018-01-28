using System;

namespace EntityModels.Abstractions
{
    public interface IEntrant
    {
        string Name { get; set; }
        string Surname { get; set; }
        DateTime BDay { get; set; }
    }
}
