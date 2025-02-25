the current project structure is base on modules, each modules is base on clean architect but with slight changes
1. structure is a bit differet
OLD:
	API -> INFRA -> APPLICATION -> DOMAIN
		-> APPLICATION
CURRENT: 
	INFRA -> API -> APPLICATION -> DOMAIN
		  -> APPLICATION
now infrastructure hold the top, it references other project, and the main entry point is the API folders main project



CURRENT GENERAL LAYOUT


																API
																|
																|
							----------------------------------------------------------------------------------------
							|					
					Event-Module
				Infrastructure
					|
	----------------|
	|				|
Application <--- Presentation 
	|
  Domain
				