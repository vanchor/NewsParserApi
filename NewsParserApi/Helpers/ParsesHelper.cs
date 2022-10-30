namespace NewsParserApi.Helpers
{
    public static class ParsesHelper
    {
        public static async Task<string> CallUrl(string fullUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5 (.NET CLR 3.5.30729)");
                var response = await client.GetStringAsync(fullUrl);
                return response;
            }
        }
    }
}
