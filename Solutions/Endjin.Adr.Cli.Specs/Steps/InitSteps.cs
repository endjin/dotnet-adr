namespace Corvus.Configuration.Specs.Steps;
using Endjin.Adr.Cli;
using NUnit.Framework;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

[Binding]
public class InitSteps
{
    private readonly ScenarioContext scenarioContext;

    public InitSteps(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }

    [Given("I ask the adr cli to initialise a new repo in the '(.*)' directory")]
    public void GivenIAskTheAdrCliToInitialiseANewRepoInTheDirectory(string directory)
    {
        this.scenarioContext.Set(directory, "Directory");
    }

    [When("I execute the adr cli")]
    public async Task WhenIExecuteTheAdrCli()
    {
        string[] args = new string[] { "init",  this.scenarioContext.Get<string>("Directory") };

        int result = await Program.Main(args).ConfigureAwait(false);

        this.scenarioContext.Set(result, "Result");
    }

    [Then(@"a new ADR repository has been created with an initial readme\.md file")]
    public void ThenANewADRRepositoryHasBeenCreatedWithAnInitialReadme_MdFile()
    {
        Assert.AreEqual(0, this.scenarioContext.Get<int>("Result"));
    }
}