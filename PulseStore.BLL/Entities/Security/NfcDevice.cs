namespace PulseStore.BLL.Entities.Security;

public class NfcDevice
{
    public int Id { get; set; }
    public string NUID { get; set; }
    public ICollection<SecurityUser> SecurityUsers { get; set; }
}