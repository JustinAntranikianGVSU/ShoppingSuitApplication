import { Role } from "./role"
import { LocationBasic } from "./location"
import { AccessListBasic } from "./accessList"

export class UserBasic {
  id: number
  firstName: string
  lastName: string
  fullName: string
  initals: string
}

export class User extends UserBasic {
  roles: Role[]
  locations: LocationBasic[]
  accessLists: AccessListBasic[]
  email: string
  clientIdentifier: string
}

export class UserUpdateDto {

  constructor(
    public firstName: string,
    public lastName: string,
    public email: string,
    public accessListIds: number[],
    public roleIds: string[]
  ) {}
}

export class UserSearchViewModel {

  public firstName: string
  public lastName: string
  public email: string
  public locationName: string
  public locationCount: number
  public accessListName: string
  public accessListCount: number
  public roleName: string
  public roleCount: number
  public skip: number = 0
  public take: number = 10
  public sortByField: number = 0
  public sortDescending: boolean = false  
}