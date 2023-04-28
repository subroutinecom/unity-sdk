
using System;
using System.Collections.Generic;

namespace Subroutine.API
{
  public class GameAPI
  {
    private readonly Core.Executor Executor;

    internal GameAPI(Core.Executor executor)
    {
      Executor = executor;
    }

    internal static readonly string AssetDefinitionFragment = @"
fragment AssetDefinitionFragment on AssetDefinition {
  id
  developerAssetId
  name
  description
  cap
  tags
  isConsumable
  isTradable
  asFungible {
    decimalPlaces
  }
}
";

    public class GetAssetDefinitionProps : BaseQueryProps
    {
      public string AssetId { get; set; }
    }
    public class GetAssetDefinitionResponse : SubroutineResponse
    {
      public GraphQLCodeGen.Types.AssetDefinition Asset { get; set; }
    }
    public void GetAssetDefinition(GetAssetDefinitionProps props, Action<GetAssetDefinitionResponse> callback)
    {
      Executor.Query(
        new GraphQLQuery
        {
          OperationName = "GetAssetDefinition",
          Query = AssetDefinitionFragment + @"
          
query GetAssetDefinition($assetId: AssetRef!) {
  player {
    game {
      assetDefinition(assetId: $assetId) {
        ...AssetDefinitionFragment
      }
    }
  }
}
",
          Variables = new Dictionary<string, object> {
            {"assetId", props.AssetId}
          }
        },
        cacheConfig: props.CacheConfig,
        (exception, response) =>
        {
          callback(new GetAssetDefinitionResponse
          {
            Error = exception,
            Asset = response?.data?.player?.game?.assetDefinition
          });
        }
      );

    }

    public class GetAssetDefinitionsProps : BaseQueryProps
    {
      public int First { get; set; }
      public string After { get; set; } = null;
      public string WithTag { get; set; } = null;
      public GraphQLCodeGen.Types.AssetType[] AssetTypes { get; set; } = {
        GraphQLCodeGen.Types.AssetType.COMPOSABLE_ASSET, GraphQLCodeGen.Types.AssetType.FUNGIBLE_ASSET, GraphQLCodeGen.Types.AssetType.UNIQUE_ASSET
      };
    }

    public class GetAssetDefinitionsResponse : SubroutineResponse
    {
      public GraphQLCodeGen.Types.AssetDefinitionConnection Assets { get; set; }
    }
    public void GetAssetDefinitions(GetAssetDefinitionsProps props, Action<GetAssetDefinitionsResponse> callback)
    {
      Executor.Query(
        new GraphQLQuery
        {
          OperationName = "GetAssetDefinitions",
          Query = AssetDefinitionFragment + @"

query GetAssetDefinitions($first: Int!, $after: String, $assetTypes: [AssetType!]!, $tag: Tag!) {
  player {
    game {
      assetDefinitions(first:$first, after:$after, filters:{
        tag: $tag,
        assetTypes: $assetTypes,
      }) {
        edges {
          node {
            ...AssetDefinitionFragment
          }
          cursor
        }
      }
    }
  }
}",
          Variables = new Dictionary<string, object> {
  {"first", props.First},
  {"after", props.After},
  {"tag", props.WithTag},
  {"assetTypes", props.AssetTypes},
}
        },
        cacheConfig: props.CacheConfig,
        (exception, response) =>
        {
          callback(new GetAssetDefinitionsResponse
          {
            Error = exception,
            Assets = response?.data?.player?.game?.assetDefinitions
          });
        }
      );
    }

  }
}