import { JwtPayload } from "jwt-decode";

export namespace IUser {
    export interface LoginRequest {
        email: string;
        password: string;
    }

    export interface Token extends JwtPayload{
        id: string;
        fullname: string;
        email: string;
        role: string;
    }
}