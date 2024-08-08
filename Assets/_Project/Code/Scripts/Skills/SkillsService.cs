namespace _Project.Scripts.Skills
{
    public interface ISkillsService
    {
        
    }
    
    public class SkillsService : ISkillsService
    {
        private readonly SkillsServiceConfig _config;
        public SkillsService(SkillsServiceConfig config)
        {
            _config = config;
        }
    }
}