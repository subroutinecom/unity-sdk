
using System;
using System.Collections.Generic;

namespace Subroutine.API
{
  public class BuyOfferingProps
  {
    public string OfferingId { get; set; }
  }

  public class GetOfferingsProps : BaseQueryProps
  {
    public int First { get; set; }
    public string After { get; set; } = null;

    public GraphQLCodeGen.Types.OfferingFiltersInput Filters { get; set; } = null;
  }

  public class StorefrontAPI
  {
    private readonly Core.Executor Executor;
    internal StorefrontAPI(Core.Executor executor)
    {
      Executor = executor;
    }

    public class GetOfferingsResponse : SubroutineResponse
    {
      public GraphQLCodeGen.Types.OfferingConnection OfferingsConnection;
    }

    /// Fetches all offerings with tag, tag is optional. Returns connection.
    public void GetOfferings(GetOfferingsProps props, Action<GetOfferingsResponse> callback)
    {
      Executor.Query(new GraphQLQuery
      {
        Query = @"
query GetOfferings($filters: OfferingFiltersInput, $first: Int!, $after: String) {
  player {
    __typename
    id

    game {
      __typename
      id

      offerings(first: $first, after: $after, filters: $filters) {
        edges {
          cursor
          node {
            __typename

            id
            name
            maxCapReached
            hasRequiredInputs
          }
        }
      }
    }
  }
}
        ",
        Variables = new Dictionary<string, object> {
          {"filters", props.Filters},
          {"first", props.First},
          {"after", props.After},
        }
      }, props.CacheConfig, (exception, response) =>
      {
        callback(new GetOfferingsResponse { Error = exception, OfferingsConnection = response.data.player.game.offerings });
      });
    }

    public class BuyOfferingResponse : SubroutineResponse
    {
      public GraphQLCodeGen.Types.Offering Offering;
    }

    public void BuyOffering(BuyOfferingProps props, Action<BuyOfferingResponse> callback)
    {
      String idempotencyToken = System.Guid.NewGuid().ToString();
      Executor.Mutation(new GraphQLQuery
      {

        Query = @"
mutation BuyOffering($offerId: NodeRef!, $idempotencyToken: String!) {
  buyOffering(
    input: { offerId: $offerId, idempotencyToken: $idempotencyToken }
  ) {
    id
  }
}
",
        Variables = new Dictionary<string, object> {
                                    {"offerId", props.OfferingId},
                                    {"idempotencyToken", idempotencyToken}
                                }
      }, (exception, response) =>
      {
        callback(new BuyOfferingResponse { Error = exception, Offering = response.data.buyOffering });
      });

    }
  }
}