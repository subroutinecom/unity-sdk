
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Subroutine.API
{
  public class BuyOfferingProps
  {
    public string OfferingId { get; set; }
  }

  public class GetOfferingsByTagProps : BaseQueryProps
  {
    public int Count { get; set; }
    public string WithTag { get; set; } = null;
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
    public void GetOfferingsByTag(GetOfferingsByTagProps props, Action<GetOfferingsResponse> callback)
    {
      Executor.Query(new GraphQLQuery
      {
        Query = @"
query GetOfferings($offeringTag: Tag, $count: Int!) {
  player {
    __typename
    id

    game {
      __typename
      id

      offerings(first: $count, filters: { tag: $offeringTag }) {
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
          {"offeringTag", props.WithTag},
          {"count", props.Count},
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
      String transactionId = System.Guid.NewGuid().ToString();
      Executor.Mutation(new GraphQLQuery
      {

        Query = @"
mutation BuyOffering($offerId: NodeRef!, $transactionId: String!) {
  buyOffering(
    input: { offerId: $offerId, clientTransactionId: $transactionId }
  ) {
    id
  }
}
",
        Variables = new Dictionary<string, object> {
                                    {"offerId", props.OfferingId},
                                    {"transactionId", transactionId}
                                }
      }, (exception, response) =>
      {
        callback(new BuyOfferingResponse { Error = exception, Offering = response.data.buyOffering });
      });

    }
  }
}