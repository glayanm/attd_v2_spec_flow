using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace attd_v2_spec_flow.Steps;

[Binding]
public class HomeworkStepDefinition
{
    private static RemoteWebDriver? _webDriver;


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
}