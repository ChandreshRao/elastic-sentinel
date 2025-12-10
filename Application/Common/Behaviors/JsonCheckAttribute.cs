using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace ElasticSentinel.Application.Common.Behaviors
{
    public class JsonCheckAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value != null)
            {
                try
                {
                    string? val = value as string;
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        var obj = JsonConvert.DeserializeObject<dynamic>(val);
                    }
                }
                catch (Exception)
                {
                    return false;
                }

            }
            return true;
        }
    }
}
