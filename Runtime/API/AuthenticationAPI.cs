
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.X509;
using UnityEngine;
using UnityEngine.Networking;

namespace Subroutine.API
{

  public class PublicKeyInfo
  {
    [JsonProperty("base64_encoded_public_key")]
    public string Base64EncodedPublicKey { get; set; }
  }

  public class KeyBasedAuthentication
  {
    [JsonProperty("device_key_signature")]
    public PublicKeyInfo PublicKeyInfo { get; set; }
  }

  public class LoginRequest
  {
    [JsonProperty("auth")]
    public KeyBasedAuthentication Auth { get; set; }

    [JsonProperty("timestamp")]
    public int Timestamp
    {
      get
      {
        return (int)DateTimeOffset.Now.ToUnixTimeSeconds();
      }
    }

    public byte[] GetBytes()
    {
      return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore
      }));
    }
  }

  public class AuthData
  {
    [JsonProperty("public_key")]
    public string PublicKey { get; set; }
  }

  public class RegisterRequest
  {
    [JsonProperty("game_id")]
    public string GameId { get; set; }

    [JsonProperty("api_token")]
    public string ApiToken { get; set; }

    [JsonProperty("auth")]
    public AuthData Auth { get; set; }

    [JsonProperty("timestamp")]
    public int Timestamp
    {
      get
      {
        return (int)DateTimeOffset.Now.ToUnixTimeSeconds();
      }
    }

    public byte[] GetBytes()
    {
      return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore
      }));
    }
  }

  public class RegisterResponseShape
  {
    [JsonProperty("player_id")]
    public string PlayerId { get; set; }
  }

  public class KeypairPem
  {
    public string Public { get; set; }
    public string Private { get; set; }
  }

  public class AuthenticationAPI
  {
    private readonly Core.Executor Executor;
    private readonly MonoBehaviour CoroutineRunner;

    public class LoginPlayerProps
    {
      public string PublicKey { get; set; }
      public string PrivateKey { get; set; }
    }

    public class LoginPlayerResponse : SubroutineResponse
    {
      public string JWTToken { get; set; }
    }

    public class RegisterPlayerProps { }

    public class RegisterPlayerResponse : SubroutineResponse
    {
      public string PrivateKey { get; set; }
      public string PublicKey { get; set; }
      public string PlayerId { get; set; }
    }

    internal AuthenticationAPI(MonoBehaviour coroutineRunner, Core.Executor executor)
    {
      Executor = executor;
      CoroutineRunner = coroutineRunner;
    }


    public void LoginPlayer(LoginPlayerProps props, Action<LoginPlayerResponse> callback)
    {
      CoroutineRunner.StartCoroutine(LoginPlayerInternal(new LoginPlayerInternalProps
      {
        PublicKey = props.PublicKey,
        PrivateKey = props.PrivateKey
      }, callback));
    }

    public void LogoutPlayer()
    {
      Executor.ResetJWTToken();
    }

    public void RegisterPlayer(RegisterPlayerProps props, Action<RegisterPlayerResponse> callback)
    {
      CoroutineRunner.StartCoroutine(InternalGetIdAndRegisterPlayer(new InternalGetIdAndRegisterPlayerProps { }, callback));
    }

    public class RegisterPlayerInternalProps
    {
      public string GameId { get; set; }
    }


    private IEnumerator RegisterPlayerInternal(RegisterPlayerInternalProps props, Action<RegisterPlayerResponse> callback)
    {
      var keypair = GenerateKeyPair();
      var registerRequest = new RegisterRequest
      {
        Auth = new AuthData
        {
          PublicKey = keypair.Public
        },
        GameId = props.GameId,
        ApiToken = Executor.ApiToken,
      };

      var registerRequestBytes = registerRequest.GetBytes();

      var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(new StringReader(keypair.Private));
      var KeyParameter = (Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair)pemReader.ReadObject();
      var signer = SignerUtilities.GetSigner("SHA256withRSA");
      signer.Init(true, KeyParameter.Private);
      signer.BlockUpdate(registerRequestBytes, 0, registerRequestBytes.Length);
      var signResult = signer.GenerateSignature();
      var encodedResult = System.Convert.ToBase64String(signResult);

      var www = new UnityWebRequest(String.Format("{0}/user/register", Executor.AuthUrl), "POST")
      {
        uploadHandler = (UploadHandler)new UploadHandlerRaw(registerRequestBytes),
        downloadHandler = (DownloadHandler)new DownloadHandlerBuffer()
      };
      www.SetRequestHeader("Content-Type", "application/json");
      www.SetRequestHeader("X-Subroutine-Signature", encodedResult);
      if (Executor.Namespace != null)
      {
        www.SetRequestHeader("X-Subroutine-Namespace", Executor.Namespace);
      }

      yield return www.SendWebRequest();

      byte[] results = www.downloadHandler.data;
      string responseBody = Encoding.UTF8.GetString(results);

      if (www.result != UnityWebRequest.Result.Success)
      {
        callback(new RegisterPlayerResponse
        {
          Error = new HttpException(String.Format("{0} {1}", responseBody, www.error))
        });
        yield break;
      }
      else
      {
        var response = JsonConvert.DeserializeObject<RegisterResponseShape>(responseBody);
        callback(new RegisterPlayerResponse
        {
          PrivateKey = keypair.Private,
          PublicKey = keypair.Public,
          PlayerId = response.PlayerId
        });
      }
      www.Dispose();
    }

    public class LoginPlayerInternalProps
    {
      public string PublicKey { get; set; }
      public string PrivateKey { get; set; }
    }

    private IEnumerator LoginPlayerInternal(LoginPlayerInternalProps props, Action<LoginPlayerResponse> callback)
    {
      var loginRequest = new LoginRequest
      {
        Auth =
        new KeyBasedAuthentication
        {
          PublicKeyInfo =

         new PublicKeyInfo
         {
           Base64EncodedPublicKey = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(props.PublicKey))
         }
        }
      };

      var loginRequestBytes = loginRequest.GetBytes();

      var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(new StringReader(props.PrivateKey));
      var KeyParameter = (Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair)pemReader.ReadObject();
      var signer = SignerUtilities.GetSigner("SHA256withRSA");
      signer.Init(true, KeyParameter.Private);
      signer.BlockUpdate(loginRequestBytes, 0, loginRequestBytes.Length);
      var signResult = signer.GenerateSignature();
      var encodedResult = System.Convert.ToBase64String(signResult);

      var www = new UnityWebRequest(String.Format("{0}/user/authenticate", Executor.AuthUrl), "POST")
      {
        uploadHandler = (UploadHandler)new UploadHandlerRaw(loginRequestBytes),
        downloadHandler = (DownloadHandler)new DownloadHandlerBuffer()
      };
      www.SetRequestHeader("Content-Type", "application/json");
      www.SetRequestHeader("X-Subroutine-Signature", encodedResult);
      if (Executor.Namespace != null)
      {
        www.SetRequestHeader("X-Subroutine-Namespace", Executor.Namespace);
      }

      yield return www.SendWebRequest();

      byte[] results = www.downloadHandler.data;
      string responseBody = Encoding.UTF8.GetString(results);

      if (www.result != UnityWebRequest.Result.Success)
      {
        callback(new LoginPlayerResponse
        {
          Error = new HttpException(www.error),
          JWTToken = responseBody
        });
        yield break;
      }
      else
      {
        Executor.JWTToken = responseBody;
        callback(new LoginPlayerResponse
        {
          JWTToken = responseBody
        });
      }
      www.Dispose();
    }

    public class InternalGetIdAndRegisterPlayerProps { }

    private IEnumerator InternalGetIdAndRegisterPlayer(InternalGetIdAndRegisterPlayerProps props, Action<RegisterPlayerResponse> callback)
    {
      String fetchedGameId = null;
      yield return GetGameId((gameId) =>
      {
        fetchedGameId = gameId;
      });

      if (fetchedGameId == null)
      {
        Debug.LogError("Fetched Game ID failed");
      }

      yield return RegisterPlayerInternal(new RegisterPlayerInternalProps { GameId = fetchedGameId }, callback);
    }

    private IEnumerator GetGameId(Action<String> callback)
    {
      var vars = new Dictionary<string, object>
      {
        { "organizationName", Executor.OrganizationName },
        { "gameName", Executor.GameName }
      };
      var query = new GraphQLQuery
      {
        OperationName = "GetGameId",
        Query = @"
query GetGameId($organizationName: String!, $gameName: String!){
  getGameId(organizationName:$organizationName, gameName:$gameName)
}
                ",
        Variables = vars
      };

      return Executor.QueryInternal(
        query,
        // rely on default cache configuration
        cacheConfig: null,
        (e, resp) =>
        {
          if (e == null)
          {
            callback(resp.data.getGameId);
          }
        });
    }



    byte[] GeneratePrivatePem(AsymmetricKeyParameter priv)
    {
      PrivateKeyInfo pkInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(priv);
      var encodable = pkInfo.ParsePrivateKey();
      var primitive = encodable.ToAsn1Object();
      return primitive.GetEncoded();
    }

    byte[] GeneratePublicKey(AsymmetricKeyParameter pub)
    {
      var spkInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pub);
      var primitive = spkInfo.ParsePublicKey();
      return primitive.GetEncoded();
    }

    string ConvertKeyToPem(string title, byte[] bytes)
    {
      var sb = new StringBuilder();
      var pemObject = new PemObject(title, bytes);
      var stringWriter = new StringWriter(sb);
      var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(stringWriter);
      pemWriter.WriteObject(pemObject);
      return sb.ToString();
    }

    public KeypairPem GenerateKeyPair()
    {
      var gen = new Org.BouncyCastle.Crypto.Generators.RsaKeyPairGenerator();
      gen.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
      var keypair = gen.GenerateKeyPair();

      return new KeypairPem
      {
        Public = ConvertKeyToPem("RSA PUBLIC KEY", GeneratePublicKey(keypair.Public)),
        Private = ConvertKeyToPem("RSA PRIVATE KEY", GeneratePrivatePem(keypair.Private))
      };
    }


  }
}