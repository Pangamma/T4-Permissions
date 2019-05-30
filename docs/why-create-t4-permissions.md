
## Why I created the T4-Permission system ##
I was browsing through the code one day when I realized our developers were struggling to organize our growing RBAC system. It was a mess. And the logic was brittle as well. A few examples of what I was seeing:

![Duplicate code. Duplicate code everywhere.](https://raw.githubusercontent.com/Pangamma/T4-Permissions/docs/docs/includes/duplicate-code-duplicate-code-everywhere.jpg)


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
[ValidateAntiForgeryToken]
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

```C#
// From somewhere in the C# code.
if (!(HttpContext.Current?.User.IsInRole("Admin") 
|| HttpContext.Current?.User.IsInRole("Editor") 
|| HttpContext.Current?.User.IsInRole("CEO"))){
    // have some kind of unauthorized response here, or 
    // perform unauthorized type logic
}
```

Every time we wanted to add a new feature, we were having to add a new set of these role lists. The lists get longer and longer, and the risk of any of the checks falling out of sync increases every time. Then business asks for a new role with all the features of <b>X</b>, but also a new feaure called <b>Z</b>. Should be easy except <b>how do you even know what X can do?</b> The only way to figure it out is to do a <b>full text search</b> on the entire solution for any mention of that role and then making sure you're adding the new role to all of those places. 

As you're going through this hoard of info <b>the intentions of each if/else block are not 100% clear</b>. You have to read through the inner logic to fully understand. "Why is THIS role listed and not this OTHER role?" If group X is more powerful than group Z, should group X be able to access this feature as well? Why was it not included? Was group X added <b>AFTER</b> group Z and the other devs forgot to add the right permissions? <b>You start questioning if code is there on purpose or by mistake.</b> Especially if no one has a full (updated) list of what each role is supposed to be able to do. 

So the code is a mess. It's difficult to maintain. It's confusing and ambiguous. 

[Let's fix it.](docs/fixing-the-issues.md)

![Tony Stark with a wrench](https://raw.githubusercontent.com/Pangamma/T4-Permissions/docs/docs/includes/lets-fix-it-2.png)


I was working on one of our websites when I noticed some particularly ugly code surrounding RBACthe developers on our team were struggli
In reality, we don't really care about the user's role. We care about what that role can do.
H
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

