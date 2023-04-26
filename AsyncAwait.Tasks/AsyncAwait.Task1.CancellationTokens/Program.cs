/*
* Study the code of this application to calculate the sum of integers from 0 to N, and then
* change the application code so that the following requirements are met:
* 1. The calculation must be performed asynchronously.
* 2. N is set by the user from the console. The user has the right to make a new boundary in the calculation process,
* which should lead to the restart of the calculation.
* 3. When restarting the calculation, the application should continue working without any failures.
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal class Program
{
    private static CancellationTokenSource cancelTokenSource;

    /// <summary>
    /// The Main method should not be changed at all.
    /// </summary>
    /// <param name="args"></param>
    private static void Main(string[] args)
    {
        Console.WriteLine("Mentoring program L2. Async/await.V1. Task 1");
        Console.WriteLine("Calculating the sum of integers from 0 to N.");
        Console.WriteLine("Use 'q' key to exit...");
        Console.WriteLine();

        Console.Write("Enter N: ");

        var input = Console.ReadLine();
        while (input.Trim().ToUpper() != "Q")
        {
            bool threadStarted = false;
            cancelTokenSource = new();
            if (int.TryParse(input, out var n))
            {
                CancellationToken token = cancelTokenSource.Token;
                CalculateSum(n, token);
                threadStarted = true;
            }
            else
            {
                Console.WriteLine($"Invalid integer: '{input}'. Please try again.");
                Console.Write("Enter N: ");
            }

            input = Console.ReadLine();
            if (threadStarted) cancelTokenSource.Cancel();
        }

        Console.WriteLine("Press any key to continue");
        Console.ReadKey();

        cancelTokenSource?.Dispose();
    }

    private static void CalculateSum(int n, CancellationToken token)
    {
        Task.Run(() => {
            var sum = Calculator.Calculate(n, token);

            if (token.IsCancellationRequested)
                Console.WriteLine($"Sum for {n} cancelled...");
            else
                Console.WriteLine($"Sum for {n} = {sum}.");
        }, token);

        Console.WriteLine($"The task for {n} started...");
        Console.WriteLine("Enter N to cancel the request:");
    }
}