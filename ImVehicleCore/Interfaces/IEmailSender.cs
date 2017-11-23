using System.Threading.Tasks;

namespace ImVehicleCore.Interfaces
{

    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
