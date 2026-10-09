using GroveGames.DependencyInjection;

namespace GroveGames.SecureStorage.Unity
{
    public static class SecureStorageContainerBuilderExtensions
    {
        public static IContainerBuilder AddSecureStorage(this IContainerBuilder builder)
        {
            return builder.AddSingleton<ISecureStorage, SecureStorage>();
        }
    }
}
