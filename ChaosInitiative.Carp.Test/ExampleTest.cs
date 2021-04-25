using NUnit.Framework;

namespace ChaosInitiative.Carp.Test
{
    /// <summary>
    /// Example of a test class
    /// </summary>
    public class ExampleTests
    {

        private ExampleClass _exampleClass;
        
        /// <summary>
        /// This is run once before every test. Use <c>OneTimeSetUp</c> if you want something to run before everything
        /// </summary>
        [SetUp]
        public void Setup()  
        {
            _exampleClass = new ExampleClass();
        }
        
        /// <summary>
        /// Gets run after each test.
        /// You don't actually need to make everything null again, I just put this here as an example
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            _exampleClass = null;
        }

        [Test]
        public void Test1()
        {
            Assume.That(_exampleClass, Is.Not.Null);
            
            int c = -1;
            Assert.That(() => c = _exampleClass.Fibonacci(9), Throws.Nothing);
            
            Assert.That(c, Is.EqualTo(55));
        }
    }
}