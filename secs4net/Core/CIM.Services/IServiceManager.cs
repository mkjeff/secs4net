namespace Cim.Services {
    public interface IServiceManager<T> {
        void Publish(string serviceId,string serviceUrl);
    }
}
