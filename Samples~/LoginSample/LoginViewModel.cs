namespace Frenil.MVVM.Samples
{
    /// <summary>
    /// 로그인 화면 ViewModel.
    /// ID/PW 입력 상태에 따라 버튼 활성화 여부를 자동으로 계산한다.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        public Observable<string> Id { get; } = new Observable<string>("");
        public Observable<string> Password { get; } = new Observable<string>("");
        public Observable<bool> IsLoginButtonInteractable { get; } = new Observable<bool>(false);
        public Observable<string> StatusMessage { get; } = new Observable<string>("");

        public LoginViewModel()
        {
            Subscribe(Id, _ => RefreshLoginButtonState());
            Subscribe(Password, _ => RefreshLoginButtonState());
        }

        private void RefreshLoginButtonState()
        {
            IsLoginButtonInteractable.Value =
                !string.IsNullOrEmpty(Id.Value) &&
                !string.IsNullOrEmpty(Password.Value);
        }

        public void OnLoginClicked()
        {
            if (Id.Value == "admin" && Password.Value == "1234")
                StatusMessage.Value = "로그인 성공!";
            else
                StatusMessage.Value = "아이디 또는 비밀번호가 올바르지 않습니다.";
        }
    }
}
