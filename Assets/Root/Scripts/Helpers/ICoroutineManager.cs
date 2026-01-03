using System.Collections;

namespace MySmashHit.Helpers
{
    public interface ICoroutineManager
    {

        public void AddCoroutine (string key, IEnumerator routine);

        public void RemoveCoroutine (string key);
    }
}