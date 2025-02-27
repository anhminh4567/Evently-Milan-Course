the current project structure is base on modules, each modules is base on clean architect but with slight changes
1. structure is a bit differet
OLD:
	API -> INFRA -> APPLICATION -> DOMAIN
		-> APPLICATION
CURRENT: 
	INFRA -> API -> APPLICATION -> DOMAIN
		  -> APPLICATION
now infrastructure hold the top, it references other project, and the main entry point is the API folders main project

----------------------------------------------------------------COMMON----------------------------------------------------------------------------------------------------
						* Referenced by all project respective to their type (app,domain,infra,presentation)
									
								 Common.Infrastructure
										|
		Common.Application <---- Common.Presentation
			|
			|
		Common.Domain

CURRENT GENERAL LAYOUT

----------------------------------------------------------------Clean architect orignial (milan version)----------------------------------------------------------------------------------------------------

																API
																|
																|
							----------------------------------------------------------------------------------------
							|					
						Event-Module
							||
					Infrastructure
							|
			----------------|
			|				|
		Application <--- Presentation 
			|
		  Domain



----------------------------------------------------------------4.0: Module Communication-----------------------------------------------------------------------------------------


																						Event.API
																							|
																							|
											---------------------------------------------------------------------------------------------
											|												|											|
										Event-Module									User-Module									Ticketing-Module
											||											(same)											(same)
											||												|										    |
									  Infrastructure										|										    |
											|												.										    .
							----------------|........X--------- (no longer used)			.										    .
							|				|				  |								.										    .
							|				|				  |								.										    .
					---- Application <--- Presentation .....> PublicAPI				  < reference									< reference
					|		    |   						(synchronouse)				 from Presentation >							from Presentation >
					|		    |															.										    .
					|		    |															.										    .
				Domain	    	|														    .										    .
								|															.										    .
								|															|										    |
							IntegrationEvents <-----------------------------------------------------------------------------------------
								
							(As-synchronouse)