namespace Corvus.Configuration.Specs.Steps
{
    using Endjin.Adr.Cli;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using TechTalk.SpecFlow;

    [Binding]
    public class InitSteps
    {
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;

        public InitSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
        }

        [Given(@"I ask the adr cli to initialise a new repo in the '(.*)' directory")]
        public void GivenIAskTheAdrCliToInitialiseANewRepoInTheDirectory(string directory)
        {
            this.scenarioContext.Set(directory, "Directory");
        }
        
        [When(@"I execute the adr cli")]
        public void WhenIExecuteTheAdrCli()
        {
            var args = new string[] { "--init",  this.scenarioContext.Get<string>("Directory") };
            
            var result = AdrCli.Main(args);

            this.scenarioContext.Set(result, "Result");
        }
        
        [Then(@"a new ADR repository has been created with an initial readme\.md file")]
        public void ThenANewADRRepositoryHasBeenCreatedWithAnInitialReadme_MdFile()
        {
            Assert.AreEqual(0, this.scenarioContext.Get<int>("Result"));
        }
    }
}
