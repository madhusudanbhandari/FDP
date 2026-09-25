import { useEffect, useState } from "react";
import api from "../../services/api";

function DeliverySection() {
    const [deliveries, setDeliveries] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        fetchDeliveries();
    }, []);

    async function fetchDeliveries() {
        try {
            setLoading(true);
            setError("");

            const response = await api.get("/Delivery");

            console.log("DELIVERIES:", response.data);

            setDeliveries(response.data || []);

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

    function getStatusClass(status) {
        switch (status) {
            case "Pending":
                return "bg-yellow-100 text-yellow-700";

            case "Assigned":
                return "bg-blue-100 text-blue-700";

            case "PickedUp":
                return "bg-purple-100 text-purple-700";

            case "OutForDelivery":
                return "bg-orange-100 text-orange-700";

            case "Delivered":
                return "bg-green-100 text-green-700";

            case "Failed":
                return "bg-red-100 text-red-700";

            default:
                return "bg-gray-100 text-gray-700";
        }
    }

    if (loading) {
        return (
            <div className="bg-white rounded-xl shadow p-8">
                Loading deliveries...
            </div>
        );
    }

    if (error) {
        return (
            <div className="bg-white rounded-xl shadow p-8">
                <p className="text-red-500">
                    {error}
                </p>

                <button
                    onClick={fetchDeliveries}
                    className="mt-4 bg-indigo-600 text-white px-4 py-2 rounded-lg"
                >
                    Try Again
                </button>
            </div>
        );
    }

    return (
        <div>

            {/* Section Header */}
            <div className="flex justify-between items-center mb-6">

                <div>
                    <h2 className="text-2xl font-bold text-gray-800">
                        Delivery Management
                    </h2>

                    <p className="text-gray-500">
                        Manage and assign deliveries
                    </p>
                </div>

                <button
                    onClick={fetchDeliveries}
                    className="bg-gray-800 text-white px-4 py-2 rounded-lg hover:bg-gray-900"
                >
                    Refresh
                </button>

            </div>


            {/* Empty State */}
            {deliveries.length === 0 && (
                <div className="bg-white rounded-xl shadow p-10 text-center">

                    <h3 className="text-xl font-semibold">
                        No Deliveries
                    </h3>

                    <p className="text-gray-500 mt-2">
                        There are currently no deliveries.
                    </p>

                </div>
            )}


            {/* Deliveries */}
            <div className="space-y-4">

                {deliveries.map((delivery) => (

                    <div
                        key={delivery.id}
                        className="bg-white rounded-xl shadow p-6"
                    >

                        <div className="flex justify-between items-start">

                            <div>

                                <h3 className="text-xl font-bold">
                                    Delivery #{delivery.id}
                                </h3>

                                <p className="text-gray-500 mt-1">
                                    Order #{delivery.orderId}
                                </p>

                            </div>


                            <span
                                className={`px-3 py-1 rounded-full text-sm font-semibold ${getStatusClass(
                                    delivery.deliveryStatus
                                )}`}
                            >
                                {delivery.deliveryStatus}
                            </span>

                        </div>


                        {/* Details */}
                        <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mt-6">

                            <div className="bg-gray-50 rounded-lg p-4">

                                <p className="text-sm text-gray-500">
                                    Delivery Person
                                </p>

                                <p className="font-semibold mt-1">
                                    {delivery.deliveryPersonId
                                        ? `User #${delivery.deliveryPersonId}`
                                        : "Not assigned"}
                                </p>

                            </div>


                            <div className="bg-gray-50 rounded-lg p-4">

                                <p className="text-sm text-gray-500">
                                    Assigned At
                                </p>

                                <p className="font-semibold mt-1">
                                    {delivery.assignedAt
                                        ? new Date(
                                              delivery.assignedAt
                                          ).toLocaleString()
                                        : "-"}
                                </p>

                            </div>


                            <div className="bg-gray-50 rounded-lg p-4">

                                <p className="text-sm text-gray-500">
                                    Delivered At
                                </p>

                                <p className="font-semibold mt-1">
                                    {delivery.deliveredAt
                                        ? new Date(
                                              delivery.deliveredAt
                                          ).toLocaleString()
                                        : "-"}
                                </p>

                            </div>

                        </div>

                    </div>

                ))}

            </div>

        </div>
    );
}

export default DeliverySection;