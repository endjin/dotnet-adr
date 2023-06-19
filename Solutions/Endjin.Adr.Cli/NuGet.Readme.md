# dotnet adr - Make Future You Thank Past You.

A cross platform .NET Global Tool for [creating and managing Architectural Decision Records (ADR)](https://github.com/endjin/dotnet-adr/).

## TLDR;

Architectural Decision Records (ADRs) are simple Markdown documents used to record technical choices for a project by summarizing the context, the decision, and the consequences. dotnet `adr` is a tool and a bundle of the most common ADR templates you can use to create and maintain ADRs in your solution. 

Install using:

`dotnet tool install -g adr`

Install the default ADR templates using:

`adr templates package set adr.templates`

`adr templates package install`

Create a new ADR using:

`adr new <TITLE>`

## More Information

An extensive readme is available in the [GitHub repository](https://github.com/endjin/dotnet-adr/).