using System;
using UnityEngine;
using UnityEngine.Networking;

// API Class provides helpers for accessing Overchain API, but is not required to use.
// You can continue using OverchainClient directly to execute more tailored queries.
namespace Subroutine.API
{

  public class Client
  {

    public Client(
      MonoBehaviour coroutineRunner,
      string graphQLUrl,
      string authUrl,
      string organizationName,
      string gameName,
      string apiToken,
      string overchainNamespace,
      CacheConfig cacheConfig,
      ICache cache,
      bool debug = false)
    {
      Executor = new Core.Executor(coroutineRunner, graphQLUrl, authUrl, organizationName, gameName, apiToken, overchainNamespace, cacheConfig, cache, debug);
      Exchange = new ExchangeAPI(Executor);
      Player = new PlayerAPI(Executor);
      Authentication = new AuthenticationAPI(coroutineRunner, Executor);
      Storefront = new StorefrontAPI(Executor);
      Game = new GameAPI(Executor);
      IsDebug = debug;
    }

    public Core.Executor Executor { get; private set; }
    public ExchangeAPI Exchange { get; private set; }
    public PlayerAPI Player { get; private set; }
    public AuthenticationAPI Authentication { get; private set; }
    public StorefrontAPI Storefront { get; private set; }
    public GameAPI Game { get; private set; }

    // If set to true, provides detailed logging.
    public bool IsDebug { get; private set; }
  }
}