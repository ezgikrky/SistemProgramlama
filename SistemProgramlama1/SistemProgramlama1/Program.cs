/*  İLK DERS

 Bu derste thread oluşturup, bu threadleri core lar üzerinde çalıştıracağız



CPU

Cpu üzerinde işlem gerçekleştiriyoruz. cpu main board üzerinde bulunuyordu ve main board üzerinde bulunan 2 tane donanım elemanı var biri ram diğeri ekran kartı

Cpu en çok çalıştığı birim RAM'dir.
cpu ram ile bilikte hareket edip burada çalışıyor. 
cpu nun en önemli özelliği fetch-decode-execute

fetch: ramden komutu oku
decode: okunan komutu yorumla
execute: yorumlanan komutu çalıştır

cpu tek başına tüm donanım üzerinde yetkili

cpu içerisinde register, alu ve control unit var. Normal şartlarda bir komut çalışması için gerekli olan elemanlar bunlardır.

CPU rame gidip ramden okuma yapıyor. okumayı gerçekleştirirken cpu' nun tüm elemanları bekler yani cpu bekleme durumunda. veri geldi decode(yorumlama) ediyoruz. yorumlama yaparken 
bekleyenler var yine, ramden veri alabiliriz alamıyorum alu da komutları çalıştırabiliriz çalıştıramıyoruz, sadece yorumlama yapabiliriz.
execute ederken aslında fetch ve decode yapabilirdik yapamıyoruz.
CPU 3 farklı iş yapıyor demiştik . F E D bunlar birbirine engel değil 
Fetch ile uğraşırken e ve d süreçlerini es geçiyoruz yani decode ve execute edemiyoruz. decode ' e geçtik bu sefer f ve e edeiyoruz. execute'e geçtik bu seferde f ve d olmuyor. üçü içinde aynı
Bunun için de F,D,E işlemini gerçekleştirirken sistem aynı anda 3 işlem yapmaya çalışsın yani ramden fetch ederken aynı anda decode ve execute yapsın. 
3 adet işi aynı anda gerçekleştirebilsin. Yani önceden cpu rame gittiğinde de decode ve execute bölümleri boşta kalıp çalışmıyordu
Şimdi fetch ederken bir önceki komutu decode ediyor 2 önceki komutu execute ediyor yani fetch aldığı komutları bir kuyruğa atıyor ve bu kuyruktan decode birimi alıp işliyor 
sonra da çalışacak konuma getiriyor ve çalışıyor.
Bu bizim performansımızı arttırıyor ve Superscaler bir yapı oldu
Bir önceki scaler yapıydı

Burada her zaman F,D,E aynı anda giderken komutların büyüklüğüne, çalışma sürelerine bakıldığında aynı anda 3 adet değilde de 2 adet daha performanslı olduğu görülmüştür. yani aynı anda
F,D,E yapmıyoruz aynı anda F,D,E 'tan 2 tanesini gerçekleştiriyoruz.
Böyle olunca Dual Core cpu ortaya çıkıyor ve biz bir cpu çalıştırırken aynı anda 2 iş yapabildik. Aynı anda 2 iş yapabiliyorsak cpu üzerinde farklı 2 program çalıştırabiliriz. burada 
cpu nun aynı anda 2 iş yapan mekanizması biraz daha özelleştiriliyor ve ortaya core yapısı çıkıyor. biz artık 1 cpu içerisinde 2 ya da 4 core tutabiliyoruz




CORE VE CPU FARKLARI

-cpu artık bir şey üretmiyor. cpu bir sınır oldu ve içerisinde corelar var. cpu eskiden bir şey üretip çalışabiliyordu. şimdi cpu sanal konumlara geldi, tanımlama yapıyoruz.
cpu core'lara dönüşüp iki tane mekanizma oluştu ve donanımsal olarak farklılıklar var
-core lar ram e aynı anda ulaşamaz, tane tane ulaşırlar. 
-her core un kontrol üniteleri farklı ve aynı zamanda en önemlisi registerlar farklı, L1 cache ler farklı. Yani Bir corun diğer coredan farklı registerları, cacheleri diğer alu ve kontrol 
ünitesi yapıya göre değişkenlik gösterir.

cpu nun içerisindeki corelar kendine özgü registerları, cacheleri, alu ve kontrol üniteleri var. Tasarıma göre değişebiliyor. 
net olan register ve cache yapısının bağımsızlığıdır.





REGİSTER

Register coreların içerisinde olan minik hızlı değişkenler. bu değişkenleri programda kullandığımız değişkenler olarak düşünebiliriz. assembly dilinin değişkenleridir ve sabittir.
yazdığımız tüm programlar compiler tarafından assembly diline dönüştürülür. En hatasız ve en hızlı dönüştürme işi compiler'ın işidir ve compiler'lar rekabet ararlar.

Yazılan tüm komutlar, döngüler, forlar, classlar, farklı veritabanı bağlantıları ... bütün bu komutlar compiler tarafından cpu nun kontrol ünitesinde saklı olan komutlarına dönüştürülür.

Komut seti, kontrol ünitesindedir ve her mikroişlemci üreticisinin kendine göre komut seti vardır.

Registerlardan bir tanesi en önemli registerdır ve accumulator denir. genelde A ile başlar. AX intel mimarisinde accumulatordur. Registerlar 8,16,32, 64 bit olarak 4 tanedir.

4 register var AX,BX,CX,DX  
16 bitlik registerlar ah ve al olarak AX 2 ye ayrılır.h-high(yüksek) , l-low(düşük) . Düşük seviyeli 8 biti AL, Yüksek sevileyli 8 biti AH

her bir registerın kullanım amacı vardır. En çok kullanılan register AX'tir. bx genellikle stacklarla çalışırken kullanılır, cx döngü registerları , dx data diye geçer portlara in ve out
komutu var mesela in al dendiğinde al registerına 8 bitlik bir veri gelir. bu veri gizli olarak dx registerını kullanır yani in komutunu kullanmadan önce mov dx,123 yazıp sonra in al deriz.
o zaman al reg.  123. portun 8 bitlik verisi transfer edilir


Segment registerlar: code segment ,data segment, stack segment, ve extra segment
CS: komutlar var
ds: tanımladığınız global değişkenler vardır. 
ss: stack yapısı vardır ve biz burada metot çalıştırırız. çalıştırdığımız metotların geçici verileri burada saklanır.
ES: Veri transferi için kullanılır. özellikle stringlerdeki transferlerde kullanılır.

BP, sp, si, di bunlarda yardımcı registerlar
si ve di  bazı komutların default ya da gizli değişkenleridir

32BİT EAX, 64bit ise RAX

Flag register da var. ALU da son gerçekleştirilen işlein sonucunu barındırır. Sonuç matematikselse sonuç 0 olabilir bunun içinde zero flag değişkeni vardır yani sıfır bayrağı.
işlemimiz alu da 0 sa zero flag set edilir yani ZF 1 olur. en sonki işlem negatif ise sign flag 1 olur (SF 1).  Taşma varsa carry flag değişir.

Carry flag: 2 basamaklı toplama yaptığımızda 12+22=34 , 99+10= 109 oldu => 109 iki basamaklı değilde üç basamaklıdır dolayısıyla 99+10 toplamı 2 basamaklı 
bakarsak 09 olur ve veri kaybı olur bu da ciddi bir problem demektir. Carry flag bu yüzden bizim için önemlidir.

Compare(CMP) kıyaslama yapmak demektir. cpu cmp komutunu çıkarma yaparak gerçekleştirir. CMP a,10 yani a dan 10 u çıkartıyor. a dan 10 u çıkartınca(a-10) sonuç sıfırsa
yani ZF 1 ise a 10 a eşittir demektir. a nın 10 dan büyük olması ise sign flagın (SF) 0 ve ZF 0 olması demektir. eğer 10 a >= ise SF ve ZF 'a bakmaya gerek yok demekti


Her corun kendine göre register ve cache i var





CACHE

Cache 2 ye ayrılır biri instruction diğeri de data diye 2 farklı cache vardır..

L1: Core'un kendi cache'i
L2: birden fazla core için geçerlidir.
L3: varsa cpu nun tüm core'larını içeren genel bir cache dir. 

10ns ramse, 1ns registera ulaşma süresi, 2ns de l1 cache'dir.

                             
herhangi bir mimariye örnek:

                       Main Memory
 
                          CPU

                           L3
               L2      L2      L2     L2
               L1      L1      L1     L1
              Core1   Core2   Core3  Core4


* 4 core var Core'un l1 cache'i , l2 cache üzerinde de l3 cache'i var. Ancak L2 cache hep böyle olacak değil cache'in performansı iyi olmakla birlikte maliyet gerektiren bir yapı olduğu için
  tüm yapılarda böyle kullanılmıyor. l2 yeri geldiğinde core 1 ve core 2 için l2 kullanılabiliyor ya da l2 hiç olmuyor ama L1 genelde her core için vardır ve bu 
  olmadığı sürece multithread yapı çok verimli çalışmaz.




Bir başka mimari:   ss alındı 07.03  23.56

* cpu clock geldiği an bütün elemanlar çalışır. Her clock pustta cpu bir işlem yapar. L1 cache'i instruction dan dataya geçiyor. ram yöneticisinden komutlaaru alırız l1 cache veriler
  gelir ve l1 cache gelen veri hangi kısma gidecek? Data kısmı ve ınstruction kısmı var. Instruction pointer ve Instruction reister birlikte çalışır. veri geldikten sonra
  alu ya gidilecekse aluya gidlir. alu ya gidilmeyecekse kontrol ünitesinde direkt işlenecek komutsa oraya gönderilir. ınterrupt oluşturulup alu ya gerek kalmayabilir.
  komutların çoğu alu da çalışır
  Accumulator dediği de AX register genel bir tanımlama yapılmış



! IP, Instruction register ve genel registerlar L1 cache il beraber kesinlikle core ' a özgüdür. Ancak mimariye göre alu'yu ve kontrol ünitesini çoğaltabiliriz.
  çoğalırlarsa performans ve maliyet artar.


aynı anda rame 4 core birlikte gidilmez. rame aynı anda bir core gider.





Core lar ne amaçla kullanılır?

1. Uzun işlem yükü gerektiren uygulamalarda zaman kazandırır ve problemin çözümünü kısaltır. Her problem corelar sayesinde hızlı çözülmez daha çok zaman kaybedebiliriz. 
   Hızlandırılacak olan problemler kategoriktir.
2. Core programlamayı aynı anda farklı hizmetler için kullanırız. yani bizim kulladığımız yazılımlar daha esnek ve daha fonksiyonel olur. 
   Aynı anda birden fazla işlem yapmak ancak corelarla olur.
3. En büyük problem senkronizasyon 







 */