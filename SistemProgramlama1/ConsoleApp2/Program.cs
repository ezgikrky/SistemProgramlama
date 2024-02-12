
using System.Threading.Channels;

class Program
{

    static Semaphore _pool = new Semaphore(5, 5);
    static int _padding;

    static void Semaphore(object threadId) 
    {
        Console.WriteLine("Thread {0} kritik bölgeye girmek için bekliyor.", threadId); //sıra numarası verildi
        _pool.WaitOne(); //WaitOne() ile kritik alana geçmek istedi YANİ İZİN İSTEDİ. senkronizayon mekanizması WaitOne ile devreye girdi.

        //Buradan Aşağısı KRİTİK ALAN. BURAYI THREADLER ARASINDA BÖLÜŞTÜRÜCEZ
        int padding = Interlocked.Add(ref _padding, 100); //atomik işlemle padding ile toplama işlemi gerçekleştirildi padding=padding+100 demek

        Console.WriteLine("Thread {0} kritik bölgeye girdi.", threadId);
        Thread.Sleep(1000 + padding); 

        Console.WriteLine("Thread {0} kritik bölgeden çıktı.", threadId); 
        _pool.Release();   //kritik alandan çıkmanın şartı işlemler ittikten sonra Release'i kullanmak.
    }
    static void Main(string[] args)
    {
        for (int i = 1; i <= 5; i++)
        {
            Thread thread = new Thread(Semaphore);
            thread.Start(i);
        }
    }






}
