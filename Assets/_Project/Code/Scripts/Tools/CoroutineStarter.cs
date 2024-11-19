using UnityEngine;

namespace Project.Scripts.Tools
{
    public interface ICoroutineStarter
    {
        MonoBehaviour Starter { get; }
    }
    
    public class CoroutineStarter : MonoBehaviour, ICoroutineStarter
    {
        public MonoBehaviour Starter => this;
    }
}