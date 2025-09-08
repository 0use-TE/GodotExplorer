using Microsoft.Extensions.ObjectPool;
using PlatformExplorer.PlayerScript;

namespace PlatformExplorer.ObjectPool
{
    public class TestPoliy : PooledObjectPolicy<Player>
    {
        public override Player Create()
        {
            throw new System.NotImplementedException();
        }

        public override bool Return(Player obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
