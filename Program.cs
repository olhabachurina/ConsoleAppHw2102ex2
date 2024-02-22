
using System.Diagnostics;

namespace ConsoleAppHw2102ex2;

 class Program
{
    private static int counter = 0;
    private static readonly object lockObject = new object();
    private static Mutex mutex = new Mutex();
    static void Main()
    {
        Console.WriteLine("Начало работы с Lock:");
        IncreaseCounterWithLock();
        ResetCounter();
        Console.WriteLine("\nНачало работы с Monitor:");
        IncreaseCounterWithMonitor();
        ResetCounter();
        Console.WriteLine("\nНачало работы с Mutex:");
        IncreaseCounterWithMutex();
        ResetCounter();
        Console.WriteLine("Все задачи завершены. Нажмите любую клавишу для выхода.");
        Console.ReadKey();
    }
    private static void IncreaseCounterWithLock()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        Thread[] threads = new Thread[5];
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                
                for (int j = 0; j < 1000000; j++)
                {
                    lock (lockObject)
                    {
                        counter++;
                    }
                }
               
            });
            threads[i].Start();
        }
        foreach (var thread in threads) thread.Join();
        stopwatch.Stop();
        Console.WriteLine($"Lock: Затраченное время: {stopwatch.ElapsedMilliseconds} ms.");
    }
    private static void IncreaseCounterWithMonitor()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        Thread[] threads = new Thread[5];
        for (int i = 0; i < threads.Length; i++)
        {
           
            threads[i] = new Thread(() =>
            {
                for (int j = 0; j < 1000000; j++)
                {
                    bool lockTaken = false;
                    try
                    {
                        Monitor.Enter(lockObject, ref lockTaken);
                        counter++;
                    }
                    finally
                    {
                        if (lockTaken) Monitor.Exit(lockObject);
                    }
                   
                }
            });
            threads[i].Start();
        }
        foreach (var thread in threads) thread.Join();
        stopwatch.Stop();
        Console.WriteLine($"Monitor: Затраченное время: {stopwatch.ElapsedMilliseconds} ms.");
    }
    private static void IncreaseCounterWithMutex()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        Thread[] threads = new Thread[5];
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
               
                for (int j = 0; j < 1000000; j++)
                {
                    mutex.WaitOne();
                    counter++;
                    mutex.ReleaseMutex();
                }
              
            });
            threads[i].Start();
        }
        foreach (var thread in threads) thread.Join();
        stopwatch.Stop();
        Console.WriteLine($"Mutex: Затраченное время: {stopwatch.ElapsedMilliseconds} ms.");
    }
    private static void ResetCounter()
    {
        counter = 0;
    }
}
