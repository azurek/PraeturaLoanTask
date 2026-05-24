using LoanLogic.Settings;

namespace PraeturaLoanTask.BuilderDependencies
{
    public static  class ConfigurationSettings
    {
        public static EligibilitySettings AddEligiblitySettigns(this IServiceCollection services, IConfiguration configuration)
        {
            var eligibilitySettings = new EligibilitySettings();
            configuration.GetSection("EligibilitySettings").Bind(eligibilitySettings);
            services.AddSingleton(eligibilitySettings);
            return eligibilitySettings;
        }
    }
}
