using System.Threading.Tasks;

namespace Escant_App.Interfaces
{
    public interface IShareFile
    {
        Task Show(string title, string message, string filePath);
    }
}
