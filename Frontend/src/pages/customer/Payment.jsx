import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import api from "../../services/api";

function Payment() {
    const { orderId } = useParams();
    const navigate = useNavigate();

    const [paymentMethod, setPaymentMethod] =
        useState("CashOnDelivery");

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    async function handlePayment() {
        try {
            setLoading(true);
            setError("");

            const response = await api.post(
                `/Payment/order/${orderId}`,
                {
                    paymentMethod: paymentMethod
                }
            );

            console.log("PAYMENT:", response.data);

            navigate("/orders");

        } catch (error) {
            console.error(error);

            setError(
                error.response?.data?.message ||
                "Payment failed"
            );
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="min-h-screen bg-gray-100 p-8">

            <div className="max-w-xl mx-auto">

                <button
                    onClick={() => navigate("/dashboards")}
                    className="text-indigo-600"
                >
                    ← Dashboard
                </button>

                <div className="bg-white rounded-xl shadow p-8 mt-6">

                    <h1 className="text-3xl font-bold">
                        Payment
                    </h1>

                    <p className="text-gray-500 mt-2">
                        Order #{orderId}
                    </p>

                    {error && (
                        <div className="bg-red-100 text-red-700 p-4 rounded-lg mt-6">
                            {error}
                        </div>
                    )}

                    <div className="mt-8">

                        <h2 className="font-semibold text-lg">
                            Select Payment Method
                        </h2>

                        <div className="space-y-3 mt-4">

                            <label className="flex items-center gap-3 border rounded-lg p-4 cursor-pointer">

                                <input
                                    type="radio"
                                    value="CashOnDelivery"
                                    checked={
                                        paymentMethod ===
                                        "CashOnDelivery"
                                    }
                                    onChange={(e) =>
                                        setPaymentMethod(
                                            e.target.value
                                        )
                                    }
                                />

                                <div>
                                    <p className="font-semibold">
                                        Cash on Delivery
                                    </p>

                                    <p className="text-sm text-gray-500">
                                        Pay when your food arrives.
                                    </p>
                                </div>

                            </label>


                            <label className="flex items-center gap-3 border rounded-lg p-4 cursor-pointer">

                                <input
                                    type="radio"
                                    value="Card"
                                    checked={
                                        paymentMethod === "Card"
                                    }
                                    onChange={(e) =>
                                        setPaymentMethod(
                                            e.target.value
                                        )
                                    }
                                />

                                <div>
                                    <p className="font-semibold">
                                        Card
                                    </p>

                                    <p className="text-sm text-gray-500">
                                        Pay using your card.
                                    </p>
                                </div>

                            </label>


                            <label className="flex items-center gap-3 border rounded-lg p-4 cursor-pointer">

                                <input
                                    type="radio"
                                    value="DigitalWallet"
                                    checked={
                                        paymentMethod ===
                                        "DigitalWallet"
                                    }
                                    onChange={(e) =>
                                        setPaymentMethod(
                                            e.target.value
                                        )
                                    }
                                />

                                <div>
                                    <p className="font-semibold">
                                        Digital Wallet
                                    </p>

                                    <p className="text-sm text-gray-500">
                                        Pay using a digital wallet.
                                    </p>
                                </div>

                            </label>

                        </div>

                    </div>

                    <button
                        onClick={handlePayment}
                        disabled={loading}
                        className="w-full bg-indigo-600 hover:bg-indigo-700 disabled:bg-gray-400 text-white py-3 rounded-lg mt-8"
                    >
                        {loading
                            ? "Processing..."
                            : "Confirm Payment"}
                    </button>

                </div>

            </div>

        </div>
    );
}

export default Payment;