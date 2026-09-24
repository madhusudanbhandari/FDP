import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import api from "../../services/api";

function Orders() {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        fetchOrders();
    }, []);

    async function fetchOrders() {
        try {
            setLoading(true);
            setError("");

            const response = await api.get("/Order/my-orders");

            console.log("MY ORDERS:", response.data);

            setOrders(response.data || []);

        } catch (error) {
            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to load orders"
            );
        } finally {
            setLoading(false);
        }
    }

    async function cancelOrder(orderId) {
        const confirmed = window.confirm(
            "Are you sure you want to cancel this order?"
        );

        if (!confirmed) {
            return;
        }

        try {
            await api.patch(`/Order/${orderId}/cancel`);

            await fetchOrders();

        } catch (error) {
            console.error(error);

            alert(
                error.response?.data?.message ||
                "Failed to cancel order"
            );
        }
    }

    function getStatusClass(status) {
        switch (status) {
            case "Pending":
                return "bg-yellow-100 text-yellow-700";

            case "Confirmed":
                return "bg-blue-100 text-blue-700";

            case "Preparing":
                return "bg-purple-100 text-purple-700";

            case "ReadyForPickup":
                return "bg-orange-100 text-orange-700";

            case "Delivered":
                return "bg-green-100 text-green-700";

            case "Cancelled":
                return "bg-red-100 text-red-700";

            default:
                return "bg-gray-100 text-gray-700";
        }
    }

    if (loading) {
        return (
            <div className="min-h-screen bg-gray-100 p-8">
                <div className="max-w-5xl mx-auto">
                    Loading orders...
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-gray-100 p-8">

            <div className="max-w-5xl mx-auto">

                <Link
                    to="/dashboards"
                    className="text-indigo-600"
                >
                    ← Dashboard
                </Link>

                <div className="flex justify-between items-center mt-6">

                    <div>
                        <h1 className="text-3xl font-bold">
                            My Orders
                        </h1>

                        <p className="text-gray-500 mt-2">
                            Track your food orders.
                        </p>
                    </div>

                    <button
                        onClick={fetchOrders}
                        className="bg-gray-200 hover:bg-gray-300 px-4 py-2 rounded-lg"
                    >
                        Refresh
                    </button>

                </div>

                {error && (
                    <div className="bg-red-100 text-red-700 p-4 rounded-lg mt-6">
                        {error}
                    </div>
                )}

                {orders.length === 0 ? (

                    <div className="bg-white rounded-xl shadow p-10 mt-8 text-center">

                        <div className="text-5xl mb-4">
                            🍽️
                        </div>

                        <h2 className="text-2xl font-bold">
                            No Orders Yet
                        </h2>

                        <p className="text-gray-500 mt-2">
                            Your placed orders will appear here.
                        </p>

                        <Link
                            to="/restaurants"
                            className="inline-block mt-6 bg-indigo-600 text-white px-6 py-3 rounded-lg"
                        >
                            Browse Restaurants
                        </Link>

                    </div>

                ) : (

                    <div className="space-y-6 mt-8">

                        {orders.map((order) => (

                            <div
                                key={order.id}
                                className="bg-white rounded-xl shadow p-6"
                            >

                                <div className="flex justify-between items-start">

                                    <div>
                                        <h2 className="text-xl font-bold">
                                            Order #{order.id}
                                        </h2>

                                        <p className="text-gray-500 mt-1">
                                            {order.restaurantName}
                                        </p>

                                        <p className="text-sm text-gray-400 mt-1">
                                            {new Date(
                                                order.createdAt
                                            ).toLocaleString()}
                                        </p>
                                    </div>

                                    <span
                                        className={`px-3 py-1 rounded-full text-sm font-semibold ${getStatusClass(
                                            order.status
                                        )}`}
                                    >
                                        {order.status}
                                    </span>

                                </div>

                                <div className="border-t mt-5 pt-5">

                                    <h3 className="font-semibold mb-3">
                                        Items
                                    </h3>

                                    {order.items?.map((item) => (

                                        <div
                                            key={item.id}
                                            className="flex justify-between py-2"
                                        >

                                            <div>
                                                <span className="font-medium">
                                                    {item.menuItemName}
                                                </span>

                                                <span className="text-gray-500 ml-2">
                                                    × {item.quantity}
                                                </span>
                                            </div>

                                            <span>
                                                Rs. {item.subTotal}
                                            </span>

                                        </div>

                                    ))}

                                </div>

                                <div className="border-t mt-4 pt-4 flex justify-between items-center">

                                    <span className="text-lg font-bold">
                                        Total
                                    </span>

                                    <span className="text-lg font-bold">
                                        Rs. {order.totalAmount}
                                    </span>

                                </div>

                                {(order.status === "Pending" ||
                                    order.status === "Confirmed") && (

                                    <button
                                        onClick={() =>
                                            cancelOrder(order.id)
                                        }
                                        className="mt-5 text-red-600 hover:text-red-700 font-medium"
                                    >
                                        Cancel Order
                                    </button>

                                )}

                            </div>

                        ))}

                    </div>

                )}

            </div>

        </div>
    );
}

export default Orders;