using System.Threading.Tasks;

namespace Src
{
    public interface ITokenProvider
    {
        public Task<string> GetToken();
    }
}