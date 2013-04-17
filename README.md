ScoopIt .NET Api
===============
A basic .NET framework to access the Scoop.it API and a test project (ASP MVC3).

INSTRUCTIONS

- Modify the web.config with yours consumer key and consumer secret (https://www.scoop.it/apps)
- Do not hesitate to contribute!

Web.config:

	 ```xml
	<add key="CONSUMER_KEY" value="Your consumer key"/>
    <add key="CONSUMER_SECRET" value="Your consumer secret"/>
    <add key="CALLBACK" value="/Scoopit/CallbackFromScoopIt"/>
    <!--  Below you will enter your topic Id and the number of posts that you want on each page -->
    <add key="TOPIC_Name" value="asp-web-development"/>
    <add key="NumberPostPerPage" value="10"/>
    ```

LINKS:
- Scoop.it Developers Home : http://www.scoop.it/dev
- Scoop.it API Talk : http://groups.google.com/group/scoopit-api-talk