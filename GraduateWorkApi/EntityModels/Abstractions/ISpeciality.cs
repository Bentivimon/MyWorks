namespace EntityModels.Abstractions
{
    public interface ISpeciality
    {
        string Code { get; set; }
        string Name { get; set; }
        float AdditionalFactor { get; set; }
        int CountOfStatePlaces { get; set; }
    }
}
