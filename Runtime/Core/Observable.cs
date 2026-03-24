using System;

namespace Frenil.MVVM
{
    /// <summary>
    /// 값 변경 시 이벤트를 발행하는 제네릭 반응형 프로퍼티.
    /// 외부 라이브러리 없이 델리게이트 기반으로 구현.
    /// </summary>
    public class Observable<T>
    {
        private T _value;

        public event Action<T> OnChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value)) return;
                _value = value;
                OnChanged?.Invoke(_value);
            }
        }

        public Observable(T initialValue = default)
        {
            _value = initialValue;
        }

        /// <summary>
        /// 이벤트를 발행하지 않고 내부 값만 갱신.
        /// 초기 데이터 세팅 등 알림이 불필요한 경우에 사용.
        /// </summary>
        public void SetWithoutNotify(T value)
        {
            _value = value;
        }

        public override string ToString() => _value?.ToString() ?? "null";
    }
}
