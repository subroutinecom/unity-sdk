using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Subroutine.API
{
  public delegate void OnQueryDelegate(Exception e, Core.QueryShape response);
  public delegate void OnMutationDelegate(Exception e, Core.MutationShape response);

  delegate void OnReceivingGameId(String gameId);

  public class HttpException : Exception
  {
    public HttpException(string message) : base(message) { }
    public HttpException(string message, Exception inner) : base(message, inner) { }
  }

  public class GraphQLException : Exception
  {
    public GraphQLException(string message) : base(message) { }
    public GraphQLException(string message, Exception inner) : base(message, inner) { }
  }
  public abstract class SubroutineResponse
  {
    public Exception Error { get; set; } = null;
  }

  public enum CachePolicy
  {
    CacheWithTTL,
    NoCache,
  }

  public class CacheConfig
  {
    public CachePolicy Policy = CachePolicy.NoCache;
    public int TimeToLiveSeconds = 0;
  }

  namespace Core
  {

    public class QueryError
    {
      public string message;
    }

    public class QueryShape
    {
      public GraphQLCodeGen.Types.PublicQuery data;
      public List<QueryError> errors;
    }

    public class MutationShape
    {
      public GraphQLCodeGen.Types.PublicMutation data;
      public List<QueryError> errors;
    }

    public class Executor
    {
      private MonoBehaviour CoroutineRunner { get; set; }

      private string PlayerAPIUrl { get; set; }
      internal string AuthUrl { get; set; }

      internal string ApiToken { get; set; }
      internal string InternalNamespace { get; set; }
      internal bool IsDebug { get; set; }

      public string JWTToken { get; private set; } = "";
      public bool TestPlayerOverride { get; set; } = false;

      public CacheConfig CacheConfig { get; set; }

      private Subroutine.API.ICache Cache { get; set; }

      internal Executor(ClientOptions options)
      {
        CoroutineRunner = options.CoroutineRunner;
        PlayerAPIUrl = options.PlayerApiUrl;
        AuthUrl = options.AuthenticationUrl;
        ApiToken = options.GameAPIToken;
        InternalNamespace = options.InternalSubroutineNamespace;
        CacheConfig = options.CacheConfig;
        Cache = options.Cache;
        IsDebug = options.Debug;
      }

      public bool IsLoggedIn
      {
        get
        {
          return JWTToken != "";
        }
      }

      public void Query(GraphQLQuery query, CacheConfig cacheConfig, OnQueryDelegate callback)
      {
        CoroutineRunner.StartCoroutine(this.QueryInternal(query, cacheConfig, callback));
      }

      private byte[] GetBytesForQuery(string jsonString)
      {
        Regex regex = new("[\\s\\t]+");

        return Encoding.UTF8.GetBytes(regex.Replace(jsonString, " "));
      }

      internal IEnumerator QueryInternal(GraphQLQuery query, CacheConfig cacheConfig, OnQueryDelegate callback)
      {

        string querySerialized = query.Serialize();
        if (IsDebug)
        {
          Debug.LogFormat("Query request: {0}", querySerialized);
        }

        // See if we can return cached value based on TTL value
        var cacheConfigToUse = cacheConfig ?? CacheConfig;
        if (cacheConfigToUse.Policy == CachePolicy.CacheWithTTL)
        {
          var cachedValue = Cache.Read(querySerialized, cacheConfigToUse.TimeToLiveSeconds);
          if (cachedValue != null)
          {
            callback(null, cachedValue);
          }
        }

        var www = new UnityWebRequest(PlayerAPIUrl, "POST")
        {
          uploadHandler = (UploadHandler)new UploadHandlerRaw(GetBytesForQuery(querySerialized)),
          downloadHandler = (DownloadHandler)new DownloadHandlerBuffer()
        };
        www.SetRequestHeader("Content-Type", "application/json");
        if (InternalNamespace != null)
        {
          www.SetRequestHeader("X-Subroutine-Namespace", InternalNamespace);
        }

        if (JWTToken != "")
        {
          www.SetRequestHeader("Authorization", String.Format("Bearer {0}", JWTToken));
        }


        yield return www.SendWebRequest();


        byte[] results = www.downloadHandler.data;
        string responseBody = Encoding.UTF8.GetString(results);

        if (www.result != UnityWebRequest.Result.Success)
        {
          callback(new HttpException(www.error), null);
        }
        else
        {
          Exception responseError = null;
          if (IsDebug)
          {
            Debug.LogFormat("Query response: {0}", responseBody);
          }
          var response = JsonConvert.DeserializeObject<QueryShape>(responseBody);
          if (!query.SuppressGraphQLErrors && response.errors != null && response.errors.Count > 0)
          {
            responseError = new GraphQLException(response.errors[0].message);
          }
          if (response.errors == null || response.errors.Count == 0)
          {
            Cache.Write(querySerialized, response);
          }
          callback(responseError, response);
        }
        www.Dispose();
      }

      public void Mutation(GraphQLQuery query, OnMutationDelegate callback)
      {
        CoroutineRunner.StartCoroutine(this.MutationInternal(query, callback));
      }

      private IEnumerator MutationInternal(GraphQLQuery query, OnMutationDelegate callback)
      {
        var www = new UnityWebRequest(PlayerAPIUrl, "POST");
        string querySerialized = query.Serialize();
        if (IsDebug)
        {
          Debug.LogFormat("Mutation request: {0}", querySerialized);
        }

        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(GetBytesForQuery(querySerialized));
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        if (InternalNamespace != null)
        {
          www.SetRequestHeader("X-Subroutine-Namespace", InternalNamespace);
        }

        if (JWTToken != "")
        {
          www.SetRequestHeader("Authorization", String.Format("Bearer {0}", JWTToken));
        }

        yield return www.SendWebRequest();

        byte[] results = www.downloadHandler.data;
        string responseBody = Encoding.UTF8.GetString(results);

        if (www.result != UnityWebRequest.Result.Success)
        {

          callback(new HttpException(www.error), null);
        }
        else
        {

          Exception responseError = null;
          if (IsDebug)
          {
            Debug.LogFormat("Mutation response: {0}", responseBody);
          }
          var response = JsonConvert.DeserializeObject<MutationShape>(responseBody);
          if (!query.SuppressGraphQLErrors && response.errors != null && response.errors.Count > 0)
          {
            responseError = new GraphQLException(response.errors[0].message);
          }
          callback(responseError, response);
        }
        www.Dispose();
      }

      public void ResetJWTToken()
      {
        JWTToken = "";
        TestPlayerOverride = false;
      }

      public void UpdateJWTToken(String newToken)
      {
        if (TestPlayerOverride)
        {
          Debug.LogWarning("Cannot update JWT token with test player override in place.");
        }
        else
        {
          JWTToken = newToken;
        }
      }

    }

  }
}