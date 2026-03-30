using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;

namespace IFS.ApiTests.Helpers
{
    public static class SchemaValidator
    {
        private static JSchema Load(string schemaFileName)
        {
            var path = Path.Combine(
                AppContext.BaseDirectory, "TestData", "Schemas", schemaFileName);

            var schemaJson = File.ReadAllText(path);
            return JSchema.Parse(schemaJson);
        }

        public static void Validate(string responseContent, string schemaFileName)
        {
            var schema = Load(schemaFileName);
            var token = JToken.Parse(responseContent);

            // handle both single object and array
            var itemsToValidate = token.Type == JTokenType.Array
                ? token.Children()
                : new[] { token }.AsEnumerable<JToken>();

            var errors = new List<string>();

            foreach (var item in itemsToValidate)
            {
                if (item is JObject obj)
                {
                    obj.IsValid(schema, out IList<string> itemErrors);
                    errors.AddRange(itemErrors);
                }
            }

            Assert.That(errors, Is.Empty,
                $"Schema validation failed for {schemaFileName}:\n{string.Join("\n", errors)}");
        }
    }
}