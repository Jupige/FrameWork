using System.ServiceModel;

namespace CB.Service
{
    [ServiceContract]
    public interface ICBService
    {
        [OperationContract]
        void SayHello();
    }
}
