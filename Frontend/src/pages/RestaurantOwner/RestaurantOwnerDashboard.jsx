import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../services/api";
import { getUserFromToken } from "../../utils/auth";

import MenuSection from "./Menu";
import CreateRestaurantForm from "./CreateRestaurant";
import RestaurantSection from "./MyRestaurant";
import OrdersSection from "./OrderSection";


function RestaurantOwnerDashboard() {

    const navigate = useNavigate();

    const [restaurant, setRestaurant] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const user = getUserFromToken();

    const name =
        user?.[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
        ] || "Restaurant Owner";

    const email =
        user?.[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
        ] || "";


    useEffect(() => {
        loadRestaurant();
    }, []);


    async function loadRestaurant() {

        try {

            const response = await api.get(
                "Restaurant/my-restaurants"
            );

            setRestaurant(response.data);

        } catch (error) {

            if (error.response?.status === 404) {

                setRestaurant(null);

            } else {

                console.error(error);

                setError(
                    error.response?.data?.message ||
                    "Failed to load restaurant"
                );
            }

        } finally {

            setLoading(false);

        }
    }


    function handleLogout() {

        localStorage.removeItem("token");

        navigate("/login", {
            replace: true
        });
    }


    function handleCreateRestaurant() {

        setRestaurant({
            creating: true
        });

    }


    function handleRestaurantCreated(data) {

        setRestaurant(data);

    }


    function handleRestaurantDeleted() {

        setRestaurant(null);

    }


    function handleRestaurantUpdated(data) {

        setRestaurant(data);

    }


    if (loading) {

        return (
            <div className="min-h-screen flex items-center justify-center bg-gray-100">

                <div className="text-center">

                    <div className="text-2xl font-semibold">
                        Loading dashboard...
                    </div>

                    <p className="text-gray-500 mt-2">
                        Please wait
                    </p>

                </div>

            </div>
        );
    }


    return (

        <div className="min-h-screen bg-gray-100">

            {/* NAVBAR */}

            <nav className="bg-white shadow-sm">

                <div className="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">

                    <div>

                        <h1 className="text-2xl font-bold text-indigo-600">
                            FDP
                        </h1>

                        <p className="text-sm text-gray-500">
                            Restaurant Owner
                        </p>

                    </div>


                    <div className="flex items-center gap-6">

                        <div className="text-right">

                            <p className="font-semibold text-gray-800">
                                {name}
                            </p>

                            <p className="text-sm text-gray-500">
                                {email}
                            </p>

                        </div>


                        <button
                            onClick={handleLogout}
                            className="bg-red-500 hover:bg-red-600 text-white px-5 py-2 rounded-lg"
                        >
                            Logout
                        </button>

                    </div>

                </div>

            </nav>


            {/* MAIN */}

            <main className="max-w-7xl mx-auto px-6 py-10">

                <h1 className="text-3xl font-bold text-gray-800">
                    Welcome, {name}
                </h1>

                <p className="text-gray-500 mt-2">
                    Manage your restaurant and menu.
                </p>


                {error && (

                    <div className="bg-red-100 text-red-700 p-4 rounded-lg mt-6">
                        {error}
                    </div>

                )}


                {/* NO RESTAURANT */}

                {!restaurant && (

                    <div className="bg-white rounded-2xl shadow-sm p-10 mt-8 text-center">

                        <div className="text-5xl mb-5">
                            🍽️
                        </div>

                        <h2 className="text-2xl font-bold">
                            Please create a restaurant
                        </h2>

                        <p className="text-gray-500 mt-3">
                            You don't have a restaurant yet.
                            Create one to start managing your
                            food business.
                        </p>

                        <button
                            onClick={handleCreateRestaurant}
                            className="mt-7 bg-indigo-600 hover:bg-indigo-700 text-white px-7 py-3 rounded-lg"
                        >
                            + Create Restaurant
                        </button>

                    </div>

                )}


                {/* CREATE RESTAURANT */}

                {restaurant?.creating && (

                    <CreateRestaurantForm
                        onCreated={handleRestaurantCreated}
                        onCancel={() => setRestaurant(null)}
                    />

                )}


                {/* EXISTING RESTAURANT */}

                {restaurant && !restaurant.creating && (

                    <>

                        <RestaurantSection
                            restaurant={restaurant}
                            onUpdated={handleRestaurantUpdated}
                            onDeleted={handleRestaurantDeleted}
                        />


                        <MenuSection
                            restaurant={restaurant}
                        />

                        <OrdersSection />

                    </>

                )}

            </main>

        </div>
    );
}

export default RestaurantOwnerDashboard;