
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

### So the code is a mess. It's difficult to maintain, confusing, and ambiguous. ###

[Let's fix it.](docs/fixing-the-issues.md)

![Tony Stark with a wrench](https://raw.githubusercontent.com/Pangamma/T4-Permissions/docs/docs/includes/lets-fix-it-2.png)
