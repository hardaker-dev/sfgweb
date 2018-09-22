import { Customer } from "./customer";

export class User {

  constructor(

    public customer: Customer,
    public authToken: string,
    public expiresIn: string
  ) {
  }

}
