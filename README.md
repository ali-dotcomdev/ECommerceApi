# 🚀 E-Commerce API (Clean Architecture & Dockerized)

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)

Bu proje, modern yazılım geliştirme prensipleri, **Onion (Clean) Architecture** ve **Domain Driven Design (DDD)** yaklaşımları kullanılarak geliştirilmiş, ölçeklenebilir bir **.NET 10 Web API** projesidir.

Proje, **Docker** üzerinde konteynerize edilmiş olup, veritabanı olarak **PostgreSQL** kullanmaktadır.

## 🛠️ Teknolojiler ve Mimari

Proje, bağımlılıkların dışarıdan içeriye doğru aktığı **Clean Architecture** prensibine göre tasarlanmıştır.

| Teknoloji | Açıklama |
|-----------|----------|
| **.NET 10 Core Web API** | En güncel LTS backend framework ve runtime. |
| **Onion Architecture** | Gevşek bağlı (Loose Coupled) ve test edilebilir mimari yapısı. |
| **Entity Framework Core 10** | Code-First yaklaşımı ile veritabanı yönetimi (ORM). |
| **PostgreSQL** | İlişkisel veritabanı yönetim sistemi. |
| **Docker & Compose** | Uygulama ve veritabanı orkestrasyonu. |
| **Generic Repository** | Veri erişim katmanında (DAL) kod tekrarını önleyen tasarım deseni. |
| **AutoMapper** | Entity ve DTO nesneleri arasında otomatik dönüşüm. |
| **Global Exception Handling** | Merkezi hata yönetimi middleware'i. |

## 📂 Proje Yapısı

```bash
ECommerceApi (Solution)
├── 📂 ECommerceApi.API           # Presentation Layer
│   ├── 📂 Controllers            # API Uç Noktaları
│   ├── 📂 Middlewares            # Global Exception Handling
│   └── 📄 appsettings.json       # Konfigürasyonlar
│
├── 📂 ECommerceApi.Application   # Application Layer (Business Logic)
│   ├── 📂 DTOs                   # Veri Transfer Nesneleri
│   ├── 📂 Interfaces             # Service ve Repository Soyutlamaları
│   ├── 📂 Mappings               # AutoMapper Profilleri
│   ├── 📂 Services               # İş Kuralları (Business Implementations)
│   ├── 📂 Validators             # FluentValidation Kuralları
│   └── 📂 Wrapper                # Standart Response Modelleri
│
├── 📂 ECommerceApi.Domain        # Domain Layer (Core)
│   ├── 📂 Common                 # BaseEntity gibi ortak sınıflar
│   └── 📂 Entities               # Veritabanı Tablo Karşılıkları
│
├── 📂 ECommerceApi.Infrastructure # Infrastructure Layer (Data Access)
│   ├── 📂 Data                   # DbContext Yapılandırması
│   ├── 📂 Migrations             # Veritabanı Versiyonları
│   └── 📂 Repositories           # Generic Repository Implementasyonu
│
└── 📂 Tests                      # Birim Testler