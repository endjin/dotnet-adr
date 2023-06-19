# dotnet adr - Make Future You Thank Past You.

A cross platform .NET Global Tool for [creating and managing Architectural Decision Records (ADR)](https://github.com/endjin/dotnet-adr/).

## TLDR;

Architectural Decision Records (ADRs) are simple Markdown documents used to record technical choices for a project by summarizing the context, the decision, and the consequences. dotnet `adr` is a tool and a bundle of the most common ADR templates you can use to create and maintain ADRs in your solution. 

## ADR Templates and ADR Template Packages

### Which ADR templates are available out of the box?

We have collected a number of popular ADR templates.

> NOTE: the status of the Open Source License for some of the templates is unclear. See each template for more details.

#### Alexandrian Pattern
ADR using the Alexandrian [Pattern Language Approach](https://en.wikipedia.org/wiki/Pattern_language) coined by Architect Christopher Alexander et. al in 1977, which distils the decision record into the following headings:

- Prologue (Summary)
- Discussion (Context)
- Solution (Decision)
- Consequences (Results)

Source [Joel Parker Henderson](https://github.com/joelparkerhenderson/architecture-decision-record/), see this [issue about licensing](https://github.com/joelparkerhenderson/architecture-decision-record/issues/30).

Set as the default template using `adr templates default set alexandrian`

#### Business Case Pattern
Emphasizes creating a business case for a decision, including criteria, candidates, and costs, [created by Joel Parker Henderson](https://github.com/joelparkerhenderson/architecture-decision-record/blob/main/templates/decision-record-template-for-business-case/index.md), which distils the decision record into the following headings:

- Title
- Status
- Evaluation criteria
- Candidates to consider
- Research and analysis of each candidate
  - Does/doesn't meet criteria and why
  - Cost analysis
  - SWOT analysis
  - Opinions and feedback
- Recommendation

Source [Joel Parker Henderson](https://github.com/joelparkerhenderson/architecture-decision-record/), see this [issue about licensing](https://github.com/joelparkerhenderson/architecture-decision-record/issues/30).

Set as the default template using `adr templates default set business-case`

#### Markdown Architectural Decision Records (MADR)
Architectural Decisions using Markdown and Architectural Decision Records, by [Oliver Kopp](https://adr.github.io/madr/), which distils the decision record into the following headings:

- Title 
- Context and Problem Statement
- Decision Drivers
- Considered Options
- Decision Outcome
  - Positive Consequences
  - Negative Consequences
- Pros and Cons of the Options
  - [option 1]
  - [option 2]
  - [option 3]
- Links

[Available](https://adr.github.io/madr/) as dual-license under [MIT](https://opensource.org/licenses/MIT) and [CC0](https://creativecommons.org/share-your-work/public-domain/cc0/). You can choose between one of them if you use this work.

Set as the default template using `adr templates default set madr`

#### Merson Pattern
An adaptation of the [Nygard pattern](#nygard-pattern), by [Paulo Merson](https://github.com/pmerson/ADR-template) which adds the rationale behind the decision. It distils the decision record into the following headings:

- Title
- Status
- Decision
- Rationale
- Consequences

[Available](https://github.com/pmerson/ADR-template) under the [MIT License](https://github.com/pmerson/ADR-template/blob/master/LICENSE).

Set as the default template using `adr templates default set merson`

#### Nygard Pattern
A simple, low-friction "Agile" ADR approach by [Michael Nygard](http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions), which distils the decision record into the following headings:

- Title
- Status
- Context
- Decision
- Consequences

[Available](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions) under [CC0 1.0 Universal (CC0 1.0) Public Domain Dedication](https://creativecommons.org/publicdomain/zero/1.0/).

Set as the default template using `adr templates default set nygard`

#### Planguage Pattern
A Quality Assurance oriented approach by [John Terzakis](http://www.iaria.org/conferences2012/filesICCGI12/Tutorial%20Specifying%20Effective%20Non-func.pdf), which distils the decision record into the following headings:

- Title
- Status
- Tag
- Gist
- Requirement
- Rationale
- Priority
- Stakeholders
- Owner
- Author
- Revision
- Date
- Assumptions
- Risks
- Defined

Source [Joel Parker Henderson](https://github.com/joelparkerhenderson/architecture-decision-record/), see this [issue about licensing](https://github.com/joelparkerhenderson/architecture-decision-record/issues/30).

Set as the default template using `adr templates default set planguage`

#### Tyree and Akerman Pattern
ADR approach by [Jeff Tyree and Art Akerman](https://personal.utdallas.edu/~chung/SA/zz-Impreso-architecture_decisions-tyree-05.pdf), Capital One Financial, which distils the decision record into the following headings:

- Title
- Status
- Issue
- Decision
- Group
- Assumptions
- Constraints
- Positions
- Argument
- Implications
- Related decisions
- Related requirements
- Related artifacts
- Related principles
- Notes

Source [Joel Parker Henderson](https://github.com/joelparkerhenderson/architecture-decision-record/), see this [issue about licensing](https://github.com/joelparkerhenderson/architecture-decision-record/issues/30).

Set as the default template using `adr templates default set tyree-ackerman`

## More Information

An extensive readme is available in the [GitHub repository](https://github.com/endjin/dotnet-adr/).