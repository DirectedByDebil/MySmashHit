using System.Collections;

namespace Core
{
    public interface ICoroutineManager
    {

        public void AddCoroutine (string key, IEnumerator routine);

        public void RemoveCoroutine (string key);
    }
}