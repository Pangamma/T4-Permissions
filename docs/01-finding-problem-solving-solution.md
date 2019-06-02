
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
[Authorize(Roles = "Admin,Editor,CEO, Writer, Financial Admin 2, Tech Support, Tech Support 2, Super Admin, Assistant, Jimmy")]
public ActionResult Preview(SomeViewModel model)
{
    // Some publishing logic here
    return View();
}

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
### Random Code ###
![Horrible code](https://raw.githubusercontent.com/Pangamma/T4-Permissions/master/docs/includes/terrible-code-1.png)

### TL;DR ###
![Duplicate code. Duplicate code everywhere.](https://raw.githubusercontent.com/Pangamma/T4-Permissions/master/docs/includes/bad-code-bad.jpg)

* Does not scale well
* Easy to miss things as system expands
* Could add variables to the view model, but the “OR” logic still remains.
* Very verbose
* No easy way to figure out what each role can do.
* No easy way to add new roles and ensure they have all the permissions they need. 
* Looks messy


Every time we wanted to add a new feature, we were having to add a new set of these role lists. The lists get longer and longer, and the risk of any of the checks falling out of sync increases every time. Then business asks for a new role with all the features of <b>X</b>, but also a new feaure called <b>Z</b>. Should be easy except <b>how do you even know what X can do?</b> The only way to figure it out is to do a <b>full text search</b> on the entire solution for any mention of that role and then making sure you're adding the new role to all of those places. 

As you're going through this hoard of info <b>the intentions of each if/else block are not 100% clear</b>. You have to read through the inner logic to fully understand. "Why is THIS role listed and not this OTHER role?" If group X is more powerful than group Z, should group X be able to access this feature as well? Why was it not included? Was group X added <b>AFTER</b> group Z and the other devs forgot to add the right permissions? <b>You start questioning if code is there on purpose or by mistake.</b> Especially if no one has a full (updated) list of what each role is supposed to be able to do. 

### So the code is a mess. It's difficult to maintain, confusing, and ambiguous. ###
### Let's fix it. ###

![Tony Stark with a wrench](https://raw.githubusercontent.com/Pangamma/T4-Permissions/master/docs/includes/lets-fix-it-2.png)





# The Solution #
So how do we get...

| From here | To here? |
| ------ | ------ |
| ![Messy Wires](https://raw.githubusercontent.com/Pangamma/T4-Permissions/master/docs/includes/wires-messy.jpg) | ![Organized Wires](https://raw.githubusercontent.com/Pangamma/T4-Permissions/master/docs/includes/wires-organized.jpg) |

We need a solution that is easy to use or else the rest of the team won't want to actually use it. Developers usually take the path of least resistance. So take some ideas from the gaming scene. In the gaming scene there is a concept known as permissions. 

![permissions yaml example](https://raw.githubusercontent.com/Pangamma/T4-Permissions/master/docs/includes/permissions-yaml-config.png)

To give a user the ability to teleport you would give them the "<b>Essentials.teleport.self</b>" permission node. To give a user the ability to teleport themselves and others you could give them <b>Essentials.teleport.self</b> and <b>Essentials.teleport.others</b> or you could simplify it and give them <b>Essentials.teleport.*</b>. This makes it easier to grant a ton of permissions really quickly. It also helps to group similar permission nodes together. 

To CHECK a permission we usually do something like player.hasPermission("Essentials.teleport.others"). Or we use an enum or a constant to make sure we're using the right permission node value. So I wante to make that happen in the T4 permission system. In order to do that, we need to change how we're doing authentication. These two flowcharts show where we will need to make our changes.


| Before | After |
| ---- | ----- |
| ![roles](https://raw.githubusercontent.com/Pangamma/T4-Permissions/master/docs/includes/auth-flow-roles.png) | ![permissions](https://raw.githubusercontent.com/Pangamma/T4-Permissions/master/docs/includes/auth-flow-permissions.png) |

## After some changes, the new code looks like this: ##

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

It makes it easy to add new roles, or see what each role is capable of; all from viewing and editing a central configuration file. 

## Next we can take a look at how the configuration works. ##

The config on the left generates the C# class on the right. 

| Input | Output |
| ----- | ------ |
| [View File](https://github.com/Pangamma/T4-Permissions/blob/master/code/Finished_Version/ImagineBrownBag/App_Start/Permissions.tt) | [View File](https://github.com/Pangamma/T4-Permissions/blob/master/code/Finished_Version/ImagineBrownBag/App_Start/Permissions.cs)
| ![config input](https://raw.githubusercontent.com/Pangamma/T4-Permissions/master/docs/includes/config-input.png) | ![config input](https://raw.githubusercontent.com/Pangamma/T4-Permissions/master/docs/includes/config-output.png) |

## Installation Guide: ##
[Here is the link](https://docs.google.com/document/d/1yRXrL8dZZngt1U7UTdUEU2qj5n4uhcAO/edit)
