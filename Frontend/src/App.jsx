import { BrowserRouter, Routes, Route } from "react-router-dom";

import Login from "./pages/Login";
import Register from "./pages/Register";
import Dashboard from "./pages/Dashboard";
import ProtectedRoute from "./components/ProtectedRoute";
import RestaurantList from "./pages/customer/RestaurantList";
import RestaurantDetails from "./pages/customer/RestaurantDetail";
import Cart from "./pages/customer/Cart";


function App() {
    return (
        <BrowserRouter>
            <Routes>

                <Route path="/" element={<Login />} />

                <Route path="/login" element={<Login />} />

                <Route path="/register" element={<Register />} />

                <Route path="/dashboards" 
                       element={
                       <ProtectedRoute>
                          <Dashboard />
                      </ProtectedRoute>} />

                <Route path="/restaurants"
                        element={<RestaurantList/>}></Route>
                  
                <Route path="/restaurants/:id"
                       element={<RestaurantDetails/>}
                  />


                <Route
                    path="/cart"
                    element={<Cart/>}></Route>

            </Routes>
        </BrowserRouter>
    );
}

export default App;