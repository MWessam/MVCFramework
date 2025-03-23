An MVC framework inspired by ASP.NET MVC.

Features:
1. View Group: A couple of views/partial views that are related to a certain controller. When using View() method or PartialView() method it will find if the corresponding view is found under the view group.
2. Controller Actions: View() PartialView() RedirectToAction() and Back()
   View(viewName, viewModel = null) Takes in a view name that will be rendered, and an optional view model to bind. The type of the viewmodel MUST match what the view expects.
     Will push the previous view if found into a stack and transition out of it so it can be returned to when using Back().
   PartialView(viewName, viewModel = null) Takes in a partial view name and renders it. Partial view must be a child of a view object in the scene hierarchy.
     (It's not exactly the best system currently, but it's useful for when u have a settings panel for examplew ith multiple sub menus depending on which tab is chosen)
   RedirectToAction(actionName, controllerName = "") Finds the specified action in either the current controller (if no controller name supplided) or in the controller whose name is supplied.
   Back() Attempts to transitions out of the current view to the previous one.

3. View Actions:
   ButtonAction(actionName, controllerName): it's the same as redirecttoaction but for views.

4. ViewModels
   Views must always be bound to a view model.
   you create a view model in your controller and bind it using view or partial view methods.


An example scene and code is shipped with the package.
