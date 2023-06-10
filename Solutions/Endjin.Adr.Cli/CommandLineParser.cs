// <copyright file="CommandLineParser.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

/*
namespace Endjin.Adr.Cli
{
  using System.Threading.Tasks;
  using Endjin.Adr.Cli.Configuration.Contracts;
  using Microsoft.Extensions.DependencyInjection;

  public class CommandLineParser
  {
    private readonly IAppEnvironment appEnvironment;
    private readonly IAppEnvironmentManager appEnvironmentManager;
    private readonly ITemplateSettingsManager templateSettingsManager;
    private readonly IServiceCollection services;

    public CommandLineParser(
      IAppEnvironment appEnvironment,
      IAppEnvironmentManager appEnvironmentManager,
      ITemplateSettingsManager templateSettingsManager,
      IServiceCollection services)
    {
      this.services = services;
      this.appEnvironment = appEnvironment;
      this.appEnvironmentManager = appEnvironmentManager;
      this.templateSettingsManager = templateSettingsManager;
    }

    public delegate Task EnvironmentInit(string path);

    public delegate Task NewAdr(string title, int? id, ITemplateSettingsManager templateSettingsManager);

    public Parser Create(EnvironmentInit environmentInit = null, NewAdr newAdr = null)
    {
      // Set up intrinsic commands that will always be available.
      RootCommand rootCommand = new()
      {
        Name = "adr",
        Description = "Architectural Decision Records for .NET",
      };

      rootCommand.AddCommand(Init());
      rootCommand.AddCommand(Environment());
      rootCommand.AddCommand(New());
      rootCommand.AddCommand(Templates());

      var commandBuilder = new CommandLineBuilder(rootCommand);

      return commandBuilder.UseDefaults().Build();

      Command New()
      {
        var cmd = new Command("new", "Creates a new Architectural Decision Record, from the default ADR Template.");
        var option = new Option<int?>("--id", "Id of ADR to supersede.");
        var arg = new Argument<string>("title") { Description = "Title of the ADR" };

        cmd.Add(option);
        cmd.Add(arg);

        cmd.SetHandler(async (context) =>
        {
          string title = context.ParseResult.GetValueForArgument(arg);
          int? id = context.ParseResult.GetValueForOption(option);
          await newAdr(title, id, this.templateSettingsManager);
        });

        return cmd;
      }

      Command Init()
      {
        var cmd = new Command("init", "Initialises a new ADR repository.");
        var arg = new Argument<string>("path")
        {
          Arity = ArgumentArity.ZeroOrOne,
        };

        cmd.Add(arg);

        cmd.SetHandler(async (context) =>
        {
          string path = context.ParseResult.GetValueForArgument(arg);
          await environmentInit(path).ConfigureAwait(false);
        });

        return cmd;
      }

      Command Templates()
      {
        var cmd = new Command("templates", "Perform operations on ADR templates.")
        {
          Default(),
          Set(),
          List(),
          Packages(),
          Update(),
        };

        return cmd;

        Command Update()
        {
          var cmd = new Command("update", "Updates to the latest version of the ADR Templates Package.");
          return cmd;
        }

        Command Packages()
        {
          var cmd = new Command("package", "Operations that can be performed against the NuGet ADR Template package.");

          cmd.Add(Set());
          cmd.Add(Show());

          return cmd;

          Command Show()
          {
            var cmd = new Command("show", "Shows the default NuGet ADR Template package.");

            return cmd;
          }

          Command Set()
          {
            var cmd = new Command("set", "Sets the default NuGet ADR Template package.");
            cmd.AddArgument(new Argument<string>("packageId"));

            return cmd;
          }
        }

        Command List()
        {
          var cmd = new Command("list", "Lists all installed ADR Templates.");
          var option = new Option<string>("--ids-only");

          cmd.Add(option);

          return cmd;
        }

        Command Set()
        {
          var cmd = new Command("set", "Sets the default ADR Template.");

          cmd.AddArgument(new Argument<string>("templateId"));

          return cmd;
        }

        Command Default()
        {
          var cmd = new Command("default", "Operations that can be performed against the default ADR templates.");

          cmd.Add(Show());

          return cmd;
        }

        Command Show()
        {
          var cmd = new Command("show", "Shows the default ADR Template.");

          // cmd.SetHandler(async (context) => { });
          return cmd;
        }
      }

      Command Environment()
      {
        var cmd = new Command("environment", "Manipulate the dotnet-adr environment.") { Reset() };

        return cmd;

        Command Reset()
        {
          var resetCmd = new Command("reset", "Resets the dotnet-adr environment back to its default settings.");

          resetCmd.SetHandler(async (context) => { await this.appEnvironmentManager.ResetDesiredStateAsync().ConfigureAwait(false); });

          return resetCmd;
        }
      }
    }
  }
}*/