using System;
using System.Collections.Generic;

namespace Frenil.MVVM
{
    /// <summary>
    /// 모든 ViewModel의 베이스 클래스.
    /// MonoBehaviour를 상속하지 않아 Unity 라이프사이클에 독립적이며,
    /// 순수 C# 클래스로서 단위 테스트가 가능하다.
    /// </summary>
    public abstract class ViewModelBase : IDisposable
    {
        private readonly List<Action> _cleanupActions = new List<Action>();
        private bool _disposed;

        protected void Subscribe<T>(Observable<T> observable, Action<T> handler)
        {
            observable.OnChanged += handler;
            _cleanupActions.Add(() => observable.OnChanged -= handler);
        }

        public virtual void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            foreach (var action in _cleanupActions)
                action?.Invoke();

            _cleanupActions.Clear();
        }
    }
}
