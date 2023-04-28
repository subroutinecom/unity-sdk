using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Subroutine.API
{

  namespace Core
  {
    public class QueryVariable
    {
      public string Name { get; set; }
      public string Type { get; set; }
    }

    public class QueryFragment
    {
      public string Name { get; set; }
      public string Query { get; set; }
      public List<QueryVariable> Variables { get; set; }
    }
  }

  public class QueryBuilder
  {

    String QueryName { get; set; }
    Dictionary<string, object> Variables { get; set; }
    List<Core.QueryFragment> Fragments { get; set; }

    String FragmentQueries
    {
      get
      {
        return String.Join("\n", Fragments.Select(fragment =>
        {
          return $@"
                    fragment {fragment.Name} on Player {{
                        {fragment.Query}
                    }}
                    ";
        }));
      }
    }

    String FragmentIncludes
    {
      get
      {
        return String.Join("\n", Fragments.Select(fragment => $"...{fragment.Name}"));
      }
    }

    String FragmentVariables
    {
      get
      {
        List<string> allVars = new();
        foreach (var fragment in Fragments)
        {
          if (fragment.Variables != null)
          {
            foreach (var varType in fragment.Variables)
            {
              allVars.Add(String.Format("${0}:{1}", varType.Name, varType.Type));
            }
          }
        }
        return String.Join(", ", allVars);
      }
    }

    public QueryBuilder(string name)
    {
      QueryName = name;
      Variables = new Dictionary<string, object>();
      Fragments = new List<Core.QueryFragment>();
    }

    public GraphQLQuery Build()
    {
      var vars = FragmentVariables;
      var queryVars = vars.Length > 0 ? String.Format("({0})", vars) : "";

      return new GraphQLQuery
      {
        Query = String.Format(@"
                {0}

                query {1}{2} {{
                    player {{
                        __typename
                        id

                        {3}
                    }}
                }}
                ", FragmentQueries, QueryName, queryVars, FragmentIncludes),
        Variables = Variables
      };
    }
  }

  public class GraphQLQuery
  {
    [JsonIgnore]
    public bool SuppressGraphQLErrors { get; set; } = false;
    [JsonProperty("operationName")]
    public string OperationName { get; set; }
    [JsonProperty("variables")]
    public Dictionary<string, object> Variables { get; set; }
    [JsonProperty("query")]
    public string Query { get; set; }

    public static QueryBuilder Create(string name)
    {
      return new QueryBuilder(name);
    }

    public string Serialize()
    {
      var jsonString = JsonConvert.SerializeObject(this, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore,
        Converters = new List<Newtonsoft.Json.JsonConverter>() {
                    new Newtonsoft.Json.Converters.StringEnumConverter()
                }
      });
      return jsonString;
    }


  }
}
