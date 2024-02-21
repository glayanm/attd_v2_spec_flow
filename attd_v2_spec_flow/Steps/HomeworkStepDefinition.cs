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
    private readonly MyDbContext _db;
    private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
    private HttpClient? _client;
    private string? _googleResult;
    private string? _token;
    private string? _userName;

    public HomeworkStepDefinition(ISpecFlowOutputHelper specFlowOutputHelper)
    {
        _specFlowOutputHelper = specFlowOutputHelper;
        _db = new MyDbContext();
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        _webDriver = null;
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
            if (!_db.Users.Any(p => p.UserName == userName))
            {
                _db.Users.Add(new User { UserName = userName, Password = password });
                _db.SaveChanges();
            }

            _userName = userName;
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

    [AfterScenario]
    public void AfterScenario()
    {
        var removeUsers = _db.Users.Where(p => p.UserName == _userName);
        _db.Users.RemoveRange(removeUsers);
        _db.SaveChanges();

        _specFlowOutputHelper.WriteLine("Data Remove");

        GetWebDriver().Quit();
        _specFlowOutputHelper.WriteLine("Web driver quit");
    }

    [When(@"在谷歌搜索关键字""(.*)""")]
    public void When在谷歌搜索关键字(string cucumber)
    {
        try
        {
            var webDriver = GetWebDriver();

            webDriver.Navigate().GoToUrl("https://www.google.com");
            Thread.Sleep(2000);

            var webElement = webDriver.FindElement(By.Id("APjFqb"));
            webElement.SendKeys("Cucumber");
            webElement.Submit();

            Thread.Sleep(2000);
            var resultStats = webDriver.FindElement(By.Id("result-stats"));
            _googleResult = resultStats.Text;

            Thread.Sleep(500);
            // webDriver.Quit();
        }
        catch (Exception e)
        {
            _specFlowOutputHelper.WriteLine(e.Message);
            throw;
        }
    }

    [Then(@"打印谷歌为您找到的相关结果数")]
    public void Then打印谷歌为您找到的相关结果数()
    {
        _specFlowOutputHelper.WriteLine(_googleResult);
        Assert.IsNotEmpty(_googleResult);
    }
}