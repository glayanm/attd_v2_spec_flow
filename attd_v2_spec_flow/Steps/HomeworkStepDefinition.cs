using NUnit.Framework;

namespace attd_v2_spec_flow.Steps;

[Binding]
public class HomeworkStepDefinition
{
    [When(@"環境測試")]
    public void When環境測試()
    {
        Assert.Pass();
    }
}