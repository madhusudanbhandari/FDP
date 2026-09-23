import { useState } from "react";
import api from "../../services/api";

function RestaurantSection({
    restaurant,
    onUpdated,
    onDeleted
}) {

    const [error, setError] = useState("");
    const [deleting, setDeleting] = useState(false);


    async function handleDeleteRestaurant() {

        const confirmed = window.confirm(
            "Are you sure you want to delete your restaurant?"
        );

        if (!confirmed) {
            return;
        }

        setDeleting(true);
        setError("");

        try {

            await api.delete(
                `/Restaurant/delete?id=${restaurant.id}`
            );

            onDeleted();

        } catch (error) {

            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to delete restaurant"
            );

        } finally {

            setDeleting(false);

        }
    }


    function handleUpdateRestaurant() {

        /*
         * We will implement the update form here.
         *
         * Keeping this function here means the dashboard
         * itself does not need to know how restaurant
         * updating works.
         */

        console.log(
            "Update restaurant:",
            restaurant
        );

    }


    return (

        <div className="bg-white rounded-2xl shadow-sm p-8 mt-8">

            {error && (

                <div className="bg-red-100 text-red-700 p-4 rounded-lg mb-6">
                    {error}
                </div>

            )}


            <div className="flex justify-between items-start">

                <div>

                    <p className="text-sm text-indigo-600 font-semibold uppercase">
                        Your Restaurant
                    </p>

                    <h2 className="text-3xl font-bold mt-2">
                        {restaurant.name}
                    </h2>

                    <p className="text-gray-500 mt-2">
                        📍 {restaurant.address}
                    </p>

                </div>


                {restaurant.isOpen ? (

                    <span className="bg-green-100 text-green-700 px-4 py-2 rounded-full">
                        ● Open
                    </span>

                ) : (

                    <span className="bg-red-100 text-red-700 px-4 py-2 rounded-full">
                        ● Closed
                    </span>

                )}

            </div>


            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-5 mt-8">

                <div className="bg-gray-50 rounded-xl p-5">

                    <p className="text-gray-500 text-sm">
                        Rating
                    </p>

                    <p className="text-2xl font-bold mt-2">
                        ⭐ {restaurant.rating}
                    </p>

                </div>


                <div className="bg-gray-50 rounded-xl p-5">

                    <p className="text-gray-500 text-sm">
                        Capacity
                    </p>

                    <p className="text-2xl font-bold mt-2">
                        {restaurant.capacity}
                    </p>

                </div>


                <div className="bg-gray-50 rounded-xl p-5">

                    <p className="text-gray-500 text-sm">
                        Speciality
                    </p>

                    <p className="text-xl font-bold mt-2">
                        {restaurant.special}
                    </p>

                </div>


                <div className="bg-gray-50 rounded-xl p-5">

                    <p className="text-gray-500 text-sm">
                        Restaurant ID
                    </p>

                    <p className="text-2xl font-bold mt-2">
                        #{restaurant.id}
                    </p>

                </div>

            </div>


            <div className="flex flex-wrap gap-4 mt-8">

                <button
                    onClick={handleUpdateRestaurant}
                    className="bg-indigo-600 text-white px-5 py-3 rounded-lg hover:bg-indigo-700"
                >
                    ✏️ Update Restaurant
                </button>


                <button
                    onClick={handleDeleteRestaurant}
                    disabled={deleting}
                    className="bg-red-500 text-white px-5 py-3 rounded-lg hover:bg-red-600 disabled:opacity-50"
                >
                    {deleting
                        ? "Deleting..."
                        : "🗑️ Delete Restaurant"}
                </button>

            </div>

        </div>
    );
}

export default RestaurantSection;