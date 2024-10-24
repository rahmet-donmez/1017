# Account Management Project

## Kullanılan Teknolojiler ve Yapılar
- **.NET Core 6.0**
- **HTML, CSS, JavaScript**
- **Entity Framework Core**
- **MVC**
- **N Katmanlı Mimari**
- **Generic Tasarım Deseni**
- **PostgreSQL Veri Tabanı**

## Proje Hakkında
Bu proje, katmanlı mimari kullanılarak iş yüklerinin farklı katmanlara dağıtıldığı bir hesap yönetim sistemidir. 

Arayüz kodları Razor view dosyalarına ayrılarak kod okunurluğu artırılmıştır.

Veritabanı modelleri `BaseEntity` sınıfına bağlanarak kod tekrarından kaçınılmıştır. Silme işlemlerinde kayıtların kaybolmaması için **`IsDeleted`** değişkeni kullanılmış ve işlemler bunun üzerinden gerçekleştirilmiştir.

Birden fazla veri tabanı işleminin yürütüldüğü kod bloklarında **transaction-rollback** yapısı kullanılmıştır. 

Uygulama giriş ve yetkilendirme işlemi için cookie mantığı kullanılmıştır. Admin kullanıcıya özel menü seçenekleri bulunmaktadır. Link üzerinden bu sayfalara erişim sağlanmak istendiğinde, erişim bulunmadığına dair bilgi veren sayfaya yönlendirme yapılmaktadır.

Uygulamaya başlangıç verilerinin eklenmesi için seedData dosyası kullanılmıştı.
## Eklenebilecek Özellikler
- TC ve şifre bilgileri hashlenerek güvenli bir şekilde veritabanında saklanması.
- Controller'daki iş yüklerinin servislere dağıtılması.

## Projenin Çalıştırılması
1. **Visual Studio** editöründe `AccountExample.sln` dosyasını açın.
2. Projenin `\AccountExample\appsettings.json` dosyasında bulunan **`PostgreSqlConnection`** connection bilgilerini kendi veritabanınıza uygun şekilde güncelleyin.
3. **Paket Manager Console**'u açarak "default Project" seçeneğinden **`AccountManagement.Repository`**'yi işaretleyin. Aşağıdaki komutları çalıştırarak veritabanını oluşturma işlemini gerçekleştirin:
    ```powershell
    add-migration initialMig
    update-database
    ```

4. Projenin derlenmesi için terminali açarak şu komutları çalıştırın:
    ```bash
    cd AccountExample
    dotnet run
    ```

## Örnek Kullanıcı Bilgileri
**Admin Kullanıcı:**
- **Kullanıcı Adı:** ayse.kara@example.com
- **Şifre:** Password2 

**Erişimi Kısıtlı Normal Kullanıcı:**
- **Kullanıcı Adı:** rahmetdonmez@gmail.com
- **Şifre:** Password1


