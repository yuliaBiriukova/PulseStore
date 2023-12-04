using PulseStore.BLL.Entities.Security.Enums;

namespace PulseStore.BLL.Entities.Security;

public class SecurityUser
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public SecurityUserType UserType { get; set; }
    public int NfcDeviceId { get; set; }
    public NfcDevice NfcDevice { get; set;}
    public ICollection<Stock> Stocks { get; set; }
}