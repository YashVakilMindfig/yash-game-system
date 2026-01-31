using System;

namespace Yash.GameSystem.Core
{
    public interface IManager
    {
        Type ServiceType { get; }
        void Initialize();
    }
}