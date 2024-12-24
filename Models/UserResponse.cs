using System.Text.Json.Serialization;

namespace BizQPOS.Models
{
    public class ShopListResponse
    {
        public long? count { get; set; }
        public bool? currentPageHasMoreData { get; set; }
        public List<UserEntity>? shops { get; set; }
    }

    public abstract class BaseEntity
    {
        [JsonPropertyName("areaCode")]
        public string? AreaCode { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("language")]
        public Language? Language { get; set; }

        [JsonPropertyName("country")]
        public AreaModel? Country { get; set; }

        [JsonPropertyName("province")]
        public AreaModel? Province { get; set; }
    }

    public class UserEntity : BaseEntity
    {
        // Shop-specific properties
        [JsonPropertyName("pictures")]
        public List<Uri>? Pictures { get; set; }

        [JsonPropertyName("industries")]
        public List<string>? Industries { get; set; }

        [JsonPropertyName("publicStatus")]
        public PublicStatus? PublicStatus { get; set; }

        [JsonPropertyName("status")]
        public Status? Status { get; set; }

        [JsonPropertyName("outId")]
        public required string OutId { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("companyName")]
        public string? CompanyName { get; set; }

        [JsonPropertyName("zipcode")]
        public object? Zipcode { get; set; }

        [JsonPropertyName("street")]
        public string? Street { get; set; }

        [JsonPropertyName("type")]
        public TypeEnum? Type { get; set; }

        [JsonPropertyName("legalPerson")]
        public string? LegalPerson { get; set; }

        [JsonPropertyName("logo")]
        public Uri? Logo { get; set; }

        [JsonPropertyName("cif")]
        public string? Cif { get; set; }

        [JsonPropertyName("banner")]
        public List<Banner>? Banners { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("nameChangedTime")]
        public long? NameChangedTime { get; set; }

        [JsonPropertyName("subDomain")]
        public string? SubDomain { get; set; }

        [JsonPropertyName("onlyPayManually")]
        public bool? OnlyPayManually { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("allowSocial")]
        public bool? AllowSocial { get; set; }

        [JsonPropertyName("emailVerified")]
        public bool? EmailVerified { get; set; }

        [JsonPropertyName("shopScore")]
        public long? ShopScore { get; set; }

        [JsonPropertyName("transportScore")]
        public long? TransportScore { get; set; }

        [JsonPropertyName("industriesDetail")]
        public List<IndustriesDetail>? IndustriesDetail { get; set; }

        [JsonPropertyName("location")]
        public Location? Location { get; set; }

        [JsonPropertyName("clientEmail")]
        public string? ClientEmail { get; set; }

        [JsonPropertyName("website")]
        public string? Website { get; set; }

        [JsonPropertyName("owner")]
        public UserEntity? Owner { get; set; }

        // User-specific properties
        [JsonPropertyName("factoryId")]
        public string? FactoryId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("createdAt")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonPropertyName("countryId")]
        public string? CountryId { get; set; }

        [JsonPropertyName("headImg")]
        public Uri? HeadImg { get; set; }

        [JsonPropertyName("provinceId")]
        public string? ProvinceId { get; set; }

        [JsonPropertyName("passwordCreated")]
        public bool? PasswordCreated { get; set; }

        [JsonPropertyName("followedCatIds")]
        public List<string>? FollowedCatIds { get; set; }

        [JsonPropertyName("remark")]
        public string? Remark { get; set; }

        [JsonPropertyName("myshops")]
        public List<SimpleShop>? MyShops { get; set; }
    }

    public class Banner
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("picture")]
        public Uri? Picture { get; set; }
    }

    public class IndustriesDetail
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("catId")]
        public string? CatId { get; set; }

        [JsonPropertyName("esName")]
        public string? EsName { get; set; }

        [JsonPropertyName("zhName")]
        public string? ZhName { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }
    }

    public partial class AreaModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool? IsDeleted { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("areaCode")]
        public string? AreaCode { get; set; }

        [JsonPropertyName("shortName")]
        public string? ShortName { get; set; }

        [JsonPropertyName("parentId")]
        public string? ParentId { get; set; }
    }

    public partial class SimpleShop
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("logo")]
        public Uri? Logo { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Language { es, zh, en }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PublicStatus
    {
        [JsonPropertyName("noShowPrice")]
        NoShowPrice,

        [JsonPropertyName("private")]
        Private,

        [JsonPropertyName("public")]
        Public
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        [JsonPropertyName("normal")]
        Normal,

        [JsonPropertyName("reviewing")]
        Reviewing,

        [JsonPropertyName("ok")]
        OK
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TypeEnum
    {
        [JsonPropertyName("manufacturer")]
        Manufacturer,

        [JsonPropertyName("retailer")]
        Retailer,

        [JsonPropertyName("wholesaler")]
        Wholesaler
    }
}
