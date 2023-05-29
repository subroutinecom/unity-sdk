using System;
using UnityEngine;
using UnityEngine.Networking;

// API Class provides helpers for accessing Subroutine API, but is not required to use.
// You can continue using SubroutineClient directly to execute more tailored queries.
namespace Subroutine.API
{

  public class ClientOptions
  {
    public MonoBehaviour CoroutineRunner { get; set; }
    public string PlayerApiUrl { get; set; } = "https://player.api.subroutine.com";
    public string AuthenticationUrl { get; set; } = "https://auth.api.subroutine.com";
    public string AdminApiUrl { get; set; } = "https://admin.api.subroutine.com";

    public string GameAPIToken { get; set; }

    public string InternalSubroutineNamespace { get; set; } = "";

    public bool Debug { get; set; } = false;

    /// TestUserAuthToken can be provided to override the Authentication API. When present, all requests
    /// will be authenticated using this token.
    public string TestPlayerAuthToken { get; set; }

    public CacheConfig CacheConfig { get; set; } = new CacheConfig
    {
      Policy = CachePolicy.CacheWithTTL,
      TimeToLiveSeconds = 60,
    };

    public ICache Cache { get; set; } = new InMemoryCache();
  }

  public class Client
  {

    public Client(ClientOptions options)
    {
      if (options.GameAPIToken == null)
      {
        throw new Exception("GameAPIToken must be provided");
      }
      Executor = new Core.Executor(options);
      if (options.TestPlayerAuthToken != null && options.TestPlayerAuthToken.Length > 0)
      {
        Executor.UpdateJWTToken(options.TestPlayerAuthToken);
        Executor.TestPlayerOverride = true;
      }
      Exchange = new ExchangeAPI(Executor);
      Player = new PlayerAPI(Executor);
      Authentication = new AuthenticationAPI(options.CoroutineRunner, Executor);
      Storefront = new StorefrontAPI(Executor);
      Game = new GameAPI(Executor);
      IsDebug = options.Debug;
      TestPlayerAuthToken = options.TestPlayerAuthToken;
    }

    public Core.Executor Executor { get; private set; }
    public ExchangeAPI Exchange { get; private set; }
    public PlayerAPI Player { get; private set; }
    public AuthenticationAPI Authentication { get; private set; }
    public StorefrontAPI Storefront { get; private set; }
    public GameAPI Game { get; private set; }

    // If set to true, provides detailed logging.
    public bool IsDebug { get; private set; }

    private string TestPlayerAuthToken { get; set; }
  }
}