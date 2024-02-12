/*
 IsAlive property'si var threadlerin. IsAlive canlı mı?,hayatta mı demek true ise çalışıyor demektir. Bir thread running moddaysa IsAlive özelliği true olarak döner

 new Thread (Go).Start();   bir threadi böyle çağırabiliriz ancak bu threade ulaşamayız bunu bekleyemeyiz yani Join yapamayız.
 Thread (Go).Start() isimsiz bir thread oluşturduk ve run ettik Go' yu da main Thread çalıştırmış oldu yani bu işlemi hem main thread çalıştırmış oldu hem de child thread 
 çalıştırmış oldu.

 Her threadin kendine özgü registerları ve stack yapısı var.
 
 1 hread aynı anda 2 core çalıştıramaz çalıştırabilmesi için round robin yapması lazım. interruptlar geliyordu context switch yapması lazımdı ama performans düşüyordu. 
max peerformans için kesinlikle thread max core sayısını geçmemeli. round robiine dahil olmamak lazım
 
 */

/*
   UNSAFE KOD
     //safe bir program değil unsafe. Beklenildiği kadar hızlı çalışmama ihtimali var
    bool done;
    static void Main()
    {
        ThreadTest tt= new ThreadTest();
        new Thread(tt.Go).Start();
        tt.Go();
    }

    void Go()
    {
        // done false ise true yap ve ekrana Done yazdır.
        if (!done)
        {  
            done = true; Console.WriteLine("Done");  // burası kritik alan 
        } 
    }
 
 


2.ÖRNEK


static void Go()
{
    // done false ise true yap ve ekrana Done yazdır.
    if (!done) {  Console.WriteLine("Done"); done = true; } //çıktı   Done
                                                            //        Done          yani 2 tane Done yazdı
}
*/


/*
 
Join:

Threadi bekletir ve bloke eder. Bloke etmesi demek biz o anda cpu tarafından, core tarafından çalıştırılmayız
bizim beklemeye alınmamız sistemin toplam performansı açısından iyidir.


Sleep:

Join yerine Thread.Sleep kullanırız. içerisine yazılan milisaniye cinsindendir ve yazılan değer kadarda bekler.

Thread.Sleep(0): Özel bir duruma sahiptir ve çalışmış olduğumuz core'da sırayı bir sonraki thread'e verir.
yani çalışmasını sonlandırıp, quantum süresini doldurmaz hakkını sonraki threadlere verir onları çalıştırılmasını söyler. 
Sırada başka thread varsa sıra ona geçer ama yoksa Thread.Sleep(0) bir işe yaramaz
 
 
 
 */

using System;
using System.Threading.Channels;

class Program
{


    //YUKARIDAKİ UNSAFE KODU SAFE HALE GETİRME
    static bool done;
    static readonly object locker = new object();
    static void Go()
    {
        lock (locker)  //lock özel bir yapıdır. Bu özel yapı locker değişkeninin üzerine kilit atar
                       //ve bu da aynı anda bu değişken üzerine 1 tane thread kilit atabilir demektir. locker ı hangi thread çalıştırdıysa diğerleri lock'a geldiğinde beklemek zorundalar
        {
            Thread.Sleep(1000); //10 sn uyudu aşağıda işlemini yapıp bu lock'tan çıkar sıra sıra dahil olurlar. Ama sıralama olmadığı için hangi thread dahil olur bilemeyiz.
            if (!done) { Console.WriteLine("Done"); done = true; }
        }
        Console.WriteLine("Thread sonlandı"); //10'ar saniye aralarla çalışırlar beklerler.
    }







    //ÖRNEK2
    static void Go1()
    {
        Console.WriteLine("hello");
    }



    //ÖRNEK3
    static void Print(object message) // object message diyerek threadlere parametre verdik. parametre içeriinde birden fazla value tutabilir.
    {
        Console.WriteLine(message.ToString());
    }





    //ÖRNEK6
    static void Go3()
    {
        Console.WriteLine("Hello from " + Thread.CurrentThread.Name);
    }    
    
    
    //ÖRNEK7
    static void Go4()
    {
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(100);
            Console.WriteLine("Hello from " + Thread.CurrentThread.Name);

        }
    }



    //ÖRNEK8

    //static void Go5()
    //{ 
    //    throw null; 
    //}


    //ÖRNEK9
    static void Go6()
    {
        Console.WriteLine("Hello from " + Thread.CurrentThread.Name);
    }



    //ÖRNEK10
    static string DownloadString(string uri)
    {
        using (var wc = new System.Net.WebClient())
            return wc.DownloadString(uri);
    }


    static void Main(string[] args)
    {
        //ÖRNEK1

        //new Thread(Go).Start();
        //new Thread(Go).Start();
        //new Thread(Go).Start();
        //new Thread(Go).Start();
        //Go(); //5.thread
        ////Çıktı: ekrana 5 tane 10'ar saniye ile ' Thread sonlandı '  yazdırır

        Console.WriteLine();





        //ÖRNEK2
        Thread t= new Thread(Go1);
        t.Start();
        Go1();
        //Çıktı: ekrana 2 tane hello basıyor birisi t.Start(); dolayı diğeri de Go1() den dolayı

        Console.WriteLine();
        




        //ÖRNEK3
        //Threadlere parametre verme
        Thread th = new Thread(() => Print("Hello from th!"));
        th.Start();

        Thread th1= new Thread(Print);
        th1.Start("merhaba");


        //Threadi lamda exp. ( ()=> ) ile direkt oluşturmuş ve çalıştırmış
        new Thread(()=>
        {
            Console.WriteLine("I'm running on another thread");
            Console.WriteLine("This is so easy!");
        }).Start();

        Console.WriteLine();
        Console.WriteLine();





        //ÖRNEK4
        //lambda expressions bulundukları metotun değişkenlerini kullanabiliyorlar.
        for (int i = 0; i < 10; i++)
        {
            int temp = i; // böylelikle i deki değişiklik yansımıyor.
            new Thread(()=> Console.Write(temp)).Start();
        }
        //Çıktı: 0123456789 

        //.NET'te değişkenler blok bazlıdır. her blokta yenilenir yani yeni bir temp yeni bir temp diye alır.
        Console.WriteLine();
        Console.WriteLine(); 


        //ÖRNEK5
        Console.WriteLine("---");
      
        string text = "t1";
        Thread t1=new Thread(()=> Console.WriteLine(text)); //buraya t1 i aktardı

        text = "t2";
        Thread t2= new Thread(() => Console.WriteLine(text));

        t1.Start();  //t1 e start verince text'in son değeri t2 olduğu için t2 geldi
        t2.Start();
        //ÇIKTI: t2
        //       t2           yani 2 tane t2




        //ÖRNEK6
        //Thread'lere İsim Verme Olayı
        Thread.CurrentThread.Name = "main"; //Main Threadin adını 'main' olarak verdik
        Thread worker = new Thread(Go3);
        worker.Name = "worker"; //burada da name özelliği var
        worker.Start();
        Go3();

        Console.WriteLine();
        Console.WriteLine();



        /*Foreground ve BackGround Threadler 
         
        Threadin core'daki çalışma sistemini değiştirmez. Background threadler 
        Foreground thread, main thread kapansa bile çalışır. Foreground thread özel olarak kapatılmazsa kendi isteği ile kapanır.
        Ancak background threadler main thread kapandığında kapanır. uygulama sonlanınca o da sonlanır.
        Threadler defaultda foreground’dır. Yani default olarak bütün threadler Foreground'tır
        Bir thread default olarak background değildir.
        Performans farkı yoktur
        
        
        
         */


        //ÖRNEK7
        //Thread'lere İsim Verme Olayı
        Thread.CurrentThread.Name = "main"; //Main Threadin adını 'main' olarak verdik
        Thread worker1 = new Thread(Go4);
        worker1.Name = "worker"; //burada da name özelliği var
        //worker1.IsBackground = true;  //worker1 thread bizim background threadimiz olmuş oldu.
        worker1.Start();

        Console.WriteLine("Main thread çıkıyor...");

        //ÇIKTI: worker1.IsBackground = true; ile Background thread olduğu için ekrana sadece "Main thread çıkıyor..." çıktısını veririr
        //eğer o satır yazılmasaydı ekrana "Main thread çıkıyor..." ve 10 tane de "Hello from worker" yazdırır







        /*PRIORTY (ÜSTÜNLÜK) :
          
  
         * Bir thread diğer bir threade göre daha çok CPU zamanı alabilir yani quantum süresini daha çok uzatabiliyor.
         * Bir thread diğer threde göre daha uzun çalışabilir.

        Bütün processler normal olarak çalışmaya başlar. 5 tip öncelik var:
        Lowest(En düşük)
        Belownormal(Normal altı)
        Normal
        Abovenormal(Normal üstü)
        Highest(En yüksek)


        NOT:  Multithread bir sistemde a processin 2 threadi var, b nin 4 threadi var
              a processi 2*quantum olarak çalışır, b processi 4*quantum olarak çalışır

        */




        /*
         






         EXCEPTİON HANDLİNG(HATA YAKALAMA)    try/catch/finally

        Bir thread başka bir threaddeki hatayı yakalayamaz. Her threadin hata yakalama mekanizması kendisine özgürdür.
        Ör. Main thread içerisinde başlatılan bir thread’in çalışma esnasında hata ile karşılaşınca 
        kullanılacak olan try-catch işlemi Main thread’de yakalanamaz. Main thread yerine 
        çalıştığı fonksyion içerisinde try-catch işlemi yapılmalıdır

        try-catch : Program kırılmalarının önüne geçen, hataları yönettiğimiz bir blok
        try bloğunda hata olabileceğini beklediğimiz kodu yazarız
        catch : hata çıktığında oraya yönlendiriliyor
        finally: hata olsa da olmasa da en son çalışan bölüm
         
         */


        //ÖRNEK8

        //HATAYI MAİN THREAD YAKALAYACAK ama main thread child threadde oluşan bir hatayı yakalayamaz o yüzden bu kod sağlıklı değil.
        // try bloğunun bize faydalı olması new Thread(Go) diyoruz ya ola ki main thread tarafında bir hata oldu yani Ram de, bağlamada ya da çalıştırma seviyesinde. 
        //Daha worker threade geçmeden main threadde hata olursa buraya geliyoruz. Ama statrt ı verdikten sonra bu thread başlı başına bir program.
        // Bir thread hata üretirse o hata o threadin içerisinde çözülmesi gerekiyor. Her thread kendi hata mekanimasını kurgulamak ve çalıştırmakla yükümlü.
        // Go5 metodu ayrı bir threadde çalışıyor ve bu thread kendi kendine bir hata üretti o zaman Go yu kim çalıştırıyorsa o thread bu hatayı yakalamalı.
        //Main threadle alakası yok. Main threadin görevi o threadi run etmek

        //try
        //{
        //    new Thread(Go5).Start();  
        //}
        //catch (Exception ex)
        //{
        //    // We'll never get here!
        //    Console.WriteLine("Exception!");
        //}








        /*
         
         THREAD POOLİNG(THREAD HAVUZU)

        Programda eğer uzun sürüyorsa new Thread yapısı kullanılmalı. Ancka kısa aralıklarla çok 
        miktarda thread çalıştırılacak ise thread havuzu kullanılır. Performansı olumlu yönde 
        etkiler. Threadin oluşması zaman alabilir o yüzden havuz kullanılırsa new etme süresi 
        beklenemez.Thread havuzunda kullanılan threadlere name özelliği verilmez. Threadin 
        çalışma performansı değil oluşma performansı için kullanılır.
         


        Thread pool'u sık sık thread lkullanan yapılar için önerilir.
        Yazılım geliştiriyoruz thread kullanıoruz ama threadlerin çalışma süreleri saatlerle ifade ediliyor ya da program boyunca sürekli çalışıyor
        o zaman thread pool kullanmaya gerek yok. Thread pool kısa süreli çalışmalar için

        Thread.CurrentThread.IsThreadPoolThread. Current threadden sonra IsThreadPoolThread diye sorabiliyoruz 
        yani kullanılan thread thread havuzunun threadi mi yoksa kendi threadin mi bunu sorgularsın

        Thread havuzuna isim verilmez

        Thread havuzunun threadleri Background thread olarak tanımlanır bunu da değiştiremeyiz.

        Bloke olma özelliği zaman alıyor. thread havuzunun bloke olma özelliği zayıf
         */




        //ÖRNEK9

        //Task.Factory.StartNew(Go6); //ÖRNEK 8 deki try içindekine göre daha az zaman diliminde çalışmaya hazırlanır


        //ÖRNEK10

        Task<string> task = Task.Factory.StartNew<string>
     (() => DownloadString("http://kuzemrandevu.kku.edu.tr")); //geri dönüş değeri olan bir task
        string result = task.Result; //task.Result demek çalışma sonucunu bize getirir


        //QueueUserWorkItem : geri değer göndermez
    }


}





//https://www.albahari.com/threading/