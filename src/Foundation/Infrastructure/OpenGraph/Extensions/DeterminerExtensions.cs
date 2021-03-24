using Boilerplate.Web.Mvc.OpenGraph;

namespace Foundation.Infrastructure.OpenGraph.Extensions
{
    public static class DeterminerExtensions
    {
        public static string ToLowercaseString(this OpenGraphDeterminer determiner)
        {
            switch (determiner)
            {
                case OpenGraphDeterminer.A: return "a";
                case OpenGraphDeterminer.An: return "an";
                case OpenGraphDeterminer.Auto: return "auto";
                case OpenGraphDeterminer.The: return "the";
                default: return string.Empty;
            }
        }
    }
}