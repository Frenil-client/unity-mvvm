using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Frenil.MVVM.Samples
{
    /// <summary>
    /// 로그인 화면 View.
    /// Bind()에서 ViewModel의 Observable을 직접 구독하고,
    /// UI 컴포넌트 조작은 각 핸들러에서 일괄 처리한다.
    /// </summary>
    public class LoginView : ViewBase<LoginViewModel>
    {
        [Header("UI Components")]
        [SerializeField] private TMP_InputField _idInput;
        [SerializeField] private TMP_InputField _passwordInput;
        [SerializeField] private Button _loginButton;
        [SerializeField] private TMP_Text _statusText;

        protected override void Bind(LoginViewModel viewModel)
        {
            // ViewModel > View
            viewModel.IsLoginButtonInteractable.OnChanged += interactable =>
                _loginButton.interactable = interactable;

            viewModel.StatusMessage.OnChanged += message =>
                _statusText.text = message;

            // View > ViewModel
            _idInput.onValueChanged.AddListener(value => viewModel.Id.Value = value);
            _passwordInput.onValueChanged.AddListener(value => viewModel.Password.Value = value);
            _loginButton.onClick.AddListener(viewModel.OnLoginClicked);
        }
    }
}
