export interface AuthResponse {

  userId:number;

  fullName:string;

  email:string;

  roleId:number;

  accessToken:string;

  refreshToken:string;

  expiry:string;

  isNewUser:boolean;

}