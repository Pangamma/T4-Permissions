# The Solution #
So how do we get...

| From here | To here? |
| ------ | ------ |
| ![Messy Wires](https://raw.githubusercontent.com/Pangamma/T4-Permissions/docs/docs/includes/wires-messy.jpg) | ![Organized Wires](https://raw.githubusercontent.com/Pangamma/T4-Permissions/docs/docs/includes/wires-organized.jpg) |



The first snippet shows what people usually do when checking if a user can access a feature. When systems scale this approach gets messy. It's too verbose, it isn't clear which feature is being unlocked, and it is prone to human error. The second snippet shows a better alternative. In the second snippet we immediately know which feature is being unlocked. And we don't have to worry about remembering which roles have access to which features. The centralized configuration file takes care of all that for us. 



The second snippet shows why the default solution needs improvement. It's too verbose. It isn't clear what feature is being unlocked. And if you ever want to know what the difference is between an "Admin" and an "Editor" you will have to 
Before T4-Permissions we would scan through code files looking for 


It makes it easy to add new roles, or see what each role is capable of; all from viewing and editing a single configuration file. 





```diff
<!-- CSHTML -->

- @if (HttpContext.Current?.User.IsInRole("Admin") 
- || HttpContext.Current?.User.IsInRole("Editor") 
- || HttpContext.Current?.User.IsInRole("CEO"))
+ @if (Html.HasPermission(Permissions.CanPublishArticles))
{
    <button type="button">Publish</button>
    <!-- Look at all that duplicated code! This is very verbose. -->
}


- @if (HttpContext.Current?.User.IsInRole("Admin") 
- || HttpContext.Current?.User.IsInRole("Editor") 
- || HttpContext.Current?.User.IsInRole("CEO")
- || HttpContext.Current?.User.IsInRole("Writer"))
+ @if (Html.HasPermission(Permissions.CanEditArticles))
{
    <button type="button">Edit</button>
}


- @if (HttpContext.Current?.User.IsInRole("Admin") == false)
+ @if (Html.HasPermission(Permissions.CanDoTheThing) == false)
{
    <span>I'm sorry Dave. I can't let you do that, Dave.</span>
}

```
```diff
// C# Controller 

[HttpPost]
[ValidateAntiForgeryToken]
- [Authorize(Roles = "Admin,Editor,CEO")]
+ [HasPermission(Permissions.CanPublishArticles)]
public ActionResult Publish(SomeViewModel model)
{
    // Some publishing logic here
    return View();
}


[HttpGet]
- [Authorize(Roles = "Admin,Editor,CEO,Writer")]
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
