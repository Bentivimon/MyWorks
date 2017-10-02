using System.ServiceModel;

namespace Server
{
    [ServiceContract]
    public interface IContract
    {
        [OperationContract]
        string Send(string input, bool flag);
    }
}