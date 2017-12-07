using System.Security.Principal;

namespace Socona.ImVehicle.Core.Interfaces
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}
