using NUnit.Framework;

namespace Frenil.MVVM.Tests
{
    public class ObservableTests
    {
        [Test]
        public void InitialValue_IsStored()
        {
            var obs = new Observable<int>(5);
            Assert.AreEqual(5, obs.Value);
        }

        [Test]
        public void OnChanged_FiresWithNewValue()
        {
            var obs = new Observable<int>(0);
            int fired = 0;
            int last = 0;
            obs.OnChanged += v => { fired++; last = v; };

            obs.Value = 10;

            Assert.AreEqual(1, fired);
            Assert.AreEqual(10, last);
        }

        [Test]
        public void OnChanged_NotFiredWhenValueUnchanged()
        {
            var obs = new Observable<int>(10);
            int fired = 0;
            obs.OnChanged += _ => fired++;

            obs.Value = 10;

            Assert.AreEqual(0, fired);
        }

        [Test]
        public void SetWithoutNotify_UpdatesValue_ButDoesNotFire()
        {
            var obs = new Observable<int>(0);
            int fired = 0;
            obs.OnChanged += _ => fired++;

            obs.SetWithoutNotify(42);

            Assert.AreEqual(42, obs.Value);
            Assert.AreEqual(0, fired);
        }
    }
}
