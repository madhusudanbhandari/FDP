import { useState } from "react";
import { useNavigate } from "react-router-dom";
import DeliverySection from "./DeliverySection";

function AdminDashboard() {
    const navigate = useNavigate();
    const [activeSection, setActiveSection] = useState("deliveries");

    function logout() {
        localStorage.removeItem("token");
        navigate("/login");
    }

    return (
        <div className="min-h-screen bg-gray-100">

            {/* Header */}
            <header className="bg-white shadow">
                <div className="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">

                    <div>
                        <h1 className="text-2xl font-bold text-gray-800">
                            Admin Dashboard
                        </h1>

                        <p className="text-sm text-gray-500">
                            Food Delivery Platform
                        </p>
                    </div>

                    <button
                        onClick={logout}
                        className="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600"
                    >
                        Logout
                    </button>

                </div>
            </header>


            {/* Navigation */}
            <div className="max-w-7xl mx-auto px-6 pt-6">

                <div className="bg-white rounded-lg shadow p-2 flex gap-2">

                    <button
                        onClick={() => setActiveSection("deliveries")}
                        className={`px-4 py-2 rounded-lg ${
                            activeSection === "deliveries"
                                ? "bg-indigo-600 text-white"
                                : "bg-gray-100 text-gray-700"
                        }`}
                    >
                        Deliveries
                    </button>

                </div>

            </div>


            {/* Main Content */}
            <main className="max-w-7xl mx-auto px-6 py-6">

                {activeSection === "deliveries" && (
                    <DeliverySection />
                )}

            </main>

        </div>
    );
}

export default AdminDashboard;