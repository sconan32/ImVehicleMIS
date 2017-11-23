using System.Security.Principal;

namespace ImVehicleCore.Interfaces
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}
