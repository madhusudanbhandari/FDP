import { BrowserRouter, Routes, Route } from "react-router-dom";

import Login from "./pages/Login";
import Register from "./pages/Register";
import Dashboard from "./pages/Dashboard";
import ProtectedRoute from "./components/ProtectedRoute";
import RestaurantList from "./pages/customer/RestaurantList";
import RestaurantDetails from "./pages/customer/RestaurantDetail";
import Cart from "./pages/customer/Cart";
import Orders from "./pages/customer/Orders";
import Notifications from "./pages/customer/Notifications";
import Payment from "./pages/customer/Payment";

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
                    path="/orders"
                    element={
                        <ProtectedRoute>
                            <Orders />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/cart"
                    element={<Cart/>}></Route>

                <Route
                    path="/notifications"
                    element={
                        <ProtectedRoute>
                            <Notifications />
                        </ProtectedRoute>
                    }
                />
                <Route
                path="/payment/:orderId"
                element={
                    <ProtectedRoute>
                        <Payment />
                    </ProtectedRoute>
                }
            />

            </Routes>
        </BrowserRouter>
    );
}

export default App;