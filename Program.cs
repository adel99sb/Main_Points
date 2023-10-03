// See https://aka.ms/new-console-template for more information
using System.Diagnostics.Metrics;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
//mapping && Dictinary && Pathes && Crypto && Get Sme Vaues Frm API Site ike Cntries && E-MaiSend && Pagination

Console.WriteLine("Hello, World!");
Console.WriteLine(NtifecatinTtpe.recive);
Console.WriteLine(NtifecatinTtpe.recive.TEngish());
string? EXT = Console.ReadLine();
Console.WriteLine(Constants.Map(EXT.ToLower()));
string CreateingPath = Paths.FinaPathAfterCnvert(Guid.NewGuid());
Console.WriteLine(CreateingPath);
string? VaueTCnvertFrSingiter = Console.ReadLine();
Console.WriteLine(Utilities.GenerateSignature(VaueTCnvertFrSingiter));
var names = await Countries.GetAll();
if (names is null || !names.Any())
    return;

var countries = names.Select(name => new MyCountry()
{
    Flag = name.flag,
    CallCode = name.callingCodes[0],
    CountryAr = name.translations.fa,
    CountryEn = name.name
}).Where(c => !string.IsNullOrEmpty(c.CountryAr) && !string.IsNullOrEmpty(c.CountryEn))
    .ToList();
foreach (var c in countries)
{
    Console.WriteLine(c.CallCode);
}
EmailManager EmailManager = new();
EmailManager.SendEmail("User@gmail.com");
var Prducts = new Prduct[]
        {
            new Prduct() { Name = "P1"},            
            new Prduct() { Name = "P2"},            
            new Prduct() { Name = "P3"},            
            new Prduct() { Name = "P4"},            
            new Prduct() { Name = "P5"},            
            new Prduct() { Name = "P6"},            
            new Prduct() { Name = "P7"},            
            new Prduct() { Name = "P8"},            
            new Prduct() { Name = "P9"},            
            new Prduct() { Name = "P10"},            
            new Prduct() { Name = "P11"},            
            new Prduct() { Name = "P12"},            
            new Prduct() { Name = "P13"},            
            new Prduct() { Name = "P14"},            
            new Prduct() { Name = "P15"},
            new Prduct() { Name = "P16"},
            new Prduct() { Name = "P17"},
            new Prduct() { Name = "P18"},
            new Prduct() { Name = "P19"},
            new Prduct() { Name = "P20"},
            new Prduct() { Name = "P21"},
            new Prduct() { Name = "P22"},
            new Prduct() { Name = "P23"},
            new Prduct() { Name = "P24"},
            new Prduct() { Name = "P25"},
            new Prduct() { Name = "P26"},
            new Prduct() { Name = "P27"},
            new Prduct() { Name = "P28"},
            new Prduct() { Name = "P29"},
            new Prduct() { Name = "P30"},
        };

var res = Prducts.Paginate(3,5);
foreach (var item in res)
{
    Console.WriteLine($"Name = {item.Name}");
}
public static class PaginationExtension
{
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int page = 1, int size = 10) where T : class
    {
        if (page <= 0)
        {
            page = 1;
        }

        if (size <= 0)
        {
            size = 10;
        }

        var total = source.Count();

        var pages = (int)Math.Ceiling((decimal)total / size);

        var result = source.Skip((page - 1) * size).Take(size);

        return result;
    }

}
public class Prduct
{
    public string Name { get; set; }
}
public class EmailManager
{
    public bool SendEmail(string recipient)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("almuhami.app.dev@gmail.com", "salam4rmu"),
            EnableSsl = true,
        };

        smtpClient.Send("name@gmail.com", recipient, "mail testing", "Hello From Al Muhami");

        return true;
    }
}
public class MyCountry
{
    public string Flag { get; set; }
    public string CallCode { get; set; }
    public string CountryEn { get; set; }
    public string CountryAr { get; set; }
}
public class Countries
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task<List<Rec_Country>> GetAll()
    {
        HttpResponseMessage response = await client.GetAsync("https://restcountries.com/v2/all");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Rec_Country>>(responseBody);
    }
}

#region Json to classes
public struct Rec_Country
{
    public string name { get; set; }
    public string[] callingCodes { get; set; }
    public string nativeName { get; set; }
    public Translations translations { get; set; }
    public string flag { get; set; }
}

public struct Translations
{
    public string fa { get; set; }
}
#endregion
public enum NtifecatinTtpe
{
    send,
    recive,
    seen
}
public enum CourtServicesDetailsDocumentTypes
{
    None,
    Image,
    Document
}
public enum AreaType
{
    Admin,
    Client
}
public static class Paths
{
    private const string SmePath = $"/{nameof(AreaType.Client)}/Lawyer/Card/{{Id}}";
    public static string FinaPathAfterCnvert(Guid id) =>
        new StringBuilder("SmePath").Append(SmePath).Replace("{Id}", id.ToString()).ToString();
}
public static class TransateNtifecatinMapper
{
    public static string TEngish(this NtifecatinTtpe ntifecatin)
    {
        return ntifecatin switch
        {
            NtifecatinTtpe.send => "Message Had been Sent",
            NtifecatinTtpe.seen => "Message Had been Saw",
            NtifecatinTtpe.recive => "Message Had been Recived",
            _ => "natha"
        };
    }
}
public static class Constants
{
    //using t vaidat
    public static string[] AllowedDocumentExtensions = new[] { ".docx", ".pdf", ".jpg", ".jpeg" };

    private static readonly Dictionary<string, CourtServicesDetailsDocumentTypes> DocumentTypeMap = new()
    {
        {".docx", CourtServicesDetailsDocumentTypes.Document},
        {".pdf", CourtServicesDetailsDocumentTypes.Document},
        {".jpg", CourtServicesDetailsDocumentTypes.Image},
        {".jpeg", CourtServicesDetailsDocumentTypes.Image}
    };
    public static CourtServicesDetailsDocumentTypes Map(string extension)
    {
        return DocumentTypeMap[extension];
    }
}
public class Utilities
{
    public static string GenerateSignature(string value)
    {
        var Sb = new StringBuilder();

        using (var hash = SHA256.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(value));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }
}