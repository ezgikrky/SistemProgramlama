/*
SEMAPHORE

Semaphore process için kullanılabiliyor
Semaphore processler arasında kullanılabiliyor
Kritik alana birden fazla thred ya da processin girmesine müsade ediyor
Kilidi kim attıysa o kapatır kuralı burada geçerli değil isteyen herkes kilidi kaldırabilir


Bütün senkronizasyon nesnelerinin amacı kritik alanı korumaya almaya çalışmaktır. 
Kritik alan sistem değişkenlerinin, global değişkenerinin olduğu bölgedir.
O bölgeye giriş çıkış kontrol altına alınırsa meydana gelecek belirsizliklerin önüne geçilmiş olunur.
Senkronizasyon nesneleri ile kritik alanı kontrol altına almaya çalışıyoruz.
Genelde kritik alanda bir tane thread bulunurken Semaphore ile bu sayıyı arttırabiliriz.
Bütün senkronizasyon nesneleri kritik alanda kilit atmak ister, kritik alandan çıktıktan sonrada kilidi açar.
 

WaitOne() ile threadler izin ister ve bunlardan uygun olanlar kritik alana geçer

Interlocked atomik işlemler için kullanılır. 

Atomik işlem değişkenin değerinin değişmesini doğru olarak yapılmasını sağlar. Atomik komut ile cpu da sadece bir tanesi onun değişikliğini değiştiriyor. 
yani diğer threadler bekleme moduna geçer.Yani atomik komut demek cpu üzerinde sadece bir kişi tarafından çalıştırılması garanti edilir. 
Sadece işlemi yaparken diğerleri o değişkenle işleri varsa beklerler.



AutoResetEvent :
Set'ten sonra kapının otomatik kapanmasını ve sadece 1 kişinin geçip diğerlerinin beklemesin garanti eder.

ManualResetEvent :
Kişilerin geçmesini garanti eder ama kaç kişinin geçtiğini bilemez. Kapıyı Set ile açar bizim geri Reset ile kapatmamız gerekir.
pdfte var


https://learn.microsoft.com/en-us/dotnet/api/system.threading.manualresetevent?view=net-8.0
https://www.albahari.com/threading/part2.aspx





ATOMİK KOMUTLAR:
INTERLOCKED: 
Atomik komutlara Interlock sınıfı güzel bir örnek. ÖRNEK4

Interlocked.Add(ref _padding,100);    padding değeri ile100 ü toplamamızı sağlar
Interlocked.Increment(ref a);           a değerini 1 arttırır
Interlocked.Decrement(ref a);            referans değerini 1 i çıkartıyor yani a=a-1 yapıyor
Interlocked.Exchange(ref a ,123);  // Exchange ile a ya 123 değerini yükle dedik

Exchange (=)
Atomik bir eşitleme işlemi için kullanılır. ("=" yerine kullanılır)

            

 */

using System.Reflection;
using System.Threading;
using System.Threading.Channels;

class Program
{

  //ÖRNEK1
  static Semaphore _pool;
  static int _padding;

    static void Worker(object num) //sıra numarası verildi
    {

        Console.WriteLine("Thread {0} begins and waits for the semaphore.", num); //sıra numarasını yazdı
        _pool.WaitOne(); //WaitOne() ile kritik alana geçmek istedi YANİ İZİN İSTEDİ. senkronizayon mekanizması WaitOne ile devreye girdi.

        //Buradan Aşağısı KRİTİK ALAN. BURAYI THREADLER ARASINDA BÖLÜŞTÜRÜCEZ
        int padding = Interlocked.Add(ref _padding, 100); //atomik işlemle padding ile toplama işlemi gerçekleştirildi padding=padding+100 demek

        Console.WriteLine("Thread {0} enters the semaphore.", num);
        Thread.Sleep(1000 + padding);

        Console.WriteLine("Thread {0} releases the semaphore.", num);
        Console.WriteLine("Thread {0} previous semaphore count: {1}", num, _pool.Release()); //kritik alandan çıkmanın şartı işlemler ittikten sonra Release'i kullanmak.
                                                                                             //Kendisi gelip buradan çıkmış oldu bu çıkar çıkmaz sırada bekleyen diğeri gelir ve sırada 3 kişi bekliyor
    }







    //ÖRNEK2
    static EventWaitHandle _ready = new AutoResetEvent(false);
    static EventWaitHandle _go = new AutoResetEvent(false);
    static readonly object _locker = new object();
    static string _message;


    static void Work()
    {
        while (true)   //sonsuz while döngüsüne girdi
        {
            _ready.Set();                          // ready 'i çalıştırdı ve mesajı ooo diye yazdırdı
            _go.WaitOne();                         // şimdi worker thread bekleme moduna geçti. bloke oldu.
            lock (_locker)                         //WORKER THREAD ÇALIŞMAYA BAŞLADI AMA READY İ AŞAĞIDA BEKLETTİK
            {
                if (_message == null) return;        // locker ile mesahj nulsa çıkacak değilse ekrana yazacak
                Console.WriteLine(_message);
            }
        }
    }











    //ÖRNEK3
    static ManualResetEvent mre = new ManualResetEvent(false);  //Manuel reset event kapı kapalı

    static void threadFunction()
    {
        string name = Thread.CurrentThread.Name;                      //Name'i aldı
        Console.WriteLine("Ben Geldim " + name);                      //Ben Geldim diyip yazdı
        mre.WaitOne();                                                //mre.WaitOne'ı çalıştırdı. mre.WaitOne çalışınca ben aslında kilit istiyorum senden onay bekliyorum ifadesini göndermiş oldu. Main kısmına döndük
        Console.WriteLine();
        Console.WriteLine("Kritik Alana Girdi ve Bitti " + name);
        Console.WriteLine();
    }

    // mre.WaitOne();  noktasında birden fazla bekleyen olabilir. Yine kapıyı 1 kişi açacak. 3 thread bekliyor. Kapı set ile açılacak ve açıldıktan sonra WaitOnedan sonra kağı açılınca 3 kişiş buraya dahil oldu
    //Çünkü bu manuelResetEvent yani Set'ten sonra kapıyı sen kapatacaksın demek otomatik kapanmayacak anlamına gelir. Açtın kapatmadın, bekleyen herkes buradan geçer.
    //Eğerki AutoResetEvent olsaydı set'ten sonra 1 tanesi geçecekti, bekleyen 2 kişi içinde 2 tane set etmek gerekecekti.
    //Set'ten sonra WaitOne çalışır hemen ardından da Reset çalışır.






    //ÖRNEK5

    static int sem_ = 0;
    static void bekle()
    {
        while (sem_ == 1) ; // sem_ 1 olduğu sürece bekle
        sem_ = 1;           //sem_ 1 olmaktan çıktıktan sonra buraya gel ve sem_  i 1 yap
        Console.WriteLine("sem açıldı");


    }
    static void coz()
    {
        sem_ = 0;
    }
    



    //ÖRNEK6
    //kendi semaphor'umuzu yazmış olduk
    static int sem1_=0;
    static void waitone()
    {
        while (Interlocked.CompareExchange(ref sem1_ ,1,0)==1) ; 

        Console.WriteLine("sem açıldı");
  

    }
    static void release()
    {
        Interlocked.Exchange(ref sem1_ ,0);
    }

    static void w() //sıra ile kritik alana girecek
    {
        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        waitone(); //waitone ile bekledik
        Console.WriteLine("Kritik alan"); //waitone dan sonra 1 tanesi buraya girdi
        Thread.Sleep(1000);
        release(); //release ile bir sonraki sisteme dahil olacak.

    }




    static void Main(string[] args)
    {

        //ÖRNEK 1
        //_pool = new Semaphore(initialCount: 0, maximumCount: 3); //kritik alana aynı anda 3 kişi bulunabilir.  initialCount: 0 ; şu anda kritik alana hiç kimse geçemez müsade yok demek


        //for (int i = 1; i <= 5; i++)
        //{
        //    Thread t = new Thread(Worker);
        //    t.Start(i); //Start verildi
        //}

        //Thread.Sleep(500); //main thread uyuyor
        //Console.WriteLine("Main thread calls Release(3).");
        //_pool.Release(releaseCount: 3);  //3'ü serbest yazdık ve çıktık. Releaseyapılmadan kilit atılamaz

        //Console.WriteLine("Main thread exits.");

        //Console.WriteLine(  );







        //ÖRNEK2 AutoResetEvent kodu

        //burada senkronizayon nesnelerinde zamanla yarışmaya başladık. Kritik alan gibi değilde kritik zaman gibi düşünülebilir.
        //threadler birbirini bekleme moduna geçmiş oluyorlar. biri set biri waitone yazıyor yani biri aktifken diğerini bekliyor, diğeri aktiftrn diğeri bekleme durumuna geliyor.
        //WaitOne'ı Set ile aktif hale getiriyor.
        new Thread(Work).Start();  //start verdik  metota gitti

        _ready.WaitOne();                  // ready ile main threadi çalıştırdık. main thread olarak beklemedeyim müsade edersen aşağıdaki satıra geçicem 
        lock (_locker) _message = "ooo";
        _go.Set();                         // go ile worker thread çalıştırdık

        _ready.WaitOne();
        lock (_locker) _message = "ahhh";  // Give the worker another message
        _go.Set();
        _ready.WaitOne();                   //beklemedeyiz
        lock (_locker) _message = null;    //mesaj null oldu
        _go.Set();                         //go set oldu


        //ÇIKTI: ooo ahhh
        Console.WriteLine();






        //ÖRNEK3 ManuelResetEvent kodu
        for (int i = 1; i <= 3; i++)                     //Burada 3 tane thread çalıştırdık. Main thread uyuyor threadler çalıştı kendine geldi
        {
            Thread t1 = new Thread(threadFunction);
            t1.Name = "Thread " + i;
            t1.Start();                                 //Start'ı verdik
        }
        Thread.Sleep(1000);                             //Main thread uyuyor. Main thread uyuyor demek yukarıdaki threadler çalıştı kendine geldi demek. Metota gittik buradan

        Console.WriteLine("Threadler Gönderiliyor "); 

        mre.Set();                                     //Metot kısmından gelip mre 'yi Set ettik

        for (int i = 4; i <= 5; i++)                  //tekrar threadleri oluşturdu ve start etti
        {
            Thread t2 = new Thread(threadFunction);
            t2.Name = "Thread " + i;
            t2.Start();
            Console.WriteLine("Thread " + i + " Gönderildi");
        }

        Thread.Sleep(1000);                          //uyuttu

        Console.WriteLine("Kapı Kapandı " + mre.Reset());     //Resetledi.Reset kapının kapanması demek. Resetle kapattıktan sonra Set ile kapıları açmak lazım.

        Thread t3 = new Thread(threadFunction);
        t3.Name = "Son Thread";
        t3.Start();

        Thread.Sleep(500);
        Console.WriteLine("Kapı Tekrar Açılıyor...");
        Thread.Sleep(500);

        mre.Set();


    


        //ÖRNEK4
        //Interlocked:

        int a = 1;
        Console.WriteLine(Interlocked.Add(ref a, 100));  //a=a+100
        Console.WriteLine(a);      //Çıktı 101 101 yukarıda da cw içine yazdığımız için 2 tane 101 çıktısı verdi

        Console.WriteLine();

        a = 1;
        Interlocked.Increment(ref a); //Increment 1 arttır manasına gelen bir komut
        Console.WriteLine(a);   //Çıktı 2

        Console.WriteLine();

        a = 10;
        Interlocked.Decrement(ref a);  //a dan 1 çıkartıyor
        Console.WriteLine(a);          //Çıktı 9

        Console.WriteLine();

        a = 1; 
        Interlocked.Exchange(ref a ,123);  // Exchange ile a ya 123 değerini yükle dedik
        Console.WriteLine(a);              //Çıktı 123
                                           
        Console.WriteLine();

        a = 1; 
        Interlocked.CompareExchange(ref a ,123,1);  // CompareExchange ile a değişkeni 1 e eşitse 123 yükle. Yani a =1 ise a yı 123 yap
        Console.WriteLine(a);              //Çıktı 123 oldu

        Console.WriteLine();





        //ÖRNEK5
        //sem_ = 0;
        //bekle();
        //Console.WriteLine("1");

        //coz();
        //bekle();
        //Console.WriteLine("2");

        //coz();
        //bekle();
        //Console.WriteLine("3");

        Console.WriteLine();


        //ÖRNEK6


        for (int i = 0; i < 5; i++)
        {
            new Thread(w).Start();
        }


    }

}
