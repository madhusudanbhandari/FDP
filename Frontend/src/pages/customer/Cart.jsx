import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import api from "../../services/api";

function Cart() {
    const [cart, setCart] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        fetchCart();
    }, []);

    async function fetchCart() {
        try {
            setLoading(true);

            const response = await api.get("/Cart");

            console.log("CART:", response.data);

            setCart(response.data);
        } catch (error) {
            console.error(error);
            setError("Failed to load cart");
        } finally {
            setLoading(false);
        }
    }

    async function updateQuantity(cartItemId, quantity) {
        if (quantity < 1) {
            return;
        }

        try {
            await api.patch(
                `/Cart/items/${cartItemId}`,
                {
                    quantity: quantity
                }
            );

            await fetchCart();
        } catch (error) {
            console.error(error);
            alert("Failed to update quantity");
        }
    }

    async function removeItem(cartItemId) {
        try {
            await api.delete(
                `/Cart/items/${cartItemId}`
            );

            await fetchCart();
        } catch (error) {
            console.error(error);
            alert("Failed to remove item");
        }
    }

    if (loading) {
        return (
            <div className="p-8">
                Loading cart...
            </div>
        );
    }

    if (error) {
        return (
            <div className="p-8 text-red-500">
                {error}
            </div>
        );
    }

    if (!cart || !cart.cartItems || cart.cartItems.length === 0) {
        return (
            <div className="min-h-screen bg-gray-100 p-8">

                <div className="max-w-5xl mx-auto">

                    <Link
                        to="/restaurants"
                        className="text-indigo-600"
                    >
                        ← Continue Shopping
                    </Link>

                    <div className="bg-white rounded-xl shadow p-8 mt-6 text-center">

                        <h1 className="text-2xl font-bold">
                            Your Cart is Empty
                        </h1>

                        <p className="text-gray-500 mt-2">
                            Add some delicious food first.
                        </p>

                        <Link
                            to="/restaurants"
                            className="inline-block mt-6 bg-indigo-600 text-white px-6 py-3 rounded-lg"
                        >
                            Browse Restaurants
                        </Link>

                    </div>

                </div>

            </div>
        );
    }

    return (
        <div className="min-h-screen bg-gray-100 p-8">

            <div className="max-w-5xl mx-auto">

                <Link
                    to="/restaurants"
                    className="text-indigo-600"
                >
                    ← Continue Shopping
                </Link>

                <h1 className="text-3xl font-bold mt-6">
                    My Cart
                </h1>

                <div className="bg-white rounded-xl shadow mt-6">

                    {cart.cartItems.map((item) => (

                        <div
                            key={item.id}
                            className="border-b p-6 flex justify-between items-center"
                        >

                            <div>
                                <h2 className="text-xl font-semibold">
                                    {item.menuItem?.name}
                                </h2>

                                <p className="text-gray-500">
                                    Rs. {item.menuItem?.price}
                                </p>
                            </div>

                            <div className="flex items-center gap-4">

                                <button
                                    onClick={() =>
                                        updateQuantity(
                                            item.id,
                                            item.quantity - 1
                                        )
                                    }
                                    className="bg-gray-200 px-3 py-1 rounded"
                                >
                                    −
                                </button>

                                <span className="font-semibold">
                                    {item.quantity}
                                </span>

                                <button
                                    onClick={() =>
                                        updateQuantity(
                                            item.id,
                                            item.quantity + 1
                                        )
                                    }
                                    className="bg-gray-200 px-3 py-1 rounded"
                                >
                                    +
                                </button>

                                <button
                                    onClick={() =>
                                        removeItem(item.id)
                                    }
                                    className="text-red-500 ml-4"
                                >
                                    Remove
                                </button>

                            </div>

                        </div>

                    ))}

                </div>

                <div className="bg-white rounded-xl shadow p-6 mt-6">

                    <div className="flex justify-between text-xl font-bold">
                        <span>Total</span>

                        <span>
                            Rs. {cart.totalAmount}
                        </span>
                    </div>

                    <button
                        className="w-full bg-indigo-600 text-white py-3 rounded-lg mt-6"
                    >
                        Proceed to Checkout
                    </button>

                </div>

            </div>

        </div>
    );
}

export default Cart;