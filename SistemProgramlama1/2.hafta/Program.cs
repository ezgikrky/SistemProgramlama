/*
DELEGATE
 
Metodları gösteren(point eden) bir yapıdır.
➔ Esneklik sağlar. (Kodlamada kolaylık sağlar.)
➔ Metodlara parametre gönderebilir.
➔ Birden fazla metod için çalıştırılabilir.
 
 
 */


class Program
{
    public delegate int Del(string message);
    public static void DelegateMethod(string message)
    {
        Console.WriteLine(message);
    }



    // delegate tanımlarken isim verip yeni bir tane daha üreticez buradaki gibi bunu kısaltabiliriz.
    public delegate int islemtip(int a, int b); //artık islemtip bir class formunda
    static int topla(int a, int b)
    {
        Console.WriteLine(a+b);
        return a + b;
    }

    static int fark(int a1, int a2)
    {
        Console.WriteLine(a1-a2);
        return a1 - a2;
    }

    static int carp(int a, int b)
    {
        Console.WriteLine(a * b);
        return a * b;
    }


    static int bolme(int a, int b)
    {
        Console.WriteLine(a / b);
        return a / b;
    }

    
    static void hepsi(int a, int b, islemtip islemler) // 'islemtip islemler ' şeklinde delegate ler metotlara parametre olarak verilebiliyorlar.
    {
        islemler(a,b);
    }


    static int d11() //parametre yok
    {
        return 5;  //geri dönüş tipi var
    }

    static string d12(int a)
    {
        return a.ToString(); // geriye stringe dönüştürerek döndürmüş olduk
    }

    static string d13(int a, int b)
    {
        return (a*b).ToString();
    }


    //Action

    static void m1(int a)
    {
        Console.WriteLine(a);
    }










    static void Main(string[] args)
    {
        //Del handler = DelegateMethod; //hata veriyor sebebini sor
        //handler("merhaba");

        DelegateMethod("merhaba");

        Console.WriteLine();

        islemtip islem = topla;
       // Console.WriteLine("Toplam Sonuc: {0}", islem(3,12));

        islem = fark;
        //Console.WriteLine("Fark sonucu: {0}",islem(3,12));

        islem = carp;
      //  Console.WriteLine($"Çarpma sonucu: {islem(3,12)}");

        islem = new islemtip(bolme); // eski sürümlerde bu şekilde tanımlamalar oluyordu ama yukarıdakiler daha pratik
      //  Console.WriteLine("bölme sonucu: {0}",islem(12,3));


        // integer da performans artarken floatta performans düşer


        islem = topla; 
        islem += fark;
        islem += carp;
        islem += bolme;

        // bir delegate yani bir
        // pla, fark, carp, bolme gibi


        islem(3, 12);

        //Console.WriteLine(islem(3, 12)); yukarıdaki yerine bunu yazsaydık sonuncu bölme old. için bir 0 daha verir
        

        //islem -= bolme;
        //Console.WriteLine(islem(3, 12));  denildiğinde de bu sefer yukarıda 1 tane bölme düştüğümüz içi çıktı da 1 tane 0 olur.
        Console.WriteLine("çalışma bitti");

        //metotun imzası parametre isimlerine bağlı değildir.
        //1 delegate e birden fazla metot yüklenebilirmiş.
        //aynı zamanda da bir metota parametre oluşturabiliyor

        Console.WriteLine("-----------------");

        hepsi(3, 12, islem);
        hepsi(3, 12, topla);

        Console.WriteLine("-----------------");



        /*Function
        kısaltma
        .NET önceden delagte olarak tanımlamış olduğu delegateler var o delegate leri kullanabiliriz. o delegate lere function ismini veririz
        function önceki programlama dillerinde prosedürlerle birlikte kullanılırdı. prosedürler geriye değer döndürmez.
        function'lar geriye bir return type ı olan metotlardı. şimdi function ve prosedürler metotlara indiirgendi her birine metot dedik
         Func<> hazır bir delegate

        */

        //geri dönüş tipi en sonda yazılıyor. soldan sağa doğru ilk ikisi parametreye bakıyor
        //tanımlanan imza : int, int, int, delegate' e verilen isim: islemler , carp: uygun metot
        Console.WriteLine(carp(3,12));

        Console.WriteLine("-----------------");



        //delegate'ler funcion 'a indirgenince sadece delagte'in instance ismini(d1) yazdık. burada class tanımlamadık. ama instance a uygun bir metot(d11) tanımlamamız lazım
        Func<int> d1= d11; //Func<int>'deki geriye int  değer döndürüyor demektir. Burada geriye parametresi olmayan bir metot tanımladık
        Console.WriteLine(d1());

        Func<int, string> d2 = d12; // parametre olarak int gönderip sonuçta da string alalım
        Console.WriteLine(d2(9));

        Func<int, int, string> d3 = d13; // Func<int, int, string > 2 parametre var geri dönüşte string 
        Console.WriteLine(d3(3,2));

        Console.WriteLine("-----------------");
      




        //Lambda Expression

        Func<int> e1 = () => 5;  // burada artık metotları isim vermeden anonim olarak tanımlayabiliyoruz.yani lambda expression kullanarak metotları başka bir metotun içine gömdük
                                 // e1 instance'ımız geri dönüş değeri int olan bir fonksiyona bakıyor. metot yazmadan parametre yok geriye de 5 göndersin dedik. Buna lambda expression denir

     
        // Lambda Expression 2 bölümden oluşur
        // 1- () içindeki parametre bölümü
        // 2- {} 'lerle (body) ilgili olan executable kodun olduğu bölüm

        Func<int, string> e2 = (a) => { return (a * a * a).ToString(); };   // Function int alsın string göndersin. a integer. a^3 ü  string halinde geriye döndürdük.
        Console.WriteLine(e2(2));  //çıktı 8


        //16 tane farklı parametre döndürebilir. 1 tane de geri dönüş döndüren Func<> döndürebiliriz.
        //yani 16 parametreli bir func yazabiliyoruz.


        int abc = 2;

        Func<int, int, int, int> e3 = (a, b, c) => a * b * c;
        //Func<int, int, int, int> e3 = (a, b, c) => { return a * b * c;}; yukarıdaki ile bu aynı direkt a*b*c yazsakta yeterli.
        Console.WriteLine(e3(2,3,4));  //çıktı 24


        Console.WriteLine("-----------");

        Func<int, int, int, int> e4 = (a, b, c) => abc * a * b * c;
        Console.WriteLine(e4(2, 3, 4));  // çıktı 48

        e4 = (a, b, c) => { abc = 9; return abc * a * b * c; };

        Console.WriteLine(abc); // 2 çünkü e4 ü daha atamadık.  bir asağıdaki kodda atıyoruz.
        Console.WriteLine(e4(2,3,4));  // çıktı 216. artık abc 9 oldu
        Console.WriteLine(abc);  // çıktı 9. çağırdığımızdan dolayı 9 oldu


        Console.WriteLine();






        Console.WriteLine("ACTION");


        /*Action : geri dönüş değeri void olan metot adresleridir.

        Action ile functionın tersi yapılıyor. func ile geriye değer döndürülüyor. action ile geriye değer döndürmez
        Action tanımladıysak o onun parametresidir.

        geriye değeri olmayan void olan metotlarımız var.

        Action'ı lambda expression ilede kullanabiliriz

        Action'ların da 16 parametresi var
        Actionlar parametre almayadabilir.
        


        */



        Action<int> act1 = m1;
        act1 = (a) => { Console.WriteLine(a * a + abc); };
        act1(2); // act1 i 2 ile çağırdık. çıktı 13 oldu a=2 abc=9;  2*2+9=13

        Action act2 = () => { int a = 5; int b = 7; }; //parametre almayan action tanımladık
        //Action<int, int, int, float> : 4 parametre alıyor demek oluyor. geriye değer döndürmediğini unutmamak lazım

    }






}







