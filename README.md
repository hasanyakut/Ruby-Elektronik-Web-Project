# Ruby Elektronik Microservices

Bu proje, Ruby Elektronik için geliştirilmiş mikroservis mimarisinde bir e-ticaret sistemidir.

## Proje Yapısı

- **ProductService**: Ürün yönetimi
- **OrderService**: Sipariş yönetimi  
- **UserService**: Kullanıcı yönetimi
- **Frontend**: ASP.NET Core MVC web uygulaması

## Veritabanı Kurulumu

### PostgreSQL Kurulumu
1. PostgreSQL'i bilgisayarınıza kurun
2. Veritabanı oluşturun:
   ```sql
   CREATE DATABASE "RubyElektronik";
   ```

### Bağlantı Ayarları
Tüm servisler aşağıdaki PostgreSQL bağlantı stringini kullanır:
```
Host=localhost;Port=5432;Database=RubyElektronik;Username=postgres;Password=1234
```

## Kurulum Adımları

### 1. Paketleri Yükleyin
```bash
# Her servis için ayrı ayrı
cd ProductService
dotnet restore

cd ../OrderService  
dotnet restore

cd ../UserService
dotnet restore

cd ../frontend
dotnet restore
```

### 2. Veritabanı Migration'larını Çalıştırın
```bash
# ProductService
cd ProductService
dotnet ef database update

# OrderService
cd ../OrderService
dotnet ef database update

# UserService
cd ../UserService
dotnet ef database update
```

### 3. Servisleri Başlatın

#### Otomatik Başlatma (Windows)
```bash
start-services.bat
```

#### Manuel Başlatma
```bash
# Terminal 1 - ProductService
cd ProductService
dotnet run

# Terminal 2 - OrderService  
cd OrderService
dotnet run

# Terminal 3 - UserService
cd UserService
dotnet run

# Terminal 4 - Frontend
cd frontend
dotnet run
```

## API Endpoints

### ProductService (https://localhost:7001)
- `GET /products` - Tüm ürünleri listele
- `GET /products/{id}` - Ürün detayı
- `POST /products` - Yeni ürün ekle
- `PUT /products/{id}` - Ürün güncelle
- `DELETE /products/{id}` - Ürün sil

### OrderService (https://localhost:7002)
- `GET /orders` - Tüm siparişleri listele
- `GET /orders/{id}` - Sipariş detayı
- `POST /orders` - Yeni sipariş oluştur
- `PUT /orders/{id}` - Sipariş güncelle
- `DELETE /orders/{id}` - Sipariş sil

### UserService (https://localhost:7003)
- `GET /users` - Tüm kullanıcıları listele
- `GET /users/{id}` - Kullanıcı detayı
- `POST /users` - Yeni kullanıcı ekle
- `PUT /users/{id}` - Kullanıcı güncelle
- `DELETE /users/{id}` - Kullanıcı sil

## Swagger UI

Her servis için Swagger dokümantasyonu mevcuttur:
- ProductService: https://localhost:7001/swagger
- OrderService: https://localhost:7002/swagger
- UserService: https://localhost:7003/swagger

## Teknolojiler

- **Backend**: ASP.NET Core 8.0
- **Veritabanı**: PostgreSQL
- **ORM**: Entity Framework Core
- **Frontend**: ASP.NET Core MVC
- **API Documentation**: Swagger/OpenAPI

## Özellikler

- ✅ Mikroservis mimarisi
- ✅ PostgreSQL veritabanı entegrasyonu
- ✅ Entity Framework Core ile ORM
- ✅ RESTful API endpoints
- ✅ Swagger API dokümantasyonu
- ✅ CORS desteği
- ✅ Seed data ile örnek veriler
- ✅ Soft delete desteği
- ✅ Audit fields (CreatedAt, UpdatedAt)

## Geliştirme

### Yeni Migration Oluşturma
```bash
cd [ServiceName]
dotnet ef migrations add [MigrationName]
dotnet ef database update
```

### Veritabanını Sıfırlama
```bash
cd [ServiceName]
dotnet ef database drop
dotnet ef database update
```

## Lisans

Bu proje Ruby Elektronik için özel olarak geliştirilmiştir.

