using IFS.ApiTests.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IFS.ApiTests.TestData
{
    public static class TestDataLoader
    {
        private static JObject Load(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "TestData", fileName);
            var json = File.ReadAllText(path);
            return JObject.Parse(json);
        }

        public static class Posts
        {
            private static readonly JObject _data = Load("PostTestData.json");

            public static int[] ValidPostIds =>
                _data["validPostIds"]!.ToObject<int[]>()!;

            public static int[] InvalidPostIds =>
                _data["invalidPostIds"]!.ToObject<int[]>()!;

            public static Post ValidNewPost =>
                _data["validNewPost"]!.ToObject<Post>()!;

            public static Post UpdatedPost =>
                _data["updatedPost"]!.ToObject<Post>()!;

            public static Post EmptyTitlePost =>
                _data["emptyTitlePost"]!.ToObject<Post>()!;
        }

        public static class Users
        {
            private static readonly JObject _data = Load("UserTestData.json");

            public static int[] ValidUserIds =>
                _data["validUserIds"]!.ToObject<int[]>()!;

            public static int[] InvalidUserIds =>
                _data["invalidUserIds"]!.ToObject<int[]>()!;
        }
    }
}