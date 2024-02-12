/*
TASKLAR

Taskler, background threadler gibi çalışır. Yani Main Thread sonlandığında Task’lerde sonlanır.
Task'larda bir thread aslında ama thread havuzundan bunları kullanırız. Thread havuzundan kullanmamızın faydası threadin newlememiz vakit alıyordu dolayısıyla kısa aralıklarla 
sık sık thread kullanacaksak eğer havuzdan kullanmmaız performans açısından daha iyi olacağı için. Program boyunca kullanaksak thread oluşturmamız daha mantıklı
Thread havuzu ile normal thread arasında herhangi bir performans farkı yok

Taskların özellikleri:
1- Tasklara isim verilemez 
2- Tasklar background olmalı
3- Tasklardan çalıştığında geriye dönüş alabiliyoruz
 
Taskları creat ettikten sonra Start() ile, StartNew() ile ve Run ile çalıştırabiliyoruz.
 
Taskları ve threadleri çalıştırırken metot veririz.

Tasklardaki Wait() , threadlerdeki Join() 'e benzer.

task.Wait(10000); bu wait en fazla 10 saniye bekler demek oluyor. İlla 10 saniye beklemez 1 saniyede bittiyse 1 snde döner ama max bekleme süresi 10 oluyor

IsCompleted :Bir Task'ın tamamlanıp tamamlanmadığın anlamak için IsCompleted özelliği kullanılabilir.

WaitAny(); dizi içerisinden gelen taskların çalışmasını bekler ilk kim bitirdiyse onun sıra numarasını alır.
Taskları çalışmasını beklemeye geçirir ve herhangi biri bitirdiğinde onun ıd değerini taşır
 

WaitAl(); Array'in içindeki tüm Tasklerin bitmesini bekler.

Parallel.Invoke(); birden fazla parametreyi içinde bulundurabilir. 
Parallel.Invoke(() => DoSomeWork(), () => DoSomeOtherWork());   lambda expressions ile çalıştırabiliyoruz. 
Tasklara isim vermeden doğrudan çalıştırabiliyouz


Taskların geri dönüş değerleri var

Taskları sistem bazen aynı core içinde çalıştırabiliyor. 


https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=net-8.0
https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming
https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.status?view=net-7.0




İşletim sisteminde 3 tane ana state vardı: Run, Ready, Block
Unstarted; daha start edilmemiş
Running; çalışma durumuna ge.ilir
WaitSleepJoin; Thread bloke edilmişse WaitSleepJoin olur.



Lock ile diğer threadler bekleme moduna geçer



Monitor.Enter dan sonra Monitor.Exit kullanılmalı:

Monitor.Enter (_locker);
try
{
  if (_val2 != 0) Console.WriteLine (_val1 / _val2);
  _val2 = 0;
}
finally { Monitor.Exit (_locker); } 






bool lockTaken = false;
try
{
  Monitor.Enter (_locker, ref lockTaken);  //_locker ile nesnemizi veriyoruz sonra da lockTaken Enter kilidi attıysa true değerini verecektir.
  // lockTaken; Enter kilidi attıysa  true olur
}
finally { if (lockTaken) Monitor.Exit (_locker); }  kilit atılmamışsa bizim Exit dememiz soru  oluşturur





//List kullanacaksak kilitlememiz lazım. çoklu threadde kullanacaksak kilit atmak mecburiyetindeyiz.
class ThreadSafe
{
  List <string> _list = new List <string>();
 
  void Test()
  {
    lock (_list)
    {
      _list.Add ("Item 1");
      ...



//lock'ı kilitlediysek bir daha bir daha kilitleyebiliriz. Ama ne kadar kilit attıysak o kadar kilidi çözmemiz lazım.






DEADLOCK:
*Deadlock'a düşmemeye çalışacağız. Eğer düştüysek orada processler ya da threadlerden biri öldürülür.
*Deadlock'a düşüldüyse algoritmik bir problem var demektir.
*Deadlock'a düşebilmek için en az 2 senkronizasyon nesnesi(locker1,locker2) gerekiyor. En az 2 tane de thread(main thread,child thread) olmalı
*Senkronizasyon nesnelerine priority yani üstünlük sıra numarası vericez. Yani her thread 2 tane senkronizasyon nesnesi kullanacaksa 
o zaman priority'sine göre önce 1'i sonra 2'yi kullanacak






MUTEX:
Mutex'i hem process arasında hemde process içinde kullanılır. Birden fazla processte kullanmak istersek unique string bir isim verilir.


WaitOne: Bekleme yapılır. süre verirsen o kadar bekler ve ardından cevap vermzse false gelir.




SemaphoreSlim semaphore ile aynı


Process içerisindeyse lock ve monitor kullanırız. Processler arasındaysa semaphore kullanırız

Semaphore ve Mutex'in farkı interprocess olması

Semaphore'un farkı
1-kritik alana birden fazla thread geçebilir
2-Kritik alana geçtikten sonra kilidi başka bir thread açabilir.

Semaphore kilidi kim attıyssa sadece o açabilirden ziyade isteyen thread açabilir özelliğine sahip


https://www.albahari.com/threading/part2.aspx

 */



class Program
{
    static void Main(string[] args)
    {

        //ÖRNEK1
        //Taskları 3 şekilde başlatabiliriz.
        Action<object> action = (object obj) =>
        {
            Console.WriteLine("Task={0}, obj={1}, Thread={2}",
            Task.CurrentId, obj,
            Thread.CurrentThread.ManagedThreadId);
        };


        //1- Start() ile
        Task t1 = new Task(action, "alpha");
        t1.Start();
        Console.WriteLine("t1 has been launched. (Main Thread={0})",Thread.CurrentThread.ManagedThreadId);
        t1.Wait();


        //2- Task.Factory.StartNew(action, parametre) ile
        Task t2 = Task.Factory.StartNew(action, "beta");
        t2.Wait();




        //3. Task.Run(Action) ile
        String taskData = "delta";
        Task t3 = Task.Run(() => {
            Console.WriteLine("Task={0}, obj={1}, Thread={2}",
                                                     Task.CurrentId, taskData,
                                                      Thread.CurrentThread.ManagedThreadId);
        });
        t3.Wait();

        Console.WriteLine();





        //ÖRNEK2
        Task t = Task.Factory.StartNew(() => {

            int ctr = 0;
            for (ctr = 0; ctr <= 1000000; ctr++)
            { }
            Console.WriteLine("Finished {0} loop iterations",
                              ctr);
        });
        t.Wait(); //tasklar background thread oldukları için Wait  yazmamız şart.





        //ÖRNEK3
        Task taskA = Task.Run(() => Thread.Sleep(2000));
        Console.WriteLine("taskA Status: {0}", taskA.Status); //taskA.Status taskA'nın o anki durumundan bahseder. Çalışmaya hazırım bekliyorum der
        try
        {
            taskA.Wait();  //Task'ı bekliyoruz
            Console.WriteLine("taskA Status: {0}", taskA.Status); //Task'ın status'unu istiyoruz ve çalışmasını bitiriyor
        }
        catch (AggregateException)
        {
            Console.WriteLine("Exception in taskA.");
        }
        Console.WriteLine();





        //ÖRNEK4
        //IsCompleted :Bir Task'ın tamamlanıp tamamlanmadığın anlamak için IsCompleted özelliği kullanılabilir.

        Task taskB = Task.Run(() => Thread.Sleep(2000));
        try
        {
            taskB.Wait(1000);       // Wait for 1 second. 1 saniyede çıktık
            bool completed = taskB.IsCompleted;
            Console.WriteLine("Task B completed: {0}, Status: {1}",
                             completed, taskB.Status); //Status çalışır modda
            if (!completed) //bitmediyse eğer süresini doldurmadan çıkmış olduk
                Console.WriteLine("Timed out before task B completed.");
        }
        catch (AggregateException)
        {
            Console.WriteLine("Exception in taskB.");
        }

        Console.WriteLine();




        //ÖRNEK5
        var tasks = new Task[3]; //3 task var
        var rnd = new Random();
        for (int ctr = 0; ctr <= 2; ctr++)
            tasks[ctr] = Task.Run(() => Thread.Sleep(rnd.Next(500, 3000))); //her bir taskı 5ms ile 3 sn arasındaki değerlerde uyutuyor

        try
        {
            int index = Task.WaitAny(tasks); //parametre olarak tasks dizimizin bütün elemanlarının çalışmasına bak herhangi biri bitirdiğinde onun sıra numarasını ver demek.
            Console.WriteLine("Task #{0} completed first.\n", tasks[index].Id);
            Console.WriteLine("Status of all tasks:");
            foreach (var a in tasks)
                Console.WriteLine("   Task #{0}: {1}", a.Id, a.Status);
        }
        catch (AggregateException)
        {
            Console.WriteLine("An exception occurred.");
        }
        Console.WriteLine();






        //ÖRNEK6

        //DEADLOCK'tan kurtulma hali. senkronizasyon nesnelerine sıra numarası verdik öncelik yani

        object locker1 = new object();
        object locker2 = new object();
        Console.WriteLine("1");

        new Thread(() => { 
            lock (locker1)  
            {
                Console.WriteLine("2");  
                Thread.Sleep(1000);     
                lock (locker2) ;       
                Console.WriteLine("3");
            }
        }).Start();

       
        lock (locker1)
        {
            Console.WriteLine("4"); 
            Thread.Sleep(1000);     
            lock (locker2) ;       
            Console.WriteLine("5");
        }


        Console.WriteLine();

        //YUKARIDAKİ İLE KARIŞMASIN DİYE YORUM SATIRINA ALDIM

        ////DEADLOCK
        ////burada main thread locker1 i bekliyor. child threadde locker2 yi bekliyor. Yani birbirlerini karşılıklı olarak sonsuza kadar bekliyorlar.
        ////burada locker1=locker3 gibi düşünelin locker2=locker4
        ///
        //object locker3 = new object();
        //object locker4 = new object();
        //Console.WriteLine("1");

        //new Thread(() =>
        //{ //child thread
        //    lock (locker3)  //locker1 i child thread kilitledi.
        //    {
        //        Console.WriteLine("2");  //2 yi yazdı
        //        Thread.Sleep(1000);     //uyudu
        //        lock (locker4) ;       // Deadlock. uyanınca locker2 ye kilit atmak istedi. child thread 2 yi kilitledi 1 i kilitleek istiyor
        //        Console.WriteLine("3");
        //    }
        //}).Start();

        ////locker1 buradan çıkabilmek için locker2 yi bekleyecek
        //lock (locker4) //main thread. main thread locker2 yi kilitledi. 
        //{
        //    Console.WriteLine("4");  //4 yazdı
        //    Thread.Sleep(1000);     //uyudu
        //    lock (locker3) ;       // Deadlock. uyanınca locker1 e kilitlemek istedi yani main thread 2 yi kilitledi 1 i kilitlemek istiyor
        //    Console.WriteLine("5");
        //}
        ////ÇIKTI: 1 4 2    -->  3 ile 5 çıkmadı sebebi de ölümcül kilit var. 


    }
}