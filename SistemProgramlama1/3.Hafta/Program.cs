//THREAD 
/*
 * Threadleri 2 amaç için kullanıyorduk birisi performans diğeri esneklik
 * Performanstan kasıt bir problemi daha kısa sürede çözmek. Threadler uygun kullanıldığında performansa etki eder. 
 her problem threadlerle çözülmez. özellikle birbirini bekleyen problemlerde yani aynı bölgede birbirini bekleyerek çalışma yapacaklarsa threadler performans düşer
Her thread kendi ile ilgili olan veriler üzerinde çalışmalı. Tüm threadler ortak veriler üzerinde çalışırsa o zaman performansı sağlayamayız, 
hızlandırma şansımız zayıf olur
 
*performans için max performans en fazla core sayısı kadar thread ile olur
*8 core varsa 8 core ile max performans elde ederiz. 8 core'u geçince performans düşer. Sebebi de tek cpu da aynı anda birden fazla işlem yaptığımızda round robin kullanırdık
*round robin; zaman paylaşımlı bir algoritma; yani quantum süresince her process çalışır
*bir processin bir processe geçmesi için interrupta ihtiyaç  vardı. ınterrupt geldiğinde aktif process durduruluyor ve process/ thread table a yedeklenir ve
sıradaki process/ thread aktif hale getirilir
Bu sürecin adı context switch maliyeti de quantum=20ms ; context-switch =5ms; 
Dolayısıyla 8 core varken 9 core çalıştırırsak; core lardan biri aynı anda 2 thread çalıştırmak zorunda bu da mümkün olmadığı için round robin yapar
yani birini çalıştırıp birini bekletir
ve 20 ms çalışıp 5 ms diğer threadi bekletecek %80 verim oluyor ve performans düşüyor. Ondan dolayı da thread bazlı bir yerde core sayısını geçmemeliyiz.

2.kural: 1 core ile 1 program 20 ms'de çalışıyor
2 core !=10ms ; 4 core la çalıştırıldığında da 5' e eşit olmaz çünk geri planda işletim sisteminde, donanımında yoğun bir şekilde çalıştırılması gerekiyor.
eşit şekilde çalışması mümkün değil. Core'ların performans artışları q/n değildir.
core lar ortak donanımda çalışır ve bundan olayı da yeri gelince birbirlerini beklerler.
yani 1 core süresi> 2 core süresinden
1C >2C > 3C ..... <=core sayısına kadar gider bu değerler küçük eşittir core sayısı olurlar
yukarıdaki teorik değer; sebebi de sistemde başka threadlerde mevcut sistem performansı düşer

Thread safe ve thread unsafe yani threadler tarafından çalıştırıldığında güvenli ve güvensiz ya da
Thread algoritması -> 2 veya daha fazla thread kullanan yapı
Bir thread algoritması için; thread safe de algoritmada threadler son derece güvenli çalışacak ve her çalıştığında beklenilen çıktı verilecek.
Her zamna aynı input ve aynı output ddemek
Thread unsafe de her zaman beklenilen aynı input için aynı output olmayabilir. unsafe kodlar semantic hatalardır ve hatalar hemen yakalanmayabilir. 
syntax hatası değildir. unsafe modda çalışan yazıımın ne zaman zarar vereceği analşılmaz. 
Kritik sistem kullanıyorsanız; zaman ve mal kaybına sebebiyet veren sistemlerdir.

thread soyut bir kavramdır ve core üzerinde çalışan bir metottur.
 */



using System.Diagnostics;
using System.Diagnostics.Metrics;

internal class Program
{


    static void th1() //geriye değer döndürmeyecek void diyoruz çünkü
    {
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine("Merhaba bilgisayar mühendisliği öğrencileri");
        }
    }






    //3.ÖRNEK 

    static int abc = 1;
    //static int counter = 0;
    static void th2()
    {

        for (int i = 0; i < 10000000; i++)
        {
            abc++;
        }
        //counter = 1;
    }



    //4.ÖRNEK 
    //Thread unsafe oldu sebebi de th3 abc1 değişkenini kullanıyor bu aşağıdaki thread bir core oldu. öylesine bir abc1 değişkenine giriyorlar ki son sürat
    static int abc1 = 0;
    static object obj12 = new object();

    static void th3()
    {

        for (int i = 0; i < 100000000; i++)

            lock (obj12)
                abc1++;



        /*
          f-d-e derken komut Okundu yorumlandı ve hafızadan abc yi oku sonra bunu alu ya getir 1 arttır yerine yaz
         abc yi örneğin 100 olarak okuduk aluya götürdük Bu arada öbür thread 100 olarak okudu ve 101 olarak yazdık
        yani abc okudum 100, 101 yazacağım; 101 yazdım=>  100 101 üst üste veri yazdık buna da kritik alan diyoruz.
        Kritik alan bir ya da birden fazla thread/processin ortak sistem kaynaklarına aynı anda erişim elde etmesi 
        Kritik alan denilen olgu birden fazla threadin aynı bölgeye hücum etmesi, bu problemin adı da Race Condition yarış durumu. yarış durumu senkronizasyon nesneleri ile çözülüyordu 
        Senkronizasyon nesneleri trafik ışıkları gibi düşünülebilir. Bizim performansı arttırmaz ama trafik safe bir modda olmasını sağlar. Senkronizasyon varsa biraz vakit var

       
         
         
         */
    }




    //6.ÖRNEK 

    static int[] x12 = new int[100];
    static void th5()
    {

        for (int i = 0; i < 100000000; i++) x12[0]++;

    }



    //7.ÖRNEK 


    static int[] x13 = new int[1000];
    static void th6()
    {

        for (int i = 0; i < 100000000; i++) x13[0]++;

    }

    static void th7()
    {

        for (int i = 0; i < 100000000; i++) x13[100]++;

    }

    static void th8()
    {

        for (int i = 0; i < 100000000; i++) x13[200]++;

    }

    static void th9()
    {

        for (int i = 0; i < 100000000; i++) x13[300]++;

    }






    //2.ÖRNEK 
    static void WriteY()
    {
        for (int i = 0; i < 1000; i++)
        {
            Console.Write("y");
        }
    }


    public static void Main(string[] args)
    {


        //her thread 1 defa creat edilir, kullanılır 2.defa kullanılmaz. th'yi new'lemek lazım ve ona yenir bir metot vermek lazım
        // tüm programlar en az  1 adet thread sahibidir. çünkü programın kendisi de thread oluyor. core da çalışıyor, core'a main metot veriyorsun
        // main metodun main threadi çalışırken 2.threadi oluşturuyor. dolayısıyla th.start dedikten sonra
        //aynı anda eşzamanlı(concurrent) çalışacak
        //main thread; özellikli demek değil

        // Her thread process içerisinde oluşturulur. Yani processler oluşturulur sonra threadler çalışır. 
        //Bütün işletim sistemi artık multi threading yapıda yani artık process table'dan ziyadde thread table bizim için önemli 
        //.ve round robin mekanizması threadler üzerinden yürür.
        //Bir program yazıp hiç thread kullanmasak bile en az 1 main thread vardır. Çünkü process denilen olgu 
        //çalışma mekanizmasını threade indirgemiştir.
        // f-d-e sadece bir process sınırı içerisinde sadece 1 f-d-e yaparken artık threadle birden fazla f-d-e yapabiliyor ama core sayısını geçmemeli
        //Mevcut sistemlerde thradler core da çalışan threadler
        //main threadlerde çalışan threadler main threadler diğerleri user thread
        // Bir process sınırı vardır; ne kadar ram kapladığı, dosyalar,ne zaman başladı, kullanıcı özellikleri ne gibi çok sayıda bilgi var.
        //threadler kendine özgü registerı ve stackı olan yapıdır.
        //threadlerin tüm kullandığı her şey process'e aittir.
        //thread yapısı core da çalışan bir metot




        Console.WriteLine("Thread başlayacak"); //1.thread. programımız hali hazırda bir thread oluşturuyor


        //bu 2.thread oldu
        Thread th = new Thread(th1); // thread denilen olgu core üzerinde metot çalıştırma özelliğidir. core metotu çalıştırırken o yapıya thread deriz
        //thread th1 metoduna bağlandı
        th.Start(); //çalıştırdık

        Thread.Sleep(100); //start ettikten sonra 100 ms beklettik 
        Console.WriteLine("Main thread çalışmayı bitirdi");
        Console.WriteLine();






        Console.WriteLine("3.ÖRNEK");


        //safe bir program şu an 
        //main thread işini bitirdiğinde diğer threadler kapanmaz

        Thread t = new Thread(th2);
        t.Start();

        //geçen süre bir process create etmekten daha kısadır. 1/50 gibidir ortalama

        int abc2 = 0;
        for (int i = 0; i < 10000000; i++) abc2++;


        //Thread.Sleep(100);  //thread beklenir. sonra da bu. bu safe mod değil 


        Console.WriteLine("Thread sonuçları");
        Console.WriteLine(abc2);


        //while (counter == 0) ;  //main thread bekleme moduna aldı kendisini.thread beklenir. safe mod. thread sleep ten daha iyi bir versiyon. en kötü thread bekleme metodu

        t.Join(); //thread beklenir  safe mod. yukarıdaki while ile aynı şey gerçekleşti. threadin işini bitirmesini bekler. her thread birbirini bekleyebilir.

        Console.WriteLine(abc);



        Console.WriteLine();
        Console.WriteLine();






        Console.WriteLine("4.ÖRNEK");

        //Thread unsafe oldu sebebi de th3 abc1 değişkenini kullanıyor . lock(obj12) yi ekleyince safe hale geldi çünkü kritik alana sokmuş olduk. senkronizasyon nesnesi kullandık
        //ama threadler birbirlerini beklediği için performans düştü. KRİTİK ALANA SIRA İLE GİRİLİYORSA, KRİTİK ALANDA İŞLEMLER TEK TEK GERÇEKLEŞTİRİYORSA O ZAMAN BİZİM THREADLER PERFORMANS SAĞLAMAZ.


        Thread t4 = new Thread(th3);
        t4.Start();

        for (int i = 0; i < 100000000; i++)
        
            lock (obj12)
            abc1++;
        

        Console.WriteLine("Thread sonuçları");

        t4.Join();

        Console.WriteLine(abc1);

        //kritik alanda işlem yapılan problemler (kesinlikle) performans arttırımı olmaz


        Console.WriteLine();






        Console.WriteLine("5.ÖRNEK");

        t4 = new Thread(th3);
        t4.Start();

        Stopwatch sw = new Stopwatch();
        sw.Start();

        for (int i = 0; i < 10000000; i++) lock (obj12) abc1++;
        Console.WriteLine("Thread sonuçları");
        t4.Join();
        sw.Stop();


        Console.WriteLine(sw.ElapsedMilliseconds); //çıktı 2601 yaklaşık 2.6 saniyede çözdü

        //kritik alanda işlem yapılan problemler (kesinlikle) performans arttırımı olmaz
        Console.WriteLine();









        Console.WriteLine("6.ÖRNEK");
        //lock ile çalıştırırsak süre 2555 yani yaklaşık 2.5 saniye ama burada loc a gerek yok

        Thread t5 = new Thread(th5);
        t5.Start();

        Stopwatch sw2 = new Stopwatch();
        sw2.Start();

        for (int i = 0; i < 10000000; i++) x12[99]++;
        Console.WriteLine("Thread sonuçları");
        t5.Join();
        sw2.Stop();


        Console.WriteLine(sw2.ElapsedMilliseconds);  //çıktı 284. senkronizasyon nesnesi bizim zamanımızı çalmış onu anlamış olduk


        // x12[99]++; değilde x12[1] ++; deseydik çıktı 302 olurdu 

        Console.WriteLine();





        Console.WriteLine("7.ÖRNEK");

        Thread t6 = new Thread(th6);
        t6.Start();

        Thread t7 = new Thread(th7);
        t7.Start();
        Thread t8 = new Thread(th8);
        t8.Start();
        Thread t9 = new Thread(th9);
        t9.Start();

        sw2 = new Stopwatch();
        sw2.Start();

        for (int i = 0; i < 10000000; i++) x13[400]++;
        Console.WriteLine("Thread sonuçları");
        t6.Join();
        t7.Join();
        t8.Join();
        t9.Join();
        sw2.Stop();


        Console.WriteLine(sw2.ElapsedMilliseconds);  //çıktı 1138


        //4 threadi bbir dizinin elemanlarını arttırma görevi verdik 1138 sn sürdü

        Console.WriteLine();

        /*eğer  x13[4]++; static voidlerde de x13[1]++; x13[2]++ ; x13[3]++; yaparsak yani 100 e bölünmüş halini yazarsak o zaman çıktı 3000 binlerde çıkar yani performans düşer
        Sebebi de; false sharing yani 4 kişi var 4 ünün üzerinde örnekleme yapılırsa eğer: her biri bir thread ve dördü de hafızadan veri alıp yazarlar yani rame 
        gidiyorlar ama her birinin ayrı ayrı L1 cache i var. L1 Cache denilen olgu cache line'larla gider 32byte, 64byte, 128byte, 256byte gibi parçalara ayrılmış 
        buna cache line denir. Cache line demek bir çırpıda ram den cache'e okunan veri demektir. L1 cache'imiz cache line kadar doldurur ilk başta performanstan dolayı




        */


        Console.WriteLine("2.ÖRNEK");
        //bu thread unsafe bir programdır.

        Thread t1 = new Thread(WriteY);
        t1.Start();
        for (int i = 0; i < 1000; i++)
        {
            Console.Write("x");
        }



    }





}

