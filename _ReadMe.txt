Greetings!

You will find the visual studio solution in:
~\LitmusProject_simple\MyLitmusApp.sln

Ctrl+F5 to run the app

You will find an Index page that shows you time left to use this app. 
(my account was a free 30 day account)

There are 2 buttons at the top of the page "Show Emails" and "Show Browsers"
Each will open the corresponding page. An API call is made when pages load in the following way:
Index page calls "accounts.xml"
EmailClients page calls "/emails/clients.xml"
BrowserClients page calls "/pages/clients.xml"
I also have an Error page in case there is no internet connection, wrong password, or wrong api address.
The last 2 errors should never happen since those values are hard coded :) 


This was a great little project!
Thank you
