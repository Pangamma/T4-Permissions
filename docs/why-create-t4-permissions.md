
## Why I created the T4-Permission system ##
I was browsing through the code one day when I realized our developers were struggling to organize our growing RBAC system. It was a mess. And the logic was brittle as well. A few examples of what I was seeing:


```CSHTML 
<!-- CSHTML -->

@if (HttpContext.Current?.User.IsInRole("Admin") 
|| HttpContext.Current?.User.IsInRole("Editor") 
|| HttpContext.Current?.User.IsInRole("CEO")){
    <button type="button">Publish</button>
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
