using NUnit.Framework;

namespace Frenil.MVVM.Tests
{
    public class ViewModelBaseTests
    {
        // Subscribe 가 protected 라 테스트용 구체 ViewModel 로 감싼다.
        private class CounterViewModel : ViewModelBase
        {
            public Observable<int> Count { get; } = new Observable<int>(0);
            public int Handled { get; private set; }

            public CounterViewModel()
            {
                Subscribe(Count, _ => Handled++);
            }
        }

        [Test]
        public void Subscribe_ReceivesObservableChanges()
        {
            var vm = new CounterViewModel();

            vm.Count.Value = 1;

            Assert.AreEqual(1, vm.Handled);
        }

        [Test]
        public void Dispose_UnsubscribesHandlers()
        {
            var vm = new CounterViewModel();
            vm.Count.Value = 1;

            vm.Dispose();
            vm.Count.Value = 2;

            Assert.AreEqual(1, vm.Handled);
        }

        [Test]
        public void Dispose_IsIdempotent()
        {
            var vm = new CounterViewModel();

            Assert.DoesNotThrow(() =>
            {
                vm.Dispose();
                vm.Dispose();
            });
        }
    }
}
