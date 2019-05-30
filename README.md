# Project Description
T4-Permissions is a permissions system that allows developers to manage expanding complexity with ease. The solution was created to simplify role-based-access-control / authentication and give more control over individual permissions. Before T4-Permissions, I was seeing a lot of bad code and it was clear that we needed a more maintainable solution than just checking roles seemingly arbitrarily all over the solution. 

## Features ##
* Central configuration file makes code easy to maintain and review
* Restrict features by permission instead of by role(s)
* T4 speeds up development time for updates and adds IDE ready auto-complete features based on the generated system.
* Generates filter attributes for ASP.NET controllers.
* Generates filter attributes for Web API controllers.
* Generates HtmlHelper extension method for use in cshtml view pages.
* O(1) look-up time once compiled.
* Uses an Enum pattern so developers can take advantage of autocomplete in their IDE’s.

## Why I created the project ##
In reality, we don't really care about the user's role. We care about what that role can do. 

```C# 
// Rather than listing individual roles,
this.User.IsInRole("Admin") || this.User.IsInRole("Editor") || this.User.IsInRole("CEO")

// it is more helpful to see the labeled permission node
Permissions.HasPermission(Permissions.CanPublishContent)
```

The first snippet shows what people usually do when checking if a user can access a feature. When systems scale this approach gets messy. It's too verbose, it isn't clear which feature is being unlocked, and it is prone to human error. The second snippet shows a better alternative. In the second snippet we immediately know which feature is being unlocked. And we don't have to worry about remembering which roles have access to which features. The centralized configuration file takes care of all that for us. 

The second snippet shows why the default solution needs improvement. It's too verbose. It isn't clear what feature is being unlocked. And if you ever want to know what the difference is between an "Admin" and an "Editor" you will have to 
Before T4-Permissions we would scan through code files looking for 


It makes it easy to add new roles, or see what each role is capable of; all from viewing and editing a single configuration file. 


