using System.Net.Http.Headers;
using System.Net.Http.Json;
using attd_v2_spec_flow.Context;
using attd_v2_spec_flow.Entities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow.Infrastructure;

namespace attd_v2_spec_flow.Steps;

[Binding]
public class HomeworkStepDefinition
{
    private const string BaseUrl = "http://localhost:10081/";
    private static RemoteWebDriver? _webDriver;
    private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
    private HttpClient? _client;
    private string? _token;

    public HomeworkStepDefinition(ISpecFlowOutputHelper specFlowOutputHelper)
    {
        _specFlowOutputHelper = specFlowOutputHelper;
    }


    [When(@"環境測試")]
    public void When環境測試()
    {
        var webDriver = GetWebDriver();
        webDriver.Navigate().GoToUrl("http://host.docker.internal:10081/");
        var findElement = webDriver.FindElements(By.XPath("//*[text()='登录']"));
        Assert.IsNotEmpty(findElement);
        webDriver.Quit();
    }

    private WebDriver GetWebDriver()
    {
        if (_webDriver == null) CreateWebDriver();

        return _webDriver!;
    }

    private static void CreateWebDriver()
    {
        var chromeOptions = new ChromeOptions();
        _webDriver = new RemoteWebDriver(new Uri("http://web-driver.tool.net:4444"), chromeOptions);
    }

    [Given(@"存在用户名为""(.*)""和密码为""(.*)""的用户")]
    public void Given存在用户名为和密码为的用户(string userName, string password)
    {
        try
        {
            var db = new MyDbContext();
            if (!db.Users.Any(p => p.UserName == userName))
            {
                db.Users.Add(new User { UserName = userName, Password = password });
                db.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [When(@"通过API以用户名为""(.*)""和密码为""(.*)""登录时")]
    public void When通过api以用户名为和密码为登录时(string userName, string password)
    {
        var client = GetHttpClient();
        var postModel = new { userName, password };
        var response = client.PostAsJsonAsync("users/login", postModel).Result;

        if (response.IsSuccessStatusCode) _token = response.Headers.GetValues("token").FirstOrDefault();
    }

    private HttpClient GetHttpClient()
    {
        if (_client != null) return _client;

        _client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return _client;
    }

    [Then(@"打印Token")]
    public void Then打印Token()
    {
        _specFlowOutputHelper.WriteLine(_token);
        Assert.IsNotEmpty(_token);
    }
}