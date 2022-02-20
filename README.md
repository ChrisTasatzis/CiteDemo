# CiteDemo
This is a Demo project as part of my interview process for Cite.

The project is broken down to these components: 
- Database Assignment Queries
- CiteDemo Business Logic Layer
- CiteDemo Api
- CiteDemo Presentation Layer

# Database Assignment Queries
This part contains the T-SQL scripts for the database assignment.

# CiteDemo Business Logic Layer
This is the first layer of the web app assignment. I modified the given database (added some columns to the employee table and changed the datetime to datetime2 to be able to map it to the .net code easier). Here i handle all CRUD operations for the entities as well as some more complex business logic operatons (in a bigger project i would probably add another layer with wrappers for the CRUD operations)

# CiteDemo Api
This is the second layer, here i create the controllers that expose the functionality of the business logic layer for http requests. Here is also the place where i validate my data using Data annotations. THere is a basic documentation for the api on the postman collection that can be found in the repo.

# CiteDemo Presentation Layer
This is the thrid layer. I initially tried to build the whole thing with plain html and JQuery but i wasn't happy with the resulting code, so instead i coded the front end with ASP.NET MVC where i am more familiar with project structure and best practices. The Map page although is mostly created with JQuery. 

# Further improvments 
- Front end design (Css, make use of html elements for dates, checkboxes, better validation-error messages).
- Directions redesign (i had an issue when making the api call for directions, so i had to make the api call from the frontend which is not elegant and also not secure as the API key is exposed).
- Create separate layer for Data Logic and Business Logic.
- Code refactorings (i am sure that there are logical errors and places where i don't follow conventions that i didn't have the chance to find and fix because i was in a hurry.
- Use a secret manager for database connection strings/api keys

# Building and running 
- Create a Database called CiteDemo
- Open Solution in VS 
- Make sure that multiple project startup is enforced and both api and web app are ticked. 
- Run

