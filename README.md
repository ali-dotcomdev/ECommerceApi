# 🚀 E-Commerce API (Yüksek Performanslı & Güvenli Backend)

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![Security](https://img.shields.io/badge/Security-Token%20Rotation-red?style=for-the-badge&logo=security&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)

Bu proje, modern yazılım geliştirme prensipleri, **Onion (Clean) Architecture** ve **Domain Driven Design (DDD)** yaklaşımları kullanılarak geliştirilmiş, endüstriyel standartlarda ölçeklenebilir bir **.NET 10 Web API** projesidir.

Sıradan API projelerinden farklı olarak; **İleri Seviye Güvenlik (Token Rotation)**, **Performans Optimizasyonu (Memory Management)** ve **Production Simülasyonu (Docker)** konularına odaklanmıştır.

## 🛠️ Teknolojiler ve Mimari

Proje, bağımlılıkların dışarıdan içeriye doğru aktığı, test edilebilir ve gevşek bağlı (loose coupled) bir mimariye sahiptir.

| Teknoloji | Açıklama |
|-----------|----------|
| **.NET 10 Core Web API** | En güncel LTS backend framework ve runtime. |
| **Clean Architecture** | Sorumlulukların katmanlara ayrıldığı sürdürülebilir mimari. |
| **Token Rotation Pattern** | Refresh Token'ın her kullanımda yenilendiği (Revoke & Rotate) üst düzey güvenlik mekanizması. Token işlemlerde Heap Allocation'ı önleyen performans optimizasyonu. |
| **Entity Framework Core 10** | Code-First yaklaşımı ile veritabanı yönetimi (ORM). |
| **PostgreSQL** | Yüksek performanslı ilişkisel veritabanı. |
| **Docker & Compose** | Uygulama ve veritabanının izole ortamda (Container) çalıştırılması. |
| **Generic Repository** | Veri erişim katmanında (DAL) kod tekrarını önleyen tasarım deseni. |
| **FluentValidation** | Request modellerinin (DTO) tutarlılığını sağlayan validasyon kütüphanesi. |
| **Global Exception Handling** | Tüm hataların merkezi bir noktadan yönetildiği Middleware yapısı. |

## 📂 Proje Yapısı

```bash
ECommerceApi (Solution)
├── 📂 ECommerceApi.API              # Presentation Layer (Sunum Katmanı)
│   ├── 📂 Controllers               # API Uç Noktaları (Auth, Products, Categories)
│   ├── 📂 Middlewares               # Global Exception Handling
│   └── 📄 Program.cs                # DI Container & Pipeline Config
│
├── 📂 ECommerceApi.Application      # Application Layer (İş Mantığı)
│   ├── 📂 DTOs                      # Veri Transfer Nesneleri
│   ├── 📂 Interfaces                # Service ve Repository Soyutlamaları
│   ├── 📂 Services                  # İş Kuralları (AuthService, ProductService)
│   ├── 📂 Validators                # FluentValidation Kuralları
│   ├── 📂 Mappings                  # Extension Method ile Mapping
│   └── 📂 Wrapper                   # Standart Response Modelleri
│
├── 📂 ECommerceApi.Domain           # Domain Layer (Çekirdek)
│   ├── 📂 Common                    # BaseEntity gibi ortak sınıflar
│   └── 📂 Entities                  # Veritabanı Tablo Karşılıkları (User, RefreshToken)
│
└── 📂 ECommerceApi.Infrastructure   # Infrastructure Layer (Veri Erişimi)
    ├── 📂 Data                      # DbContext Yapılandırması
    ├── 📂 Migrations                # Veritabanı Versiyonları
    └── 📂 Repositories              # Generic & Özel Repository Implementasyonları
```
 Öne Çıkan Özellikler
1. Token Rotation & Revocation (Güvenlik)
Standart JWT yapılarındaki güvenlik açıklarını kapatmak için Refresh Token Rotation stratejisi uygulanmıştır:

Kısa Ömürlü Access Token: Saldırı penceresi minimize edilmiştir.

Refresh Token Rotation: Her yenileme isteğinde eski Refresh Token veritabanında iptal edilir (Revoked) ve kullanıcıya tamamen yeni bir token verilir.

Reuse Detection: İptal edilmiş bir token kötü niyetle kullanılmaya çalışılırsa, sistem bunu bir hırsızlık girişimi olarak algılar ve güvenlik gereği oturumu sonlandırır.

2. Global Exception Middleware (Sağlamlık)
Projede try-catch blokları ile kod kirletilmemiştir. Merkezi bir Middleware, fırlatılan hataları yakalar, güvenli bir şekilde loglar ve istemciye standart bir JSON formatında (404, 400, 401, 500) yanıt döner. Stack trace bilgisi asla dışarı açılmaz.

3. Yüksek Performanslı Bellek Yönetimi
Token üretimi gibi yoğun kriptografik işlemlerde Heap Allocation'dan kaçınmak için stackalloc ve Span<T> kullanılmıştır. Bu sayede Garbage Collector (GC) üzerindeki baskı azaltılmış ve throughput artırılmıştır.

 Kurulum ve Çalıştırma (Docker)
Bu proje tamamen konteynerize(Containerization) edilmiştir. Yerel makinenize PostgreSQL veya .NET SDK kurmanıza gerek kalmadan, sadece Docker ile projeyi ayağa kaldırabilirsiniz.

Docker Compose ile Başlatın:

Bash
docker-compose up --build
Bu komut API imajını(image) derler, PostgreSQL veritabanını hazırlar ve gerekli ağ bağlantılarını kurar.
