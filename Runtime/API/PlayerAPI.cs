
using System;
using System.Collections.Generic;

namespace Subroutine.API
{
  public class PlayerAPI
  {

    private readonly Core.Executor Executor;

    internal PlayerAPI(Core.Executor executor)
    {
      Executor = executor;
    }

    public class GetAssetHoldingResponse : SubroutineResponse
    {
      public GraphQLCodeGen.Types.AssetHolding Holding { get; set; }
    }

    public class GetAssetHoldingProps : BaseQueryProps
    {
      public string AssetDefinitionId { get; set; }
    }
    /// Fetches single fungible asset holding.
    public void GetAssetHolding(GetAssetHoldingProps props, Action<GetAssetHoldingResponse> callback)
    {
      Executor.Query(new GraphQLQuery
      {
        Query = GameAPI.AssetDefinitionFragment + @"

query PlayerAssetHolding($assetId: AssetRef!) {
  player {
    assetHolding(assetId: $assetId) {
      assetDefinition {
        ...AssetDefinitionFragment
      }
      quantity
    }
  }
}
",
        Variables = new Dictionary<string, object> {
          {"assetId", props.AssetDefinitionId},
        }
      },
      props.CacheConfig,
      (exception, response) =>
      {
        callback(new GetAssetHoldingResponse
        {
          Error = exception,
          Holding = response.data.player.assetHolding
        });
      });
    }

    public class GetInventoryProps : BaseQueryProps
    {
      public GraphQLCodeGen.Types.AssetDefinitionConnectionFilters Filters { get; set; }

      public GetInventoryProps()
      {
        Filters = new GraphQLCodeGen.Types.AssetDefinitionConnectionFilters
        {
          tag = null,
          assetTypes = new List<GraphQLCodeGen.Types.AssetType>
            {
                GraphQLCodeGen.Types.AssetType.UNIQUE_ASSET,
                GraphQLCodeGen.Types.AssetType.COMPOSABLE_ASSET,
                GraphQLCodeGen.Types.AssetType.FUNGIBLE_ASSET
            }
        };
      }
    }

    public class GetInventoryResponse : SubroutineResponse
    {
      public List<GraphQLCodeGen.Types.AssetHoldingEdge> Holdings { get; set; }
    }

    /// Returns all fungible assets that belong to the player
    public void GetInventory(GetInventoryProps props, Action<GetInventoryResponse> callback)
    {
      Executor.Query(new GraphQLQuery
      {
        Query = GameAPI.AssetDefinitionFragment + @"

query PlayerInventory($assetHoldingFilters: AssetDefinitionConnectionFilters!) {
  player {
    assetHoldings(first: 100, filters: $assetHoldingFilters) {
      edges {
        node {
          assetDefinition {
            ...AssetDefinitionFragment
          }
          quantity
        }
      }
    }
  }
}
",
        Variables = new Dictionary<string, object> {
          {"assetHoldingFilters", props.Filters},
        }

      },
      props.CacheConfig,
      (exception, response) =>
      {
        callback(new GetInventoryResponse { Error = exception, Holdings = response.data.player.assetHoldings.edges });
      });
    }

  }
}