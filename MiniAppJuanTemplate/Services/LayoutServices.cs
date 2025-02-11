using MiniAppJuanTemplate.Data;

namespace MiniAppJuanTemplate.Services
{
    public class LayoutServices
    {
        private readonly JuanAppDbContext _juanAppDbContext;

        public LayoutServices(JuanAppDbContext juanAppDbContext)
        {
            _juanAppDbContext = juanAppDbContext;
        }
        public Dictionary<string,string> GetSettings()
        {
            return _juanAppDbContext.Settings.ToDictionary(s=>s.Key,s=>s.Value);
        }
    }
}
