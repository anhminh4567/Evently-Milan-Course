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




----------------------------------------------------------------7.0: Messaging idempotency-----------------------------------------------------------------------------------------
NOTE:
	in this chapter, the domainEvent and its consumer will be replaced ( remove mediatR from the event publishing)
	this is due to the introduction of new library called Scrutor ( allow Decorator implementation for Indempotent consumer (Video 3 in chap 7 ))

	Outbox Indempotent :
		this and outbox prcessing is in the same module, 
		and everything a domain EVent is saved to outbox table, when it is process, the IndempotentEventHandler<>
		will check if the domainEvent is processed, if no then get the handler
			the handler processs;
				if it is publish to integreate then publish to event bus
				NOTE THIS PART: we just implement indempotent for outbox, not for integration event YET ( this is when we use Inbox pattern)
			return success;
		will save the message to OutbboxMessageConsumer table


NOTE INBOX:
	in this lesssion, the consumer of integration event, is MAJORLY CHANGED
	the consumer now have 1 job only, LIsten to the Event , and save to InboxMessage tabble ( thats it)
	after this, bg job Quartz will read table inbox message and inbox message consumer tables
	to decide to process or not
	flow:
	* Module 1
		commandExe --> produce DomainEvent --> saved to OutboxMessage
												|
												picked by BGJOBs	--> publish to DomainEventHandler 
																	-->  process and saved Message to OutboxMessageConsumer
																			|
																		publish integration Event (IE) if exist
	
	* Module 2:
		havve a consumer listen to (IE), and the consumer ONLY SAVE message to InboxMessage
		InboxMessage -> picked up by BGJOBs		--> Publish to IntegrationEventHandler<(IE)> 
												--> handle success -> save to InboxMessageConsumer


	====> there are 3 things associate with IntegrateEvent (IE)
		1. the one publish from module (by DomainEventHandler) to EventBus to route to all consumer
		2. the Consumer(IE) from other modules , To store (IE) down to DB (InboxMessage)
		3. the InteggrationEventHandler<IE>	, to handle the message from db stored before
