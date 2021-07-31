
namespace Cim.Services {
    public interface ICentralService<T> {
        T GetService(string serviceId);
    }
}
