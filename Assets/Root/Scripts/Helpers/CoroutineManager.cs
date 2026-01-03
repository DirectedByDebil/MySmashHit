using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MySmashHit.Helpers
{
    internal class CoroutineManager : MonoBehaviour, ICoroutineManager
    {

        public static ICoroutineManager Instance { get => _instance; }
        private static ICoroutineManager _instance;


        private Dictionary<string, Coroutine> _coroutines;


        private void Awake()
        {
            _coroutines = new Dictionary<string, Coroutine>();

            _instance ??= this;
        }


        public void AddCoroutine (string key, IEnumerator routine)
        {
            RemoveCoroutine(key);

            Coroutine newCoroutine = StartCoroutine(routine);

            if (!_coroutines.TryAdd(key, newCoroutine))
            {
                _coroutines[key] = newCoroutine;
            }
        }


        public void RemoveCoroutine(string key) 
        {
            if (_coroutines.TryGetValue(key, out Coroutine coroutine))
            {
                StopCoroutine(coroutine);
            }
        }
    }
}
