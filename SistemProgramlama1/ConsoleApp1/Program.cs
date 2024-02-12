
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static int number;
    
    static EventWaitHandle _go = new AutoResetEvent(false);
    static EventWaitHandle myResetEvent = new AutoResetEvent(false);
    static void m2()
    {
        while (true)
        {
            myResetEvent.WaitOne();
            Console.WriteLine(number);

        }
    }
    static void Main(string[] args)
    {
        //Thread m1 = new Thread(m2);
        //m1.Name = "Reader Thread";
        //m1.Start();
        //for (int i = 1; i <= 3; i++)
        //{
        //    Console.WriteLine(i);
        //    number = i;
        //    myResetEvent.Set();
        //    Thread.Sleep(1);
        //}


        Task taskB = Task.Run(() => Thread.Sleep(2000));
        try
        {
            taskB.Wait(1000);       // Wait for 1 second. 1 saniyede çıktık
            bool completed = taskB.IsCompleted;
            Console.WriteLine(completed); //Status çalışır modda
            if (!completed) //bitmediyse eğer süresini doldurmadan çıkmış olduk
                Console.WriteLine("Timed out ");
        }
        catch (AggregateException)
        {
            Console.WriteLine("Exception in taskB.");
        }


    }
}





