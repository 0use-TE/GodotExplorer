using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;

namespace PlatformExplorer.ObjectPool
{
    public static class ObjectPoolExtensions
    {
        /// <summary>
        /// 注册对象池和对应的策略（策略只用于内部创建，不暴露出去）
        /// </summary>
        public static IServiceCollection AddObjectPool<T, TPolicy>(this IServiceCollection services)
            where T : class
            where TPolicy : PooledObjectPolicy<T>
        {
            services.AddSingleton(sp =>
            {
                var provider = sp.GetRequiredService<ObjectPoolProvider>();
                var policy = ActivatorUtilities.CreateInstance<TPolicy>(sp);
                return provider.Create(policy);
            });
            return services;
        }
    }
}
