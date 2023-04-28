using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Collections.Concurrent;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#pragma warning disable IDE1006
namespace GraphQLCodeGen {
  public class Types {
    
    /// <summary>
    /// A cache of generated JToken::ToObject[TargetType] for each __typeName
    /// </summary>
    private static ConcurrentDictionary<string, Func<JToken, object>> ToObjectForTypenameCache = new ConcurrentDictionary<string, Func<JToken, object>>();
      
    
    #region AdminAssetModificationReceipt
    /// <summary>
    /// This model describes the asset modification operation that took place.
    /// </summary>
    public class AdminAssetModificationReceipt : TransactionSource {
      #region members
      /// <summary>
      /// The kind used to discriminate the union type
      /// </summary>
      
      TransactionSourceKind TransactionSource.Kind { get { return TransactionSourceKind.AdminAssetModificationReceipt; } }
    
      [JsonProperty("date")]
      public string date { get; set; }
    
      [JsonProperty("operation")]
      public string operation { get; set; }
      #endregion
    }
    #endregion
    
    #region ApplyApplePurchaseInput
    /// <summary>
    /// Data required to apply Apple's App Store purchase.
    /// </summary>
    public class ApplyApplePurchaseInput {
      #region members
      /// <summary>
      /// Base64 encoded encrypted receipt. It's the data you receive
      /// directly from StoreKit when retrieving receipts.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string purchaseData { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region ApplyGooglePurchaseInput
    /// <summary>
    /// Data required to apply Google Play's purchase.
    /// </summary>
    public class ApplyGooglePurchaseInput {
      #region members
      /// <summary>
      /// Package name as defined in Google Store.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string packageName { get; set; }
    
      /// <summary>
      /// Product ID of the product that is being applied
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string productId { get; set; }
    
      /// <summary>
      /// Type of purchase being made.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public GooglePlayPurchaseType purchaseType { get; set; }
    
      /// <summary>
      /// Purchase token vended to the client by Play store.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string token { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region ApplyPurchaseInput
    /// <summary>
    /// Input object required for applying purchase from external payment processor
    /// to Subroutine system.
    /// </summary>
    public class ApplyPurchaseInput {
      #region members
      /// <summary>
      /// Input object required for applying purchase from external payment processor
      /// to Subroutine system.
      /// </summary>
      public ApplyApplePurchaseInput apple { get; set; }
    
      /// <summary>
      /// Input object required for applying purchase from external payment processor
      /// to Subroutine system.
      /// </summary>
      public ApplyGooglePurchaseInput google { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region AssetDefinition
    public class AssetDefinition {
      #region members
      /// <summary>
      /// If the object is vended, the asset is composable and instances of
      /// it will be composable.
      /// </summary>
      [JsonProperty("asComposable")]
      public ComposableProperties asComposable { get; set; }
    
      /// <summary>
      /// Returns null if this asset type cannot be hold as fungible holding.
      /// Otherwise vends an object that describes properties only related
      /// to fungible assets.
      /// </summary>
      [JsonProperty("asFungible")]
      public FungibleProperties asFungible { get; set; }
    
      /// <summary>
      /// Returns null if this asset type cannot be hold as unique holding.
      /// Otherwise vends an object that describes properties only related
      /// to unique assets.
      /// </summary>
      [JsonProperty("asUnique")]
      public UniqueProperties asUnique { get; set; }
    
      /// <summary>
      /// Vends current holding of this asset for the player querying.
      /// </summary>
      [JsonProperty("assetHolding")]
      public AssetHolding assetHolding { get; set; }
    
      /// <summary>
      /// Number of instances of this UniqueAssetDefinition that the player can own.
      /// </summary>
      [JsonProperty("cap")]
      public int? cap { get; set; }
    
      /// <summary>
      /// Additional Asset's description.
      /// </summary>
      [JsonProperty("description")]
      public string description { get; set; }
    
      /// <summary>
      /// Additional ID, assigned by an Organization, can be used to help managing Assets
      /// on the Game side.
      /// </summary>
      [JsonProperty("developerAssetId")]
      public string developerAssetId { get; set; }
    
      /// <summary>
      /// Game this UniqueAssetDefinition belongs to.
      /// </summary>
      [JsonProperty("game")]
      public Game game { get; set; }
    
      [JsonProperty("id")]
      public string id { get; set; }
    
      /// <summary>
      /// Defines whether the holdings can be consumed.
      /// </summary>
      [JsonProperty("isConsumable")]
      public bool isConsumable { get; set; }
    
      /// <summary>
      /// Defines whether the holdings can be traded on secondary markets.
      /// </summary>
      [JsonProperty("isTradable")]
      public bool isTradable { get; set; }
    
      /// <summary>
      /// Name of the Asset.
      /// </summary>
      [JsonProperty("name")]
      public string name { get; set; }
    
      /// <summary>
      /// Vends offerings that grant this asset.
      /// </summary>
      [JsonProperty("offerings")]
      public OfferingConnection offerings { get; set; }
    
      /// <summary>
      /// A list of Tags associated with this asset. Can be used to filter down the
      /// assets when fetching connections.
      /// </summary>
      [JsonProperty("tags")]
      public List<string> tags { get; set; }
      #endregion
    }
    #endregion
    
    #region AssetDefinitionConnection
    public class AssetDefinitionConnection {
      #region members
      /// <summary>
      /// A list of edges.
      /// </summary>
      [JsonProperty("edges")]
      public List<AssetDefinitionEdge> edges { get; set; }
    
      /// <summary>
      /// A list of nodes.
      /// </summary>
      [JsonProperty("nodes")]
      public List<AssetDefinition> nodes { get; set; }
    
      /// <summary>
      /// Information to aid in pagination.
      /// </summary>
      [JsonProperty("pageInfo")]
      public PageInfo pageInfo { get; set; }
      #endregion
    }
    #endregion
    
    #region AssetDefinitionConnectionFilters
    /// <summary>
    /// Filters applicable to connections over FungibleAssets.
    /// </summary>
    public class AssetDefinitionConnectionFilters {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public List<AssetType> assetTypes { get; set; }
    
      /// <summary>
      /// If present, limit the connection only to assets that have this tag
      /// specified in their list of tags.
      /// </summary>
      public string tag { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region AssetDefinitionEdge
    /// <summary>
    /// An edge in a connection.
    /// </summary>
    public class AssetDefinitionEdge {
      #region members
      /// <summary>
      /// A cursor for use in pagination
      /// </summary>
      [JsonProperty("cursor")]
      public string cursor { get; set; }
    
      /// <summary>
      /// The item at the end of the edge
      /// </summary>
      [JsonProperty("node")]
      public AssetDefinition node { get; set; }
      #endregion
    }
    #endregion
    
    #region AssetFungibleHoldingInput
    public class AssetFungibleHoldingInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string assetId { get; set; }
    
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string quantity { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region AssetGrantInput
    public class AssetGrantInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public List<AssetOutputInput> assets { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region AssetHolding
    public class AssetHolding {
      #region members
      [JsonProperty("assetDefinition")]
      public AssetDefinition assetDefinition { get; set; }
    
      /// <summary>
      /// Connection returning instances
      /// </summary>
      [JsonProperty("instances")]
      public AssetInstanceConnection instances { get; set; }
    
      /// <summary>
      /// Quantity held for this asset.
      /// </summary>
      [JsonProperty("quantity")]
      public string quantity { get; set; }
      #endregion
    }
    #endregion
    
    #region AssetHoldingConnection
    public class AssetHoldingConnection {
      #region members
      /// <summary>
      /// A list of edges.
      /// </summary>
      [JsonProperty("edges")]
      public List<AssetHoldingEdge> edges { get; set; }
    
      /// <summary>
      /// A list of nodes.
      /// </summary>
      [JsonProperty("nodes")]
      public List<AssetHolding> nodes { get; set; }
    
      /// <summary>
      /// Information to aid in pagination.
      /// </summary>
      [JsonProperty("pageInfo")]
      public PageInfo pageInfo { get; set; }
      #endregion
    }
    #endregion
    
    #region AssetHoldingEdge
    /// <summary>
    /// An edge in a connection.
    /// </summary>
    public class AssetHoldingEdge {
      #region members
      /// <summary>
      /// A cursor for use in pagination
      /// </summary>
      [JsonProperty("cursor")]
      public string cursor { get; set; }
    
      /// <summary>
      /// The item at the end of the edge
      /// </summary>
      [JsonProperty("node")]
      public AssetHolding node { get; set; }
      #endregion
    }
    #endregion
    
    #region AssetHoldingInput
    public class AssetHoldingInput {
      #region members
      public AssetFungibleHoldingInput fungible { get; set; }
    
      public string instance { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region AssetHoldingInstanceFilters
    public class AssetHoldingInstanceFilters {
      #region members
      public string tag { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region AssetInstance
    public class AssetInstance : TransactionSource {
      #region members
      /// <summary>
      /// The kind used to discriminate the union type
      /// </summary>
      
      TransactionSourceKind TransactionSource.Kind { get { return TransactionSourceKind.AssetInstance; } }
    
      /// <summary>
      /// Time in milliseconds since this instance has been created.
      /// </summary>
      [JsonProperty("ageInMilliseconds")]
      public int ageInMilliseconds { get; set; }
    
      [JsonProperty("asComposable")]
      public ComposableProperties asComposable { get; set; }
    
      [JsonProperty("assetDefinition")]
      public AssetDefinition assetDefinition { get; set; }
    
      /// <summary>
      /// Marks if this instance is composable. This means it can be resolved and
      /// opened. Upon opening an asset, it will be consumed.
      /// </summary>
      [JsonProperty("composable")]
      public bool composable { get; set; }
    
      /// <summary>
      /// Marks whether this asset has already been consumed (if it's consumable).
      /// Consumed assets won't show up unless explicitly queried for.
      /// </summary>
      [JsonProperty("consumed")]
      public bool consumed { get; set; }
    
      [JsonProperty("id")]
      public string id { get; set; }
    
      /// <summary>
      /// Additional Metadata regarding the Asset's ownership.
      /// </summary>
      [JsonProperty("ownershipMetadata")]
      public string ownershipMetadata { get; set; }
    
      [JsonProperty("tags")]
      public List<string> tags { get; set; }
      #endregion
    }
    #endregion
    
    #region AssetInstanceConnection
    public class AssetInstanceConnection {
      #region members
      /// <summary>
      /// A list of edges.
      /// </summary>
      [JsonProperty("edges")]
      public List<AssetInstanceEdge> edges { get; set; }
    
      /// <summary>
      /// A list of nodes.
      /// </summary>
      [JsonProperty("nodes")]
      public List<AssetInstance> nodes { get; set; }
    
      /// <summary>
      /// Information to aid in pagination.
      /// </summary>
      [JsonProperty("pageInfo")]
      public PageInfo pageInfo { get; set; }
      #endregion
    }
    #endregion
    
    #region AssetInstanceEdge
    /// <summary>
    /// An edge in a connection.
    /// </summary>
    public class AssetInstanceEdge {
      #region members
      /// <summary>
      /// A cursor for use in pagination
      /// </summary>
      [JsonProperty("cursor")]
      public string cursor { get; set; }
    
      /// <summary>
      /// The item at the end of the edge
      /// </summary>
      [JsonProperty("node")]
      public AssetInstance node { get; set; }
      #endregion
    }
    #endregion
    
    #region AssetInstanceFilters
    public class AssetInstanceFilters {
      #region members
      public string assetId { get; set; }
    
      public List<AssetType> assetTypes { get; set; }
    
      public string tag { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region AssetOutput
    public class AssetOutput {
      #region members
      [JsonProperty("assetDefinition")]
      public AssetDefinition assetDefinition { get; set; }
    
      [JsonProperty("fungibleQuantity")]
      public string fungibleQuantity { get; set; }
    
      [JsonProperty("instanceQuantity")]
      public string instanceQuantity { get; set; }
      #endregion
    }
    #endregion
    
    #region AssetOutputInput
    public class AssetOutputInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string assetId { get; set; }
    
      public string fungibleQuantity { get; set; }
    
      public string instanceQuantity { get; set; }
    
      /// <summary>
      /// For instances created, these are the tags that will be applied to the instances
      /// </summary>
      public List<string> instancesTags { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region AssetQuantity
    public class AssetQuantity {
      #region members
      [JsonProperty("assetDefinition")]
      public AssetDefinition assetDefinition { get; set; }
    
      [JsonProperty("quantity")]
      public string quantity { get; set; }
      #endregion
    }
    #endregion
    public enum AssetType {
      COMPOSABLE_ASSET,
      FUNGIBLE_ASSET,
      UNIQUE_ASSET
    }
    
    
    #region Auction
    public class Auction {
      #region members
      [JsonProperty("assets")]
      public List<AssetOutput> assets { get; set; }
    
      /// <summary>
      /// The auction house that this Auction is posted in.
      /// </summary>
      [JsonProperty("auctionHouse")]
      public AuctionHouse auctionHouse { get; set; }
    
      [JsonProperty("buyNowPrice")]
      public string buyNowPrice { get; set; }
    
      [JsonProperty("endTime")]
      public string endTime { get; set; }
    
      [JsonProperty("highestBid")]
      public string highestBid { get; set; }
    
      [JsonProperty("highestBidder")]
      public Player highestBidder { get; set; }
    
      [JsonProperty("id")]
      public string id { get; set; }
    
      [JsonProperty("isActorHighestBidder")]
      public bool isActorHighestBidder { get; set; }
    
      [JsonProperty("seller")]
      public Player seller { get; set; }
    
      [JsonProperty("startingPrice")]
      public string startingPrice { get; set; }
    
      [JsonProperty("status")]
      public AuctionStatus status { get; set; }
      #endregion
    }
    #endregion
    
    #region AuctionBid
    public class AuctionBid {
      #region members
      [JsonProperty("auction")]
      public Auction auction { get; set; }
    
      [JsonProperty("id")]
      public string id { get; set; }
    
      [JsonProperty("maxPrice")]
      public string maxPrice { get; set; }
    
      [JsonProperty("status")]
      public AuctionBidStatus status { get; set; }
      #endregion
    }
    #endregion
    
    #region AuctionBidConnection
    public class AuctionBidConnection {
      #region members
      /// <summary>
      /// A list of edges.
      /// </summary>
      [JsonProperty("edges")]
      public List<AuctionBidEdge> edges { get; set; }
    
      /// <summary>
      /// A list of nodes.
      /// </summary>
      [JsonProperty("nodes")]
      public List<AuctionBid> nodes { get; set; }
    
      /// <summary>
      /// Information to aid in pagination.
      /// </summary>
      [JsonProperty("pageInfo")]
      public PageInfo pageInfo { get; set; }
      #endregion
    }
    #endregion
    
    #region AuctionBidEdge
    /// <summary>
    /// An edge in a connection.
    /// </summary>
    public class AuctionBidEdge {
      #region members
      /// <summary>
      /// A cursor for use in pagination
      /// </summary>
      [JsonProperty("cursor")]
      public string cursor { get; set; }
    
      /// <summary>
      /// The item at the end of the edge
      /// </summary>
      [JsonProperty("node")]
      public AuctionBid node { get; set; }
      #endregion
    }
    #endregion
    public enum AuctionBidStatus {
      /// <summary>
      /// Bid has been acknowledged, the funds were taken into escrow, the bid has not yet been processed
      /// against the auction.
      /// </summary>
      ACKNOWLEDGED,
      /// <summary>
      /// Bid has been processed and is currently the highest bid. Money is kept in the escrow.
      /// </summary>
      ACTIVE,
      /// <summary>
      /// Bid has been outbid - the money has been refunded to the bidder.
      /// </summary>
      INACTIVE_REFUNDED,
      /// <summary>
      /// Bid has won the auction, money moved from escrow to the seller.
      /// </summary>
      WON
    }
    
    
    #region AuctionConnection
    public class AuctionConnection {
      #region members
      /// <summary>
      /// A list of edges.
      /// </summary>
      [JsonProperty("edges")]
      public List<AuctionEdge> edges { get; set; }
    
      /// <summary>
      /// A list of nodes.
      /// </summary>
      [JsonProperty("nodes")]
      public List<Auction> nodes { get; set; }
    
      /// <summary>
      /// Information to aid in pagination.
      /// </summary>
      [JsonProperty("pageInfo")]
      public PageInfo pageInfo { get; set; }
      #endregion
    }
    #endregion
    
    #region AuctionEdge
    /// <summary>
    /// An edge in a connection.
    /// </summary>
    public class AuctionEdge {
      #region members
      /// <summary>
      /// A cursor for use in pagination
      /// </summary>
      [JsonProperty("cursor")]
      public string cursor { get; set; }
    
      /// <summary>
      /// The item at the end of the edge
      /// </summary>
      [JsonProperty("node")]
      public Auction node { get; set; }
      #endregion
    }
    #endregion
    
    #region AuctionFilters
    /// <summary>
    /// Filters that can be applied to the auctions connection.
    /// </summary>
    public class AuctionFilters {
      #region members
      /// <summary>
      /// Id of an auction house that the orders should be limited to.
      /// </summary>
      public string auctionHouseId { get; set; }
    
      /// <summary>
      /// By default only Active auctions will be vended. By providing
      /// this parameter, you can specify different statuses.
      /// </summary>
      public List<AuctionStatus> auctionStatuses { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region AuctionHouse
    public class AuctionHouse {
      #region members
      /// <summary>
      /// Allows fetching an auction by it's ID directly from the auction house.
      /// </summary>
      [JsonProperty("auction")]
      public Auction auction { get; set; }
    
      [JsonProperty("auctions")]
      public AuctionConnection auctions { get; set; }
    
      /// <summary>
      /// What is the currency that this exchange operates in?
      /// </summary>
      [JsonProperty("currency")]
      public AssetDefinition currency { get; set; }
    
      /// <summary>
      /// Game this exchanged is owned by.
      /// </summary>
      [JsonProperty("game")]
      public Game game { get; set; }
    
      [JsonProperty("id")]
      public string id { get; set; }
    
      /// <summary>
      /// Name of the exchange.
      /// </summary>
      [JsonProperty("name")]
      public string name { get; set; }
    
      [JsonProperty("tags")]
      public List<string> tags { get; set; }
      #endregion
    }
    #endregion
    
    #region AuctionHouseAuctionFilters
    /// <summary>
    /// Filters that can be applied to the auctions connection.
    /// </summary>
    public class AuctionHouseAuctionFilters {
      #region members
      /// <summary>
      /// By default only Active auctions show up. This allows specifying
      /// which statuses should be included in the response.
      /// </summary>
      public List<AuctionStatus> auctionStatuses { get; set; }
    
      /// <summary>
      /// Id of an auction house that the orders should be limited to.
      /// </summary>
      public string playerId { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region AuctionHouseConnection
    public class AuctionHouseConnection {
      #region members
      /// <summary>
      /// A list of edges.
      /// </summary>
      [JsonProperty("edges")]
      public List<AuctionHouseEdge> edges { get; set; }
    
      /// <summary>
      /// A list of nodes.
      /// </summary>
      [JsonProperty("nodes")]
      public List<AuctionHouse> nodes { get; set; }
    
      /// <summary>
      /// Information to aid in pagination.
      /// </summary>
      [JsonProperty("pageInfo")]
      public PageInfo pageInfo { get; set; }
      #endregion
    }
    #endregion
    
    #region AuctionHouseEdge
    /// <summary>
    /// An edge in a connection.
    /// </summary>
    public class AuctionHouseEdge {
      #region members
      /// <summary>
      /// A cursor for use in pagination
      /// </summary>
      [JsonProperty("cursor")]
      public string cursor { get; set; }
    
      /// <summary>
      /// The item at the end of the edge
      /// </summary>
      [JsonProperty("node")]
      public AuctionHouse node { get; set; }
      #endregion
    }
    #endregion
    
    #region AuctionHouseFilters
    /// <summary>
    /// Filters applicable to connection over Auction House.
    /// </summary>
    public class AuctionHouseFilters {
      #region members
      /// <summary>
      /// If present, return only the exchanges name of which contains this string.
      /// </summary>
      public string nameContains { get; set; }
    
      /// <summary>
      /// If present, return only the exchanges which carry this tag in their tag
      /// list.
      /// </summary>
      public string tag { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    public enum AuctionStatus {
      ACTIVE,
      FINISHED
    }
    
    
    #region BidOnAuctionInput
    public class BidOnAuctionInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string auctionId { get; set; }
    
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string maxPrice { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region BuyOfferingInput
    /// <summary>
    /// Input object used for purchasing Offerings.
    /// </summary>
    public class BuyOfferingInput {
      #region members
      /// <summary>
      /// clientTransactionId is an idempotency token of this transaction.
      /// It is used by the backend to ensure that client doesn't accidentally submit multiple
      /// transactions.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string clientTransactionId { get; set; }
    
      /// <summary>
      /// An optional directives specifying which assets must be used as part of this
      /// transaction. If nothing is provided, by default, the fungible assets will be
      /// deducted. This is the most common scenario.
      /// </summary>
      public List<AssetHoldingInput> inputSelection { get; set; }
    
      /// <summary>
      /// Id of an Offering the player intends to buy.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string offerId { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region CancelOrderInput
    /// <summary>
    /// Input type required to cancel an order.
    /// </summary>
    public class CancelOrderInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string orderId { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region ComposableAssetInput
    /// <summary>
    /// ComposableAssetInput is the main interface for all composition types possible in
    /// composable asset system.
    /// </summary>
    public class ComposableAssetInput {
      #region members
      /// <summary>
      /// ComposableAssetInput is the main interface for all composition types possible in
      /// composable asset system.
      /// </summary>
      public DurationAssetInput duration { get; set; }
    
      /// <summary>
      /// ComposableAssetInput is the main interface for all composition types possible in
      /// composable asset system.
      /// </summary>
      public AssetGrantInput grant { get; set; }
    
      /// <summary>
      /// ComposableAssetInput is the main interface for all composition types possible in
      /// composable asset system.
      /// </summary>
      public GroupAssetInput group { get; set; }
    
      /// <summary>
      /// ComposableAssetInput is the main interface for all composition types possible in
      /// composable asset system.
      /// </summary>
      public RandomAssetInput random { get; set; }
    
      /// <summary>
      /// ComposableAssetInput is the main interface for all composition types possible in
      /// composable asset system.
      /// </summary>
      public RevocableAssetInput revocable { get; set; }
    
      /// <summary>
      /// ComposableAssetInput is the main interface for all composition types possible in
      /// composable asset system.
      /// </summary>
      public SealedAssetInput @sealed { get; set; }
    
      /// <summary>
      /// ComposableAssetInput is the main interface for all composition types possible in
      /// composable asset system.
      /// </summary>
      public TimedAssetInput timed { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region ComposableProperties
    public class ComposableProperties {
      #region members
      [JsonProperty("asset")]
      public string asset { get; set; }
    
      [JsonProperty("isComposable")]
      public bool isComposable { get; set; }
      #endregion
    }
    #endregion
    
    #region ConsumeAssetInput
    /// <summary>
    /// Input object required to consume (or burn) an asset.
    /// </summary>
    public class ConsumeAssetInput {
      #region members
      /// <summary>
      /// Asset to be burned. The server will perform
      /// validation regarding whether that asset is
      /// even possible to burn.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public AssetHoldingInput asset { get; set; }
    
      /// <summary>
      /// Idempotency Token. Protects from client accidentally
      /// sending the same request multiple times.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string clientTransactionId { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region CreateAuctionInput
    public class CreateAuctionInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public List<AssetHoldingInput> assets { get; set; }
    
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string auctionHouseId { get; set; }
    
      public string buyNowPrice { get; set; }
    
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string endTime { get; set; }
    
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string startingPrice { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region CreateOrderInput
    /// <summary>
    /// Input object required to create an exchange order.
    /// </summary>
    public class CreateOrderInput {
      #region members
      /// <summary>
      /// Asset that is being traded.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string assetId { get; set; }
    
      /// <summary>
      /// Exchange on which the order should be posted.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string exchangeId { get; set; }
    
      /// <summary>
      /// Desired order expiration.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public OrderExpiration expiration { get; set; }
    
      /// <summary>
      /// Idempotency token to be used with this order.
      /// This field is used to ensure multiple consecutive
      /// requests wouldn't cause us to create multiple
      /// exchange orders.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string idempotencyToken { get; set; }
    
      /// <summary>
      /// Type of the order.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public OrderTypeInput orderType { get; set; }
    
      /// <summary>
      /// Desired price (only when it applies).
      /// </summary>
      public string price { get; set; }
    
      /// <summary>
      /// Transaction size
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string quantity { get; set; }
    
      /// <summary>
      /// Order side.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public OrderSide side { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region DurationAssetInput
    public class DurationAssetInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public int afterDurationSecs { get; set; }
    
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public ComposableAssetInput asset { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region DynamicallyCreateComposableAssetInput
    /// <summary>
    /// Input object required to create a composable asset.
    /// </summary>
    public class DynamicallyCreateComposableAssetInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public ComposableAssetInput asset { get; set; }
    
      /// <summary>
      /// Player to receive the asset.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string playerId { get; set; }
    
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public List<string> tags { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region Exchange
    public class Exchange {
      #region members
      [JsonProperty("assetDataOverTime")]
      public ExchangeAssetSumPriceData assetDataOverTime { get; set; }
    
      /// <summary>
      /// What is the currency that this exchange operates in?
      /// </summary>
      [JsonProperty("currency")]
      public AssetDefinition currency { get; set; }
    
      /// <summary>
      /// Game this exchanged is owned by.
      /// </summary>
      [JsonProperty("game")]
      public Game game { get; set; }
    
      [JsonProperty("id")]
      public string id { get; set; }
    
      /// <summary>
      /// Name of the exchange.
      /// </summary>
      [JsonProperty("name")]
      public string name { get; set; }
    
      /// <summary>
      /// Returns current price spread for given asset.
      /// </summary>
      [JsonProperty("priceSpreadForAsset")]
      public ExchangeAssetPriceSpread priceSpreadForAsset { get; set; }
    
      /// <summary>
      /// Which order types will this exchange accept?
      /// </summary>
      [JsonProperty("supportedOrderTypes")]
      public List<OrderType> supportedOrderTypes { get; set; }
      #endregion
    }
    #endregion
    
    #region ExchangeAssetPriceSpread
    /// <summary>
    /// ExchangeAssetPriceSpread represents current spread for given
    /// asset on a given exchange. Note that due to low volume the
    /// information about ask/bid may be missing.
    /// </summary>
    public class ExchangeAssetPriceSpread {
      #region members
      [JsonProperty("ask")]
      public string ask { get; set; }
    
      [JsonProperty("bid")]
      public string bid { get; set; }
      #endregion
    }
    #endregion
    
    #region ExchangeAssetSumPriceData
    public class ExchangeAssetSumPriceData {
      #region members
      [JsonProperty("fiveMinutesSum")]
      public ExchangeAssetSumPriceDataEntry fiveMinutesSum { get; set; }
    
      [JsonProperty("hourSum")]
      public ExchangeAssetSumPriceDataEntry hourSum { get; set; }
    
      [JsonProperty("minuteSum")]
      public ExchangeAssetSumPriceDataEntry minuteSum { get; set; }
      #endregion
    }
    #endregion
    
    #region ExchangeAssetSumPriceDataEntry
    public class ExchangeAssetSumPriceDataEntry {
      #region members
      [JsonProperty("quantity")]
      public string quantity { get; set; }
    
      [JsonProperty("totalPrice")]
      public string totalPrice { get; set; }
      #endregion
    }
    #endregion
    
    #region ExchangeConnection
    public class ExchangeConnection {
      #region members
      /// <summary>
      /// A list of edges.
      /// </summary>
      [JsonProperty("edges")]
      public List<ExchangeEdge> edges { get; set; }
    
      /// <summary>
      /// A list of nodes.
      /// </summary>
      [JsonProperty("nodes")]
      public List<Exchange> nodes { get; set; }
    
      /// <summary>
      /// Information to aid in pagination.
      /// </summary>
      [JsonProperty("pageInfo")]
      public PageInfo pageInfo { get; set; }
      #endregion
    }
    #endregion
    
    #region ExchangeEdge
    /// <summary>
    /// An edge in a connection.
    /// </summary>
    public class ExchangeEdge {
      #region members
      /// <summary>
      /// A cursor for use in pagination
      /// </summary>
      [JsonProperty("cursor")]
      public string cursor { get; set; }
    
      /// <summary>
      /// The item at the end of the edge
      /// </summary>
      [JsonProperty("node")]
      public Exchange node { get; set; }
      #endregion
    }
    #endregion
    
    #region ExchangeFilters
    /// <summary>
    /// Filters applicable to connection over Exchanges.
    /// </summary>
    public class ExchangeFilters {
      #region members
      /// <summary>
      /// If present, return only the exchanges name of which contains this string.
      /// </summary>
      public string nameContains { get; set; }
    
      /// <summary>
      /// If present, return only the exchanges which carry this tag in their tag
      /// list.
      /// </summary>
      public string tag { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region ExchangeOrder
    public class ExchangeOrder {
      #region members
      /// <summary>
      /// Asset the order is associated with.
      /// </summary>
      [JsonProperty("assetDefinition")]
      public AssetDefinition assetDefinition { get; set; }
    
      /// <summary>
      /// Date when the order has been created.
      /// </summary>
      [JsonProperty("created")]
      public string created { get; set; }
    
      /// <summary>
      /// Date when the order will expire.
      /// </summary>
      [JsonProperty("expiration")]
      public string expiration { get; set; }
    
      [JsonProperty("id")]
      public string id { get; set; }
    
      /// <summary>
      /// Token provided when submitting an order. Used to ensure
      /// multiple transactions aren't created accidentally by the frontend.
      /// </summary>
      [JsonProperty("idempotencyToken")]
      public string idempotencyToken { get; set; }
    
      /// <summary>
      /// Original size of the order.
      /// </summary>
      [JsonProperty("originalQuantity")]
      public string originalQuantity { get; set; }
    
      /// <summary>
      /// Player that this order is owned by.
      /// </summary>
      [JsonProperty("player")]
      public Player player { get; set; }
    
      /// <summary>
      /// Desired price. Applicable to specific transaction types.
      /// </summary>
      [JsonProperty("price")]
      public string price { get; set; }
    
      /// <summary>
      /// Remaining size of the order. Orders can be partially filled as demand
      /// grows. As they do, the remaining quantity reduces.
      /// </summary>
      [JsonProperty("remainingQuantity")]
      public string remainingQuantity { get; set; }
    
      /// <summary>
      /// Side that player has taken on the transaction.
      /// </summary>
      [JsonProperty("side")]
      public OrderSide side { get; set; }
    
      /// <summary>
      /// Transaction status.
      /// </summary>
      [JsonProperty("status")]
      public OrderStatus status { get; set; }
      #endregion
    }
    #endregion
    
    #region ExchangeOrderConnection
    public class ExchangeOrderConnection {
      #region members
      /// <summary>
      /// A list of edges.
      /// </summary>
      [JsonProperty("edges")]
      public List<ExchangeOrderEdge> edges { get; set; }
    
      /// <summary>
      /// A list of nodes.
      /// </summary>
      [JsonProperty("nodes")]
      public List<ExchangeOrder> nodes { get; set; }
    
      /// <summary>
      /// Information to aid in pagination.
      /// </summary>
      [JsonProperty("pageInfo")]
      public PageInfo pageInfo { get; set; }
      #endregion
    }
    #endregion
    
    #region ExchangeOrderEdge
    /// <summary>
    /// An edge in a connection.
    /// </summary>
    public class ExchangeOrderEdge {
      #region members
      /// <summary>
      /// A cursor for use in pagination
      /// </summary>
      [JsonProperty("cursor")]
      public string cursor { get; set; }
    
      /// <summary>
      /// The item at the end of the edge
      /// </summary>
      [JsonProperty("node")]
      public ExchangeOrder node { get; set; }
      #endregion
    }
    #endregion
    
    #region ExchangeOrderFilters
    /// <summary>
    /// Filters that can be applied to the orders connection.
    /// </summary>
    public class ExchangeOrderFilters {
      #region members
      /// <summary>
      /// Id of an asset that the orders should be limited to.
      /// </summary>
      public string assetId { get; set; }
    
      /// <summary>
      /// Id of an Exchange that the orders should be limited to.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string exchangeId { get; set; }
    
      /// <summary>
      /// Set of statuses the orders should be filtered down to. For instance
      /// ["OPEN"] or ["CANCELLED", "CLOSED"]
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public List<OrderStatus> status { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region FungibleProperties
    public class FungibleProperties {
      #region members
      [JsonProperty("decimalPlaces")]
      public int decimalPlaces { get; set; }
      #endregion
    }
    #endregion
    
    #region Game
    public class Game {
      #region members
      /// <summary>
      /// Number of all active offerings.
      /// </summary>
      [JsonProperty("activeOfferingsCount")]
      public int activeOfferingsCount { get; set; }
    
      [JsonProperty("assetDefinition")]
      public AssetDefinition assetDefinition { get; set; }
    
      [JsonProperty("assetDefinitions")]
      public AssetDefinitionConnection assetDefinitions { get; set; }
    
      [JsonProperty("assetDefinitionsWithIds")]
      public List<AssetDefinition> assetDefinitionsWithIds { get; set; }
    
      [JsonProperty("auctionHouse")]
      public AuctionHouse auctionHouse { get; set; }
    
      [JsonProperty("auctionHouseWithName")]
      public AuctionHouse auctionHouseWithName { get; set; }
    
      [JsonProperty("auctionHouses")]
      public AuctionHouseConnection auctionHouses { get; set; }
    
      [JsonProperty("exchange")]
      public Exchange exchange { get; set; }
    
      /// <summary>
      /// Returns a Connection of Exchanges that belong to the game. Can be
      /// filtered down using `filters`.
      /// </summary>
      [JsonProperty("exchanges")]
      public ExchangeConnection exchanges { get; set; }
    
      [JsonProperty("id")]
      public string id { get; set; }
    
      /// <summary>
      /// Returns a connection with all active InAppProducts that can be purchased.
      /// </summary>
      [JsonProperty("inAppProducts")]
      public InAppProductEntityConnection inAppProducts { get; set; }
    
      /// <summary>
      /// Number of all active in app products.
      /// </summary>
      [JsonProperty("inAppProductsCount")]
      public int inAppProductsCount { get; set; }
    
      /// <summary>
      /// The name of the game.
      /// </summary>
      [JsonProperty("name")]
      public string name { get; set; }
    
      /// <summary>
      /// Returns an Offering with a particular Id if it exists and belongs to this game.
      /// </summary>
      [JsonProperty("offering")]
      public Offering offering { get; set; }
    
      /// <summary>
      /// Returns a connection with all the Storefront offerings the game currently offers.
      /// The connection can be filtered down using `filters`.
      /// </summary>
      [JsonProperty("offerings")]
      public OfferingConnection offerings { get; set; }
    
      /// <summary>
      /// The organization that owns this game.
      /// </summary>
      [JsonProperty("organization")]
      public Organization organization { get; set; }
      #endregion
    }
    #endregion
    
    #region GameConnection
    public class GameConnection {
      #region members
      /// <summary>
      /// A list of edges.
      /// </summary>
      [JsonProperty("edges")]
      public List<GameEdge> edges { get; set; }
    
      /// <summary>
      /// A list of nodes.
      /// </summary>
      [JsonProperty("nodes")]
      public List<Game> nodes { get; set; }
    
      /// <summary>
      /// Information to aid in pagination.
      /// </summary>
      [JsonProperty("pageInfo")]
      public PageInfo pageInfo { get; set; }
      #endregion
    }
    #endregion
    
    #region GameEdge
    /// <summary>
    /// An edge in a connection.
    /// </summary>
    public class GameEdge {
      #region members
      /// <summary>
      /// A cursor for use in pagination
      /// </summary>
      [JsonProperty("cursor")]
      public string cursor { get; set; }
    
      /// <summary>
      /// The item at the end of the edge
      /// </summary>
      [JsonProperty("node")]
      public Game node { get; set; }
      #endregion
    }
    #endregion
      /// <summary>
      /// Type of purchase supported by Google Play.
      /// </summary>
    public enum GooglePlayPurchaseType {
      /// <summary>
      /// Consumable/non-consumable product.
      /// </summary>
      PRODUCT,
      /// <summary>
      /// CURRENTLY NOT SUPPORTED. If your purchasable item is a subscription,
      /// it needs to be marked using this value.
      /// </summary>
      SUBSCRIPTION
    }
    
    
    #region GroupAssetInput
    public class GroupAssetInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public List<ComposableAssetInput> assets { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region InAppProductEntity
    public class InAppProductEntity {
      #region members
      /// <summary>
      /// Asset that is given to player upon buying this product.
      /// </summary>
      [JsonProperty("assets")]
      public List<AssetOutput> assets { get; set; }
    
      /// <summary>
      /// Game that owns this product.
      /// </summary>
      [JsonProperty("game")]
      public Game game { get; set; }
    
      [JsonProperty("id")]
      public string id { get; set; }
    
      /// <summary>
      /// Name of the InAppProduct
      /// </summary>
      [JsonProperty("name")]
      public string name { get; set; }
    
      /// <summary>
      /// ProductId as defined in the stores.
      /// </summary>
      [JsonProperty("productId")]
      public string productId { get; set; }
      #endregion
    }
    #endregion
    
    #region InAppProductEntityConnection
    public class InAppProductEntityConnection {
      #region members
      /// <summary>
      /// A list of edges.
      /// </summary>
      [JsonProperty("edges")]
      public List<InAppProductEntityEdge> edges { get; set; }
    
      /// <summary>
      /// A list of nodes.
      /// </summary>
      [JsonProperty("nodes")]
      public List<InAppProductEntity> nodes { get; set; }
    
      /// <summary>
      /// Information to aid in pagination.
      /// </summary>
      [JsonProperty("pageInfo")]
      public PageInfo pageInfo { get; set; }
      #endregion
    }
    #endregion
    
    #region InAppProductEntityEdge
    /// <summary>
    /// An edge in a connection.
    /// </summary>
    public class InAppProductEntityEdge {
      #region members
      /// <summary>
      /// A cursor for use in pagination
      /// </summary>
      [JsonProperty("cursor")]
      public string cursor { get; set; }
    
      /// <summary>
      /// The item at the end of the edge
      /// </summary>
      [JsonProperty("node")]
      public InAppProductEntity node { get; set; }
      #endregion
    }
    #endregion
    
    #region InAppPurchase
    public class InAppPurchase {
      #region members
      [JsonProperty("id")]
      public string id { get; set; }
      #endregion
    }
    #endregion
    
    #region Offering
    public class Offering : TransactionSource {
      #region members
      /// <summary>
      /// The kind used to discriminate the union type
      /// </summary>
      
      TransactionSourceKind TransactionSource.Kind { get { return TransactionSourceKind.Offering; } }
    
      /// <summary>
      /// Represents the input of the offering (or "cost" of transaction).
      /// For instance, if the game sells an item Foo for a 100 Coins, this field
      /// would hold the "100 coins".
      /// </summary>
      [JsonProperty("assetsIn")]
      public List<AssetQuantity> assetsIn { get; set; }
    
      /// <summary>
      /// Represents the output of the offering (what you buy).
      /// For instance, if the game sells an item Foo for a 100 Coins, this field would
      /// hold the item Foo.
      /// </summary>
      [JsonProperty("assetsOut")]
      public List<AssetOutput> assetsOut { get; set; }
    
      /// <summary>
      /// An additional description for the offering.
      /// </summary>
      [JsonProperty("description")]
      public string description { get; set; }
    
      /// <summary>
      /// Is the offer manually disabled?
      /// </summary>
      [JsonProperty("disabled")]
      public bool disabled { get; set; }
    
      /// <summary>
      /// Validates whether the caller has enough holdings to cover for the inputs of the transaction
      /// if they try to purchase it.
      /// </summary>
      [JsonProperty("hasRequiredInputs")]
      public bool hasRequiredInputs { get; set; }
    
      [JsonProperty("id")]
      public string id { get; set; }
    
      /// <summary>
      /// This field verifies if the offer is active.
      /// </summary>
      [JsonProperty("isActive")]
      public bool isActive { get; set; }
    
      /// <summary>
      /// maxCapReached encompasses two things:
      /// 1. Player has already reached a cap on one of the outputs of the offer, hence
      /// the offer purchase cannot be performed
      /// 2. Player has already reached the purchase cap of the offering itself, hence
      /// the offer purchase cannot be performed
      /// 
      /// Note: Result type doesn't actually inform us which result we have hit.
      /// </summary>
      [JsonProperty("maxCapReached")]
      public bool maxCapReached { get; set; }
    
      /// <summary>
      /// A name of the Offering.
      /// </summary>
      [JsonProperty("name")]
      public string name { get; set; }
    
      /// <summary>
      /// Purchase Cap for the offering. Represents maximum amount of
      /// times that this offering can be bought by a single player.
      /// </summary>
      [JsonProperty("purchaseCap")]
      public int? purchaseCap { get; set; }
    
      /// <summary>
      /// List of tags associated with the Offering. Can be used
      /// to filter down the offerings on connection fetches.
      /// </summary>
      [JsonProperty("tags")]
      public List<string> tags { get; set; }
    
      /// <summary>
      /// Shows range of dates through which the offer will be active
      /// </summary>
      [JsonProperty("validThroughDates")]
      public string validThroughDates { get; set; }
      #endregion
    }
    #endregion
    
    #region OfferingConnection
    public class OfferingConnection {
      #region members
      /// <summary>
      /// A list of edges.
      /// </summary>
      [JsonProperty("edges")]
      public List<OfferingEdge> edges { get; set; }
    
      /// <summary>
      /// A list of nodes.
      /// </summary>
      [JsonProperty("nodes")]
      public List<Offering> nodes { get; set; }
    
      /// <summary>
      /// Information to aid in pagination.
      /// </summary>
      [JsonProperty("pageInfo")]
      public PageInfo pageInfo { get; set; }
      #endregion
    }
    #endregion
    
    #region OfferingEdge
    /// <summary>
    /// An edge in a connection.
    /// </summary>
    public class OfferingEdge {
      #region members
      /// <summary>
      /// A cursor for use in pagination
      /// </summary>
      [JsonProperty("cursor")]
      public string cursor { get; set; }
    
      /// <summary>
      /// The item at the end of the edge
      /// </summary>
      [JsonProperty("node")]
      public Offering node { get; set; }
      #endregion
    }
    #endregion
    
    #region OfferingFiltersInput
    /// <summary>
    /// Filters that can be used to filter results on `offerings` edge on Game
    /// </summary>
    public class OfferingFiltersInput {
      #region members
      /// <summary>
      /// A string that must be part of the offering's name
      /// </summary>
      public string name { get; set; }
    
      /// <summary>
      /// A tag that must be present on offering itself
      /// </summary>
      public string tag { get; set; }
    
      /// <summary>
      /// Limits the returned offerings only to those valid through
      /// a particular time range
      /// </summary>
      public string validThrough { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    public enum OrderExpiration {
      DAY,
      MONTH,
      THREE_MONTHS,
      WEEK
    }
    
    public enum OrderSide {
      BUY,
      SELL
    }
    
    public enum OrderStatus {
      /// <summary>
      /// Order has been received and noted, but hasn't yet been processed.
      /// </summary>
      ACKNOWLEDGED,
      /// <summary>
      /// Cancelled order may still be partially filled. Cancellation doesn't guarantee reversal.
      /// If `original_quantity` > `remaining_quantity`, this means some fills will exist for the
      /// order.
      /// </summary>
      CANCELLED,
      /// <summary>
      /// This order has been closed for whatever reason (has been filled/killed or expired).
      /// </summary>
      CLOSED,
      /// <summary>
      /// Order is (still) open and being matched against the order book.
      /// This doesn't mean order hasn't been filled yet. It may already have
      /// partial fills.
      /// </summary>
      OPEN
    }
    
      /// <summary>
      /// Types of orders currently supported by exchanges.
      /// Note: Exchanges can select which types are going to be available on their exchanges.
      /// For instance, a game may not desire Market orders to be available and expect
      /// players to always list the price. This is extremely useful in low-volume exchanges
      /// to provide defense against malicious orders.
      /// </summary>
    public enum OrderType {
      /// <summary>
      /// Standard limit order - both buy and sell orders must define
      /// their limit prices when submitting. If the limit price
      /// or better price is available, it will be executed.
      /// </summary>
      LIMIT,
      /// <summary>
      /// This is currently not supported.
      /// </summary>
      MARKET
    }
    
      /// <summary>
      /// Order type.
      /// </summary>
    public enum OrderTypeInput {
      /// <summary>
      /// Limit order requires setting a desired price and guarantees
      /// transaction happening with that price or better.
      /// </summary>
      LIMIT,
      /// <summary>
      /// NOT SUPPORTED YET. Market order buys/sells the asset
      /// on the current market price.
      /// </summary>
      MARKET
    }
    
    
    #region Organization
    public class Organization {
      #region members
      [JsonProperty("displayName")]
      public string displayName { get; set; }
    
      [JsonProperty("game")]
      public Game game { get; set; }
    
      /// <summary>
      /// A connection that returns all games the organization is the owner of.
      /// </summary>
      [JsonProperty("games")]
      public GameConnection games { get; set; }
    
      [JsonProperty("id")]
      public string id { get; set; }
    
      /// <summary>
      /// The name of the organization.
      /// </summary>
      [JsonProperty("name")]
      public string name { get; set; }
      #endregion
    }
    #endregion
    
    #region PageInfo
    /// <summary>
    /// Information about pagination in a connection
    /// </summary>
    public class PageInfo {
      #region members
      /// <summary>
      /// When paginating forwards, the cursor to continue.
      /// </summary>
      [JsonProperty("endCursor")]
      public string endCursor { get; set; }
    
      /// <summary>
      /// When paginating forwards, are there more items?
      /// </summary>
      [JsonProperty("hasNextPage")]
      public bool hasNextPage { get; set; }
    
      /// <summary>
      /// When paginating backwards, are there more items?
      /// </summary>
      [JsonProperty("hasPreviousPage")]
      public bool hasPreviousPage { get; set; }
    
      /// <summary>
      /// When paginating backwards, the cursor to continue.
      /// </summary>
      [JsonProperty("startCursor")]
      public string startCursor { get; set; }
      #endregion
    }
    #endregion
    
    #region Player
    public class Player : Viewer {
      #region members
      /// <summary>
      /// The kind used to discriminate the union type
      /// </summary>
      
      ViewerKind Viewer.Kind { get { return ViewerKind.Player; } }
    
      [JsonProperty("assetHolding")]
      public AssetHolding assetHolding { get; set; }
    
      [JsonProperty("assetHoldings")]
      public AssetHoldingConnection assetHoldings { get; set; }
    
      /// <summary>
      /// Allows fetching holdings for multiple asset definitions at once.
      /// If there is no holding for given asset definition for player, it won't
      /// appear in the array returned by this function.
      /// </summary>
      [JsonProperty("assetHoldingsWithIds")]
      public List<AssetHolding> assetHoldingsWithIds { get; set; }
    
      /// <summary>
      /// A shortcut field that allows fetching instances of assets directly
      /// </summary>
      [JsonProperty("assetInstances")]
      public AssetInstanceConnection assetInstances { get; set; }
    
      /// <summary>
      /// Fetch your auction bid by ID
      /// </summary>
      [JsonProperty("auctionBid")]
      public AuctionBid auctionBid { get; set; }
    
      /// <summary>
      /// Allows player fetching all auctions they are a seller on
      /// </summary>
      [JsonProperty("auctions")]
      public AuctionConnection auctions { get; set; }
    
      /// <summary>
      /// Returns default wallet address for the Player.
      /// </summary>
      [JsonProperty("defaultWalletAddress")]
      public string defaultWalletAddress { get; set; }
    
      /// <summary>
      /// A game that the player belongs to.
      /// </summary>
      [JsonProperty("game")]
      public Game game { get; set; }
    
      [JsonProperty("id")]
      public string id { get; set; }
    
      /// <summary>
      /// Returns Player's exchange order with given ID if it exists.
      /// </summary>
      [JsonProperty("order")]
      public ExchangeOrder order { get; set; }
    
      /// <summary>
      /// Returns a connection with all the orders of the Player. Can be filtered down using `filters`.
      /// </summary>
      [JsonProperty("orders")]
      public ExchangeOrderConnection orders { get; set; }
    
      /// <summary>
      /// A connection that vends all the transactions that the player participated in
      /// over time. Used as a validation ledger.
      /// </summary>
      [JsonProperty("transactions")]
      public TransactionLogConnection transactions { get; set; }
    
      [JsonProperty("winningAuctionBids")]
      public AuctionBidConnection winningAuctionBids { get; set; }
      #endregion
    }
    #endregion
    
    #region PublicMutation
    public class PublicMutation {
      #region members
      /// <summary>
      /// applyPurchase takes receipt data from the client side, validates it
      /// directly with external payment processor and upon successful validation
      /// grants the assets to the player and produces InAppPurchase.
      /// </summary>
      [JsonProperty("applyPurchase")]
      public List<InAppPurchase> applyPurchase { get; set; }
    
      /// <summary>
      /// Creates a bid on an existing auction. Returns confirmation whether the bid
      /// was taken up for a processing. Bid processing happens asynchronously.
      /// Poll the auction to get the highest bid.
      /// </summary>
      [JsonProperty("bidOnAuction")]
      public AuctionBid bidOnAuction { get; set; }
    
      /// <summary>
      /// Attempts buying an offering on behalf of the viewer.
      /// Returns Offering that has been bought on success, or Error
      /// on failure, insufficient funds, incorrect id and others.
      /// </summary>
      [JsonProperty("buyOffering")]
      public Offering buyOffering { get; set; }
    
      /// <summary>
      /// Cancels ongoing order. The cancellation is not guaranteed as the order may execute before.
      /// If you receive true as reply, it means the cancel request has been queued and will be
      /// executed. Otherwise the client should retry cancellation.
      /// </summary>
      [JsonProperty("cancelExchangeOrder")]
      public bool cancelExchangeOrder { get; set; }
    
      /// <summary>
      /// Consume (or burns) an asset. The server will perform validation to ensure
      /// the viewer can burn an asset.
      /// The mutation returns a ConsumedAsset object which acts as a receipt.
      /// </summary>
      [JsonProperty("consumeAsset")]
      public TransactionLog consumeAsset { get; set; }
    
      [JsonProperty("createAuction")]
      public Auction createAuction { get; set; }
    
      /// <summary>
      /// Create a composable asset and grant it to a specific player.
      /// This operation can be used as a way of granting more complicated
      /// assets such as ones that unlock on given date, or that have
      /// a random drop.
      /// Requires Organization authorization.
      /// </summary>
      [JsonProperty("createComposableAssetDynamically")]
      public AssetInstance createComposableAssetDynamically { get; set; }
    
      /// <summary>
      /// Creates an order on an exchange on behalf of the viewer.
      /// Returns order upon successful creation.
      /// </summary>
      [JsonProperty("createExchangeOrder")]
      public ExchangeOrder createExchangeOrder { get; set; }
    
      /// <summary>
      /// Attempt revoking an existing asset. Asset is only revokable
      /// if top-level node of the Composable Asset is wrapped in a
      /// RevokableAsset. This can be useful mostly if organization is
      /// validating additional conditions externally and can decide
      /// to revoke the asset.
      /// Requires Organization authorization.
      /// </summary>
      [JsonProperty("revokeAsset")]
      public RevokeStatus revokeAsset { get; set; }
    
      /// <summary>
      /// Attempt revoking all assets with a given tag. See `revokeAsset` for
      /// more details.
      /// Requires Organization authorization.
      /// </summary>
      [JsonProperty("revokeAssetsTagged")]
      public Dictionary<string, string> revokeAssetsTagged { get; set; }
    
      /// <summary>
      /// Attempt to open an asset with given id.
      /// This functionality will take asset that is fully resolved (see `resolveAsset`)
      /// and grant all the output from this asset to the player.
      /// Asset must be resolved before it can be opened.
      /// </summary>
      [JsonProperty("unsealAsset")]
      public UnsealResult unsealAsset { get; set; }
    
      /// <summary>
      /// Attempt to open all assets with a given tag. See `openAsset`for
      /// more details.
      /// </summary>
      [JsonProperty("unsealAssetsTagged")]
      public List<UnsealResult> unsealAssetsTagged { get; set; }
    
      /// <summary>
      /// Attempt to resolve an asset with given id.
      /// This functionality explicitly tries to unwrap composable asset's
      /// layers until it gets to a final stage where assets can be granted
      /// to the player. At this point the asset's state change to `ReadyToOpen`
      /// and the viewer can call `openAsset` to be granted the assets in this
      /// composable asset.
      /// </summary>
      [JsonProperty("unwrapAsset")]
      public UnwrapResult unwrapAsset { get; set; }
    
      /// <summary>
      /// Attempt to resolve all assets with a given tag. See `resolveAsset`
      /// for more details.
      /// </summary>
      [JsonProperty("unwrapAssetsTagged")]
      public List<UnwrapResult> unwrapAssetsTagged { get; set; }
    
      /// <summary>
      /// This mode validates that the nonce attached as an input is the nonce generated
      /// by the last request to the server made by this player.
      /// 
      /// This query can only be run if the request is signed via delegated authority.
      /// </summary>
      [JsonProperty("validateNonce")]
      public bool validateNonce { get; set; }
      #endregion
    }
    #endregion
    
    #region PublicQuery
    public class PublicQuery {
      #region members
      /// <summary>
      /// Load game by game ID. Allows to load up some common data about game without having
      /// a Player account of that game.
      /// </summary>
      [JsonProperty("game")]
      public Game game { get; set; }
    
      /// <summary>
      /// getGameId allows retrieving gameId knowing the organization's name and game's name for it.
      /// This then allows viewers to interact with the Game object via `game` field.
      /// </summary>
      [JsonProperty("getGameId")]
      public string getGameId { get; set; }
    
      /// <summary>
      /// Shortcut that allows `Player` viewers to directly interact with the model.
      /// Equivalent of calling:
      /// query {
      /// viewer {
      /// ... on Player {
      /// // field selection
      /// }
      /// }
      /// }
      /// </summary>
      [JsonProperty("player")]
      public Player player { get; set; }
    
      /// <summary>
      /// Represents any viewer that can access the public Subroutine schema.
      /// </summary>
      [JsonProperty("viewer")]
      [JsonConverter(typeof(CompositionTypeConverter))]
      public Viewer viewer { get; set; }
      #endregion
    }
    #endregion
    
    #region PurchaseReceipt
    public class PurchaseReceipt : TransactionSource {
      #region members
      /// <summary>
      /// The kind used to discriminate the union type
      /// </summary>
      
      TransactionSourceKind TransactionSource.Kind { get { return TransactionSourceKind.PurchaseReceipt; } }
    
      [JsonProperty("productId")]
      public string productId { get; set; }
    
      [JsonProperty("quantity")]
      public int quantity { get; set; }
    
      [JsonProperty("source")]
      public PurchaseSource source { get; set; }
      #endregion
    }
    #endregion
      /// <summary>
      /// PurchaseSource determines which system processed the purchase.
      /// </summary>
    public enum PurchaseSource {
      /// <summary>
      /// Apple App Store.
      /// </summary>
      APPLE,
      /// <summary>
      /// Google Play store.
      /// </summary>
      GOOGLE
    }
    
    
    #region RandomAssetInput
    public class RandomAssetInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public List<RandomDropEntryInput> randomAssetList { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region RandomDropEntryInput
    public class RandomDropEntryInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public ComposableAssetInput asset { get; set; }
    
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string odds { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region RevocableAssetInput
    public class RevocableAssetInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public ComposableAssetInput asset { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region RevokeByTagInput
    /// <summary>
    /// Input object required to revoke all assets with specific tag.
    /// </summary>
    public class RevokeByTagInput {
      #region members
      /// <summary>
      /// Player the assets of which are being revoked.
      /// </summary>
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string playerId { get; set; }
    
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string tag { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
      /// <summary>
      /// Status returned upon attempt to revoke the asset.
      /// </summary>
    public enum RevokeStatus {
      /// <summary>
      /// Asset is not revokable.
      /// </summary>
      NOT_REVOKABLE,
      /// <summary>
      /// Asset has been successfully revoked.
      /// </summary>
      SUCCESSFUL,
      /// <summary>
      /// Asset has already been opened and cannot be revoked.
      /// </summary>
      UNAVAILABLE
    }
    
    
    #region SealedAssetInput
    public class SealedAssetInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public List<ComposableAssetInput> assets { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region TimedAssetInput
    public class TimedAssetInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public ComposableAssetInput asset { get; set; }
    
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string grantedAfter { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region TransactionFilters
    /// <summary>
    /// Filters that can be used to filter results on `offerings` edge on Game
    /// </summary>
    public class TransactionFilters {
      #region members
      /// <summary>
      /// If not None, filters the transaction to specified TransactionType.
      /// </summary>
      public TransactionType? transactionType { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region TransactionLog
    public class TransactionLog {
      #region members
      [JsonProperty("id")]
      public string id { get; set; }
    
      /// <summary>
      /// Based on the Transaction Type it returns a proper object that originated
      /// the transaction.
      /// </summary>
      [JsonProperty("source")]
      [JsonConverter(typeof(CompositionTypeConverter))]
      public TransactionSource source { get; set; }
      #endregion
    }
    #endregion
    
    #region TransactionLogConnection
    public class TransactionLogConnection {
      #region members
      /// <summary>
      /// A list of edges.
      /// </summary>
      [JsonProperty("edges")]
      public List<TransactionLogEdge> edges { get; set; }
    
      /// <summary>
      /// A list of nodes.
      /// </summary>
      [JsonProperty("nodes")]
      public List<TransactionLog> nodes { get; set; }
    
      /// <summary>
      /// Information to aid in pagination.
      /// </summary>
      [JsonProperty("pageInfo")]
      public PageInfo pageInfo { get; set; }
      #endregion
    }
    #endregion
    
    #region TransactionLogEdge
    /// <summary>
    /// An edge in a connection.
    /// </summary>
    public class TransactionLogEdge {
      #region members
      /// <summary>
      /// A cursor for use in pagination
      /// </summary>
      [JsonProperty("cursor")]
      public string cursor { get; set; }
    
      /// <summary>
      /// The item at the end of the edge
      /// </summary>
      [JsonProperty("node")]
      public TransactionLog node { get; set; }
      #endregion
    }
    #endregion
    
          /// <summary>
      /// An enum representing the possible values of TransactionSource
      /// </summary>
    public enum TransactionSourceKind {
      /// <summary>
      /// AdminAssetModificationReceipt
      /// </summary>
      AdminAssetModificationReceipt,
      /// <summary>
      /// AssetInstance
      /// </summary>
      AssetInstance,
      /// <summary>
      /// Offering
      /// </summary>
      Offering,
      /// <summary>
      /// PurchaseReceipt
      /// </summary>
      PurchaseReceipt
    }
    
        
    /// <summary>
    /// TransactionSource is a Union of all the objects that could've originated
    /// a Transaction.
    /// </summary>
    public interface TransactionSource {
      /// <summary>
      /// Kind is used to discriminate by type instances of this interface
      /// </summary>
      TransactionSourceKind Kind { get; }
    }
        
      /// <summary>
      /// Transaction Type represents all options for different types of transactions
      /// that can move assets across the system.
      /// </summary>
    public enum TransactionType {
      /// <summary>
      /// Represents action taken by organization/game admins.
      /// </summary>
      ADMIN_ASSET_MODIFICATION,
      /// <summary>
      /// Represents an opening of a ComposedAsset.
      /// </summary>
      ASSET_OPENING,
      /// <summary>
      /// Represents an asset's move to escrow as part of an Auction creation.
      /// </summary>
      AUCTION,
      /// <summary>
      /// Represents an asset's move to escrow as part of an Auction bid.
      /// </summary>
      AUCTION_BID,
      /// <summary>
      /// Represents consuming (burning) of an asset.
      /// </summary>
      CONSUMED_ASSET,
      /// <summary>
      /// Represents an asset's move to escrow as part of an ExchangeOrder creation, or asset release
      /// on order cancellation.
      /// </summary>
      EXCHANGE_ORDER,
      /// <summary>
      /// Represents an asset's move between parties on exchange's order fill. Fills can be partial
      /// hence single Exchange order may have multiple Exchange order fills.
      /// </summary>
      EXCHANGE_ORDER_FILL,
      /// <summary>
      /// Represents a purchase of a Storefront offering.
      /// </summary>
      OFFERING,
      /// <summary>
      /// Represents a purchase of an InAppProduct.
      /// </summary>
      PRODUCT_PURCHASE
    }
    
    
    #region UniqueProperties
    public class UniqueProperties {
      #region members
      [JsonProperty("isUnique")]
      public bool isUnique { get; set; }
      #endregion
    }
    #endregion
    
    #region UnsealByTagInput
    /// <summary>
    /// Input object required to open all assets with specific tag.
    /// </summary>
    public class UnsealByTagInput {
      #region members
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string tag { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region UnsealResult
    /// <summary>
    /// Represents a result of opening a ComposableAsset.
    /// </summary>
    public class UnsealResult {
      #region members
      [JsonProperty("assets")]
      public List<AssetOutput> assets { get; set; }
    
      /// <summary>
      /// Status of opening a composable asset.
      /// </summary>
      [JsonProperty("status")]
      public UnsealStatus status { get; set; }
      #endregion
    }
    #endregion
      /// <summary>
      /// Status returned upon attempt to open the asset.
      /// </summary>
    public enum UnsealStatus {
      /// <summary>
      /// Asset cannot yet be opened.
      /// </summary>
      IN_PROGRESS,
      /// <summary>
      /// If asset isnt sealed, unsealing it won't do anything
      /// </summary>
      NOT_SEALED,
      /// <summary>
      /// Asset was successfuly opened.
      /// </summary>
      SUCCESS,
      /// <summary>
      /// Asset was opened/revoked.
      /// </summary>
      UNAVAILABLE
    }
    
    
    #region UnwrapByTagInput
    /// <summary>
    /// Input object required to unwrap all assets with specific tag.
    /// </summary>
    public class UnwrapByTagInput {
      #region members
      /// <summary>
      /// Player for which the assets will be unwraped.
      /// Important!
      /// `resolveByTag` can be called both by player and organization.
      /// If you're calling this function from client side as a `Player`
      /// viewer, you must not provide the `player_id`.
      /// </summary>
      public string playerId { get; set; }
    
      [System.ComponentModel.DataAnnotations.Required]
      [JsonRequired]
      public string tag { get; set; }
      #endregion
    
      #region methods
      public dynamic GetInputObject()
      {
        IDictionary<string, object> d = new System.Dynamic.ExpandoObject();
    
        var properties = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
          var value = propertyInfo.GetValue(this);
          var defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
    
          var requiredProp = propertyInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0;
    
          if (requiredProp || value != defaultValue)
          {
            d[propertyInfo.Name] = value;
          }
        }
        return d;
      }
      #endregion
    }
    #endregion
    
    #region UnwrapResult
    public class UnwrapResult {
      #region members
      [JsonProperty("assets")]
      public List<AssetOutput> assets { get; set; }
    
      /// <summary>
      /// Status of opening a composable asset.
      /// </summary>
      [JsonProperty("status")]
      public UnwrapStatus status { get; set; }
      #endregion
    }
    #endregion
      /// <summary>
      /// Status return upon attempt to unwrap the asset.
      /// </summary>
    public enum UnwrapStatus {
      /// <summary>
      /// Asset has been used/revoked.
      /// </summary>
      UNAVAILABLE,
      /// <summary>
      /// Asset is still not resolvable - some asset conditions may still not
      /// have been meet.
      /// </summary>
      UNCHANGED,
      /// <summary>
      /// Asset has been fully unwrapped and has been marked as consumed.
      /// If new assets were generated as part of unwrapping, they are
      /// in the player's inventory.
      /// </summary>
      UNWRAPPED
    }
    
    
          /// <summary>
      /// An enum representing the possible values of Viewer
      /// </summary>
    public enum ViewerKind {
      /// <summary>
      /// Player
      /// </summary>
      Player
    }
    
        
    /// <summary>
    /// This type is a Union of all different types that can act as a viewer
    /// for the public Subroutine schema.
    /// </summary>
    public interface Viewer {
      /// <summary>
      /// Kind is used to discriminate by type instances of this interface
      /// </summary>
      ViewerKind Kind { get; }
    }
        
    
    /// <summary>
    /// Given __typeName returns JToken::ToObject[__typeName]. (via cache to improve performance)
    /// </summary>
    /// <param name="typeName">The __typeName</param>
    /// <returns>JToken::ToObject[__typeName]</returns>
    public static Func<JToken, object> GetToObjectMethodForTargetType(string typeName)
    {
      if (!ToObjectForTypenameCache.ContainsKey(typeName))
      {
        // Get the type corresponding to the typename
        Type targetType = typeof(Types).Assembly
          .GetTypes()
          .ToList()
          .Where(t => t.Name == typeName)
          .FirstOrDefault();
    
        // Create a parametrised ToObject method using targetType as <TypeArgument>
        var method = typeof(JToken).GetMethods()
            .Where(m => m.Name == "ToObject" && m.IsGenericMethod && m.GetParameters().Length == 0).FirstOrDefault();
        var genericMethod = method.MakeGenericMethod(targetType);
        var toObject = (Func<JToken, object>)genericMethod.CreateDelegate(Expression.GetFuncType(typeof(JToken), typeof(object)));
        ToObjectForTypenameCache[typeName] = toObject;
      }
    
      return ToObjectForTypenameCache[typeName];
    }
    
    /// <summary>
    /// Converts an instance of a composition type to the appropriate implementation of the interface
    /// </summary>
    public class CompositionTypeConverter : JsonConverter
    {
      /// <inheritdoc />
      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
      {
        if (reader.TokenType == JsonToken.Null)
        {
          return null;
        }
    
        var loadedObject = JObject.Load(reader);
        var typeNameObject = loadedObject["__typename"];
    
        if (typeNameObject == null)
        {
          throw new JsonSerializationException($"CompositionTypeConverter Exception: missing __typeName field when parsing {objectType.Name}. Requesting the __typename field is required for converting Composition Types");
        }
    
        var typeName = loadedObject["__typename"].Value<string>();
        var toObject = GetToObjectMethodForTargetType(typeName);
        return toObject(loadedObject);
      }
    
      /// <inheritdoc />
      public override bool CanConvert(Type objectType)
      {
        throw new NotImplementedException();
      }
    
      /// <inheritdoc />
      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
        throw new NotImplementedException("Tried to write a GQL Composition type to JSON");
      }
    }
    
    /// <summary>
    /// Converts a list of instances of a composition type to the appropriate implementation of the interface
    /// </summary>
    public class CompositionTypeListConverter : JsonConverter
    {
      /// <inheritdoc />
      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
      {
        if (reader.TokenType == JsonToken.Null)
        {
          return null;
        }
    
        var items = JArray.Load(reader).Children();
        IList list = Activator.CreateInstance(objectType) as IList;
    
        foreach (var item in items)
        {
          var typeNameObject = item["__typename"];
    
          if (typeNameObject == null)
          {
            throw new JsonSerializationException($"CompositionTypeListConverter Exception: missing __typeName field when parsing {objectType.Name}. Requesting the __typename field is required for converting Composition Types");
          }
    
          var typeName = item["__typename"].Value<string>();
          var toObject = GetToObjectMethodForTargetType(typeName);
          object objectParsed = toObject(item);
    
          list.Add(objectParsed);
        }
    
        return list;
      }
    
      /// <inheritdoc />
      public override bool CanConvert(Type objectType)
      {
        throw new NotImplementedException();
      }
    
      /// <inheritdoc />
      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
        throw new NotImplementedException("Tried to write a GQL Composition type list to JSON");
      }
    }
      
  }
  
}
