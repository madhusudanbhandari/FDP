import  {jwtDecode} from "jwt-decode"
import { use } from "react";

export function getUserFromToken(){
    const token=localStorage.getItem("token");

    if(!token){
        return null;
    }

    try{
        return jwtDecode(token);
    }catch(error){
        console.error("Invalid token:",error);
        return null;
    }
}
export function getUserRole() {
    const user = getUserFromToken();

    if (!user) {
        return null;
    }

    const role =
        user["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

    return role || null;
}