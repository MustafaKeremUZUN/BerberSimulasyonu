# Berber Simülasyonu

## Proje Açıklaması
Bu proje, ASP.NET Core MVC kullanılarak geliştirilmiş bir berber randevu ve hizmet simülasyon sistemidir. Kullanıcılar saç ve sakal modellerini seçebilir, fiyatlandırma otomatik olarak hesaplanır ve müşteri geçmişine göre indirim uygulanır.

## Özellikler
- Müşteri kayıt sistemi
- Saç ve sakal modelleri seçimi
- Otomatik fiyat hesaplama
- Ziyaret sayısına göre indirim sistemi:
  - 10+ ziyaret → %10 indirim
  - 30+ ziyaret → %20 indirim
- Randevu kaydı oluşturma

## Kullanılan Teknolojiler
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- C#

## İndirim Mantığı
- 0-10 ziyaret: indirim yok
- 11-30 ziyaret: %10 indirim
- 31+ ziyaret: %20 indirim

## Proje Amacı
Kullanıcıların berber hizmetlerini seçebildiği ve fiyatlandırmanın dinamik olarak hesaplandığı bir simülasyon sistemi geliştirmek.

## Kurulum
1. Projeyi klonla
2. NuGet paketlerini yükle
3. Migration çalıştır:
   - Add-Migration InitialCreate
   - Update-Database
4. Projeyi çalıştır