

/*
 COUNTDOWNEVENT

 Bir olayın farklı threadlerle ya da farklı sayıda yapılması istenildiğinde(20 defa, 30 defa, 50 defa gibi bir döngü içerisinde olması istenildiğinde) kullanılır
 Bir threadin kendisinin belirlemiş olduğu miktarda işlemi diğer threadlerin yapmasını bekleme olayına CountdownEvent denir.

 Örneğin 3 Threadimiz var bu 3 adet threadin çalışması bittiğinde farklı bir iş yapacağımızı farz edelim. 
 t1.Join(); t2.Join(); t3.Join(); burada 3 adet threadi çalıştırıp sırasıyla beklemiş olduk sonra 3 ünden de olumlu cevap geldiğinde main thread kaldığı yerde devam etti
 Bu işlemi Join'lerle de yapabiliriz. CountdownEvent ile de

 İşin özünde birden fazla threadin herhangi bir süreci n defa tekralamasını sağlar bize.
 
 */





/*
 BARRIER CLASSI 
 
 Threadlerin senkronizasyonu için kullanılan sınıflardan birisidir. Thread'e verilen görevler birkaç aşamadan oluşabilir. 

 Örneğin 3 tane thread var ve bu threadlerin 4 turu/aşaması var. 3 thread'in de tüm turlarının aynı anda bitirmesi isteniyor. Yani 3 thread de 1. turu bitirmeden diğer
turlara geçmesin.

 Birbirleri ile senkron hareket edilsin isteniyor. Farklı konumlarda olsa da aynı seviyede, aynı katmanda yürümesini istediğimiz işleri zamansal boyutta eşitliyoruz.
 Yani hepsi farklı zamanlarda bitse de amacımız bu farklı sürede iş yapanları aynı zaman diliminde 2. işe başlatmak. Bu işi en yavaş kim yapıyorsa o kadar bekletilecek.
 Barrier'de belirlemiş olduğumuz threadlere katman bazlı bakacak olursak işlerini bitirip birbirlerini bekleyip sonra 2. işe geçecekler

 
 */





/*
 
 https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.parallel.for?view=net-7.0

 PARALLEL.FOR Methodu
 
 .NET tarafından bizim için oluşturulmuş ve threadlerle aynı işi kolayca çözmemizi sağlayan For döngüsünü paravize eden bir ifade. Yani For döngüsünü paralel çalıştırmış oluyor.

 Yapılacak olan for işlemini core sayısı kadar Thread'e ayırırız. Ancak işlemler sıralı gerçekleşmez. İç içe Parallel For kullanılabilir.

 Parallel For'un 2 temel amacı vardır:
 1-Performans kazanmak: Her zaman performans kazanılmaz. Bazen normal for'dan daha kötü sonuç verebilir.
 2-Daha fazla iş yapmak



-->  Oluşturma

 Parallel.For(0, 100, (i) => { Console.WriteLine(i) };)  

1. parametre: Döngü başlangıç değeri --> 0
2. parametre: Döngü bitiş değeri     --> 100
3. parametre: Çalıştırılacak Action.(Lambda olarak verilebilir.) Yani döngü değişkeni yazılacak int olarak döndürüyor  --> (i)

Parallel.For 1'er 1'er ilerler
 




-->  5 Parametreli Oluşturma

Buradaki amaç gereksiz yere lock kullanımını önleyip performans kazanmaktır. Her bir Thread
lokalindeki parametre ile işlem yaparak sistem değişkenlerine erişmez dolayısıyla sadece sistem
değişkeni olan genelToplam değişkenine lokaldeki değerleri toplamak için lock kullanılır.Parallel.For(1, 1000,
    () => 0.0,
    (index, state, lokalToplam) => lokalToplam + Math.Sqrt(index),
    localTotal => {lock(locker) genelToplam += lokalToplam; });1. parametre: Döngü başlangıç değeri
2. parametre: Döngü bitiş değeri
3. parametre: Her bir Thread'in lokalToplam değişkeninin ilk atanacağı değer. (Double olarak
atanmış)
4. parametre: Her bir Thread'in lokalinde lokalToplam değişkeni ile yapacağı işlem
5. parametre: Her bir Thread, tamamen işlerini bitirdikten sonra yapılacak işlem
Paralel For'da çalışacak her bir Thread gereksiz yere lock kullanmaması açısında ilgili toplam
parametresi lokal olarak indirgenir. Yani her bir Thread kendine ait lokalToplam parametresini
artırır ve işlerini bitirdikten sonra bu lokalToplam parametreleri toplanarak genelToplam sonucu
elde edilir.
 */



using System.Net.Sockets;

class Program
{


    #region ÖRNEK1-CountdownEvent
    static void MetinYaz(object msg)
    {
        Thread.Sleep(1000);
        Console.WriteLine(msg);
        cde.Signal(); //işlerimizi bitirdik CountdownEvent'i uyarıyoruz
                      //CountdownEvent 3 ile oluşmuştu her sinyalde 3 değeri 1 düşecek. Oluşturduktan sonra 2-1-0  a düşecek 0 a düşünce bekleme moduna geçecek

    }

    static CountdownEvent cde = new CountdownEvent(3);

    //CountdownEvent sağladıı imkan; bir counter belirleyip bu counter sayacı her sinyalden sonra 1 düşüyor. 0'a geldiğinde bunu bekleyeni aktif hale getiriyor
    #endregion



    #region ÖRNEK1.2 CountdownEvent 
    static int toplam5 = 0;
    static CountdownEvent cde2 = new CountdownEvent(10);
    static void threadFunction()
    {
        for (int i = 0; i < 10000000; i++)
        {
            Interlocked.Increment(ref toplam5);
        }
        cde2.Signal();
    }
    #endregion


    #region Örnek2-Barrier
    static Barrier br = new Barrier(3); //buradaki 3, 3 thread birbirini bekleyerek işlem yapacak demek oluyor
    static void Yaz()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.Write(i + "-");
            br.SignalAndWait(); //3 sinyal gelince aktif hale gelecek ve sırasıyla veriler ekrana gelecek
        }
    }


    #endregion


    


    static void Main(string[] args)
    {


        #region ÖRNEK1-CountdownEvent
        //Thread oluşturma
        //unsafe bir kod çıktısı karışacak
        new Thread(MetinYaz).Start("Merhaba Bilgisayar Mühendisliği Öğrencileri");
        new Thread(MetinYaz).Start("Merhaba Bilgisayar Mühendisliği Öğrencileri");
        new Thread(MetinYaz).Start("Merhaba Bilgisayar Mühendisliği Öğrencileri");

        cde.Wait(); // bunu yazınca yukarıdaki 3'ünü yazar daha sonra  "Sizi aramızda görmek çok güzel." çıktısını verecek. Bu satır olmasaydı önce aşağıdakinin çıktısını 
                    //verip sonra 3 ünü ekrana basacaktı
                    // burada bu main threaddi Main thread 3 adet threadi gönderdi her biri kendine göre ekranda bir iş yaptı ve getirdi.
                    //Wait metodu ile main thread CountdownEvent'in sayısının 0'a düşmesini bekler.


        Console.WriteLine("Sizi aramızda görmek çok güzel.");
        #endregion

        Console.WriteLine();





        #region ÖRNEK1.2 CountdownEvent 

        for (int i = 0; i < 10; i++)
        {
            new Thread(threadFunction).Start();
        }
        cde2.Wait();
        Console.WriteLine("Toplam: " + toplam5);

        #endregion
        Console.WriteLine();





        #region Örnek2-Barrier
        new Thread(Yaz).Start();
        new Thread(Yaz).Start();
        new Thread(Yaz).Start();
        //çıktı: 0-0-0-1-1-1-2-2-2-3-3-3-4-4-4-5-5-5-6-6-6-7-7-7-8-8-8-9-9-9-

        #endregion

        Console.WriteLine();


        #region Örnek3-Barrier

        // https://learn.microsoft.com/en-us/dotnet/api/system.threading.barrier?view=net-7.0

       
            int count = 0;

            //Farklı bir yöntem
            Barrier barrier = new Barrier(3, (x) =>
            {
                Console.WriteLine("Aşama: " + x.CurrentPhaseNumber);

                //if (x.CurrentPhaseNumber == 2)   //bu satırları açınca 2.aşamadan sonrasına geçemiyorsun diğer kodlarda çalışmıyor
                //{
                //    throw new Exception("2. Aşamadan Sonrasına Geçemen");
                //}

            });

            /*

             Her 3 lük tamamlandığında 1 işlem yaptırabiliriz

             1. parametre: Kaç Thread'in çalışacağını belirler. Yani 3
             2. parametre: Count değeri 0 olduğunda çalışacak fonksiyondur. (x) Yani her aşamaya geçişte bir
            çalışır. Gönderilen parametrenin mevcut aşamasını görmek için CurrentPhaseNumber
            özelliği kullanılır. Hata fırlatılarak ileriki aşamalara geçilmesi engellenebilir. Yakalamak için
            try-catch kullanılabilir.


             */


            barrier.AddParticipants(2); //metodu ile parametre gönderilen değer Barrier sınıfın 1.parametresini artırır.
                                        // Yani 1.parametrenin 3 olduğunu varsayarsak. Artık 3 değil de 2 tane daha ekledik 5 adet oldu
                                        //5 adet Thread sinyal gönderdiğinde bir sonraki aşamaya geçilir.



            barrier.RemoveParticipant(); //yukarıdakinin tam tersi şekilde çalışır. Silme işlemi yapar. 
                                         //5 tane Thread için çalışan Barrier artık 3 tane thread için çalışır.


            Action action = () => // Action; geri dönüşü olmayan metotlardı. Function; geri dönüşü olan metotlar
                                  // parametre almadığı için () şartımız vardı
            {
                Interlocked.Increment(ref count);   // count değerini arttırmış
                barrier.SignalAndWait();           // burada bekliyor. SignalAndWait metodu, ilgil thread işini bitirdekten sonra Barrier'e sinyal yollar ve diğerleri işini bitirmediyse beklemeye başlar.

                Interlocked.Increment(ref count); // count değerini bir daha arttırmış
                barrier.SignalAndWait();         // yine bekliyor

                Interlocked.Increment(ref count); //yine arttırdı

                try
                {
                    //try ile SignalAndWait'e döndü. Çünkü yukarıda exception atılacak onu yakalaması gerekiyor

                    barrier.SignalAndWait();
                }
                catch (BarrierPostPhaseException bppe)
                {
                    Console.WriteLine("Caught BarrierPostPhaseException: {0}", bppe.Message); // cw ile exception'ı (istisna) yakaladı
                }

                Interlocked.Increment(ref count); //tekrardan arttırdı
                barrier.SignalAndWait();         // son SignalAndWait ile çıkmış oldu
            };

            Parallel.Invoke(action, action, action, action); //Virgül atıldığı sürece istenildiği kadar metot paralel şekilde çalıştırılabilir.
            barrier.Dispose();


        /*
         Parallel.Invoke

         Action'ları çalıştırmak için kullanılır. İçerisinde sınırsız Action parametresi(varargs) ya da Action dizisi alabilir. Action'lar paralel bir şekilde çalışır ve işleri bitmeden durmazlar.

         //Varargs Oluşturma:
         Parallel.Invoke(action, action, action, action);

         //Dizi Oluşturma:
         Action[] actions = new Action[5];
         Parallel.Invoke(actions);

         */





        #endregion
        Console.WriteLine();


        #region Örnek4-Parallel.For

        Parallel.For(0, 100, new ParallelOptions { MaxDegreeOfParallelism = 4 }, (i) => //0 dan başlayacak 100 e kadar
                                                                                        //new ParallelOptions {MaxDegreeOfParallelism=4} aynı anda 4 thread çalıştırır demek oluyor. Thread sayısını böyle biz belirleyebiliyoruz
                                                                                        // yazmazsakta olur max core sayısı kadar yazacak
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " -- " + i); //thread ıd'sini yazdırdık

        });

        // .Net(Buradaki yapı) core sayısını geçmez. Ne kadar for kullanırsak kullanaım  bu yapı kesinlikle aynı anda çalışacağı threadlerin core sayısını geçmez.
        // Yani aynı anda kaç core varsa o kadar thread ile çalışır yapı


        #endregion

        Console.WriteLine();




        #region Örnek5-Parallel.For-5ParametreliOluşturma

        int toplam = 0;
        Object obj= new object();

        Parallel.For(0, 101,  
            () => 1,              // Kaç thread gelirse o kadar çalışacak. Her thread 1 defa bunu çalıştıracak. Her bir thread 1 den başlayacak
            (i, state, loop) =>   // 3 parametreli lambda exp var. body kısmı yani burası çalışacak
                                  // loop i ile gelen değer. ilk başta loop 0 alacak
            {
                return i + loop; // 100 defa çalışacak. Burada iç içe döngü var. 1.thread 0 aldı i=3; 0+3=3  1.thread i=7 geldi;  7+3=10 oldu böyle böyle 100 işlem yapılacak

            },
            loop => { lock (obj)
                {
                    toplam += loop;
                    Console.WriteLine(loop);
                }
            });

        Console.WriteLine("--- "+ toplam);
        #endregion

        Console.WriteLine();




        #region Örnek6-Parallel.For-5ParametreliOluşturma

        toplam = 0;
        obj = new object();

        Parallel.For(0, 10001, new ParallelOptions { MaxDegreeOfParallelism = 4 },
            () => { Console.WriteLine(Thread.CurrentThread.ManagedThreadId); return 1; }, // Kaç thread gelirse o kadar çalışacak. Her thread 1 defa bunu çalıştıracak. Her bir thread 1 den başlayacak
            (i, state, loop) =>   // 3 parametreli lambda exp var. body kısmı yani burası çalışacak
                                  // loop i ile gelen değer. ilk başta loop 0 alacak
            {
                return i + loop; // 100 defa çalışacak. Burada iç içe döngü var. 1.thread 0 aldı i=3; 0+3=3  1.thread i=7 geldi;  7+3=10 oldu böyle böyle 100 işlem yapılacak

            },
            loop => {
                lock (obj)
                {
                    toplam += loop;
                    Console.WriteLine(loop);
                }
            });

        Console.WriteLine("--- " + toplam);
        #endregion

        Console.WriteLine();





        
        #region Örnek7-Parallel.For-SayfadanAldı
        Parallel.For(0, 100, ctr =>
        {
            Random rnd = new Random(ctr * 100000);
            Byte[] bytes = new Byte[100];
            rnd.NextBytes(bytes);
            int sum = 0;
            foreach (var byt in bytes)
                sum += byt;
            Console.WriteLine("Iteration {0,2}: {1:N0}", ctr, sum);

        });
        #endregion

        Console.WriteLine();


        //https://tr.wikipedia.org/wiki/Pi_say%C4%B1s%C4%B1
        // 4(1-1/3+1/5-1/7+1/9...) çözmemizi istiyor


    }
}