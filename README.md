# 🚀 E-Commerce API (Yüksek Performanslı & Güvenli Backend)

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![Security](https://img.shields.io/badge/Security-Token%20Rotation-red?style=for-the-badge&logo=security&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)

Bu proje, **Clean Architecture (Onion)** prensiplerine uygun olarak geliştirilmiş, **ölçeklenebilir**, **güvenli** ve **production-ready** bir **.NET 10 Web API** uygulamasıdır.

Sıradan CRUD API’lerinden farklı olarak proje;  
**ileri seviye kimlik doğrulama**, **performans optimizasyonu**, **distributed caching** ve **containerization** konularına odaklanmaktadır.  

Amaç, gerçek hayattaki kurumsal backend sistemlerini simüle eden sürdürülebilir bir mimari ortaya koymaktır.

---

## 🛠️ Teknolojiler & Mimari Yaklaşım
Proje, bağımlılıkların iç katmanlara doğru aktığı, test edilebilir ve gevşek bağlı (loosely coupled) bir mimari üzerine kuruludur.

## 🛠️ Teknolojiler ve Mimari
Proje, bağımlılıkların dışarıdan içeriye doğru aktığı, test edilebilir ve gevşek bağlı (loose coupled) bir mimariye sahiptir.

## 🔍 API Test & Kullanım

Bu projede API endpoint’leri **Swagger UI yerine Postman** üzerinden test edilmiştir.

Bu tercih bilinçlidir:
- Gerçek production ortamlarında Swagger genellikle **kapalıdır** veya yalnızca internal ağda kullanılır
- JWT Authentication ve Token Rotation mekanizmalarının Postman üzerinden manuel test edilmesi,
  güvenlik akışlarının daha net gözlemlenmesini sağlamıştır

Tüm endpoint’ler, request/response örnekleri ve yetkilendirme akışları
Postman koleksiyonları kullanılarak doğrulanmıştır.

| Teknoloji / Kavram | Açıklama |
|-------------------|----------|
| **.NET 10 Web API** | Modern, yüksek performanslı backend framework |
| **Clean Architecture (Onion)** | Katmanlı, sürdürülebilir ve test edilebilir mimari |
| **Entity Framework Core** | Code-First yaklaşımı ile ORM |
| **PostgreSQL** | İlişkisel veritabanı |
| **Redis** | Distributed caching ile response süresi optimizasyonu |
| **JWT Authentication** | Access & Refresh Token tabanlı kimlik doğrulama |
| **Token Rotation Pattern** | Refresh Token’ların her kullanımda yenilendiği üst seviye güvenlik |
| **FluentValidation** | Request DTO doğrulama |
| **Serilog** | Structured logging |
| **Docker & Docker Compose** | Containerization & environment orchestration |

## 📂 Proje Yapısı

```bash
ECommerceApi (Solution)
├── Tests
│   └── ECommerceApi.Tests          # Test Layer    
├── ECommerceApi.API                # Presentation Layer
│   ├── Controllers                 # API Endpoints (Auth, Products, Categories)
│   ├── Middlewares                 # Global Exception Handling
│   └── Program.cs                  # DI & HTTP Pipeline
│
├── ECommerceApi.Application        # Application Layer
│   ├── DTOs                        # Data Transfer Objects
│   ├── Interfaces                  # Service & Repository Abstractions
│   ├── Services                    # Business Logic
│   ├── Validators                  # FluentValidation Rules
│   ├── Mappings                    # AutoMapper / Mapping Extensions
│   └── Wrapper                     # Standard API Responses
│
├── ECommerceApi.Domain             # Domain Layer
│   ├── Common                      # BaseEntity, Shared Logic
│   └── Entities                    # Domain Entities (User, RefreshToken, etc.)
│
└── ECommerceApi.Infrastructure     # Infrastructure Layer
│   ├── Data                        # DbContext Configuration
│   ├── Migrations                  # EF Core Migrations
│   ├── Repositories                # Generic & Custom Repositories
│   └── Services                    # RedisCacheService, Auth Helpers
```

⭐ Öne Çıkan Özellikler


🔐 1. Token Rotation & Revocation (Güvenlik)

Standart JWT uygulamalarındaki güvenlik risklerini azaltmak için Refresh Token Rotation stratejisi uygulanmıştır:
Short-lived Access Token ile saldırı penceresi minimize edilir
Her refresh işleminde:
Eski token revoked edilir
Kullanıcıya tamamen yeni bir token üretilir
Reuse Detection sayesinde:
İptal edilmiş bir token tekrar kullanılırsa sistem bunu güvenlik ihlali olarak algılar
Oturum otomatik olarak sonlandırılır


🧱 2. Global Exception Handling (Dayanıklılık)

Try-catch blokları ile controller ve servisler kirletilmemiştir
Merkezi bir Middleware:
Hataları yakalar
Loglar
İstemciye standart JSON response döner (400, 401, 404, 500)
Stack trace ve hassas bilgiler asla dışarı açılmaz


⚡ 3. Performans & Bellek Yönetimi

Sık erişilen veriler için Redis (Distributed Cache) kullanılarak response süreleri düşürülmüştür
Token üretimi gibi yoğun işlemlerde:
Heap allocation’dan kaçınmak için Span<T> / stackalloc yaklaşımları uygulanmıştır
Garbage Collector üzerindeki baskı azaltılmıştır


🐳 Kurulum & Çalıştırma (Docker)

Proje tamamen containerized yapıdadır.
Yerel ortamda .NET SDK veya PostgreSQL kurmaya gerek yoktur.

Docker Compose ile Başlatma
```bash
docker-compose up --build
```

Bu komut:
API image’ını build eder
PostgreSQL ve Redis servislerini ayağa kaldırır
Gerekli network yapılandırmalarını otomatik oluşturur
