# Project Description
T4-Permissions is a powerful and efficient light-weight RBAC (role based access control) system that uses permissions instead of roles for controlling access. The implementation in this example uses the T4 templating engine and C# to create the permission classes for an ASP.NET website. 

Before T4-Permissions, I was seeing a lot of bad code and it was clear that we needed a more maintainable solution than just checking roles seemingly arbitrarily all over the solution. 

## Goals ##
* Easy to use
  * Combine role checks into a single call. Can the user do the action or not?
  * Autocomplete for IDE to avoid mistakes and speed up development
  * Determine which roles can do which actions
  * Intuitive attributes and utility methods 
    * HTTP filter attributes for controllers
    * Can be used in the CsHTML view pages
    * Can be used from within the code
* Easy to configure
  * Wildcard permission nodes
  * Config is centralized to a single file
  * Easy to change how nodes are organized
* Fast and efficient
  * Look-up time is <# Roles> * O(1)
  * No IO delays
* Secure
  * It needs to work! 
 
 
## Features ##
* Central configuration file makes code easy to maintain and review
* Restrict features by permission instead of by role(s)
* T4 speeds up development time for updates and adds IDE ready auto-complete features based on the generated system.
* Generates filter attributes for ASP.NET controllers.
* Generates filter attributes for Web API controllers.
* Generates HtmlHelper extension method for use in cshtml view pages.
* O(1) look-up time once compiled.
* Uses an Enum pattern so developers can take advantage of autocomplete in their IDE’s.

## Solving the RBAC problem ##
[The story of how and why the T4 system was created](docs/01-finding-problem-solving-solution.md)

## Installing T4-Permissions in your own project ##
[Download](https://github.com/Pangamma/T4-Permissions/blob/master/docs/Article-T4-Permissions.docx?raw=true) / [View Online](https://drive.google.com/file/d/1yRXrL8dZZngt1U7UTdUEU2qj5n4uhcAO/view?usp=sharing) 
