# RecordLabel
ASP.NET MVC application designed for presentation and management of a record label's release catalogue, news and articles

It was commissioned by a specific client but then cancelled halfway through when they realized they did not have the required infrastructure to host an ASP.NET-based application. I decided to keep working on this project out of personal interest.
Currently, it features the original client-commissioned visual design and structure, but I am going to modify and imporove that in the course of further development.
Used technologies:
• Written in C#
• JavaScript/jQuery using the object-oriented approach
• EntityFramework (code-first approach)
• Bootstrap framework for UX

Some interesting features:
• All content entries can contain text in multiple languages
• Dynamic loading of items in lists
• Logged-in users (administrators) see "Edit" buttons to editable content in the same publicly accessible pages.



TODO:
• Extract and separate view models from content models
• The whole HTTPS stuff
• Use some framework to dynamically add items (list results with ajax calls / items in edit mode)
• Take care of XSS
• Fix styling of Login form
• Work on edit forms
• Sort out CSS classes
• Review HTML element sizes
• Release Edit: Finish references styling (and make their order changeable) + Finish tracklist styling (make their order changeable too) + Make Add Image to add new image purely-front in end, just like with references.
• Decide on the best way to pass data to javascript while rendering views
• Move image processing and saving from the binder into the controller

...and more
