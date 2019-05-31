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


# The Problem #
(Why I created the T4-Permission system)
I was browsing through the code one day when I realized our developers were struggling to organize our growing RBAC system. It was a mess. And the logic was brittle as well. A few examples of what I was seeing:

### CSHTML ###
```CSHTML 
<!-- CSHTML -->

@if (HttpContext.Current?.User.IsInRole("Admin") 
|| HttpContext.Current?.User.IsInRole("Editor") 
|| HttpContext.Current?.User.IsInRole("CEO")){
    <button type="button">Publish</button>
    <!-- Look at all that duplicated code! This is very verbose. -->
}

@if (HttpContext.Current?.User.IsInRole("Admin") 
|| HttpContext.Current?.User.IsInRole("Editor") 
|| HttpContext.Current?.User.IsInRole("CEO")
|| HttpContext.Current?.User.IsInRole("Writer")){
    <button type="button">Edit</button>
}

@if (HttpContext.Current?.User.IsInRole("Admin") == false){
    <span>I'm sorry Dave. I can't let you do that, Dave.</span>
}

```
### C# Controller ###
```C#
// C# Controller 

[HttpPost]
[ValidateAntiForgeryToken]
[Authorize(Roles = "Admin,Editor,CEO")]
public ActionResult Publish(SomeViewModel model)
{
    // Some publishing logic here
    return View();
}

[HttpGet]
[Authorize(Roles = "Admin,Editor,CEO")]
public ActionResult Edit(int id)
{
    // Note how the roles are out of sync with the view logic. 
    // Probably because a new role was added since the feature was first added
    // and the maintaining developer didn't know about every single place that needed
    // to be edited to enforce consistent security settings.
    return View();
}

```

### C# Code ###
```C#
// From somewhere in the C# code.
if (!(HttpContext.Current?.User.IsInRole("Admin") 
|| HttpContext.Current?.User.IsInRole("Editor") 
|| HttpContext.Current?.User.IsInRole("CEO"))){
    // have some kind of unauthorized response here, or 
    // perform unauthorized type logic
}
```

### TL;DR ###
![Duplicate code. Duplicate code everywhere.](https://raw.githubusercontent.com/Pangamma/T4-Permissions/docs/docs/includes/duplicate-code-duplicate-code-everywhere.jpg)

* Does not scale well
* Easy to miss things as system expands
* Could add variables to the view model, but the “OR” logic still remains.
* Very verbose

Every time we wanted to add a new feature, we were having to add a new set of these role lists. The lists get longer and longer, and the risk of any of the checks falling out of sync increases every time. Then business asks for a new role with all the features of <b>X</b>, but also a new feaure called <b>Z</b>. Should be easy except <b>how do you even know what X can do?</b> The only way to figure it out is to do a <b>full text search</b> on the entire solution for any mention of that role and then making sure you're adding the new role to all of those places. 

As you're going through this hoard of info <b>the intentions of each if/else block are not 100% clear</b>. You have to read through the inner logic to fully understand. "Why is THIS role listed and not this OTHER role?" If group X is more powerful than group Z, should group X be able to access this feature as well? Why was it not included? Was group X added <b>AFTER</b> group Z and the other devs forgot to add the right permissions? <b>You start questioning if code is there on purpose or by mistake.</b> Especially if no one has a full (updated) list of what each role is supposed to be able to do. 

### So the code is a mess. It's difficult to maintain, confusing, and ambiguous. ###

[Let's fix it.](fixing-the-issues.md)

![Tony Stark with a wrench](https://raw.githubusercontent.com/Pangamma/T4-Permissions/docs/docs/includes/lets-fix-it-2.png)

