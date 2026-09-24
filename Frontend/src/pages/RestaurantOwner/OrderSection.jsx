import { useEffect, useState } from "react";
import api from "../../services/api";

function OrdersSection() {

    const [orders, setOrders] = useState([]);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const [updatingOrderId, setUpdatingOrderId] = useState(null);


    useEffect(() => {

        loadOrders();

    }, []);


    async function loadOrders() {

        setLoading(true);
        setError("");

        try {

            const response = await api.get(
                "/Order/restaurant-orders"
            );

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


    async function updateOrderStatus(orderId, status) {

        setUpdatingOrderId(orderId);
        setError("");

        try {

            const response = await api.patch(
                `/Order/update-order-status?orderId=${orderId}`,
                {
                    orderStatus: status
                }
            );

            setOrders(prev =>
                prev.map(order =>
                    order.id === orderId
                        ? response.data
                        : order
                )
            );

        } catch (error) {

            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to update order status"
            );

        } finally {

            setUpdatingOrderId(null);

        }
    }


    function getStatusStyle(status) {

        switch (status) {

            case "Pending":
                return "bg-yellow-100 text-yellow-700";

            case "Confirmed":
                return "bg-blue-100 text-blue-700";

            case "Preparing":
                return "bg-purple-100 text-purple-700";

            case "ReadyForPickup":
                return "bg-green-100 text-green-700";

            case "Cancelled":
                return "bg-red-100 text-red-700";

            case "Delivered":
                return "bg-green-100 text-green-700";

            default:
                return "bg-gray-100 text-gray-700";
        }
    }


    function getNextAction(order) {

        switch (order.status) {

            case "Pending":
                return (
                    <>
                        <button
                            onClick={() =>
                                updateOrderStatus(
                                    order.id,
                                    "Confirmed"
                                )
                            }
                            disabled={
                                updatingOrderId === order.id
                            }
                            className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg disabled:opacity-50"
                        >
                            Confirm Order
                        </button>

                        <button
                            onClick={() =>
                                updateOrderStatus(
                                    order.id,
                                    "Cancelled"
                                )
                            }
                            disabled={
                                updatingOrderId === order.id
                            }
                            className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded-lg disabled:opacity-50"
                        >
                            Reject
                        </button>
                    </>
                );


            case "Confirmed":
                return (
                    <button
                        onClick={() =>
                            updateOrderStatus(
                                order.id,
                                "Preparing"
                            )
                        }
                        disabled={
                            updatingOrderId === order.id
                        }
                        className="bg-purple-600 hover:bg-purple-700 text-white px-4 py-2 rounded-lg disabled:opacity-50"
                    >
                        Start Preparing
                    </button>
                );


            case "Preparing":
                return (
                    <button
                        onClick={() =>
                            updateOrderStatus(
                                order.id,
                                "ReadyForPickup"
                            )
                        }
                        disabled={
                            updatingOrderId === order.id
                        }
                        className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg disabled:opacity-50"
                    >
                        Ready for Pickup
                    </button>
                );


            default:
                return null;
        }
    }


    if (loading) {

        return (

            <div className="bg-white rounded-2xl shadow-sm p-8 mt-8">

                <p className="text-gray-500">
                    Loading orders...
                </p>

            </div>
        );
    }


    return (

        <div className="bg-white rounded-2xl shadow-sm p-8 mt-8">

            {/* HEADER */}

            <div className="flex justify-between items-center">

                <div>

                    <p className="text-sm text-indigo-600 font-semibold uppercase">
                        Order Management
                    </p>

                    <h2 className="text-2xl font-bold mt-1">
                        Restaurant Orders
                    </h2>

                </div>


                <button
                    onClick={loadOrders}
                    className="bg-gray-100 hover:bg-gray-200 px-4 py-2 rounded-lg"
                >
                    ↻ Refresh
                </button>

            </div>


            {/* ERROR */}

            {error && (

                <div className="bg-red-100 text-red-700 p-4 rounded-lg mt-5">
                    {error}
                </div>

            )}


            {/* EMPTY */}

            {!error && orders.length === 0 && (

                <div className="border-2 border-dashed border-gray-200 rounded-xl p-10 text-center mt-6">

                    <div className="text-5xl">
                        📦
                    </div>

                    <h3 className="text-xl font-semibold mt-4">
                        No orders yet
                    </h3>

                    <p className="text-gray-500 mt-2">
                        Orders from your customers will appear here.
                    </p>

                </div>

            )}


            {/* ORDERS */}

            {orders.length > 0 && (

                <div className="space-y-6 mt-6">

                    {orders.map(order => (

                        <div
                            key={order.id}
                            className="border rounded-xl p-6"
                        >

                            {/* ORDER HEADER */}

                            <div className="flex flex-col md:flex-row md:justify-between md:items-start gap-4">

                                <div>

                                    <p className="text-sm text-gray-500">
                                        Order
                                    </p>

                                    <h3 className="text-2xl font-bold">
                                        #{order.id}
                                    </h3>

                                    <p className="text-sm text-gray-500 mt-1">
                                        {new Date(
                                            order.createdAt
                                        ).toLocaleString()}
                                    </p>

                                </div>


                                <div className="flex flex-col items-start md:items-end gap-2">

                                    <span
                                        className={`px-4 py-2 rounded-full text-sm font-semibold ${getStatusStyle(order.status)}`}
                                    >
                                        {order.status}
                                    </span>

                                    <p className="text-xl font-bold">
                                        Rs. {order.totalAmount}
                                    </p>

                                </div>

                            </div>


                            {/* CUSTOMER */}

                            <div className="mt-6">

                                <p className="text-sm text-gray-500">
                                    Customer ID
                                </p>

                                <p className="font-semibold">
                                    #{order.userId}
                                </p>

                            </div>


                            {/* ITEMS */}

                            <div className="mt-6">

                                <h4 className="font-semibold text-lg">
                                    Ordered Items
                                </h4>


                                <div className="mt-3 space-y-3">

                                    {order.items?.map(item => (

                                        <div
                                            key={item.id}
                                            className="bg-gray-50 rounded-lg p-4 flex justify-between items-center"
                                        >

                                            <div>

                                                <p className="font-semibold">
                                                    {item.menuItemName}
                                                </p>

                                                <p className="text-sm text-gray-500">
                                                    Rs. {item.unitPrice}
                                                    {" × "}
                                                    {item.quantity}
                                                </p>

                                            </div>


                                            <p className="font-semibold">
                                                Rs. {item.subTotal}
                                            </p>

                                        </div>

                                    ))}

                                </div>

                            </div>


                            {/* ACTIONS */}

                            {getNextAction(order) && (

                                <div className="flex flex-wrap gap-3 mt-6 pt-6 border-t">

                                    {getNextAction(order)}

                                </div>

                            )}

                        </div>

                    ))}

                </div>

            )}

        </div>
    );
}

export default OrdersSection;