using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Surviblewilderness
{
    public abstract class Subject<T> : MonoBehaviour
    {
        [SerializeField] protected List<T> observers = new List<T>();

        public void AddObserver(T observer) => observers.Add(observer);

        public void RemoveObserver(T observer) => observers.Remove(observer);

        public abstract void NotifyObservers ();
    }
}
