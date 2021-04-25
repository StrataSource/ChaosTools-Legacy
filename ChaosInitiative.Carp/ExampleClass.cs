namespace ChaosInitiative.Carp
{
    public class ExampleClass
    {
        /// <summary>
        /// Example class in core project
        /// </summary>
        /// <param name="amount">Which number to get in the sequence. Note that this starts at 0 = 0</param>
        /// <returns>The fibonacci number at that point</returns>
        public int Fibonacci(int amount)
        {
            int a = 0;
            int b = 1;
            int c = -1;
            for (int i = 0; i < amount; i++)
            {
                c = a + b;
                a = b;
                b = c;
            }

            return c;
        }
    }
}