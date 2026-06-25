# UGUI MVVM Framework

Unity UGUI 환경을 위한 경량 MVVM 프레임워크.  
외부 라이브러리 없이 `Observable<T>` 기반 데이터 바인딩을 제공합니다.

---

## 왜 만들었나

UI 규모가 커질수록 View와 로직의 경계가 무너지고, 테스트와 재사용이 어려워집니다.  
이 프레임워크는 Unity UGUI에 MVVM 패턴을 적용해 **UI 표현과 로직을 명확히 분리**합니다.

- View는 UI 컴포넌트 조작만 담당합니다
- ViewModel은 상태와 로직만 담당하며 Unity에 의존하지 않습니다

---

## 구조

```
Runtime/
└── Core/
    ├── Observable.cs        # 반응형 프로퍼티 (이벤트 발행)
    ├── ViewModelBase.cs     # ViewModel 베이스 (구독 수명 관리)
    └── ViewBase.cs          # View 베이스 (바인딩 생명주기)
```

View에서 Observable을 직접 구독하는 방식을 채택했습니다.  
동일한 값으로 텍스트, 게이지, 색상을 동시에 제어하는 게임 UI 특성상  
관련 처리를 한 파일에 모아두는 것이 추적과 유지보수에 유리합니다.

---

## 빠른 시작

### 1. ViewModel 작성

```csharp
public class HpViewModel : ViewModelBase
{
    public Observable<int> MaxHp { get; } = new Observable<int>(100);
    public Observable<int> Hp { get; } = new Observable<int>(100);

    public void SetHp(int value)
    {
        Hp.Value = Mathf.Min(value, MaxHp.Value);
    }
}
```

### 2. View 작성

```csharp
public class HpView : ViewBase<HpViewModel>
{
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private Image _hpBar;

    protected override void Bind(HpViewModel viewModel)
    {
        viewModel.Hp.OnChanged += hp =>
        {
            _hpText.text = $"{hp} / {viewModel.MaxHp.Value}";
            _hpBar.fillAmount = (float)hp / viewModel.MaxHp.Value;
        };
    }
}
```

구독 해제는 `ViewModelBase.Dispose()`에서 자동으로 처리됩니다.

---

## 샘플 — 로그인 화면

ID/PW 입력 상태에 따라 로그인 버튼 활성화가 자동으로 반응하는 예시입니다.

```csharp
// LoginViewModel.cs — 버튼 활성화 조건을 ViewModel이 계산
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
```

```csharp
// LoginView.cs — UI 조작만 담당
protected override void Bind(LoginViewModel viewModel)
{
    viewModel.IsLoginButtonInteractable.OnChanged += interactable =>
        _loginButton.interactable = interactable;

    viewModel.StatusMessage.OnChanged += message =>
        _statusText.text = message;

    _idInput.onValueChanged.AddListener(value => viewModel.Id.Value = value);
    _loginButton.onClick.AddListener(viewModel.OnLoginClicked);
}
```

`Example/LoginSample` 폴더에서 전체 코드를 확인할 수 있습니다.

---

## 설계 결정

### ViewModel을 MonoBehaviour로 만들지 않은 이유
ViewModel은 Unity 라이프사이클(`Awake`, `Update` 등)에 의존할 필요가 없습니다.  
순수 C# 클래스로 유지하면 Unity 없이도 로직을 단위 테스트할 수 있고,  
씬 전환 시 ViewModel의 수명을 View와 독립적으로 관리할 수 있습니다.

### Observable을 직접 구현한 이유
UniRx 등 외부 라이브러리 없이 제네릭 + 델리게이트만으로 구현했습니다.  
모바일 프로젝트에서 패키지 의존성은 빌드 사이즈와 관리 비용을 높입니다.

### Binder 컴포넌트를 두지 않은 이유
별도 Binder 클래스로 분리하면 컴포넌트 재사용성이 높아지는 장점이 있습니다.  
그러나 게임 UI는 하나의 값으로 텍스트, 게이지, 색상을 동시에 제어하는 경우가 많아  
Binder 단위 분리가 오히려 관련 처리를 여러 파일로 분산시킵니다.  
View에서 직접 구독하는 방식이 컨텍스트를 한 곳에 유지하면서 MVVM의 역할 분리를 충족합니다.

---

## 설치

### UPM (Package Manager) — 권장
`Window ▸ Package Manager ▸ + ▸ Add package from git URL` 에 입력:

```
https://github.com/Frenil-client/unity-mvvm.git
```

또는 `Packages/manifest.json` 에 직접 추가:

```json
"com.frenil.mvvm": "https://github.com/Frenil-client/unity-mvvm.git"
```

### 드롭인
`Runtime/` 폴더를 프로젝트 `Assets/` 아래에 복사합니다. (코어는 순수 C# — 외부 의존 없음)

### 샘플
Package Manager에서 이 패키지를 선택 → **Samples ▸ Import** (로그인 화면 예시, 원본: `Samples~/LoginSample`).

## 테스트

`Window ▸ General ▸ Test Runner ▸ EditMode ▸ Run All`
(`com.unity.test-framework` 필요 · EditMode 테스트 7종 — Observable / ViewModelBase)

## 환경

- Unity 2021.3 LTS 이상
- TextMeshPro 3.0.6 이상
