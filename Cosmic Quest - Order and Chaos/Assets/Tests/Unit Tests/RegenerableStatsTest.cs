using NUnit.Framework;

namespace Tests
{
    public class RegenerableStatTests
    {
        RegenerableStat instance;

        [SetUp]
        public void Setup()
        {
            instance = new RegenerableStat();
            instance.maxValue = 10f;
            instance.minValue = 0f;
        }

        [Test]
        public void Init_SetsCurrentValueToMax()
        {
            Assert.Zero(instance.CurrentValue);
            instance.Init();
            Assert.AreEqual(instance.maxValue, instance.CurrentValue);
        }

        [Test]
        public void Add_ShouldSetCurrentValueProperly()
        {
            float val1 = 5f;
            float val2 = 4f;

            instance.Add(val1);
            Assert.AreEqual(val1, instance.CurrentValue);
            instance.Add(val2);
            Assert.AreEqual(val1 + val2, instance.CurrentValue);
            instance.Add(instance.maxValue + 1f);
            Assert.AreEqual(instance.maxValue, instance.CurrentValue);
        }

        [Test]
        public void Subtract_ShouldSetCurrentValueProperly()
        {
            float val = instance.maxValue / 2;
            instance.Init();

            instance.Subtract(val);
            Assert.AreEqual(instance.maxValue - val, instance.CurrentValue);

            // Reached minimum value
            instance.Subtract(val);
            Assert.AreEqual(instance.minValue, instance.CurrentValue);

            // Stays at minimum value
            instance.Subtract(val);
            Assert.AreEqual(instance.minValue, instance.CurrentValue);
        }
    }
}
