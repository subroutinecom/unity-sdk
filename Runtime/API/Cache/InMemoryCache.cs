

using System;
using System.Collections.Generic;

namespace Subroutine.API
{
  public class InMemoryCache : ICache
  {
    private readonly Dictionary<string, (DateTime, Core.QueryShape)> Cache = new();

    public Core.QueryShape Read(string querySerialized, int timeToLiveSeconds)
    {
      var val = Cache.GetValueOrDefault(querySerialized, (DateTime.UnixEpoch, null));
      if (val.Item1 < DateTime.Now.AddSeconds(-timeToLiveSeconds))
      {
        return null;
      }
      return val.Item2;
    }

    public void Write(string querySerialized, Core.QueryShape response)
    {
      Cache[querySerialized] = (DateTime.Now, response);
    }
  }
}