# Ruby Elektronik E-Ticaret Sistemi

Bu proje, Ruby Elektronik için geliştirilmiş monolith mimarisinde bir e-ticaret sistemidir.

## Proje Yapısı

- **Frontend**: Ana ASP.NET Core MVC web uygulaması (Monolith)
  - **Ürün Yönetimi**: Ürün CRUD işlemleri
  - **Sipariş Yönetimi**: Sipariş CRUD işlemleri  
  - **Kullanıcı Yönetimi**: Kullanıcı CRUD işlemleri
  - **Servis Kayıtları**: Teknik servis kayıt yönetimi
  - **Admin Paneli**: Yönetim paneli

## Teknolojiler

- **Backend**: ASP.NET Core 8.0
- **Veritabanı**: PostgreSQL
- **ORM**: Entity Framework Core
- **Frontend**: ASP.NET Core MVC
- **API Documentation**: Swagger/OpenAPI

## Özellikler

- ✅ Monolith mimarisi (Windows hosting uyumlu)
- ✅ PostgreSQL veritabanı entegrasyonu
- ✅ Entity Framework Core ile ORM
- ✅ RESTful API endpoints
- ✅ Swagger API dokümantasyonu
- ✅ CORS desteği
- ✅ Seed data ile örnek veriler
- ✅ Soft delete desteği
- ✅ Audit fields (CreatedAt, UpdatedAt)
- ✅ Admin paneli
- ✅ PDF rapor oluşturma
- ✅ Session yönetimi

## Kurulum ve Çalıştırma

1. PostgreSQL veritabanını kurun ve çalıştırın
2. `appsettings.json` dosyasında veritabanı bağlantı bilgilerini güncelleyin
3. Projeyi derleyin: `dotnet build`
4. Projeyi çalıştırın: `dotnet run`

## API Endpoints

- `/api/products` - Ürün yönetimi
- `/api/users` - Kullanıcı yönetimi
- `/api/orders` - Sipariş yönetimi
- `/api/servicerecords` - Servis kayıtları yönetimi

## Admin Paneli

- URL: `/admin`
- Kullanıcı adı: `admin`
- Şifre: `12345`

## Windows Hosting Uyumluluğu

Bu proje Windows hosting servislerinde çalışacak şekilde monolith mimarisine dönüştürülmüştür. Tüm mikroservisler tek bir uygulamada birleştirilmiştir.

## Lisans

Bu proje Ruby Elektronik için özel olarak geliştirilmiştir.

