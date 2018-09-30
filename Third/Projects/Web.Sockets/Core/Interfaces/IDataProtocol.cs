
namespace Web.Sockets.Core.Interfaces
{
    public interface IDataProtocol
    {
        byte[] Parse(byte[] data, IReceiver receiver);
    }
}
