import { Navigate } from "react-router-dom";
import { getUserRole } from "../utils/auth";

import CustomerDashboard from "./customer/CustomerDashboard";
import DeliveryDashboard from "./DeliveryPerson/DeliveryPersonDashboard";
import AdminDashboard from "./Admin/AdminDashboard";
import RestaurantOwnerDashboard from "./RestaurantOwner/RestaurantOwnerDashboard";


function Dashboard(){
    const role=getUserRole();

    switch(role){
        case "Customer":
            return <CustomerDashboard/>;
        
        case "RestaurantOwner":
            return <RestaurantOwnerDashboard/>;
        
        case "Admin":
            return <AdminDashboard/>;
        
        case "DeliveryPerson":
            return <DeliveryDashboard/>;

        default:
            return <Navigate to="/" replace/>;
    }
}

export default Dashboard;