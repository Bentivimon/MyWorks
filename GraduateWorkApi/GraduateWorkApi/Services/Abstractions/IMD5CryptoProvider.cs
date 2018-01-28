namespace GraduateWorkApi.Services.Abstractions
{
    public interface IMD5CryptoProvider
    {
        string Encoding(string password);
    }
}
