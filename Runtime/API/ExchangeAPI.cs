
// API Class provides helpers for accessing Overchain API, but is not required to use.
// You can continue using OverchainExecutor directly to execute more tailored queries.
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Subroutine.API
{
  public class GetDefaultExchangeProps : BaseQueryProps { }


  public class ExchangeAPI
  {
    private readonly Core.Executor Executor;

    internal ExchangeAPI(Core.Executor executor)
    {
      Executor = executor;
    }

    public class GetExchangeResponse : SubroutineResponse
    {
      public GraphQLCodeGen.Types.Exchange Exchange { get; set; }
    }

    public class GetExchangesResponse : SubroutineResponse
    {
      public GraphQLCodeGen.Types.ExchangeConnection ExchangeConnection { get; set; }
    }

    public void GetDefaultExchange(GetDefaultExchangeProps props, Action<GetExchangeResponse> callback)
    {
      GetExchanges(new GetExchangesProps
      {
        Count = 1,
        CacheConfig = props.CacheConfig,
      }, (response) =>
      {
        if (response.ExchangeConnection.edges?.Count == 0)
        {
          callback(new GetExchangeResponse
          {
            Error = new Exception("No default exchange")
          });
        }
        else
        {
          callback(new GetExchangeResponse
          {
            Exchange = response.ExchangeConnection.edges?[0].node
          });
        }
      });
    }

    private readonly string OrderFragment = @"
fragment OrderFragment on ExchangeOrder {
  id
  status
  expiration
  originalQuantity
  remainingQuantity
  price
  assetDefinition {
    id
    developerAssetId
    name
    description
  }
}
";

    public class GetExchangesProps : BaseQueryProps
    {
      public Exception Error { get; set; }
      public int Count { get; set; }
      public string AfterCursor { get; set; } = null;
      public string NameFilter { get; set; } = null;
      public string WithTag { get; set; } = null;
    }

    /// Returns a connection over exchanges - filters allow filtering by name/tag.
    /// If your game has just one exchange, you can call `GetDefaultExchange` instead
    /// which is a helper function for `GetExchanges`
    public void GetExchanges(GetExchangesProps props, Action<GetExchangesResponse> callback)
    {
      Executor.Query(
          new GraphQLQuery
          {
            OperationName = "GetExchanges",
            Query = @"
query GetExchanges(
  $count: Int!
  $after: String
  $nameFilter: String
  $tag: Tag
) {
  player {
    game {
      exchanges(
        first: $count
        after: $after
        filters: { nameContains: $nameFilter, tag: $tag }
      ) {
        edges {
          cursor
          node {
            id
            name
            currency {
              id
              name
              developerAssetId
            }
            supportedOrderTypes
          }
        }
      }
    }
  }
}",
            Variables = new Dictionary<string, object> {
                        {"count", props.Count},
                        {"afterCursor", props.AfterCursor},
                        {"nameFilter", props.NameFilter},
                        {"tag", props.WithTag}
              },
          },
          props.CacheConfig,
          (exception, response) =>
          {
            callback(new GetExchangesResponse
            {
              Error = exception,
              ExchangeConnection = response?.data?.player?.game?.exchanges
            });
          }
      );
    }

    public class CreateOrderResponse : SubroutineResponse
    {
      public GraphQLCodeGen.Types.ExchangeOrder Order { get; set; }
    }

    public class CreateOrderProps
    {
      public string Exchange { get; set; }
      public string ItemId { get; set; }
      public GraphQLCodeGen.Types.OrderSide OrderSide { get; set; }
      public GraphQLCodeGen.Types.OrderTypeInput OrderType { get; set; }
      public GraphQLCodeGen.Types.OrderExpiration Expiration { get; set; }
      // price as decimal number
      public string Price { get; set; }
      // quantity as decimal number
      public string Quantity { get; set; }
    }

    public void CreateOrder(
        CreateOrderProps props,
        Action<CreateOrderResponse> callback
    )
    {
      var input = new GraphQLCodeGen.Types.CreateOrderInput
      {
        assetId = props.ItemId,
        quantity = props.Quantity,
        exchangeId = props.Exchange,
        orderType = props.OrderType,
        side = props.OrderSide,
        price = props.Price,
        expiration = props.Expiration,
        idempotencyToken = System.Guid.NewGuid().ToString(),
      };
      Executor.Mutation(
          new GraphQLQuery
          {
            Query = OrderFragment + @"
        
mutation CreateExchangeOrder($input: CreateOrderInput!) {
  createExchangeOrder(input: $input){ 
    ...OrderFragment
  }
}                    
",
            Variables = new Dictionary<string, object> {
                        {"input", input}
              }
          },
          (exception, response) =>
          {
            callback(new CreateOrderResponse
            {
              Error = exception,
              Order = response?.data?.createExchangeOrder
            });
          }
      );
    }

    public class CancelOrderProps
    {
      public string OrderId { get; set; }
    }

    public class CancelOrderResponse : SubroutineResponse
    {
      public bool Result { get; set; }
    }
    public void CancelOrder(CancelOrderProps props, Action<CancelOrderResponse> callback)
    {
      Executor.Mutation(
          new GraphQLQuery
          {
            OperationName = "CancelOrder",
            Query = @"
mutation CancelOrder($orderId: NodeRef!) {
  cancelExchangeOrder(input:{
    orderId:$orderId
  })
}",
            Variables = new Dictionary<string, object> {
    {"orderId", props.OrderId}
}
          },
          (exception, response) =>
          {
            callback(new CancelOrderResponse
            {
              Error = exception,
              Result = response.data.cancelExchangeOrder
            });
          }
      );
    }

    public class GetOrderProps : BaseQueryProps
    {
      public string OrderId { get; set; }
    }
    public class GetOrderResponse : SubroutineResponse
    {
      public GraphQLCodeGen.Types.ExchangeOrder Order { get; set; }
    }
    public void GetOrder(GetOrderProps props, Action<GetOrderResponse> callback)
    {
      Executor.Query(
          new GraphQLQuery
          {
            OperationName = "GetOrder",
            Query = OrderFragment + @"

query GetOrder($orderId: NodeRef!) {
  player {
    order(id:$orderId) {
      ...OrderFragment
    }
  }
}
                    ",
            Variables = new Dictionary<string, object> {
                        {"orderId", props.OrderId},
              }
          },
          cacheConfig: props.CacheConfig ?? new CacheConfig { Policy = CachePolicy.NoCache },
          (exception, response) =>
          {
            callback(new GetOrderResponse
            {
              Error = exception,
              Order = response?.data?.player?.order
            });
          }
      );
    }

    public class GetPriceSpreadForItemProps : BaseQueryProps
    {
      public string Exchange { get; set; }
      public string ItemId { get; set; }
    }

    public class GetPriceSpreadForItemResponse : SubroutineResponse
    {
      public GraphQLCodeGen.Types.ExchangeAssetPriceSpread Spread { get; set; }
    }
    public void GetPriceSpreadForItem(GetPriceSpreadForItemProps props, Action<GetPriceSpreadForItemResponse> callback)
    {
      Executor.Query(
          new GraphQLQuery
          {
            OperationName = "GetPriceSpreadForItem",
            Query = @"
query GetPriceSpreadForItem($exchangeId: NodeRef!, $assetId: AssetRef!){
  player {
    game{
      exchange(exchangeId: $exchangeId) {
        priceSpreadForAsset(assetId:$assetId) {
          ask
          bid
        }
      }
    }
  }
}",
            Variables = new Dictionary<string, object> {
                        {"exchangeId", props.Exchange},
                        {"assetId", props.ItemId},
              }
          },
          cacheConfig: props.CacheConfig ?? new CacheConfig { Policy = CachePolicy.NoCache },
          (exception, response) =>
          {
            callback(new GetPriceSpreadForItemResponse
            {
              Error = exception,
              Spread = response?.data?.player?.game?.exchange?.priceSpreadForAsset
            });
          }
      );
    }

    public class OpenOrdersProps : BaseQueryProps
    {
      public string Exchange { get; set; }
      public int Count { get; set; }
      public string ForAsset { get; set; } = null;
      public string After { get; set; } = null;
    }
    public class OrdersConnectionResponse : SubroutineResponse
    {
      public GraphQLCodeGen.Types.ExchangeOrderConnection OrderConnection { get; set; }
    }
    public void OpenOrders(OpenOrdersProps props, Action<OrdersConnectionResponse> callback)
    {
      Executor.Query(
          new GraphQLQuery
          {
            OperationName = "OpenOrders",
            Query = OrderFragment + @"

query OpenOrders(
  $exchange: NodeRef!
  $first: Int!
  $after: String
  $assetId: AssetRef
) {
  player {
    orders(
      filters: {
        exchangeId: $exchange
        status: [OPEN, ACKNOWLEDGED]
        assetId: $assetId
      }
      first: $first
      after: $after
    ) {
      edges {
        cursor
        node {
          ...OrderFragment
        }
      }
    }
  }
}
",
            Variables = new Dictionary<string, object> {
                        {"exchange", props.Exchange},
                        {"first", props.Count},
                        {"after", props.After},
                        {"assetId", props.ForAsset},
              }
          },
          // Fetching order status should by default not use any cache
          cacheConfig: props.CacheConfig ?? new CacheConfig { Policy = CachePolicy.NoCache },
          (exception, response) =>
          {
            callback(new OrdersConnectionResponse
            {
              Error = exception,
              OrderConnection = response?.data?.player?.orders
            });
          }
      );
    }

    public class ClosedOrdersProps : BaseQueryProps
    {
      public string Exchange { get; set; }
      public int Count { get; set; }
      public string After { get; set; } = null;
    }

    public void ClosedOrders(ClosedOrdersProps props, Action<OrdersConnectionResponse> callback)
    {
      Executor.Query(
          new GraphQLQuery
          {
            OperationName = "ClosedOrders",
            Query = OrderFragment + @"
{{0}}

query ClosedOrders($exchange: NodeRef!, $first: Int!, $after: String) {
  player {
    orders(
      filters: { exchangeId: $exchange, status: [CLOSED, CANCELLED] }
      first: $first
      after: $after
    ) {
      edges {
        cursor
        node {
          ...OrderFragment
        }
      }
    }
  }
}
",
            Variables = new Dictionary<string, object> {
                        {"exchange", props.Exchange},
                        {"first", props.Count},
                        {"after", props.After},
              }
          },
          cacheConfig: props.CacheConfig ?? new CacheConfig { Policy = CachePolicy.NoCache },
          (exception, response) =>
          {
            callback(new OrdersConnectionResponse
            {
              Error = exception,
              OrderConnection = response?.data?.player?.orders
            });
          }
      );
    }
  }
}