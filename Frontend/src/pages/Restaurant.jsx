import { useEffect, useState } from "react";
import api from "../services/api";

function Restaurant() {
  const [restaurants, setRestaurants] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const getRestaurants = async () => {
      try {
        const response = await api.get("/Restaurant/View-all-restaurants");
        setRestaurants(response.data.items);
      } catch (error) {
        console.error("Failed to fetch restaurants:", error);
        setError("Failed to load restaurants.");
      } finally {
        setLoading(false);
      }
    };

    getRestaurants();
  }, []);

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="flex flex-col items-center gap-3">
          <div className="h-10 w-10 rounded-full border-4 border-indigo-200 border-t-indigo-600 animate-spin" />
          <p className="text-gray-500 text-sm">Loading restaurants...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
        <div className="max-w-sm w-full rounded-xl bg-red-50 border border-red-200 px-6 py-5 text-center">
          <p className="text-red-600 font-medium">{error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 px-4 py-10">
      <div className="max-w-6xl mx-auto">
        <div className="mb-10 text-center">
          <h1 className="text-4xl font-bold text-gray-800">Restaurants</h1>
          <p className="mt-2 text-gray-500">
            Discover great places to eat near you
          </p>
        </div>

        {restaurants.length === 0 ? (
          <p className="text-center text-gray-500">No restaurants found.</p>
        ) : (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {restaurants.map((restaurant) => (
              <div
                key={restaurant.id}
                className="bg-white rounded-2xl shadow-md hover:shadow-xl transition-shadow duration-300 overflow-hidden flex flex-col"
              >
                <div className="h-32 bg-gradient-to-br from-indigo-500 to-purple-500 flex items-center justify-center">
                  <span className="text-white text-2xl font-bold tracking-wide">
                    {restaurant.name?.charAt(0)?.toUpperCase()}
                  </span>
                </div>

                <div className="p-5 flex flex-col flex-1">
                  <div className="flex items-start justify-between gap-2 mb-2">
                    <h2 className="text-lg font-semibold text-gray-800">
                      {restaurant.name}
                    </h2>
                    {restaurant.rating != null && (
                      <span className="shrink-0 inline-flex items-center gap-1 rounded-full bg-yellow-50 border border-yellow-200 px-2.5 py-1 text-xs font-semibold text-yellow-700">
                        ★ {restaurant.rating}
                      </span>
                    )}
                  </div>

                  <p className="text-sm text-gray-500 mb-3 flex items-start gap-1.5">
                    <span className="mt-0.5">📍</span>
                    <span>{restaurant.address}</span>
                  </p>

                  {restaurant.special && (
                    <div className="mt-auto pt-3 border-t border-gray-100">
                      <p className="text-sm text-indigo-600 font-medium">
                        ✨ {restaurant.special}
                      </p>
                    </div>
                  )}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

export default Restaurant;