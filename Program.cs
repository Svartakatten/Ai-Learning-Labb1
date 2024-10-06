using Azure;
using Azure.AI.Language.QuestionAnswering;
using Azure.AI.TextAnalytics;

namespace Ai_Learning
{
    internal class Program
    {
        // This example requires environment variables named "LANGUAGE_KEY" and "LANGUAGE_ENDPOINT" for Azure AI services
        private static readonly Uri languageEndpoint = new Uri("https://aiazurelanguageservices.cognitiveservices.azure.com/");
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("ed015d1e91fa4fceb6aec06f1491c157");
        private static readonly TextAnalyticsClient languageClient = new TextAnalyticsClient(languageEndpoint, credentials);

        // This example requires environment variables named "LANGUAGE_KEY" and "LANGUAGE_ENDPOINT" for Language Services
        private static readonly Uri qnaEndpoint = new Uri("https://ailanguageservicemodels.cognitiveservices.azure.com/");
        private static readonly AzureKeyCredential credential = new AzureKeyCredential("fedd6f16b8524774a45b85361b968620");

        static void Main(string[] args)
        {
            string projectName = "LearnFAQ";
            string deploymentName = "production";

            QuestionAnsweringClient client = new QuestionAnsweringClient(qnaEndpoint, credential);
            QuestionAnsweringProject project = new QuestionAnsweringProject(projectName, deploymentName);

            Console.WriteLine("Ask a question, type exit to quit.");

            while (true)
            {
                string question = Console.ReadLine();

                if(question.ToLower() == "exit")
                {
                    break;
                }
                try
                {
                    // NLP (Sentiment Analysis)
                    DocumentSentiment sentiment = languageClient.AnalyzeSentiment(question);
                    Console.WriteLine($"Sentiment: {sentiment.Sentiment}");

                    // QnA (Fetching an Answer)
                    Console.WriteLine("Searching for an answer...");
                    Response<AnswersResult> response = client.GetAnswers(question, project);

                    foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                    {
                        Console.WriteLine($"Q:{question}");
                        Console.WriteLine($"A:{answer.Answer}");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Request Error: {ex.Message}");
                }
            }
        }
    }
}
