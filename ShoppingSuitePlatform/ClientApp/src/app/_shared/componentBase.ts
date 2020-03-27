import { RouteConstants } from "./routeConstants";

export abstract class ComponentBase {
  public readonly LoginPage = `/${RouteConstants.LoginPage}`
  public readonly UsersPage = `/${RouteConstants.UsersPage}`
  public readonly UserEditPage = `/${RouteConstants.UserEditPage}`
  public readonly AccessListsPage = `/${RouteConstants.AccessListsPage}`
  public readonly AccessListEditPage = `/${RouteConstants.AccessListEditPage}`
  public readonly LocationsPage = `/${RouteConstants.LocationsPage}`
  public readonly ProfilePage = `/${RouteConstants.ProfilePage}`
}