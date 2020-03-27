import { UserBasic } from "./user"
import { Location } from "./location"

export class AccessListBasic {
  id: number
  name: string
}

export class AccessList extends AccessListBasic {
  locations: Location[]
  users: UserBasic[]
}

export class AccessListUpdateDto {

  constructor(
    public name: string,
    public locationIds: number[],
    public userIds: number[]
  ) {}  
}