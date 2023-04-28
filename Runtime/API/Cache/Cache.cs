// Interface describes a naive cache that caches entire queries + parameters
// together. Caller is responsible for any connection merges/deduplication.
// Normalization is not supported.

namespace Subroutine.API
{
  public interface ICache
  {
    public void Write(string querySerialized, Core.QueryShape response);

    public Core.QueryShape Read(string querySerialized, int timeToLiveSeconds);
  }
}