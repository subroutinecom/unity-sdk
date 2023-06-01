
// API Class provides helpers for accessing Subroutine API, but is not required to use.
// You can continue using SubroutineExecutor directly to execute more tailored queries.
using System;
using System.Collections.Generic;

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
        First = 1,
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
      /// Number of results that should be fetched in the connection.
      public int First { get; set; }
      /// Cursor for item after which the fetching should resume.
      public string After { get; set; } = null;
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
  $first: Int!
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
                        {"first", props.First},
                        {"after", props.After},
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
      public string AssetDefinitionId { get; set; }
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
        assetDefinitionId = props.AssetDefinitionId,
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
query GetPriceSpreadForItem($exchangeId: NodeRef!, $assetDefinitionId: AssetRef!){
  player {
    game{
      exchange(exchangeId: $exchangeId) {
        priceSpreadForAsset(assetDefinitionId:$assetDefinitionId) {
          ask
          bid
        }
      }
    }
  }
}",
            Variables = new Dictionary<string, object> {
                        {"exchangeId", props.Exchange},
                        {"assetDefinitionId", props.ItemId},
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

    public class GetOrdersProps : BaseQueryProps
    {
      public int First { get; set; }
      public string After { get; set; } = null;

      /// Use filters to decide which types of orders you'd wanna fetch.
      /// Allows to filter to specific exchange or specific asset definition type.
      /// Allows filtering to specific order types. If order types aren't defined,
      /// by default OPEN and ACKNOWLEDGED orders will be fetched.
      ///
      /// If you'd like to fetch the past orders, you can do so by setting Filters.status to 
      /// {GraphQLCodeGen.Types.OrderStatus.CLOSED, GraphQLCodeGen.Types.OrderStatus.CANCELLED}
      public GraphQLCodeGen.Types.ExchangeOrderFilters Filters { get; set; } = null;
    }
    public class OrdersConnectionResponse : SubroutineResponse
    {
      public GraphQLCodeGen.Types.ExchangeOrderConnection OrderConnection { get; set; }
    }
    public void GetOrders(GetOrdersProps props, Action<OrdersConnectionResponse> callback)
    {
      GraphQLCodeGen.Types.ExchangeOrderFilters filters = null;
      if (props.Filters != null)
      {
        filters = props.Filters;
      }
      else
      {
        filters = new GraphQLCodeGen.Types.ExchangeOrderFilters
        {
          status = { GraphQLCodeGen.Types.OrderStatus.ACKNOWLEDGED, GraphQLCodeGen.Types.OrderStatus.OPEN },
        };
      }

      if (filters.status == null || filters.status.Count < 1)
      {
        filters.status = new List<GraphQLCodeGen.Types.OrderStatus> { GraphQLCodeGen.Types.OrderStatus.ACKNOWLEDGED, GraphQLCodeGen.Types.OrderStatus.OPEN };
      }

      Executor.Query(
          new GraphQLQuery
          {
            OperationName = "GetOrders",
            Query = OrderFragment + @"

query GetOrders(
  $filters: ExchangeOrderFilters!,
  $first: Int!
  $after: String
) {
  player {
    orders(
      filters: $filters,
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
                        {"first", props.First},
                        {"after", props.After},
                        {"filters", filters},
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

  }
}