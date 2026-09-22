import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import api from "../../services/api";

function RestaurantList() {
    const [restaurants, setRestaurants] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const [search, setSearch] = useState("");

    useEffect(() => {
        fetchRestaurants();
    }, []);

    async function fetchRestaurants() {
        try {
            setLoading(true);

            const response = await api.get("/Restaurant/View-all-restaurants", {
                params: {
                    page: 1,
                    pageSize: 10,
                    search: search || undefined,
                    sortBy: "rating",
                    sortOrder: "desc"
                }
            });

            setRestaurants(response.data.items || []);
        } catch (error) {
            console.error(error);
            setError("Failed to load restaurants");
        } finally {
            setLoading(false);
        }
    }

    function handleSearch(e) {
        e.preventDefault();
        fetchRestaurants();
    }

    if (loading) {
        return <h2>Loading restaurants...</h2>;
    }

    return (
        <div className="min-h-screen bg-gray-100 p-8">

            <div className="max-w-6xl mx-auto">

                <Link
                    to="/dashboards"
                    className="text-indigo-600"
                >
                    ← Back to Dashboard
                </Link>

                <h1 className="text-3xl font-bold mt-6">
                    Restaurants
                </h1>

                <form
                    onSubmit={handleSearch}
                    className="flex gap-3 mt-6"
                >
                    <input
                        type="text"
                        placeholder="Search restaurants..."
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                        className="border p-3 rounded-lg flex-1"
                    />

                    <button
                        type="submit"
                        className="bg-indigo-600 text-white px-6 rounded-lg"
                    >
                        Search
                    </button>
                </form>

                {error && (
                    <p className="text-red-500 mt-4">
                        {error}
                    </p>
                )}

                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mt-8">

                    {restaurants.map((restaurant) => (
                        <Link
                            key={restaurant.id}
                            to={`/restaurants/${restaurant.id}`}
                            className="bg-white p-6 rounded-xl shadow hover:shadow-lg"
                        >
                            <h2 className="text-xl font-bold">
                                {restaurant.name}
                            </h2>

                            <p className="text-gray-600 mt-2">
                                📍 {restaurant.address}
                            </p>

                            <p className="mt-2">
                                ⭐ {restaurant.rating}
                            </p>

                            <p className="mt-2">
                                {restaurant.isOpen ? (
                                    <span className="text-green-600">
                                        Open
                                    </span>
                                ) : (
                                    <span className="text-red-600">
                                        Closed
                                    </span>
                                )}
                            </p>

                            <p className="text-gray-500 mt-2">
                                {restaurant.special}
                            </p>
                        </Link>
                    ))}

                </div>

            </div>

        </div>
    );
}

export default RestaurantList;