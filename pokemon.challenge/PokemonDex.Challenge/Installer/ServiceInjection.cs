using Microsoft.Extensions.DependencyInjection;
using PokemonDex.Challenge.Clients;
using PokemonDex.Challenge.Common;
using PokemonDex.Challenge.Services;

namespace PokemonDex.Challenge.Installer
{
    public static class ServiceInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IPokomonConfiguration, PokomonConfiguration>();
            services.AddTransient<IPokemonApiClient, PokemonApiClient>();

            services.AddTransient<IPokemonService, PokemonService>();
            services.AddTransient<ITranslationApiClient, TranslationApiClient>();
            services.AddTransient<ITranslationService, TranslationService>();
        }
    }
}
