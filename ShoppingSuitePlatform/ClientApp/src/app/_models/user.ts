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