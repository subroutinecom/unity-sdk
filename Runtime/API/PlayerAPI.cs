
using System;
using System.Collections.Generic;

namespace Subroutine.API
{

  public interface IAssetHoldingInput
  {
    GraphQLCodeGen.Types.AssetHoldingInput convertIntoGraphQLType();
  }

  public abstract class AssetHoldingInput
  {
    public class Fungible : IAssetHoldingInput
    {
      public string AssetDefinitionId { get; }
      public string Quantity { get; }

      public Fungible(string assetDefinitionId, string quantity)
      {
        AssetDefinitionId = assetDefinitionId;
        Quantity = quantity;
      }

      public GraphQLCodeGen.Types.AssetHoldingInput convertIntoGraphQLType()
      {
        return new GraphQLCodeGen.Types.AssetHoldingInput
        {
          fungible = new GraphQLCodeGen.Types.AssetFungibleHoldingInput
          {
            assetDefinitionId = AssetDefinitionId,
            quantity = Quantity
          }
        };
      }
    }

    public class Instance : IAssetHoldingInput
    {
      public string AssetInstanceId { get; }

      public Instance(string assetInstanceId)
      {
        AssetInstanceId = assetInstanceId;
      }

      public GraphQLCodeGen.Types.AssetHoldingInput convertIntoGraphQLType()
      {
        return new GraphQLCodeGen.Types.AssetHoldingInput
        {
          instance = AssetInstanceId
        };
      }
    }

  }

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

    /// Use to fetch asset holding for a specific Asset Definition.
    public void GetAssetHolding(GetAssetHoldingProps props, Action<GetAssetHoldingResponse> callback)
    {
      Executor.Query(new GraphQLQuery
      {
        Query = GameAPI.AssetDefinitionFragment + @"

query PlayerAssetHolding($assetDefinitionId: AssetRef!) {
  player {
    assetHolding(assetDefinitionId: $assetDefinitionId) {
      assetDefinition {
        ...AssetDefinitionFragment
      }
      quantity
    }
  }
}
",
        Variables = new Dictionary<string, object> {
          {"assetDefinitionId", props.AssetDefinitionId},
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

    public class GetAssetHoldingsProps : BaseQueryProps
    {
      public GraphQLCodeGen.Types.AssetDefinitionConnectionFilters Filters { get; set; }
      public int First { get; set; } = 100;
      public string After { get; set; } = null;

      public GetAssetHoldingsProps()
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

    public class GetAssetHoldingsResponse : SubroutineResponse
    {
      public List<GraphQLCodeGen.Types.AssetHoldingEdge> Holdings { get; set; }
    }

    /// Use that to fetch asset holdings across multiple asset definitions. Allows
    /// for filtering.
    public void GetAssetHoldings(GetAssetHoldingsProps props, Action<GetAssetHoldingsResponse> callback)
    {
      Executor.Query(new GraphQLQuery
      {
        Query = GameAPI.AssetDefinitionFragment + @"

query PlayerInventory($first: Int!, $after: String, $assetHoldingFilters: AssetDefinitionConnectionFilters!) {
  player {
    assetHoldings(first: $first, after: $after, filters: $assetHoldingFilters) {
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
          {"first", props.First},
          {"after", props.After},
        }

      },
      props.CacheConfig,
      (exception, response) =>
      {
        callback(new GetAssetHoldingsResponse { Error = exception, Holdings = response.data.player.assetHoldings.edges });
      });
    }

    public class ConsumeAssetProps : BaseQueryProps
    {
      public IAssetHoldingInput Asset { get; set; }
    }

    public class ConsumeAssetResponse : SubroutineResponse
    {
      public string TransactionId { get; set; }
    }

    public void ConsumeAsset(ConsumeAssetProps props, Action<ConsumeAssetResponse> callback)
    {
      String idempotencyToken = System.Guid.NewGuid().ToString();

      GraphQLCodeGen.Types.AssetHoldingInput asset = props.Asset.convertIntoGraphQLType();

      Executor.Mutation(new GraphQLQuery
      {
        Query = @"
mutation ConsumeAsset($asset: AssetHoldingInput!, $idempotencyToken: String!) {
  consumeAsset(input: { asset: $asset, idempotencyToken: $idempotencyToken }) {
    id
  }
}
",
        Variables = new Dictionary<string, object>
        {
          {"asset", asset},
          {"idempotencyToken", idempotencyToken},
        }

      },
            (exception, response) =>
            {
              callback(new ConsumeAssetResponse { Error = exception, TransactionId = response.data.consumeAsset.id });
            });
    }

  }
}