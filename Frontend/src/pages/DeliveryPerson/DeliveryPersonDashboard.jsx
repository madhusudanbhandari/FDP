import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../services/api";

function DeliveryPersonDashboard() {
    const navigate = useNavigate();

    const [availableDeliveries, setAvailableDeliveries] = useState([]);
    const [myDeliveries, setMyDeliveries] = useState([]);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        fetchDeliveries();
    }, []);

    async function fetchDeliveries() {
        try {
            setLoading(true);
            setError("");

            const [availableResponse, myResponse] =
                await Promise.all([
                    api.get("/Delivery/available"),
                    api.get("/Delivery/my")
                ]);

            setAvailableDeliveries(
                availableResponse.data || []
            );

            setMyDeliveries(
                myResponse.data || []
            );

        } catch (error) {
            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to load deliveries"
            );
        } finally {
            setLoading(false);
        }
    }

    async function pickupDelivery(deliveryId) {
        try {
            setError("");

            await api.patch(
                `/Delivery/${deliveryId}/pickup`
            );

            await fetchDeliveries();

        } catch (error) {
            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to pick up delivery"
            );
        }
    }

    async function outForDelivery(deliveryId) {
        try {
            setError("");

            await api.patch(
                `/Delivery/${deliveryId}/out-for-delivery`
            );

            await fetchDeliveries();

        } catch (error) {
            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to update delivery"
            );
        }
    }

    async function markDelivered(deliveryId) {
        try {
            setError("");

            await api.patch(
                `/Delivery/${deliveryId}/delivered`
            );

            await fetchDeliveries();

        } catch (error) {
            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to mark delivery as delivered"
            );
        }
    }

    function logout() {
        localStorage.removeItem("token");
        navigate("/login");
    }

    if (loading) {
        return (
            <div className="min-h-screen bg-gray-100 p-8">
                Loading deliveries...
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-gray-100">

            {/* Header */}
            <header className="bg-white shadow">
                <div className="max-w-6xl mx-auto px-6 py-4 flex justify-between items-center">

                    <div>
                        <h1 className="text-2xl font-bold">
                            Delivery Dashboard
                        </h1>

                        <p className="text-gray-500">
                            Manage your deliveries
                        </p>
                    </div>

                    <button
                        onClick={logout}
                        className="bg-red-500 text-white px-4 py-2 rounded-lg"
                    >
                        Logout
                    </button>

                </div>
            </header>


            <main className="max-w-6xl mx-auto p-6">

                {error && (
                    <div className="bg-red-100 text-red-700 p-4 rounded-lg mb-6">
                        {error}
                    </div>
                )}


                {/* Available */}
                <section>

                    <h2 className="text-2xl font-bold mb-4">
                        Available Deliveries
                    </h2>

                    {availableDeliveries.length === 0 ? (
                        <div className="bg-white rounded-xl shadow p-6 text-gray-500">
                            No deliveries are currently available.
                        </div>
                    ) : (
                        <div className="space-y-4">

                            {availableDeliveries.map((delivery) => (

                                <div
                                    key={delivery.id}
                                    className="bg-white rounded-xl shadow p-6"
                                >

                                    <h3 className="text-xl font-bold">
                                        Delivery #{delivery.id}
                                    </h3>

                                    <p className="text-gray-600">
                                        Order #{delivery.orderId}
                                    </p>

                                    <p className="mt-2">
                                        Status:
                                        <span className="font-semibold ml-2">
                                            {delivery.deliveryStatus}
                                        </span>
                                    </p>

                                    <button
                                        onClick={() =>
                                            pickupDelivery(
                                                delivery.id
                                            )
                                        }
                                        className="mt-4 bg-indigo-600 text-white px-5 py-2 rounded-lg hover:bg-indigo-700"
                                    >
                                        Pick Up
                                    </button>

                                </div>

                            ))}

                        </div>
                    )}

                </section>


                {/* My Deliveries */}
                <section className="mt-10">

                    <h2 className="text-2xl font-bold mb-4">
                        My Deliveries
                    </h2>

                    {myDeliveries.length === 0 ? (
                        <div className="bg-white rounded-xl shadow p-6 text-gray-500">
                            You have no deliveries.
                        </div>
                    ) : (
                        <div className="space-y-4">

                            {myDeliveries.map((delivery) => (

                                <div
                                    key={delivery.id}
                                    className="bg-white rounded-xl shadow p-6"
                                >

                                    <div className="flex justify-between">

                                        <div>
                                            <h3 className="text-xl font-bold">
                                                Delivery #{delivery.id}
                                            </h3>

                                            <p className="text-gray-600">
                                                Order #{delivery.orderId}
                                            </p>
                                        </div>

                                        <span className="font-semibold">
                                            {delivery.deliveryStatus}
                                        </span>

                                    </div>


                                    {delivery.deliveryStatus ===
                                        "PickedUp" && (

                                        <button
                                            onClick={() =>
                                                outForDelivery(
                                                    delivery.id
                                                )
                                            }
                                            className="mt-4 bg-orange-500 text-white px-5 py-2 rounded-lg"
                                        >
                                            Out for Delivery
                                        </button>

                                    )}


                                    {delivery.deliveryStatus ===
                                        "OutForDelivery" && (

                                        <button
                                            onClick={() =>
                                                markDelivered(
                                                    delivery.id
                                                )
                                            }
                                            className="mt-4 bg-green-600 text-white px-5 py-2 rounded-lg"
                                        >
                                            Mark Delivered
                                        </button>

                                    )}

                                </div>

                            ))}

                        </div>
                    )}

                </section>

            </main>

        </div>
    );
}

export default DeliveryPersonDashboard;