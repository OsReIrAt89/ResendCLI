using Resend;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http; 

class Program
{
    private const string ApiKey = "INPUT YOU API KEY";
    private const string FromEmail = "NAME@YOUDOMAIN.TLD"; 
//在此输入你的APIKEY和已绑定域名
    static async Task Main(string[] args)
    {
        Console.WriteLine("--- Resend 邮件发送程序 ---");
        Console.WriteLine("请按提示输入邮件信息。");
        Console.WriteLine("-----------------------------");

        Console.Write("请输入收件人邮箱：");
        string toEmail = Console.ReadLine() ?? string.Empty;

        Console.Write("请输入邮件主题：");
        string subject = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("请输入邮件内容（输入单独一行 'END' 并回车结束）：");
        string content = ReadMultilineContent();

        if (string.IsNullOrWhiteSpace(toEmail) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(content))
        {
            Console.WriteLine("错误：收件人、主题或内容不能为空。请重新运行程序。");
            return;
        }

        Console.WriteLine("-----------------------------");
        Console.WriteLine("正在发送邮件...");

        await SendEmailAsync(toEmail, subject, content);

        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }

    
    private static string ReadMultilineContent()
    {
        var contentBuilder = new StringBuilder();
        string? line;
        while ((line = Console.ReadLine()) != "END")
        {
            contentBuilder.AppendLine(line);
        }
        return contentBuilder.ToString();
    }

    private static async Task SendEmailAsync(string to, string subject, string content)
    {
        try
        {
            IResend resend = ResendClient.Create(ApiKey);

            var email = new EmailMessage()
            {
                From = FromEmail,
                To = to,
                Subject = subject,
                TextBody = content
            };

            
            var sendResponse = await resend.EmailSendAsync(email);

            if (sendResponse != null && sendResponse.Content != Guid.Empty)
            {
                
                Console.WriteLine($"邮件发送成功！邮件 ID: {sendResponse.Content.ToString()}");
            }
            else
            {
                Console.WriteLine("邮件发送失败，但未返回邮件 ID。");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"邮件发送失败，出现错误：{ex.Message}");
        }
    }
}
