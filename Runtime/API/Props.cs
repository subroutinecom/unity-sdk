namespace Subroutine.API
{
  // Base class for props for queries, allows overriding common configs like cache.
  public class BaseQueryProps
  {
    public CacheConfig CacheConfig { get; set; }
  }
}