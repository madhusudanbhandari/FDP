import { Link } from "react-router-dom";
import { getUserFromToken } from "../../utils/auth";

function CustomerDashboard() {

    const user = getUserFromToken();

    return (
        <div className="min-h-screen bg-gray-100">

            <nav className="bg-white shadow px-8 py-4 flex justify-between">
                <h1 className="text-xl font-bold text-indigo-600">
                    FDP
                </h1>

                <span className="text-gray-700">
                    Welcome, {user?.[
                        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
                    ]}
                </span>
            </nav>

            <main className="max-w-6xl mx-auto px-6 py-8">

                <h1 className="text-3xl font-bold text-gray-800">
                    Customer Dashboard
                </h1>

                <p className="mt-2 text-gray-600">
                    What would you like to do today?
                </p>

                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mt-8">

                    <Link
                        to="/restaurants"
                        className="bg-white p-6 rounded-xl shadow hover:shadow-lg transition"
                    >
                        <h2 className="text-xl font-semibold">
                            🍔 Restaurants
                        </h2>

                        <p className="mt-2 text-gray-500">
                            Browse restaurants and their menus.
                        </p>
                    </Link>

                    <Link
                        to="/cart"
                        className="bg-white p-6 rounded-xl shadow hover:shadow-lg transition"
                    >
                        <h2 className="text-xl font-semibold">
                            🛒 My Cart
                        </h2>

                        <p className="mt-2 text-gray-500">
                            View and manage your cart.
                        </p>
                    </Link>

                    <Link
                        to="/orders"
                        className="bg-white p-6 rounded-xl shadow hover:shadow-lg transition"
                    >
                        <h2 className="text-xl font-semibold">
                            📦 My Orders
                        </h2>

                        <p className="mt-2 text-gray-500">
                            View your previous and current orders.
                        </p>
                    </Link>

                    <Link
                        to="/notifications"
                        className="bg-white p-6 rounded-xl shadow hover:shadow-lg transition"
                    >
                        <h2 className="text-xl font-semibold">
                            🔔 Notifications
                        </h2>

                        <p className="mt-2 text-gray-500">
                            View your notifications.
                        </p>
                    </Link>

                </div>

            </main>

        </div>
    );
}

export default CustomerDashboard;