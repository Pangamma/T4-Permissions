# The Solution #
So how do we get...

| From here | To here? |
| ------ | ------ |
| ![Messy Wires](https://raw.githubusercontent.com/Pangamma/T4-Permissions/docs/docs/includes/wires-messy.jpg) | ![Organized Wires](https://raw.githubusercontent.com/Pangamma/T4-Permissions/docs/docs/includes/wires-organized.jpg) |

### Try out the T4-Permissions system. ###

```diff
<!-- CSHTML -->

- @if (HttpContext.Current?.User.IsInRole("Admin") 
- || HttpContext.Current?.User.IsInRole("Editor") 
- || HttpContext.Current?.User.IsInRole("CEO"))
+ @if (Html.HasPermission(Permissions.CanPublishArticles))
{
    <button type="button">Publish</button>
    <!-- 
        We shouldn't have to worry about the ROLEs. We should only have to worry about the PERMISSIONS. 
    -->
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

In the new snippets we immediately know which features are being unlocked. And we don't have to worry about remembering which roles have access to which features. The centralized configuration file will take care of all that for us. Also, if you ever want to know what the difference is between an "Admin" and an "Editor" you can just check the central configuration file.

It makes it easy to add new roles, or see what each role is capable of; all from viewing and editing a single configuration file. 




