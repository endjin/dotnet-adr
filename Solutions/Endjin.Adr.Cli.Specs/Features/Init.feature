Feature: Init
	In order to use Architecture Decision Records
	As a developer
	I want to initialize a new ADR repository

Scenario: Initialize a Repository
	Given I ask the adr cli to initialise a new repo in the 'adr\repo' directory
	When I execute the adr cli
	Then a new ADR repository has been created with an initial readme.md file
