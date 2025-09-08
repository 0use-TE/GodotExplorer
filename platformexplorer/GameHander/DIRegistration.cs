using Godot;
using Godot.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using System.Reflection;
namespace PlatformExplorer.GameHandler
{
    public partial class DIRegistration : Node2D, IServicesConfigurator
    {

        public void ConfigureServices(IServiceCollection services)
        {
            //Godot Logger
            services.AddGodotLogger();
            //Godot Services
            services.AddGodotServices();

            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            //Used for create object pools
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

            //Register Player Pool in DI（Test）
            //services.AddObjectPool<Player, TestPoliy>();
        }
    }
}