import { Link, useNavigate } from "react-router-dom";
import { getUserFromToken } from "../../utils/auth";

function RestaurantOwnerDashboard() {
    const user = getUserFromToken();
    const navigate = useNavigate();

    function handleLogout() {
        localStorage.removeItem("token");
        navigate("/login", { replace: true });
    }

    const name =
        user?.[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
        ];

    return (
        <div className="min-h-screen bg-gray-100">

            <nav className="bg-white shadow px-8 py-4 flex justify-between items-center">

                <h1 className="text-xl font-bold text-indigo-600">
                    FDP
                </h1>

                <div className="flex items-center gap-6">

                    <span className="text-gray-700">
                        Welcome, {name}
                    </span>

                    <button
                        onClick={handleLogout}
                        className="bg-red-500 text-white px-4 py-2 rounded-lg"
                    >
                        Logout
                    </button>

                </div>

            </nav>

            <main className="max-w-6xl mx-auto px-6 py-8">

                <h1 className="text-3xl font-bold text-gray-800">
                    Restaurant Owner Dashboard
                </h1>

                <p className="mt-2 text-gray-600">
                    Manage your restaurant, menu and orders.
                </p>

                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mt-8">

                    <Link
                        to="/owner/restaurant"
                        className="bg-white p-6 rounded-xl shadow hover:shadow-lg transition"
                    >
                        <h2 className="text-xl font-semibold">
                            🏪 My Restaurant
                        </h2>

                        <p className="mt-2 text-gray-500">
                            Create and manage your restaurant.
                        </p>
                    </Link>

                    <Link
                        to="/owner/menu"
                        className="bg-white p-6 rounded-xl shadow hover:shadow-lg transition"
                    >
                        <h2 className="text-xl font-semibold">
                            📋 Menu
                        </h2>

                        <p className="mt-2 text-gray-500">
                            Manage your restaurant menu.
                        </p>
                    </Link>

                    <Link
                        to="/owner/menu-items"
                        className="bg-white p-6 rounded-xl shadow hover:shadow-lg transition"
                    >
                        <h2 className="text-xl font-semibold">
                            🍔 Menu Items
                        </h2>

                        <p className="mt-2 text-gray-500">
                            Add and manage food items.
                        </p>
                    </Link>

                    <Link
                        to="/owner/orders"
                        className="bg-white p-6 rounded-xl shadow hover:shadow-lg transition"
                    >
                        <h2 className="text-xl font-semibold">
                            📦 Orders
                        </h2>

                        <p className="mt-2 text-gray-500">
                            View and manage customer orders.
                        </p>
                    </Link>

                </div>

            </main>

        </div>
    );
}

export default RestaurantOwnerDashboard;