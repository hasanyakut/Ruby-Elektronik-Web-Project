using RubyElektronik.Models;

namespace RubyElektronik.Services
{
    public class ServiceRecordPdfService
    {
        public byte[] GenerateServiceRecordPdf(ServiceRecord serviceRecord)
        {
            try
            {
                var html = GenerateServiceRecordHtml(serviceRecord);
                // HTML'i byte array'e çevir
                return System.Text.Encoding.UTF8.GetBytes(html);
            }
            catch (Exception ex)
            {
                throw new Exception($"PDF olusturma hatasi: {ex.Message}", ex);
            }
        }

        public byte[] GenerateServiceRecordsPdf(List<ServiceRecord> serviceRecords)
        {
            try
            {
                var html = GenerateServiceRecordsHtml(serviceRecords);
                // HTML'i byte array'e çevir
                return System.Text.Encoding.UTF8.GetBytes(html);
            }
            catch (Exception ex)
            {
                throw new Exception($"PDF olusturma hatasi: {ex.Message}", ex);
            }
        }

        private string GenerateServiceRecordHtml(ServiceRecord serviceRecord)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Servis Kaydi</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; }}
        .header {{ text-align: center; font-size: 24px; font-weight: bold; margin-bottom: 30px; }}
        .section {{ margin-bottom: 20px; }}
        .section-title {{ font-size: 16px; font-weight: bold; margin-bottom: 10px; }}
        .field {{ margin-bottom: 5px; }}
        .footer {{ text-align: center; font-size: 10px; margin-top: 50px; }}
        @media print {{
            body {{ margin: 0; }}
            .no-print {{ display: none; }}
        }}
    </style>
</head>
<body>
    <div class='header'>RUBY ELEKTRONIK SERVIS KAYDI</div>
    
    <div class='section'>
        <div class='section-title'>MUSTERI BILGILERI</div>
        <div class='field'>Ad Soyad: {serviceRecord.Ad} {serviceRecord.Soyad}</div>
        <div class='field'>Kullanici Tipi: {(serviceRecord.UserType == ServiceUserType.Corporate ? "Kurumsal" : "Bireysel")}</div>
        {(string.IsNullOrEmpty(serviceRecord.FirmaAdi) ? "" : $"<div class='field'>Firma Adi: {serviceRecord.FirmaAdi}</div>")}
        <div class='field'>Telefon: {serviceRecord.TelefonNumarasi}</div>
    </div>
    
    <div class='section'>
        <div class='section-title'>URUN BILGILERI</div>
        <div class='field'>Urun Turu: {serviceRecord.UrunTuru}</div>
        <div class='field'>Ariza Aciklamasi: {serviceRecord.ArizaAciklamasi}</div>
    </div>
    
    <div class='section'>
        <div class='section-title'>TARIH BILGILERI</div>
        <div class='field'>Kayit Tarihi: {serviceRecord.CreatedAt:dd.MM.yyyy HH:mm}</div>
        {(serviceRecord.UpdatedAt.HasValue ? $"<div class='field'>Tamamlanma Tarihi: {serviceRecord.UpdatedAt.Value:dd.MM.yyyy HH:mm}</div>" : "")}
        <div class='field'>Durum: {(serviceRecord.IsActive ? "Aktif" : "Tamamlandi")}</div>
    </div>
    
    <div class='footer'>
        Bu belge Ruby Elektronik tarafindan {DateTime.Now:dd.MM.yyyy HH:mm} tarihinde olusturulmustur.
    </div>
    
    <div class='no-print' style='margin-top: 20px; text-align: center;'>
        <button onclick='window.print()' style='padding: 10px 20px; font-size: 16px; background-color: #007bff; color: white; border: none; border-radius: 5px; cursor: pointer;'>
            PDF Olarak Yazdir
        </button>
    </div>
</body>
</html>";
        }

        private string GenerateServiceRecordsHtml(List<ServiceRecord> serviceRecords)
        {
            var activeCount = serviceRecords.Count(s => s.IsActive);
            var completedCount = serviceRecords.Count(s => !s.IsActive);
            
            var recordsHtml = string.Join("", serviceRecords.OrderByDescending(r => r.CreatedAt).Select(record => $@"
                <tr>
                    <td>{record.Id}</td>
                    <td>{record.Ad} {record.Soyad}</td>
                    <td>{(record.UserType == ServiceUserType.Corporate ? "Kurumsal" : "Bireysel")}</td>
                    <td>{record.TelefonNumarasi}</td>
                    <td>{record.UrunTuru}</td>
                    <td>{(record.IsActive ? "Aktif" : "Tamamlandi")}</td>
                    <td>{record.CreatedAt:dd.MM.yyyy HH:mm}</td>
                </tr>"));

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Servis Kayitlari Raporu</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; }}
        .header {{ text-align: center; font-size: 24px; font-weight: bold; margin-bottom: 30px; }}
        .summary {{ margin-bottom: 20px; }}
        .summary-title {{ font-size: 16px; font-weight: bold; margin-bottom: 10px; }}
        .summary-item {{ margin-bottom: 5px; }}
        table {{ width: 100%; border-collapse: collapse; margin-top: 20px; }}
        th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
        th {{ background-color: #f2f2f2; font-weight: bold; }}
        .footer {{ text-align: center; font-size: 10px; margin-top: 50px; }}
        @media print {{
            body {{ margin: 0; }}
            .no-print {{ display: none; }}
        }}
    </style>
</head>
<body>
    <div class='header'>RUBY ELEKTRONIK SERVIS KAYITLARI RAPORU</div>
    
    <div style='text-align: right; margin-bottom: 20px;'>
        Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}
    </div>
    
    <div class='summary'>
        <div class='summary-title'>OZET BILGILER</div>
        <div class='summary-item'>Toplam Servis Kaydi: {serviceRecords.Count}</div>
        <div class='summary-item'>Aktif Kayitlar: {activeCount}</div>
        <div class='summary-item'>Tamamlanan Kayitlar: {completedCount}</div>
    </div>
    
    <table>
        <thead>
            <tr>
                <th>ID</th>
                <th>Ad Soyad</th>
                <th>Tip</th>
                <th>Telefon</th>
                <th>Urun</th>
                <th>Durum</th>
                <th>Tarih</th>
            </tr>
        </thead>
        <tbody>
            {recordsHtml}
        </tbody>
    </table>
    
    <div class='footer'>
        Bu rapor Ruby Elektronik tarafindan {DateTime.Now:dd.MM.yyyy HH:mm} tarihinde olusturulmustur.
    </div>
    
    <div class='no-print' style='margin-top: 20px; text-align: center;'>
        <button onclick='window.print()' style='padding: 10px 20px; font-size: 16px; background-color: #007bff; color: white; border: none; border-radius: 5px; cursor: pointer;'>
            PDF Olarak Yazdir
        </button>
    </div>
</body>
</html>";
        }
    }
}