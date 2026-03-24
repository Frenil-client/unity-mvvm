using UnityEngine;

namespace Frenil.MVVM
{
    /// <summary>
    /// ViewModel의 생성과 구독 해제 생명주기를 관리하는 View 베이스 클래스.
    /// 파생 클래스는 Bind()에서 ViewModel의 Observable을 직접 구독한다.
    /// </summary>
    public abstract class ViewBase<TViewModel> : MonoBehaviour
        where TViewModel : ViewModelBase, new()
    {
        protected TViewModel ViewModel { get; private set; }

        protected virtual void Awake()
        {
            ViewModel = CreateViewModel();
            Bind(ViewModel);
        }

        protected virtual void OnDestroy()
        {
            ViewModel?.Dispose();
            ViewModel = null;
        }

        /// <summary>
        /// ViewModel 생성 방식을 커스터마이즈할 때 오버라이드.
        /// 외부에서 ViewModel을 주입받거나 공유할 경우에 활용.
        /// </summary>
        protected virtual TViewModel CreateViewModel() => new TViewModel();

        /// <summary>
        /// Observable 구독과 UI 이벤트 등록을 수행하는 구현부.
        /// ViewModel.Dispose() 호출 시 구독은 자동 해제된다.
        /// </summary>
        protected abstract void Bind(TViewModel viewModel);
    }
}
