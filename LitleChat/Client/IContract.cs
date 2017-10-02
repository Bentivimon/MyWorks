using System.ServiceModel;

namespace Client
{
    [ServiceContract]
    public interface IContract
    {
        [OperationContract]
        string Send(string input, bool flag);
    }
}
