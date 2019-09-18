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
[Download](https://github.com/Pangamma/T4-Permissions/blob/master/docs/Article-T4-Permissions.docx?raw=true) / [View Online](https://docs.google.com/document/d/1yRXrL8dZZngt1U7UTdUEU2qj5n4uhcAO/edit) 


## Before and after using T4-Permissions: ##

```diff
<!-- CSHTML -->

- @if (HttpContext.Current?.User.IsInRole("Admin") 
- || HttpContext.Current?.User.IsInRole("Editor") 
- || HttpContext.Current?.User.IsInRole("CEO"))
+ @if (Html.HasPermission(Permissions.CanPublishArticles))
{
    <button type="button">Publish</button>
    <!-- 
        We shouldn't have to worry about the ROLEs. 
        We should only have to worry about the PERMISSIONS. 
    -->
}
```
```diff
// C# Controller 

[HttpGet]
- [Authorize(Roles = "Admin,Editor,CEO,Writer,Financial Admin 2, Tech Support, Tech Support 2, Super Admin, Assistant")]
+ [HasPermission(Permissions.CanEditArticles)]
public ActionResult Edit(int id)
{
    return View();
}

```

```diff
// From somewhere in the C# code.
- if (!(HttpContext.Current?.User.IsInRole("Admin") 
- || HttpContext.Current?.User.IsInRole("Accountant") 
- || HttpContext.Current?.User.IsInRole("Sales Manager") 
- || HttpContext.Current?.User.IsInRole("CEO")))
+ if (!Permissions.HasPermission(Permissions.CanViewSalesData))
{
    // have some kind of unauthorized response here, or 
    // perform unauthorized type logic
}
```
